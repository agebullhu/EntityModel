using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    [Obsolete]
    public class RedisProxy<TRedis> : IDisposable
        where TRedis : IRedisBase
    {
        #region 测试支持
        /// <summary>
        /// 测试支持
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion

        #region 配置

        /// <summary>
        /// 所有实例
        /// </summary>
        private readonly static List<TRedis> Instances = new List<TRedis>();

        /// <summary>
        /// 配置
        /// </summary>
        public static RedisOption Option { get; set; }

        /// <summary>
        ///     初始化
        /// </summary>
        static RedisProxy()
        {
            ConfigurationManager.RegistOnChange("Redis", OnOptionChange, true);
        }

        /// <summary>
        /// 配置变更
        /// </summary>
        private static void OnOptionChange()
        {
            Option = ConfigurationManager.Get<RedisOption>("Redis") ?? new RedisOption();
            foreach (var client in Instances)
                client.OnOptionChange(Option);
        }

        /// <summary>
        /// 不关闭
        /// </summary>
        public bool NoClose => true;

        /*// <summary>
        /// 空闲连接数
        /// </summary>
        private static readonly int PoolSize;*/

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
        protected RedisProxy(int db)
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
        internal TRedis _redis;
        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public TRedis Redis
        {
            get
            {
                if (_redis != null)
                    return _redis;
                _redis = CreateClient();
                ChangeDb();
                return _redis;
            }
        }

        /// <summary>
        /// 更改
        /// </summary>
        internal TRedis ChangeDb(int db)
        {
            _db = db;
            return ChangeDb();
        }

        /// <summary>
        /// 更改
        /// </summary>
        private TRedis ChangeDb()
        {
            if (_redis == null)
            {
                var old = _redis;
                _redis = CreateClient();
                _redis.ChangeDb(_db);
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
        internal void ResetClient(TRedis client)
        {
            _redis = client;
        }

        private TRedis CreateClient()
        {
            var redis = DependencyHelper.Create<TRedis>();
            redis.Option = Option;
            if (redis == null)
                throw new DependencyException(typeof(TRedis), "未正确注册Redis对象");
            Instances.Add(redis);
            return redis;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_redis == null || _redis.NoClose)
                return;
            Instances.Remove(_redis);
            _redis.Dispose();
            _redis = default;
        }


        #endregion

    }
}