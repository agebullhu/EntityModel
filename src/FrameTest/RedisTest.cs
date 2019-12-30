using System;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Redis;

namespace FrameTest
{
    public class RedisTest
    {
        public static void TestFind()
        {
            //IocHelper.AddScoped<IRedis, StackExchangeRedis>();

            //删除Redis缓存,让前台自动更新
            using var proxy = new RedisProxy(0);
            proxy.Set("a:idx", "test",DateTime.Now.AddMinutes(5));
        }
    }
}
