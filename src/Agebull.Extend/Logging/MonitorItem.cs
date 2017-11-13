using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Agebull.Common.Logging
{
    /// <summary>
    /// 侦测信息
    /// </summary>
    [Serializable]
    class MonitorData
    {
        /// <summary>
        /// 
        /// </summary>
        public FixStack<MonitorItem> Stack;

        /// <summary>
        /// 
        /// </summary>
        public StringBuilder Texter;

        /// <summary>
        /// 侦测吗
        /// </summary>
        public bool InMonitor;
    }

    /// <summary>
    /// 侦测信息
    /// </summary>
    [Serializable]
    internal class MonitorItem
    {
        private static MonitorData Data
        {
            get
            {
                var data = ContextHelper.LogicalGetData<MonitorData>("MonitorData");
                if (data == null)
                    ContextHelper.LogicalSetData("MonitorData", data = new MonitorData());
                return data;
            }
        }

        /// <summary>
        /// 侦测吗
        /// </summary>
        internal static bool InMonitor
        {
            get { return Data.InMonitor; }
            set
            {
                if (value)
                {
                    if (Data.InMonitor)
                        return;
                    Data.InMonitor = true;
                }
                else
                {
                    ContextHelper.Remove("MonitorData");
                }
            }
        }


        private static FixStack<MonitorItem> localMonitorStack
        {
            get { return Data.Stack; }
            set { Data.Stack = value; }
        }

        private static StringBuilder localMonitorTexter
        {
            get { return Data.Texter; }
            set { Data.Texter = value; }
        }
        

        internal static FixStack<MonitorItem> MonitorStack
        {
            get
            {
                return localMonitorStack;
            }
            set
            {
                localMonitorStack = value;
            }
        }
        /// <summary>
        /// 监视的文本写入器
        /// </summary>
        internal static StringBuilder MonitorTexter
        {
            get
            {
                return localMonitorTexter;
            }
            set
            {
                localMonitorTexter = value;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Title, Space, message;
        /// <summary>
        /// 测试计数
        /// </summary>
        public int NumberA, NumberB, NumberC;
        /// <summary>
        /// 内存分配
        /// </summary>
        public long TotalAllocatedMemorySize, MonitoringTotalAllocatedMemorySize;
        /// <summary>
        /// 内存占用
        /// </summary>
        public long TotalSurvivedMemorySize, MonitoringSurvivedMemorySize;
        /// <summary>
        /// 处理器时间
        /// </summary>
        public TimeSpan MonitoringTotalProcessorTime;

        /// <summary>
        /// 总处理器时间
        /// </summary>
        public double TotalProcessorTime, TotalTime;
        /// <summary>
        /// 起止时间
        /// </summary>
        public DateTime startTime, preTime;

        /// <summary>
        /// 构造
        /// </summary>
        public MonitorItem(string title, string space = "")
        {
            Space = space;
            Title = title;

            NumberA = 0;
            NumberB = 0;
            NumberC = 0;
            TotalTime = 0F;
            TotalProcessorTime = 0F;
            TotalSurvivedMemorySize = 0;
            TotalAllocatedMemorySize = 0;


            message = string.Format("|开始| {0:HH:mm:ss} |       -       |     -    |     -    |{1}|    -     |{2}|"//    -     |     -    |     -    |
                , DateTime.Now, (AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1048576F).ToFixLenString(10, 3)
                , (AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1048576F).ToFixLenString(10, 3));

            startTime = DateTime.Now;
            Flush();
        }
        /// <summary>
        /// 刷新消息
        /// </summary>
        /// <returns></returns>
        public void FlushMessage()
        {
            var a = DateTime.Now - preTime;
            var b = AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime;
            var c = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var e = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;

            message = string.Format("| ☆ |    -     |{0}|{1}|{2}|{3}|{4}|{5}|"//{6}|{7}|{8}|
                , a.TotalMilliseconds.ToFixLenString(15, 2)
                , b.TotalMilliseconds.ToFixLenString(10, 2)
                , (c / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (e / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3)
                , NumberA.ToFixLenString(10)
                , NumberB.ToFixLenString(10)
                , NumberC.ToFixLenString(10));

        }
        /// <summary>
        /// 收集信息
        /// </summary>
        /// <returns></returns>
        public void Coll()
        {
            TotalTime += (DateTime.Now - preTime).TotalMilliseconds;
            TotalProcessorTime += (AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime).TotalMilliseconds;
            TotalSurvivedMemorySize += AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            TotalAllocatedMemorySize += AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
        }
        /// <summary>
        /// 收集信息
        /// </summary>
        /// <returns></returns>
        public void Coll(MonitorItem item)
        {
            TotalTime += item.TotalTime;
            TotalProcessorTime += item.TotalProcessorTime;
            TotalSurvivedMemorySize += item.TotalSurvivedMemorySize;
            TotalAllocatedMemorySize += item.TotalAllocatedMemorySize;
        }
        /// <summary>
        /// 刷新消息
        /// </summary>
        /// <returns></returns>
        public void Flush()
        {
            MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;

            MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;

            MonitoringTotalProcessorTime = AppDomain.CurrentDomain.MonitoringTotalProcessorTime;

            preTime = DateTime.Now;
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public void EndMessage()
        {
            var a = DateTime.Now - startTime;
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            message = string.Format("|完成| {0:HH:mm:ss} |{1}/{10}|{2}|{3}|{4}|{5}|{6}|"//{7}|{8}|{9}|
                , DateTime.Now, TotalTime.ToFixLenString(7, 1)
                , TotalProcessorTime.ToFixLenString(10, 2)
                , (TotalAllocatedMemorySize / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (TotalSurvivedMemorySize / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3)
                , NumberA.ToFixLenString(10)
                , NumberB.ToFixLenString(10)
                , NumberC.ToFixLenString(10)
                , a.TotalMilliseconds.ToFixLenString(7, 1));
        }
    }
}