// /*****************************************************
// (c)2008-2017 Copy right www.Gboxt.com
// 作者:bull2
// 工程:CodeRefactor-Agebull.Common.WpfMvvmBase
// 建立:2014-12-09
// 修改:2014-12-09
// *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#endregion

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     有属性通知的对象
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
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

        #region 属性修改事件

        /// <summary>
        ///     属性修改事件
        /// </summary>
        private event PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        ///     属性修改事件(属性为空表示删除)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged -= value;
                _propertyChanged += value;
            }
            remove { _propertyChanged -= value; }
        }


        /// <summary>
        ///     发出属性修改事件(不受阻止模式影响)
        /// </summary>
        /// <param name="args">属性</param>
        protected void RaisePropertyChangedEvent(PropertyChangedEventArgs args)
        {
            try
            {
                _propertyChanged?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "NotificationObject.RaisePropertyChangedEvent");
                //throw;
            }
        }

        /// <summary>
        ///     发出属性修改事件(同步发出)
        /// </summary>
        /// <param name="propertyName">属性</param>
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (IsEditing)
            {
                lock (_changeLock)
                {
                    _changeds.Add(propertyName);
                }
                return;
            }
            InvokeInUiThread(RaisePropertyChangedEvent, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     发出属性修改事件(外部调用)
        /// </summary>
        /// <param name="propertyName">属性</param>
        public void RaisePropertyChanged(string propertyName)
        {
            RaisePropertyChangedEvent(propertyName);
        }

        #endregion

        #region 属性修改通知

        /// <summary>
        ///     取属性名称
        /// </summary>
        /// <typeparam name="T">表达式参数</typeparam>
        /// <param name="action">表达式</param>
        /// <returns>属性名称</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            return expression.Member.Name;
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="propertyName">属性</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChangedInner(propertyName);
        }

        /// <summary>
        ///     发出属性修改事件
        /// </summary>
        /// <param name="action">属性字段</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            OnPropertyChangedInner(GetPropertyName(action));
        }


        /// <summary>
        ///     属性修改处理
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChangedInner(string propertyName)
        {
            RaisePropertyChangedEvent(propertyName);
        }

        #endregion


        #region 编辑模式

        /// <summary>
        ///     是否处于集中编辑模式
        /// </summary>
        public bool IsEditing { get; private set; }

        /// <summary>
        ///     开始编辑
        /// </summary>
        public void BeginEdit()
        {
            if (IsEditing)
                return;
            lock(_changeLock)
            {
                _changeds = new List<string>();
            }
            IsEditing = true;
        }

        /// <summary>
        ///     结束编辑
        /// </summary>
        public void EndEdit()
        {
            IsEditing = false;
            lock (_changeLock)
            {
                if (_changeds == null || _changeds.Count == 0)
                    return;
                EndEditInner(_changeds);
                RaiseEditChanges();
            }
        }

        /// <summary>
        ///     结束编辑
        /// </summary>
        protected virtual void EndEditInner(List<string> changeds)
        {
        }

        /// <summary>
        ///     发出修改列表的修改事件
        /// </summary>
        protected void RaiseEditChanges()
        {
            lock (_changeLock)
            {
                foreach (var pn in _changeds)
                    RaisePropertyChangedEvent(new PropertyChangedEventArgs(pn));
            }
        }

        /// <summary>
        ///     修改列表的锁
        /// </summary>
        private readonly object _changeLock = new object();
        /// <summary>
        ///     当前未发出事件的已修改属性
        /// </summary>
        private List<string> _changeds;

        #endregion
    }

    public class DictionaryItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class DictionaryItem<TValue>
    {
        public string Name { get; set; }
        public TValue Value { get; set; }
    }
}