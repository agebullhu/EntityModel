using System;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     设置为系统上下文模式
    /// </summary>
    public class SystemContextScope : IDisposable
    {
        #region 数据对象

        /// <summary>
        ///     上一个对象
        /// </summary>
        private readonly bool _pre;

        /// <summary>
        ///     构造
        /// </summary>
        public static SystemContextScope CreateScope()
        {
            return new SystemContextScope();
        }
        /// <summary>
        ///     构造
        /// </summary>
        private SystemContextScope()
        {
            _isDisposed = false;
            _pre = BusinessContext.Current.IsSystemMode;
            BusinessContext.Current.IsSystemMode = true;
        }


        #endregion

        #region 析构

        /// <summary>
        ///     析构
        /// </summary>
        ~SystemContextScope()
        {
            DoDispose();
        }

        /// <summary>
        ///     是否正确析构的标记
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        void IDisposable.Dispose()
        {
            DoDispose();
        }

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        private void DoDispose()
        {
            if (_isDisposed || BusinessContext._current==null)
            {
                return;
            }
            _isDisposed = true;
            GC.ReRegisterForFinalize(this);
            BusinessContext.Current.IsSystemMode = _pre;
        }

        #endregion
    }
}