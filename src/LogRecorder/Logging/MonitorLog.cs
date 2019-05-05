using System;
using System.Threading;

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   文本记录器
    /// </summary>
    partial class LogRecorderX
    {
        /// <summary>
        /// 当前上下文数据
        /// </summary>
        private static readonly AsyncLocal<MonitorItem> MonitorItemLocal = new AsyncLocal<MonitorItem>();

        [ThreadStatic]
        private static MonitorItem _monitorItem;

        /// <summary>
        /// 当前范围数据
        /// </summary>
        internal static MonitorItem MonitorItem => _monitorItem ?? (_monitorItem = MonitorItemLocal.Value ?? (MonitorItemLocal.Value = new MonitorItem()));

        /// <summary>
        /// 开始检测资源
        /// </summary>
        public static void BeginMonitor(string title)
        {
            if (!LogMonitor)
                return;
            MonitorItem.BeginMonitor(title);
        }

        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void BeginStepMonitor(string title)
        {
            if (!LogMonitor)
                return;
            MonitorItem.BeginStep(title);
        }

        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void EndStepMonitor()
        {
            if (!LogMonitor || !MonitorItem.InMonitor)
                return;
            MonitorItem.EndStepMonitor();
        }

        /// <summary>
        /// 显示监视跟踪
        /// </summary>
        public static void MonitorTrace(string message)
        {
            if (!LogMonitor || !MonitorItem.InMonitor)
                return;
            MonitorItem.Write(message, MonitorItem.ItemType.Item, false);
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string title, bool number = false)
        {
            if (!LogMonitor || !MonitorItem.InMonitor)
                return;
            MonitorItem.Flush(title, number);
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string fmt, params object[] args)
        {
            if (!LogMonitor || !MonitorItem.InMonitor)
                return;
            MonitorItem.Flush(string.Format(fmt, args));
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void EndMonitor()
        {
            if (!LogMonitor || !MonitorItem.InMonitor)
                return;
            var log = MonitorItem.End();
            if (log != null)
                RecordInner(LogLevel.Trace, "Monitor", log, LogType.Monitor);
            _monitorItem = null;
            MonitorItemLocal.Value = null;
        }
    }
}