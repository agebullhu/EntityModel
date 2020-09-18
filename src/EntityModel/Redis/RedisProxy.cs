using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;
using NServiceKit.Redis;
using NServiceKit.Text;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class RedisProxy : IDisposable
    {
        #region 配置

        /// <summary>
        /// 静态构造
        /// </summary>
        static RedisProxy()
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

        /// <summary>
        /// 系统数据
        /// </summary>
        public const int DbSystem = 15;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public const int DbWebCache = 14;

        /// <summary>
        /// 权限数据
        /// </summary>
        public const int DbAuthority = 13;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public const int DbComboCache = 12;


        #endregion

        #region 测试支持
        /// <summary>
        /// 测试支持
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion

        #region 构造


        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object LockObj = new object();
        /// <summary>
        /// 使用哪个数据库
        /// </summary>
        private readonly int _db;

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb => Client.Db;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0)
        {
            _db = db;
        }

        /*// <summary>
        /// 使用中的
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Used = new Dictionary<int, List<RedisClient>>();
        /// <summary>
        /// 空闲的
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Idle = new Dictionary<int, List<RedisClient>>();*/
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
        /// <summary>
        /// 更改
        /// </summary>
        internal RedisClient ChangeDb(long db)
        {
            if (db == Client.Db)
            {
                return _client;
            }
            var old = _client;
            _client = CreateClient(db);
            return old;
        }

        /// <summary>
        /// 更改
        /// </summary>
        internal void ResetClient(RedisClient client)
        {
            _client = client;
        }

        private RedisClient CreateClient(long db)
        {
            Monitor.Enter(LockObj);
            try
            {
                //var dbid = (this.db << 16);
                return new RedisClient(Address, Port, PassWord, db)
                {
                    RetryCount = 50,
                    RetryTimeout = 5000
                };
                /*List<RedisClient> used;
                    if (!Used.ContainsKey(dbid))
                    {
                        Used.Add(dbid, used = new List<RedisClient>());
                        Idle.Add(dbid, new List<RedisClient>());

                        _client = new RedisClient(Address, Port, null, db)
                        {
                            RetryCount = 50,
                            RetryTimeout = 5000
                        };
                        used.Add(_client);
                        return Client;
                    }
                    used = Used[dbid];
                    List<RedisClient> idle = Idle[dbid];
                    while (idle.Count > 0 && idle[0] == null)
                    {
                        idle.RemoveAt(0);
                    }
                    if (idle.Count == 0)
                    {
                        _client = new RedisClient(Address, Port, null, db)
                        {
                            RetryCount = 50,
                            RetryTimeout = 5000
                        };
                        used.Add(_client);
                        return _client;
                    }
                    _client = idle[0];
                    idle.RemoveAt(0);
#if DEBUG
                    _client.Ping();
#endif
                    used.Add(_client);
                    return _client;*/
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        public void Dispose()
        {
            if (_client == null)
                return;
            Monitor.Enter(LockObj);
            try
            {
                //int dbid = (_db << 16);
                //Used[dbid].Remove(_client);
                _client.ResetSendBuffer();
                //if (_client.HadExceptions || Idle[dbid].Count > PoolSize)
                {
                    _client.Quit();
                    _client.Dispose();
                }
                //else
                //{
                //    Idle[dbid].Add(_client);
                //}
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
            Client.Remove(key);
        }

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public void FindAndRemoveKey(string pattern)
        {
            var keys = Client.SearchKeys(pattern);
            Client.RemoveAll(keys);
        }

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.Get<string>(key);
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
            Client.Set(key, value, last);
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
            Client.Set(key, value, span);
        }

        #endregion

        #region 值读写

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
            where T : struct
        {
            return Client.Get<T>(key);
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
            Client.Set(key, value);
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
            Client.Set(key, value, last);
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
            Client.Set(key, value, span);
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
                var keys = Client.ScanKeys(ref cursor, pattern, 100);
                foreach (var key in keys)
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
            var bytes = Client.Get(key);
            if (bytes == null || bytes.Length == 0)
                return default(T);
            return JsonConvert.DeserializeObject<T>(bytes.FromUtf8Bytes());
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
            var bytes = Client.Get(key);
            if (bytes == null || bytes.Length == 0)
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(bytes.FromUtf8Bytes());
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.Set(key, JsonConvert.SerializeObject(value).ToUtf8Bytes());
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.Set(key, JsonConvert.SerializeObject(value).ToUtf8Bytes(), last);
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
            if (IsTest)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented), key);
            }
            Client.Set(key, JsonConvert.SerializeObject(value).ToUtf8Bytes(), span);
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
        public TData GetEntity<TData>(int id, TData def = null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Get<TData>(DataKey<TData>(id)) ?? def ?? new TData();
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
            Set(DataKey(data), data);
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
                keyFunc = DataKey;
            var key = keyFunc(new TData());
            Client.Set(key, DateTime.Now);
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All();
            CacheData(datas, keyFunc);
        }

        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <typeparam name="TData">数据键</typeparam>
        /// <param name="data">数据</param>
        /// <returns>数据键名</returns>
        public static string DataKey<TData>(TData data) where TData : class, IIdentityData
        {
            return $"data:{typeof(TData).Name.ToLower()}:{data.Id}";
        }

        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <typeparam name="TData">数据键</typeparam>
        /// <param name="id">数据键</param>
        /// <returns>数据键名</returns>
        public static string DataKey<TData>(object id)
        {
            return $"data:{typeof(TData).Name.ToLower()}:{id}";
        }
        /// <summary>
        /// 默认的数据键名称生成器
        /// </summary>
        /// <param name="type">数据</param>
        /// <param name="id">数据键</param>
        /// <returns>数据键名</returns>
        public static string DataKey(string type, int id)
        {
            return $"data:{type.ToLower()}:{id}";
        }
        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void TryCacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(keyFunc);
        }

        /// <summary>
        ///     如果没有缓存过这些对象,就缓存它
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns>数据</returns>
        public void TryCacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(lambda, keyFunc);
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns>数据</returns>
        public void CacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All(lambda);
            CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void ClearCache<TData>()
        {
            FindAndRemoveKey(DataKey<TData>("*"));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RefreshCache<TData, TDataAccess>(int id, Func<TData, bool> lambda = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = access.LoadByPrimaryKey(id);
            if (data != null && (lambda == null || lambda(data)))
                Set(DataKey(data), data);
            else
                Client.Remove(DataKey<TData>(id));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RemoveCache<TData>(int id)
        {
            if (id > 0)
                Client.Remove(DataKey<TData>(id));
        }

        #endregion

        #region 锁

        

        #endregion
    }
}