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
        private static readonly AsyncLocal<MonitorItem> _monitorItemLocal = new AsyncLocal<MonitorItem>();

        /// <summary>
        /// 当前范围数据
        /// </summary>
        internal static MonitorItem MonitorItem => _monitorItemLocal.Value ?? (_monitorItemLocal.Value = new MonitorItem());

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
            if (!LogMonitor)
                return;
            var item = MonitorItem;
            if (!item.InMonitor)
                return;
            item.EndStepMonitor();
        }

        /// <summary>
        /// 显示监视跟踪
        /// </summary>
        public static bool MonitorTrace(string message)
        {
            if (!LogMonitor)
                return false;
            var item = MonitorItem;
            if (!item.InMonitor)
                return false;
            item.Write(message, MonitorItem.ItemType.Item, false);
            return true;
        }

        /// <summary>
        /// 显示监视跟踪
        /// </summary>
        public static bool MonitorTrace(string message,params object[] args)
        {
            if (!LogMonitor)
                return false;
            var item = MonitorItem;
            if (!item.InMonitor)
                return false;
            item.Write(string.Format(message,args), MonitorItem.ItemType.Item, false);
            return true;
        }

        /// <summary>
        /// 显示监视跟踪
        /// </summary>
        public static bool MonitorTrace(Func<string> message)
        {
            if (!LogMonitor)
                return false;
            var item = MonitorItem;
            if (!item.InMonitor)
                return false;
            item.Write(message(), MonitorItem.ItemType.Item, false);
            return true;
        }

        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string title, bool number = false)
        {
            if (!LogMonitor)
                return;
            var item = MonitorItem;
            if (!item.InMonitor)
                return;
            item.Flush(title, number);
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void FlushMonitor(string fmt, params object[] args)
        {
            if (!LogMonitor)
                return;
            var item = MonitorItem;
            if (!item.InMonitor)
                return;
            item.Flush(string.Format(fmt, args));
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public static void EndMonitor()
        {
            if (!LogMonitor)
                return;
            var item = MonitorItem;
            if (!item.InMonitor)
                return;
            var log = item.End();
            if (log != null)
                Record(LogType.Monitor, "Monitor", log);
            _monitorItemLocal.Value = null;
        }
    }
}