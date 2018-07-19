// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     为业务处理上下文对象的范围
    /// </summary>
    public class BusinessContextScope : IDisposable
    {
        #region 数据对象

        /// <summary>
        ///     上一个对象
        /// </summary>
        private readonly IBusinessContext _preContext;

        /// <summary>
        ///     范围内对象
        /// </summary>
        private readonly IBusinessContext _nowContext;

        /// <summary>
        ///     构造
        /// </summary>
        public static BusinessContextScope CreateScope()
        {
            return new BusinessContextScope();
        }

        /// <summary>
        ///     构造
        /// </summary>
        public static BusinessContextScope CreateScope(BusinessContext context)
        {
            return new BusinessContextScope(context);
        }
        /// <summary>
        ///     构造
        /// </summary>
        private BusinessContextScope(BusinessContext context)
        {
            _preContext = BusinessContext.GetCurrentContext();
            if (context == _preContext)
                _preContext = null;
            BusinessContext.Current = _nowContext = context;
        }

        /// <summary>
        ///     构造
        /// </summary>
        private BusinessContextScope()
        {
            _preContext = BusinessContext.GetCurrentContext();
            BusinessContext.Current = _nowContext = BusinessContext.CreateContext();
        }

        #endregion

        #region 析构

        /// <summary>
        ///     析构
        /// </summary>
        ~BusinessContextScope()
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
            if (_isDisposed)
            {
                return;
            }
            GC.ReRegisterForFinalize(this);
            ((IDisposable)_nowContext).Dispose();
            _isDisposed = true;
            BusinessContext.Current = _preContext;
        }

        #endregion
    }
}