using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.Common.Ioc;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.MySql;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class RedisProxy : IDisposable
    {
        #region 配置

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
        /// 使用哪个数据库
        /// </summary>
        private int _db;

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb => Redis.CurrentDb;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0)
        {
            _db = db;
            ChangeDb();
        }

        /*// <summary>
        /// 使用中的
        /// </summary>
        private static readonly Dictionary<int, List<IRedis>> Used = new Dictionary<int, List<IRedis>>();
        /// <summary>
        /// 空闲的
        /// </summary>
        private static readonly Dictionary<int, List<IRedis>> Idle = new Dictionary<int, List<IRedis>>();*/
        /// <summary>
        /// 客户端类
        /// </summary>
        internal IRedis _redis;
        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public IRedis Redis
        {
            get
            {
                if (_redis != null)
                    return _redis;
                return _redis = CreateClient();
            }
        }
        
        /// <summary>
        /// 更改
        /// </summary>
        internal IRedis ChangeDb(int db)
        {
            _db = db;
            return ChangeDb();
        }

        /// <summary>
        /// 更改
        /// </summary>
        private IRedis ChangeDb()
        {
            if (_redis == null)
            {
                var old = _redis;
                _redis = CreateClient();
                return old;
            }
            if (_db == _redis.CurrentDb)
            {
                return _redis;
            }
            _redis.ChangeDb(_db);
            return _redis;
        }

        /// <summary>
        /// 更改
        /// </summary>
        internal void ResetClient(IRedis client)
        {
            _redis = client;
        }

        private IRedis CreateClient()
        {
            _redis= IocHelper.Create<IRedis>();
            if (_redis == null)
                throw new Exception("未正确注册Redis对象");
            ChangeDb();
            return _redis;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_redis == null || _redis.NoClose )
                return;
            _redis.Dispose();
            _redis = null;
        }


        #endregion

        #region 文本读写

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Redis.RemoveKey(key);
        }

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public void FindAndRemoveKey(string pattern)
        {
            Redis.FindAndRemoveKey(pattern);
        }

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Redis.Get<string>(key);
        }


        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void Set(string key, string value)
        {
            Redis.Set(key, value);
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
            Redis.Set(key, value, last);
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
            Redis.Set(key, value, span);
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
            return Redis.GetValue<T>(key);
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
            Redis.SetValue(key, value);
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
            Redis.SetValue(key, value, last);
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
            Redis.SetValue(key, value, span);
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
            return Redis.GetAll<T>(pattern);
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
            return Redis.Get<T>(key);
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
            return Redis.TryGet<T>(key);
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
            Redis.Set(key, value);
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
            Redis.Set(key, value, last);
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
            Redis.Set(key, value, span);
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
           return Redis.GetEntity(id, def);
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
            Redis.SetEntity(data);
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
            Redis.CacheData(datas, keyFunc);
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TDatabase"></typeparam>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess, TDatabase>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
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
        /// <typeparam name="TDatabase"></typeparam>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public void TryCacheData<TData, TDataAccess, TDatabase>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess, TDatabase>(keyFunc);
        }

        /// <summary>
        ///     如果没有缓存过这些对象,就缓存它
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns>数据</returns>
        public void TryCacheData<TData, TDataAccess, TDatabase>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
        {
            var key = DataKeyBuilder.DataKey<TData>(0);
            var date = GetValue<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess, TDatabase>(lambda, keyFunc);
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns>数据</returns>
        public void CacheData<TData, TDataAccess, TDatabase>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData, TDatabase>, new()
            where TData : EditDataObject, IIdentityData, new()
            where TDatabase : MySqlDataBase
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
            FindAndRemoveKey(DataKeyBuilder.DataKey<TData>("*"));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RefreshCache<TData, TDataAccess>(long id, Func<TData, bool> lambda = null)
            where TDataAccess : MySqlTable<TData, MySqlDataBase>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = access.LoadByPrimaryKey(id);
            if (data != null && (lambda == null || lambda(data)))
                Set(DataKeyBuilder.DataKey(data), data);
            else
                Redis.RemoveKey(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public void RemoveCache<TData>(long id)
        {
            Redis.RemoveCache<TData>(id);
        }

        #endregion

    }
}