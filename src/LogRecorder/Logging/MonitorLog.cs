using System;
using System.Threading;

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   文本记录器
    /// </summary>
    partial class LogRecorder
    {
        /// <summary>
        /// 当前上下文数据
        /// </summary>
        private static readonly AsyncLocal<MonitorItem> DataLocal = new AsyncLocal<MonitorItem>();

        [ThreadStatic]
        private static MonitorItem _data;
        /// <summary>
        /// 当前范围数据
        /// </summary>
        internal static MonitorItem Data => _data ?? (_data = DataLocal.Value ?? (DataLocal.Value = new MonitorItem()));

        /// <summary>
        /// 开始检测资源
        /// </summary>
        public static void BeginMonitor(string title)
        {
            if (!LogMonitor)
                return;
            Data.BeginMonitor(title);
        }

        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void BeginStepMonitor(string title)
        {
            if (!LogMonitor)
                return;
            Data.BeginStep(title);
        }

        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void EndStepMonitor()
        {
            if (!LogMonitor)
                return;
            Data.EndStepMonitor();
        }

        /// <summary>
        /// 显示监视跟踪
        /// </summary>
        public static void MonitorTrace(string message)
        {
            if (!LogMonitor)
                return;
            Data.Write(message, 4, false);
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string title, bool number = false)
        {
            if (!LogMonitor)
                return;
            Data.Flush(title, number);
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string fmt, params object[] args)
        {
            if (!LogMonitor)
                return;
            Data.Flush(string.Format(fmt, args));
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void EndMonitor()
        {
            if (!LogMonitor)
                return;
            var log = Data.End();
            if (log != null)
                RecordInner(LogLevel.Trace, "Monitor", log, LogType.Monitor);
            _data = null;
            DataLocal.Value = null;

        }
    }
}