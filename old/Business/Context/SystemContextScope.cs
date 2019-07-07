using System;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ����Ϊϵͳ������ģʽ
    /// </summary>
    public class SystemContextScope : IDisposable
    {
        #region ���ݶ���

        /// <summary>
        ///     ��һ������
        /// </summary>
        private readonly bool _pre;

        /// <summary>
        ///     ����
        /// </summary>
        public static SystemContextScope CreateScope()
        {
            return new SystemContextScope();
        }
        /// <summary>
        ///     ����
        /// </summary>
        private SystemContextScope()
        {
            _isDisposed = false;
            _pre = BusinessContext.Current.IsSystemMode;
            BusinessContext.Current.IsSystemMode = true;
        }


        #endregion

        #region ����

        /// <summary>
        ///     ����
        /// </summary>
        ~SystemContextScope()
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