// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     有属性通知的对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class NotificationObject
    {
        #region 属性修改通知

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        protected void OnPropertyChanged(int propertyIndex)
        {
            RecordModifiedInner(propertyIndex);
            OnPropertyChangedInner(propertyIndex);
        }

        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        protected virtual void RecordModifiedInner(int propertyIndex)
        {
        }

        /// <summary>
        ///     属性修改处理
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        protected virtual void OnPropertyChangedInner(int propertyIndex)
        {
        }

        #endregion

        #region 属性修改通知

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyName">属性</param>
        public void OnPropertyChanged(string propertyName)
        {
            RecordModifiedInner(propertyName);
            OnPropertyChangedInner(propertyName);
            OnStatusChanged(NotificationStatusType.Modified);
            RaisePropertyChangedEvent(propertyName);
        }

        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyName">属性</param>
        protected virtual void RecordModifiedInner(string propertyName)
        {
        }

        /// <summary>
        ///     属性修改处理
        /// </summary>
        /// <param name="propertyName">属性</param>
        protected virtual void OnPropertyChangedInner(string propertyName)
        {
        }

        #endregion

        #region 事件


        /// <summary>
        ///     属性修改事件
        /// </summary>
        private event PropertyChangedEventHandler propertyChanged;
        /// <summary>
        ///     属性修改事件(属性为空表示删除)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged -= value;
                propertyChanged += value;
            }
            remove => propertyChanged -= value;
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyName">属性</param>
        [Conditional("EntityEvent")]
        public void RaisePropertyChangedEvent(string propertyName)
        {
            RaisePropertyChangedInner(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     发出属性修改事件(不受阻止模式影响)
        /// </summary>
        /// <param name="args">属性</param>
        private void RaisePropertyChangedInner(PropertyChangedEventArgs args)
        {
            if (propertyChanged == null)
            {
                return;
            }
            try
            {
                propertyChanged(this, args);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "NotificationObject.RaisePropertyChangedInner");
                throw;
            }
        }

        #endregion

        #region 状态修改事件

        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="status">状态</param>
        [Conditional("EntityStatus")]
        public void OnStatusChanged(NotificationStatusType status)
        {
            StatusChangedInner(status);
        }

        /// <summary>
        ///     状态变化处理
        /// </summary>
        /// <param name="status">状态</param>
        protected virtual void StatusChangedInner(NotificationStatusType status)
        {
            OnStatusChangedInner(status);
        }

        /// <summary>
        ///     状态变化处理
        /// </summary>
        /// <param name="status">状态</param>
        protected virtual void OnStatusChangedInner(NotificationStatusType status)
        {
        }

        #endregion
    }
}

/*

        
        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="status">状态</param>
        [Conditional("EntityStatus")]
        protected internal void OnStatusChanged(string status)
        {
            OnStatusChangedInner(status);
        }



        /// <summary>
        ///     状态变化处理
        /// </summary>
        /// <param name="status">状态</param>
        protected virtual void OnStatusChangedInner(string status)
        {
        }

        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="action">属性字段</param>
        [Conditional("EntityStatus")]
        protected internal void OnStatusChanged<T>(Expression<Func<T>> action)
        {
            OnStatusChanged(GetPropertyName(action));
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="action">属性字段</param>
        [Conditional("EntityStatus")]
        public void RaisePropertyChangedEvent<T>(Expression<Func<T>> action)
        {
            RaisePropertyChangedEventInner(GetPropertyName(action));
        }
        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="action">属性字段</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChanged(GetPropertyName(action));
        }

        /// <param name="action">属性字段</param>
        protected static string GetPropertyName<T>(Expression<Func<T>> action)
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
