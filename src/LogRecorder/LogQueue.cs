using System;
using System.Collections.Generic;
using System.Threading;
using Agebull.Common.Logging;

namespace Agebull.Common
{
    /// <summary>
    /// 多生产者单消费者的同步列表（线程安全）
    /// </summary>
    /// <remarks>
    /// 1 使用双队列，便于快速切换
    /// </remarks>
    internal class LogQueue
    {
        /// <summary>
        /// 内部队列
        /// </summary>
        public List<RecordInfo> Line1 = new List<RecordInfo>();

        /// <summary>
        /// 内部队列
        /// </summary>
        public List<RecordInfo> Line2 = new List<RecordInfo>();

        private bool UseOnce = true;

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="t"></param>
        public void Push(RecordInfo t)
        {
            if (UseOnce)
                Line1.Add(t);
            else
                Line2.Add(t);
        }

        /// <summary>
        /// 开始处理队列内容
        /// </summary>
        public List<RecordInfo> Switch()
        {
            UseOnce = !UseOnce;
            return UseOnce ? Line2 : Line1;
        }
    }
}