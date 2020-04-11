using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using System;


namespace FrameTest
{
    public class LogRecorderTest
    {
        public static void TestChange()
        {
            LogRecorder.SystemLog($"Monitor:{LogRecorder.LogMonitor}");
            ConsoleKeyInfo key;
            do
            {
                using (IocScope.CreateScope(DateTime.Now.ToString()))
                {
                    LogRecorder.SystemLog("System:{0}", RandomCode.Generate(6));
                    using (MonitorScope.CreateScope("test"))
                    {
                        LogRecorder.Trace("Trace:{0}", RandomCode.Generate(6));
                        LogRecorder.Debug("Debug:{0}", RandomCode.Generate(6));
                        LogRecorder.Warning("Warning:{0}", RandomCode.Generate(6));
                    }
                }
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
