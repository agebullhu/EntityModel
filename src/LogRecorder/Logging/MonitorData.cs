using System;

namespace Agebull.Common.Logging
{
    /// <summary>
    ///     跟踪信息
    /// </summary>
    internal class MonitorData
    {
        /// <summary>
        ///     文本
        /// </summary>
        internal string Space;

        /// <summary>
        ///     文本
        /// </summary>
        internal string Message;

        /// <summary>
        ///     起止时间
        /// </summary>
        internal DateTime StartTime;

        /// <summary>
        ///     起止时间
        /// </summary>
        internal DateTime PreTime;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        ///     总处理器时间
        /// </summary>
        internal double TotalTime;


#if !NETSTANDARD
        /// <summary>
        /// 内存分配
        /// </summary>
        internal long TotalAllocatedMemorySize, MonitoringTotalAllocatedMemorySize;
        /// <summary>
        /// 内存占用
        /// </summary>
        internal long TotalSurvivedMemorySize, MonitoringSurvivedMemorySize;
        /// <summary>
        /// 处理器时间
        /// </summary>
        internal TimeSpan MonitoringTotalProcessorTime;
        /// <summary>
        /// 总处理器时间
        /// </summary>
        internal double TotalProcessorTime;

#endif
        /// <summary>
        ///     构造
        /// </summary>
        internal MonitorData()
        {
            Space = "";

            TotalTime = 0F;
#if !NETSTANDARD
            TotalProcessorTime = 0F;
            TotalSurvivedMemorySize = 0;
            TotalAllocatedMemorySize = 0;


            Message =
                $"|开始| {DateTime.Now:HH:mm:ss} |       -       |     -    |     -    |{(AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1048576F).ToFixLenString(10, 3)}|    -     |{(AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1048576F).ToFixLenString(10, 3)}|";
#else
            Message = $"|开始| {DateTime.Now:HH:mm:ss} |";
#endif

            PreTime = StartTime = DateTime.Now;

            Flush();
        }

        /// <summary>
        ///     刷新消息
        /// </summary>
        /// <returns></returns>
        internal void FlushMessage()
        {
            var a = DateTime.Now - PreTime;
#if !NETSTANDARD
            var b = AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime;
            var c = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var e = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            Message = string.Format("| ☆ |    -     |{0}|{1}|{2}|{3}|{4}|{5}|"
                , a.TotalMilliseconds.ToFixLenString(15, 2)
                , b.TotalMilliseconds.ToFixLenString(10, 2)
                , (c / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (e / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3));
#else
            Message = $"| ☆ |    -     |{a.TotalMilliseconds.ToFixLenString(15, 2)}|";
#endif
        }

        /// <summary>
        ///     收集信息
        /// </summary>
        /// <returns></returns>
        internal void Coll()
        {
            TotalTime += (DateTime.Now - PreTime).TotalMilliseconds;
#if !NETSTANDARD
            TotalProcessorTime +=
 (AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime).TotalMilliseconds;
            TotalSurvivedMemorySize +=
 AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            TotalAllocatedMemorySize +=
 AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
#endif
        }

        /// <summary>
        ///     收集信息
        /// </summary>
        /// <returns></returns>
        internal void Coll(MonitorData item)
        {
            if (item == null)
            {
                Coll();
                return;
            }
            TotalTime += item.TotalTime;
#if !NETSTANDARD
            TotalProcessorTime += item.TotalProcessorTime;
            TotalSurvivedMemorySize += item.TotalSurvivedMemorySize;
            TotalAllocatedMemorySize += item.TotalAllocatedMemorySize;
#endif
        }

        /// <summary>
        ///     刷新消息
        /// </summary>
        /// <returns></returns>
        internal void Flush()
        {
#if !NETSTANDARD
            MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            MonitoringTotalProcessorTime = AppDomain.CurrentDomain.MonitoringTotalProcessorTime;
#endif
            PreTime = DateTime.Now;
        }

        /// <summary>
        ///     刷新资源检测
        /// </summary>
        internal void EndMessage()
        {
            var a = DateTime.Now - StartTime;
#if !NETSTANDARD
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            Message = string.Format("|完成| {0:HH:mm:ss} |{1}/{7}|{2}|{3}|{4}|{5}|{6}|"
                , DateTime.Now
                , TotalTime.ToFixLenString(7, 1)
                , TotalProcessorTime.ToFixLenString(10, 2)
                , (TotalAllocatedMemorySize / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (TotalSurvivedMemorySize / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3)
                , a.TotalMilliseconds.ToFixLenString(7, 1));
#else
            Message =
                $"|完成| {DateTime.Now:HH:mm:ss} |{TotalTime.ToFixLenString(7, 1)}/{a.TotalMilliseconds.ToFixLenString(7, 1)}|";
#endif
        }
    }
}