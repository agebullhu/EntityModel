using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace Agebull.Common
{
    /// <inheritdoc />
    /// <summary>
    /// 多生产者单消费者的同步列表（线程安全）
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    /// <remarks>
    /// 1 内部使用信号量
    /// 2 用于多生产者单消费者的场景
    /// 3 使用双队列，以防止错误时无法还原
    /// </remarks>
    public class MulitToOneQueue<T> : IDisposable
    {
        /// <summary>
        /// 内部队列
        /// </summary>
        [JsonProperty]
        public Queue<T> Queue { get; } = new Queue<T>();

        /// <summary>
        /// 正在处理
        /// </summary>
        [JsonProperty]
        public Queue<T> Doing { get; } = new Queue<T>();

        /// <summary>
        /// 用于同步的信号量
        /// </summary>
        private readonly Semaphore _semaphore = new Semaphore(0, Int32.MaxValue);

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            lock (this)
            {
                return Queue.Count == 0 && Doing.Count == 0;
            }
        }

        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="t"></param>
        public void Push(T t)
        {
            lock (this)
            {
                if (_disposedValue)
                {
                    return;
                }
                Queue.Enqueue(t);
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 开始处理队列内容
        /// </summary>
        /// <param name="t">返回内容（如果返回True)</param>
        /// <param name="waitMs">等待时长</param>
        public bool StartProcess(out T t, int waitMs)
        {
            lock (this)
            {
                if (_disposedValue)
                {
                    t = default(T);
                    return false;
                }
                if (Doing.Count > 0)//之前存在失败
                {
                    lock (Doing)
                        t = Doing.Peek();
                    return true;
                }
                if (!_semaphore.WaitOne(waitMs))
                {
                    t = default(T);
                    return false;
                }
                t = Queue.Dequeue();
                Doing.Enqueue(t);
            }
            return true;
        }

        /// <summary>
        /// 完成处理队列内容
        /// </summary>
        public void EndProcess()
        {
            lock (this)
            {
                if (_disposedValue)
                    return;
                Doing.Dequeue();
            }
        }

        #region IDisposable Support
        private bool _disposedValue; // 要检测冗余调用

        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_disposedValue) return;
                if (disposing)
                {
                    _semaphore.Dispose();
                }
                _disposedValue = true;
            }
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~MulitToOneQueue() => Dispose(false);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}