using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using CSRedis;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class CSRedisEx : IDisposable, IRedis
    {
        #region 配置

        /// <summary>
        /// 不关闭
        /// </summary>
        bool IRedis.NoClose => false;

        /// <summary>
        /// 静态构造
        /// </summary>
        static CSRedisEx()
        {
            var c = ConfigurationManager.AppSettings["RedisConnectionString"].Split(',', ':');
            Address = c[0];
            Port = c.Length > 1 ? int.Parse(c[1]) : 6379;
            //PoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["RedisPoolSize"]);
            //if (PoolSize < 100)
            //    PoolSize = 100;
            if (c.Length > 2)
                PassWord = c[2];
        }

        /// <summary>
        /// 地址
        /// </summary>
        static readonly string Address;

        /// <summary>
        /// 密码
        /// </summary>
        static readonly string PassWord;
        /// <summary>
        /// 端口
        /// </summary>
        public static readonly int Port;
        /*// <summary>
        /// 空闲连接数
        /// </summary>
        private static readonly int PoolSize;*/

        #endregion

        #region 构造

        private static ConnectionPool _connectionPool;
        private RedisConnection2 _connection;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object LockObj = new object();

        /// <summary>
        /// 使用哪个数据库
        /// </summary>
        private long _db;

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb => _db;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        public CSRedisEx(long db = 0)
        {
            _db = db;
        }

        /// <summary>
        /// 使用中的
        /// </summary>
        private readonly Dictionary<long, RedisConnection2> Used = new Dictionary<long, RedisConnection2>();

        /// <summary>
        /// 客户端类
        /// </summary>
        internal RedisClient _client;
        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public RedisClient Client
        {
            get
            {
                if (_client != null)
                    return _client;
                return _client = CreateClient(_db);
            }
        }

        private string ConnectString(long db) =>
            string.IsNullOrEmpty(PassWord)
                ? $"{Address}:{Port},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240"
                : $"{Address}:{Port},password={PassWord},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240";

        private RedisClient CreateClient(long db)
        {
            Monitor.Enter(LockObj);
            try
            {
                _db = db;
                if (Used.TryGetValue(db, out _connection))
                    return _connection.Client;
                _connectionPool = new ConnectionPool
                {
                    ConnectionString = ConnectString(db)
                };
                _connection = _connectionPool.GetConnection();
                Used.Add(db, _connection);
                return _connection.Client;
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(LockObj);
            try
            {
                foreach (var con in Used.Values)
                    con.Dispose();
                Used.Clear();
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        #endregion

        #region 文本读写

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Client.Del(key);
        }

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public void FindAndRemoveKey(string pattern)
        {
            var keys = Client.Keys(pattern);
            Client.Del(keys);
        }

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.Get(key);
        }


        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Client.Set(key, value);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set(string key, string value, DateTime last)
        {
            Set(key, value, DateTime.Now - last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void Set(string key, string value, TimeSpan span)
        {
            Client.Set(key, value, (int)span.TotalSeconds);
        }

        #endregion

        #region 值读写

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetTime(string key, DateTime last)
        {
            Client.Expire(key, DateTime.Now - last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetTime(string key, TimeSpan span)
        {
            Client.Expire(key, span);
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
            where T : struct
        {
            var str = Client.Get(key);
            return string.IsNullOrEmpty(str) ? default(T) : JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value)
            where T : struct
        {
            Client.Set(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value, DateTime last)
            where T : struct
        {
            SetValue(key, value, DateTime.Now - last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetValue<T>(string key, T value, TimeSpan span)
            where T : struct
        {
            Client.Set(key, JsonConvert.SerializeObject(value), (int)span.TotalSeconds);
        }

        #endregion

        #region 对象读写


        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public List<T> GetAll<T>(string pattern)
            where T : class
        {
            List<T> lists = new List<T>();
            long cursor = 0;
            do
            {
                var result = Client.Scan(cursor, pattern, 100);
                cursor = result.Cursor;
                foreach (var key in result.Items)
                {
                    lists.Add(Get<T>(key));
                }
            } while (cursor > 0);
            return lists;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// 如果反序列化失败会抛出异常
        /// </remarks>
        public T Get<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// 试图取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>如果反序列化失败失败会访回空</returns>
        public T TryGet<T>(string key)
            where T : class
        {
            var str = Client.Get(key);
            if (string.IsNullOrEmpty(str))
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, DateTime last)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value),DateTime.Now- last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void Set<T>(string key, T value, TimeSpan span)
            where T : class
        {
            Client.Set(key, JsonConvert.SerializeObject(value), span);
        }

        #endregion

        #region 实体缓存支持

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">键的组合</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public TData GetEntity<TData>(long id, TData def = null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Get<TData>(DataKeyBuilder.DataKey<TData>(id)) ?? def ?? new TData();
        }

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">数据键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public TData GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return Get<TData>(key) ?? def ?? new TData();
        }

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">键的组合</param>
        /// <returns></returns>
        public void SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            Set(DataKeyBuilder.DataKey(data), data);
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            if (keyFunc == null)
                keyFunc = DataKeyBuilder.DataKey;
            var key = keyFunc(new TData());
            Client.Set(key, DateTime.Now);
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RemoveCache<TData>(long id)
        {
            if (id > 0)
                Client.Del(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        /// 原始对象,在不够用时扩展
        /// </summary>
        public T Original<T>() where T : class
        {
            return Client as T;
        }

        void IRedis.ChangeDb(int db)
        {
            _client = CreateClient(db);
        }

        bool IRedis.SetNx(string key, byte[] value)
        {
            return Client.SetNx(key, value);
        }

        long IRedis.Incr(string key)
        {
            return Client.Incr(key);
        }

        long IRedis.Decr(string key)
        {
            return Client.Decr(key);
        }

        #endregion

    }
}