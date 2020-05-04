// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.Common.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ��ʾһ���Զ��洢���б����
    /// </summary>
    public class ObjectCollection<TEntity> : ObservableCollection<TEntity>
        where TEntity : NotificationObject, new()
    {

        /// <summary>
        ///     ���������޸��¼�
        /// </summary>
        /// <param name="propertyName">����</param>
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }


        /// <summary>
        ///     ���������޸��¼�
        /// </summary>
        /// <param name="propertyName">����</param>
        private void OnPropertyChanged(string propertyName)
        {
#if CLIENT
            this.InvokeInUiThread(this.OnPropertyChanged, new PropertyChangedEventArgs(propertyName));
#else
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
#endif
        }

        #region ״̬�޸��¼�

        /// <summary>
        ///     ����״̬�仯�¼�
        /// </summary>
        /// <param name="status">״̬</param>
        public void RaiseStatusChanged(NotificationStatusType status)
        {
#if CLIENT
            this.InvokeInUiThread(this.RaiseStatusChangedInner, new PropertyChangedEventArgs(status.ToString()));
#else
            RaiseStatusChangedInner(new PropertyChangedEventArgs(status.ToString()));
#endif
        }

        /// <summary>
        ///     ����״̬�仯�¼�
        /// </summary>
        /// <param name="statusName">״̬</param>
        public void RaiseStatusChanged(string statusName)
        {
#if CLIENT
            this.InvokeInUiThread(this.RaiseStatusChangedInner, new PropertyChangedEventArgs(statusName));
#else
            RaiseStatusChangedInner(new PropertyChangedEventArgs(statusName));
#endif
        }

        /// <summary>
        ///     ״̬�仯�¼�
        /// </summary>
        private event PropertyChangedEventHandler statusChanged;

        /// <summary>
        ///     ״̬�仯�¼�
        /// </summary>
        public event PropertyChangedEventHandler StatusChanged
        {
            add
            {
                statusChanged -= value;
                statusChanged += value;
            }
            remove => statusChanged -= value;
        }

        /// <summary>
        ///     ����״̬�仯�¼�
        /// </summary>
        /// <param name="args">����</param>
        private void RaiseStatusChangedInner(PropertyChangedEventArgs args)
        {
            if (statusChanged == null)
            {
                return;
            }
            try
            {
                statusChanged(this, args);
            }
            catch (Exception ex)
            {
                LogRecorder.Exception(ex, "ObjectCollection.RaiseStatusChangedInner");
                throw;
            }
        }

        #endregion

#if CLIENT
        #region UI�߳�ͬ��֧��

        /// <summary>
        ///     ��UI�߳���ִ��
        /// </summary>
        /// <param name="action"></param>
        public void InvokeInUiThread(Action action)
        {
            action();
        }

        /// <summary>
        ///     ��UI�߳���ִ��
        /// </summary>
        /// <param name="action"></param>
        public void BeginInvokeInUiThread(Action action)
        {
            action();
        }

        /// <summary>
        ///     ��UI�߳���ִ��
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public void BeginInvokeInUiThread<T>(Action<T> action, T args)
        {
            action(args);
        }

        /// <summary>
        ///     ��UI�߳���ִ��
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public void InvokeInUiThread<T>(Action<T> action, T args)
        {
            action(args);
        }

        #endregion

#endif
    }
}
/*

        /// <summary>
        ///     ����״̬�仯�¼�
        /// </summary>
        /// <param name="action">�����ֶ�</param>
        public void RaiseStatusChanged<T>(Expression<Func<T>> action)
        {
            RaiseStatusChanged(GetPropertyName(action));
        }

        /// <summary>
        ///     ���������޸��¼�
        /// </summary>
        /// <param name="action">�����ֶ�</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChanged(GetPropertyName(action));
        }

        /// <summary>
        ///     ȡ�����ֶ�
        /// </summary>
        /// <typeparam name="T">�ֶ�����</typeparam>
        /// <param name="action">�ֶη���</param>
        /// <returns>�ֶ�����</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            return expression.Member.Name;
        }

        /// <summary>
        ///     ���������޸��¼�
        /// </summary>
        /// <param name="action">�����ֶ�</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChanged(GetPropertyName(action));
        }
*/
