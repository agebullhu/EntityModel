using System;
using System.Collections.Generic;
using Agebull.Common.Configuration;
using System.Diagnostics;
using System.Threading;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class StackExchangeRedis : IDisposable, IRedis
    {
        #region 配置

        /// <summary>
        /// 不关闭
        /// </summary>
        bool IRedis.NoClose => true;

        /// <summary>
        /// 静态构造
        /// </summary>
        static StackExchangeRedis()
        {
            ConnectString = ConfigurationManager.GetConnectionString("Redis", "127.0.0.1:6379");
        }

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
        private int _db;

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb => _db;

        /// <summary>
        /// 构造
        /// </summary>
        public StackExchangeRedis()
        {
            if (connect == null)
                connect = ConnectionMultiplexer.Connect(ConnectString);
            else
            {
                try
                {
                    connect.GetDatabase().Ping();
                }
                catch (Exception e1)
                {
                    LogRecorder.Exception(e1, "StackExchangeRedis.ctor");
                    try
                    {
                        connect.Close();
                        connect.Dispose();
                    }
                    catch (Exception e)
                    {
                        LogRecorder.Exception(e, "StackExchangeRedis.ctor");
                    }
                    connect = ConnectionMultiplexer.Connect(ConnectString);
                }
            }
        }

        /// <summary>
        /// 原始对象,在不够用时扩展
        /// </summary>
        T IRedis.Original<T>()
        {
            return Client as T;
        }

        /*// <summary>
        /// 使用中的
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Used = new Dictionary<int, List<RedisClient>>();
        /// <summary>
        /// 空闲的
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Idle = new Dictionary<int, List<RedisClient>>();*/
        private static ConnectionMultiplexer connect;

        /// <summary>
        /// 客户端类
        /// </summary>
        internal IDatabase _client;
        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public IDatabase Client
        {
            get
            {
                if (_client != null)
                    return _client;
                return _client = CreateClient(_db);
            }
        }
        /// <summary>
        /// 连接字符串,默认为ConnectStrings节的Redis
        /// </summary>
        public static string ConnectString { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// 更改Db
        /// </summary>
        public void ChangeDb(int db)
        {
            CreateClient(db);
        }

        private IDatabase CreateClient(int db)
        {
            Monitor.Enter(LockObj);
            try
            {
                if (_client == null)
                    return _client = connect.GetDatabase(db);

                if (_db == db)
                    return _client;

                _client = connect.GetDatabase(db);
                _db = db;
                return _client;
            }
            catch (Exception ex)
            {
                LogRecorder.Exception(ex, ConnectString);
                throw;
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            //if (connect == null)
            //    return;
            //Monitor.Enter(LockObj);
            //try
            //{
            //    connect.Close();
            //    connect.Dispose();
            //}
            //finally
            //{
            //    Monitor.Exit(LockObj);
            //}
        }


        #endregion

        #region 文本读写

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Client.KeyDelete(key);
        }

        /// <summary>
        /// 删除KEY,未实现
        /// </summary>
        /// <param name="pattern"></param>
        public void FindAndRemoveKey(string pattern)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Client.StringGet(key);
        }


        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Client.StringSet(key, value);
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
            Client.StringSet(key, value, last - DateTime.Now);
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
            Client.StringSet(key, value, span);
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
            string vl = Client.StringGet(key);
            return string.IsNullOrWhiteSpace(vl) ? default(T) : JsonConvert.DeserializeObject<T>(vl);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json, last - DateTime.Now);
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
            string json = JsonConvert.SerializeObject(value);
            Client.StringSet(key, json, span);
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
            var bytes = Client.StringGet(key);
            if (bytes.IsNullOrEmpty)
                return default(T);
            return JsonConvert.DeserializeObject<T>(bytes);
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
            var bytes = Client.StringGet(key);
            if (bytes.IsNullOrEmpty)
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(bytes);
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
            Client.StringSet(key, JsonConvert.SerializeObject(value));
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
            Client.StringSet(key, JsonConvert.SerializeObject(value), last - DateTime.Now);
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
            Client.StringSet(key, JsonConvert.SerializeObject(value), span);
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
            SetValue(key, DateTime.Now);
            foreach (var data in datas)
            {
                Set(keyFunc(data), data);
            }
        }
        /*
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
                Client.KeyDelete(DataKey<TData>(id));
        }
        
             */


        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RemoveCache<TData>(long id)
        {
            if (id > 0)
                Client.KeyDelete(DataKeyBuilder.DataKey<TData>(id));
        }

        #endregion


        #region 其它扩展
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public void SetTime(string key, DateTime last)
        {
            Client.KeyExpire(key, last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public void SetTime(string key, TimeSpan span)
        {
            Client.KeyExpire(key, span);
        }
        /// <summary>
        /// 如果值不存在，则写入，否则失败
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetNx(string key, byte[] value = null)
        {
            var re = Client.Execute("SetNx", key, value ?? new byte[0]);
            if (re.IsNull || (int)re != 1)
                return false;
            Client.KeyExpire(key, TimeSpan.FromMinutes(5));
            return true;
        }

        /// <summary>
        /// 取得一个自增值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0表示失败，其它数字成功</returns>
        public long Incr(string key)
        {
            var re = Client.Execute("Incr", key);
            return re.IsNull ? 0 : (long)re;
        }

        /// <summary>
        /// 取得一个自减值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>0表示失败，其它数字成功</returns>
        public long Decr(string key)
        {
            var re = Client.Execute("Decr", key);
            return re.IsNull ? 0 : (long)re;
        }

        #endregion
    }
}