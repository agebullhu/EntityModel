using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Redis;
using System;

namespace FrameTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LogRecorderX.Initialize();

            LogRecorderX.Debug("test");

            Console.ReadKey();
            LogRecorderX.Shutdown();

            Console.ReadKey();
        }
    }
}
