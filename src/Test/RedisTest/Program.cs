using Agebull.Common.DataModel.Redis;
using System;

namespace RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var proxy = new RedisProxy())
            {
                proxy.SetNx("test:key1");
                proxy.Incr("test:key2");
                proxy.Decr("test:key3");
            }
            Console.ReadKey();
        }
    }
}
