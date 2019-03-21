// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Ϊҵ���������Ķ���ķ�Χ
    /// </summary>
    public class BusinessContextScope : IDisposable
    {
        #region ���ݶ���

        /// <summary>
        ///     ��һ������
        /// </summary>
        private readonly BusinessContext _preContext;

        /// <summary>
        ///     ��Χ�ڶ���
        /// </summary>
        private readonly BusinessContext _nowContext;

        /// <summary>
        ///     ����
        /// </summary>
        public static BusinessContextScope CreateScope()
        {
            return new BusinessContextScope();
        }

        /// <summary>
        ///     ����
        /// </summary>
        public static BusinessContextScope CreateScope(BusinessContext context)
        {
            return new BusinessContextScope(context);
        }
        /// <summary>
        ///     ����
        /// </summary>
        private BusinessContextScope(BusinessContext context)
        {
            _preContext = BusinessContext.GetCurrentContext();
            if (context == _preContext)
                _preContext = null;
            BusinessContext.Current = _nowContext = context;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private BusinessContextScope()
        {
            _preContext = BusinessContext.GetCurrentContext();
            BusinessContext.Current = _nowContext = BusinessContext.CreateContext();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ����
        /// </summary>
        ~BusinessContextScope()
        {
            DoDispose();
        }

        /// <summary>
        ///     �Ƿ���ȷ�����ı��
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        ///     ִ�����ͷŻ����÷��й���Դ��ص�Ӧ�ó����������
        /// </summary>
        /// <filterpriority>2</filterpriority>
        void IDisposable.Dispose()
        {
            DoDispose();
        }

        /// <summary>
        ///     ִ�����ͷŻ����÷��й���Դ��ص�Ӧ�ó����������
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