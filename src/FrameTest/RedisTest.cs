using Agebull.Common.Ioc;
using Agebull.EntityModel.Redis;

namespace FrameTest
{
    class RedisTest
    {
        static void TestFind()
        {
            //IocHelper.AddScoped<IRedis, StackExchangeRedis>();

            //删除Redis缓存,让前台自动更新
            using (var proxy = new RedisProxy(0))
            {
                for (int idx = 0; idx < 100; idx++)
                {
                    proxy.Set($"a:idx", "test");
                }
                proxy.Redis.FindAndRemoveKey($"a:*");
            }
        }
    }
}
