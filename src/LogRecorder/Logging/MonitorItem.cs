using System;
using System.Linq;
using System.Text;
using System.Threading;

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
        //[ThreadStatic] private static MonitorData data;
        private static AsyncLocal<MonitorData> dataLocal = new AsyncLocal<MonitorData>();
        private static MonitorData Data
        {
            get
            {
                //return dataLocal.Value;
                ////return data ?? (data = new MonitorData());
                //var sync = new AsyncLocal<MonitorData>();
                return dataLocal.Value ?? (dataLocal.Value = new MonitorData());
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
                    Data.InMonitor = false;
                }
            }
        }
        
        internal static FixStack<MonitorItem> MonitorStack
        {
            get
            {
                return Data.Stack;
            }
            set
            {
                Data.Stack = value;
            }
        }
        /// <summary>
        /// 监视的文本写入器
        /// </summary>
        internal static StringBuilder MonitorTexter
        {
            get { return Data.Texter; }
            set { Data.Texter = value; }
        }

#if !NETSTANDARD2_0
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
        public double TotalProcessorTime;

#endif
        /// <summary>
        /// 文本
        /// </summary>
        public string Title, Space, message;
        /// <summary>
        /// 测试计数
        /// </summary>
        public int NumberA, NumberB, NumberC;

        /// <summary>
        /// 总处理器时间
        /// </summary>
        public double TotalTime;

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
#if !NETSTANDARD2_0
            TotalProcessorTime = 0F;
            TotalSurvivedMemorySize = 0;
            TotalAllocatedMemorySize = 0;


            message = string.Format("|开始| {0:HH:mm:ss} |       -       |     -    |     -    |{1}|    -     |{2}|"//    -     |     -    |     -    |
                , DateTime.Now, (AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1048576F).ToFixLenString(10, 3)
                , (AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1048576F).ToFixLenString(10, 3));
#else
            message = $"|开始| {DateTime.Now:HH:mm:ss} |";
#endif

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
#if !NETSTANDARD2_0
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
#else
            message = $"| ☆ |    -     |{a.TotalMilliseconds.ToFixLenString(15, 2)}|";
#endif


        }
        /// <summary>
        /// 收集信息
        /// </summary>
        /// <returns></returns>
        public void Coll()
        {
            TotalTime += (DateTime.Now - preTime).TotalMilliseconds;
#if !NETSTANDARD2_0
            TotalProcessorTime += (AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime).TotalMilliseconds;
            TotalSurvivedMemorySize += AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            TotalAllocatedMemorySize += AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
#endif
        }
        /// <summary>
        /// 收集信息
        /// </summary>
        /// <returns></returns>
        public void Coll(MonitorItem item)
        {
            TotalTime += item.TotalTime;
#if !NETSTANDARD2_0
            TotalProcessorTime += item.TotalProcessorTime;
            TotalSurvivedMemorySize += item.TotalSurvivedMemorySize;
            TotalAllocatedMemorySize += item.TotalAllocatedMemorySize;
#endif
        }
        /// <summary>
        /// 刷新消息
        /// </summary>
        /// <returns></returns>
        public void Flush()
        {
#if !NETSTANDARD2_0
            MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            MonitoringTotalProcessorTime = AppDomain.CurrentDomain.MonitoringTotalProcessorTime;
#endif
            preTime = DateTime.Now;
        }
        /// <summary>
        /// 刷新资源检测
        /// </summary>
        public void EndMessage()
        {
            var a = DateTime.Now - startTime;
#if !NETSTANDARD2_0
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
#else
            message = $"|完成| {DateTime.Now:HH:mm:ss} |{TotalTime.ToFixLenString(7, 1)}/{a.TotalMilliseconds.ToFixLenString(7, 1)}|";
#endif
        }
    }
}