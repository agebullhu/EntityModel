﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CSRedis
{
    /// <summary>
    ///     Connection链接池
    /// </summary>
    public class ConnectionPool
    {
        private static readonly object _lock = new object();
        private static readonly object _lock_GetConnectionQueue = new object();
        private string _connectionString;
        private string _ip = "127.0.0.1", _password = "";

        private int _poolsize = 50, _port = 6379, _database, _writebuffer = 10240;
        private bool _ssl;
        public List<RedisConnection2> AllConnections = new List<RedisConnection2>();
        public Queue<RedisConnection2> FreeConnections = new Queue<RedisConnection2>();

        public Queue<TaskCompletionSource<RedisConnection2>> GetConnectionAsyncQueue =
            new Queue<TaskCompletionSource<RedisConnection2>>();

        public Queue<ManualResetEventSlim> GetConnectionQueue = new Queue<ManualResetEventSlim>();

        private DateTime requirePingPrevTime;

        public ConnectionPool()
        {
            Connected += (s, o) =>
            {
                var rc = (RedisClient)s;
                if (!string.IsNullOrEmpty(_password))
                    rc.Auth(_password);
                if (_database > 0)
                    rc.Select(_database);
            };
        }

        internal string ClusterKey => $"{_ip}:{_port}/{_database}";
        internal string Prefix { get; set; }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                if (string.IsNullOrEmpty(_connectionString))
                    return;
                var vs = _connectionString.Split(',');
                foreach (var v in vs)
                {
                    if (v.IndexOf('=') == -1)
                    {
                        var host = v.Split(':');
                        _ip = string.IsNullOrEmpty(host[0].Trim()) == false ? host[0].Trim() : "127.0.0.1";
                        if (host.Length < 2 || int.TryParse(host[1].Trim(), out _port) == false) _port = 6379;
                        continue;
                    }

                    var kv = v.Split(new[] { '=' }, 2);

                    switch (kv[0].ToLower().Trim())
                    {
                        case "password":
                            _password = kv.Length > 1 ? kv[1] : "";
                            break;
                        case "prefix":
                            Prefix = kv.Length > 1 ? kv[1] : "";
                            break;
                        case "defaultdatabase":
                            _database = int.TryParse(kv.Length > 1 ? kv[1] : "0", out _database) ? _database : 0;
                            break;
                        case "poolsize":
                            _poolsize = int.TryParse(kv.Length > 1 ? kv[1] : "0", out _poolsize) ? _poolsize : 50;
                            break;
                        case "ssl":
                            _ssl = kv.Length > 1 && kv[1] == "true";
                            break;
                        case "writebuffer":
                            _writebuffer = int.TryParse(kv.Length > 1 ? kv[1] : "10240", out _writebuffer) ? _writebuffer : 10240;
                            break;
                    }
                }

                if (_poolsize <= 0)
                    _poolsize = 50;
                var initConns = new RedisConnection2[_poolsize];
                for (var a = 0; a < _poolsize; a++)
                    initConns[a] = GetFreeConnection();
                foreach (var conn in initConns)
                    ReleaseConnection(conn);
            }
        }

        public event EventHandler Connected;

        private RedisConnection2 GetFreeConnection()
        {
            RedisConnection2 conn = null;
            if (FreeConnections.Count > 0)
                lock (_lock)
                {
                    if (FreeConnections.Count > 0)
                        conn = FreeConnections.Dequeue();
                }

            if (conn != null || AllConnections.Count >= _poolsize)
                return conn;
            lock (_lock)
            {
                if (AllConnections.Count < _poolsize)
                {
                    conn = new RedisConnection2();
                    AllConnections.Add(conn);
                }
            }

            if (conn == null)
                return null;
            conn.Pool = this;
            var ips = Dns.GetHostAddresses(_ip);
            if (ips.Length == 0)
                throw new Exception($"无法解析“{_ip}”");
            conn.Client = new RedisClient(new IPEndPoint(ips[0], _port), _ssl, 1000, _writebuffer);
            conn.Client.Connected += Connected;

            return conn;
        }

        public RedisConnection2 GetConnection()
        {
            var conn = GetFreeConnection();
            if (conn == null)
            {
                var wait = new ManualResetEventSlim(false);
                lock (_lock_GetConnectionQueue)
                {
                    GetConnectionQueue.Enqueue(wait);
                }

                if (wait.Wait(TimeSpan.FromSeconds(10)))
                    return GetConnection();
                throw new Exception("CSRedis.ConnectionPool.GetConnection 连接池获取超时（10秒）");
            }

            if (conn.Client.IsConnected == false || DateTime.Now.Subtract(conn.LastActive).TotalSeconds > 60)
                try
                {
                    conn.Client.Ping();
                }
                catch
                {
                    var ips = Dns.GetHostAddresses(_ip);
                    if (ips.Length == 0) throw new Exception($"无法解析“{_ip}”");
                    try
                    {
                        conn.Client.Dispose();
                    }
                    catch
                    {
                    }

                    conn.Client = new RedisClient(new IPEndPoint(ips[0], _port));
                    conn.Client.Connected += Connected;
                }

            conn.ThreadId = Thread.CurrentThread.ManagedThreadId;
            conn.LastActive = DateTime.Now;
            Interlocked.Increment(ref conn.UseSum);
            return conn;
        }

        public async Task<RedisConnection2> GetConnectionAsync()
        {
            var conn = GetFreeConnection();
            if (conn == null)
            {
                var tcs = new TaskCompletionSource<RedisConnection2>();
                lock (_lock_GetConnectionQueue)
                {
                    GetConnectionAsyncQueue.Enqueue(tcs);
                }

                conn = await tcs.Task;
            }

            if (conn.Client.IsConnected == false || DateTime.Now.Subtract(conn.LastActive).TotalSeconds > 60)
                try
                {
                    await conn.Client.PingAsync();
                }
                catch
                {
                    var ips = Dns.GetHostAddresses(_ip);
                    if (ips.Length == 0) throw new Exception($"无法解析“{_ip}”");
                    try
                    {
                        conn.Client.Dispose();
                    }
                    catch
                    {
                    }

                    conn.Client = new RedisClient(new IPEndPoint(ips[0], _port));
                    conn.Client.Connected += Connected;
                }

            conn.ThreadId = Thread.CurrentThread.ManagedThreadId;
            conn.LastActive = DateTime.Now;
            Interlocked.Increment(ref conn.UseSum);
            return conn;
        }

        /// <summary>
        ///     redis-server 断开重启后，因连接池内所有连接状态无法更新，导致每个连接在重启后的第一次操作仍会失败。
        ///     csredis 内部统一处理错误，一量发现错误，连接池内所有连接下次操作将触发 Ping() 条件，解决上述问题。
        ///     缺陷：如果错误为非 socket 错误，将导致性能下降。根据异常信息优化程序使用方法，可以解决这个缺陷。
        /// </summary>
        /// <param name="ex"></param>
        internal void RequirePing(Exception ex)
        {
            var lastActive = new DateTime(2000, 1, 1);
            foreach (var conn in AllConnections) conn.LastActive = lastActive;
            var now = DateTime.Now;
            if (now.Subtract(requirePingPrevTime).TotalSeconds > 5)
            {
                requirePingPrevTime = now;
                var fcolor = Console.ForegroundColor;
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"csreids 错误【{ClusterKey}】：{ex.Message}");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("redis-server 断开重启后，因连接池内所有连接状态无法更新，导致每个连接在重启后的第一次操作仍会失败。");
                Console.WriteLine("csredis 内部统一处理错误，一量发现错误，连接池内所有连接下次操作将触发 Ping() 条件，解决上述问题。");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("缺陷：如果错误为非 socket 错误，将导致性能下降。根据异常信息优化程序使用方法，可以解决这个缺陷。");
                Console.ForegroundColor = fcolor;
                Console.WriteLine("");
            }
        }

        public void ReleaseConnection(RedisConnection2 conn, bool isReset = false)
        {
            if (isReset)
            {
                try
                {
                    conn.Client.Quit();
                }
                catch
                {
                }

                var ips = Dns.GetHostAddresses(_ip);
                if (ips.Length == 0) throw new Exception($"无法解析“{_ip}”");
                try
                {
                    conn.Client.Dispose();
                }
                catch
                {
                }

                conn.Client = new RedisClient(new IPEndPoint(ips[0], _port), _ssl, 1000, _writebuffer);
                conn.Client.Connected += Connected;
            }

            lock (_lock)
            {
                FreeConnections.Enqueue(conn);
            }

            var isAsync = false;
            if (GetConnectionAsyncQueue.Count > 0)
            {
                lock (_lock_GetConnectionQueue)
                {
                    if (GetConnectionAsyncQueue.Count > 0)
                    {
                        var tcs = GetConnectionAsyncQueue.Dequeue();
                        isAsync = tcs != null;
                        if (isAsync)
                            tcs.SetResult(GetConnectionAsync().Result);
                    }
                }
            }

            if (isAsync || GetConnectionQueue.Count <= 0)
                return;
            ManualResetEventSlim wait = null;
            lock (_lock_GetConnectionQueue)
            {
                if (GetConnectionQueue.Count > 0)
                    wait = GetConnectionQueue.Dequeue();
            }

            wait?.Set();
        }
    }

    public class RedisConnection2 : IDisposable
    {
        public RedisClient Client;
        public DateTime LastActive;
        internal ConnectionPool Pool;
        internal int ThreadId;
        public long UseSum;

        public void Dispose()
        {
            Pool?.ReleaseConnection(this);
        }
    }
}