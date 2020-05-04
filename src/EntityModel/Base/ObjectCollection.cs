// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示一个自动存储的列表对象
    /// </summary>
    public class ObjectCollection<TEntity> : ObservableCollection<TEntity>
        where TEntity : NotificationObject, new()
    {

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyName">属性</param>
        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }


        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyName">属性</param>
        private void OnPropertyChanged(string propertyName)
        {
#if CLIENT
            this.InvokeInUiThread(this.OnPropertyChanged, new PropertyChangedEventArgs(propertyName));
#else
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
#endif
        }

        #region 状态修改事件

        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="status">状态</param>
        public void RaiseStatusChanged(NotificationStatusType status)
        {
#if CLIENT
            this.InvokeInUiThread(this.RaiseStatusChangedInner, new PropertyChangedEventArgs(status.ToString()));
#else
            RaiseStatusChangedInner(new PropertyChangedEventArgs(status.ToString()));
#endif
        }

        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="statusName">状态</param>
        public void RaiseStatusChanged(string statusName)
        {
#if CLIENT
            this.InvokeInUiThread(this.RaiseStatusChangedInner, new PropertyChangedEventArgs(statusName));
#else
            RaiseStatusChangedInner(new PropertyChangedEventArgs(statusName));
#endif
        }

        /// <summary>
        ///     状态变化事件
        /// </summary>
        private event PropertyChangedEventHandler statusChanged;

        /// <summary>
        ///     状态变化事件
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
        ///     发出状态变化事件
        /// </summary>
        /// <param name="args">属性</param>
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
        #region UI线程同步支持

        /// <summary>
        ///     在UI线程中执行
        /// </summary>
        /// <param name="action"></param>
        public void InvokeInUiThread(Action action)
        {
            action();
        }

        /// <summary>
        ///     在UI线程中执行
        /// </summary>
        /// <param name="action"></param>
        public void BeginInvokeInUiThread(Action action)
        {
            action();
        }

        /// <summary>
        ///     在UI线程中执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public void BeginInvokeInUiThread<T>(Action<T> action, T args)
        {
            action(args);
        }

        /// <summary>
        ///     在UI线程中执行
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
        ///     发出状态变化事件
        /// </summary>
        /// <param name="action">属性字段</param>
        public void RaiseStatusChanged<T>(Expression<Func<T>> action)
        {
            RaiseStatusChanged(GetPropertyName(action));
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="action">属性字段</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChanged(GetPropertyName(action));
        }

        /// <summary>
        ///     取属性字段
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <param name="action">字段方法</param>
        /// <returns>字段名称</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            return expression.Member.Name;
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="action">属性字段</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChanged(GetPropertyName(action));
        }
*/
