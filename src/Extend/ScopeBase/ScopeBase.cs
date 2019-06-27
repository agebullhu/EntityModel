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
        /// 析构
        /// </summary>
        ~ScopeBase()
        {
            DoDispose();
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected abstract void OnDispose();

        /// <summary>
        /// 防止多次析构
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///   析构
        /// </summary>
        public void Dispose()
        {
            DoDispose();
        }

        /// <summary>
        ///   析构
        /// </summary>
        public void DoDispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            OnDispose();
            GC.SuppressFinalize(this);
        }
    }
}
