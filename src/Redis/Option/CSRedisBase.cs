using Agebull.Common.Configuration;
using Agebull.EntityModel.Common;
using CSRedis;
using System;
using System.Collections.Concurrent;

#pragma warning disable 693

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class CSRedisBase : IDisposable
    {
        #region 配置

        /// <summary>
        /// 配置
        /// </summary>
        public RedisOption Option { get; set; }

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb { get; internal set; }

        /// <summary>
        /// 配置变更
        /// </summary>
        /// <param name="option"></param>
        public void OnOptionChange(RedisOption option)
        {
            Option = option;
            _client = null;
            _clients.Clear();
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

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        protected string ConnectString(long db) => $"{Option.ConnectionString},defaultDatabase={db}";

        /// <summary>
        /// 客户端类
        /// </summary>
        protected CSRedisClient _client;

        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public CSRedisClient Client
        {
            get
            {
                
                if (_client != null)
                    return _client;
                return _client = CreateClient(CurrentDb);
            }
        }

        private static readonly ConcurrentDictionary<long, CSRedisClient> _clients = new ConcurrentDictionary<long, CSRedisClient>();

        /// <summary>
        /// 生成一个客户端
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        protected CSRedisClient CreateClient(long db)
        {
            if (_clients.TryGetValue(db, out var client))
                return client;

            client = new CSRedisClient(ConnectString(db));
            _clients.TryAdd(db, client);
            return client;
        }

        /// <summary>
        /// 原始对象,在不够用时扩展
        /// </summary>
        public T Original<T>() where T : class
        {
            return Client as T;
        }
        /// <summary>
        /// 修改Db
        /// </summary>
        /// <param name="db"></param>
        public void ChangeDb(int db)
        {
            _client = CreateClient(db);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //foreach (var client in _clients.Values)
            //{
            //    client.Dispose();
            //}
        }
    }
}