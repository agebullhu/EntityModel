using System.Collections.Generic;
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
        /// 是否不为空
        /// </summary>
        public bool IsEmpty => Line2.Count == 0 && Line1.Count == 0;
        
        /// <summary>
        /// 内部队列
        /// </summary>
        public List<RecordInfo> Line1 = new List<RecordInfo>();

        /// <summary>
        /// 内部队列
        /// </summary>
        public List<RecordInfo> Line2 = new List<RecordInfo>();

        private bool _useOnce = true;

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="t"></param>
        public void Push(RecordInfo t)
        {
            if (_useOnce)
                Line1.Add(t);
            else
                Line2.Add(t);
        }

        /// <summary>
        /// 等待数量
        /// </summary>
        public long WaitCount => _useOnce ? Line1.Count : Line2.Count;

        /// <summary>
        /// 开始处理队列内容
        /// </summary>
        public List<RecordInfo> Switch()
        {
            List<RecordInfo> result;
            lock (this)
            {
                _useOnce = !_useOnce;
                if (_useOnce)
                {
                    result = Line2;
                    Line2 = new List<RecordInfo>();
                }
                else
                {
                    result = Line1;
                    Line1 = new List<RecordInfo>();
                }
            }
            return result;
        }
    }
}