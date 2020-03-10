using System;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Redis;
using CSRedis;

namespace FrameTest
{
    public class RedisTest
    {
        static RedisTest()
        {
            IocHelper.AddSingleton<IRedis, CSRedisEx>();
            IocHelper.AddSingleton<IRedisAsync, CSRedisAsync>();
        }

        public static void TestFind()
        {
            //IocHelper.AddScoped<IRedis, StackExchangeRedis>();

            //删除Redis缓存,让前台自动更新
            using var proxy = new RedisProxy(0);
            proxy.Set("a:idx", "test",DateTime.Now.AddMinutes(5));
            var csr = proxy.Redis.Original<CSRedisClient>();
            Console.WriteLine(proxy.Get("a:idx"));
            Console.ReadKey();
        }
    }
}
