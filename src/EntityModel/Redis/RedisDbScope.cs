using Agebull.Common.Base;
using NServiceKit.Redis;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// Redis��Db��Χ
    /// </summary>
    public class RedisDbScope : ScopeBase
    {
        private readonly RedisProxy _proxy;
        private readonly RedisClient _client;

        public static RedisDbScope CreateScope(RedisProxy proxy, long db)
        {
            return new RedisDbScope(proxy, db);
        }

        private RedisDbScope(RedisProxy proxy, long db)
        {
            _proxy = proxy;
            _client = proxy.ChangeDb(db);
        }

        /// <summary>������Դ</summary>
        protected override void OnDispose()
        {
            _proxy.ResetClient(_client);
        }
    }
}