// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;

#endregion

namespace Agebull.Common.Base
{
    /// <summary>
    ///   范围对象的基类
    /// </summary>
    public abstract class ScopeBase : IDisposable
    {
        /// <summary>
        ///   记录失败的调用堆栈
        /// </summary>
        protected void RecordFailedStack()
        {
            //LogRecorder.RecordStackTrace(string.Format("在范围对象{0}的析构时,确认范围对象结论为失败,调用堆栈如下:", this.GetType()));
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected abstract void OnDispose();

        /// <summary>
        /// 防止多次析构
        /// </summary>
        private bool _isDisposed;
        /// <summary>
        ///   析构
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            OnDispose();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
