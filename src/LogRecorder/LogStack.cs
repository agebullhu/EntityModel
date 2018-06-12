// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using Agebull.Common.Frame;
using System.Collections.Generic;

#endregion

namespace System.Linq
{

    /// <summary>
    ///   表示一个栈底为固定值的栈
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    internal sealed class LogStack<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public LogStack()
        {
            IsEmpty = true;
        }
        private List<T> _value = new List<T>();

        /// <summary>
        /// 清栈
        /// </summary>
        public void Clear()
        {
            using (ThreadLockScope.Scope(this))
            {
                _value = new List<T>();
                Current = FixValue;
            }
        }
        /// <summary>
        /// 栈深
        /// </summary>
        public int StackCount => _value.Count;

        /// <summary>
        /// 栈内值
        /// </summary>
        public List<T> Stack => _value;

        /// <summary>
        ///   当前
        /// </summary>
        public T Current { get; private set; }

        /// <summary>
        ///  栈是否为空
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        ///   固定
        /// </summary>
        public T FixValue { get; private set; }

        /// <summary>
        ///   栈底为固定值,即保证最后栈中总有一个值
        /// </summary>
        /// <remarks>
        ///   当调用了SetDefault后为真
        /// </remarks>
        public bool FixStackBottom { get; private set; }

        /// <summary>
        ///   配置固定值(只第一次调用有效果)
        /// </summary>
        /// <param name="value"> </param>
        public void SetFix(T value)
        {
            using (ThreadLockScope.Scope(this))
            {
                if (Equals(value, default(T)) || FixStackBottom)
                {
                    return;
                }
                FixStackBottom = true;
                FixValue = Current = value;
            }
        }
        /// <summary>
        /// 设置配置固定值(只第一次调用有效果)并将栈内所有值替换为它
        /// </summary>
        /// <param name="value"> </param>
        public void SetFixAndReplaceAll(T value)
        {
            using (ThreadLockScope.Scope(this))
            {
                if (Equals(value, default(T)) || FixStackBottom)
                {
                    return;
                }
                FixStackBottom = true;
                FixValue = Current = value;
                int cnt = _value.Count;
                _value.Clear();
                for (int i = 0; i < cnt; i++)
                {
                    _value.Add(value);
                }
            }
        }

        /// <summary>
        ///   入栈
        /// </summary>
        /// <param name="value"> </param>
        public void Push(T value)
        {
            using (ThreadLockScope.Scope(this))
            {
                if (Equals(value, default(T)))
                {
                    return;
                }
                IsEmpty = false;
                _value.Add(value);
                Current = value;
            }
        }

        /// <summary>
        ///  空入栈
        /// </summary>
        public void PushNull()
        {
            using (ThreadLockScope.Scope(this))
            {
                IsEmpty = false;
                _value.Add(default(T));
                Current = default(T);
            }
        }

        /// <summary>
        ///  当前再入栈
        /// </summary>
        /// <remarks>目的是和其它人做相同次数的入栈和出栈</remarks>
        public void PushCurrent()
        {
            using (ThreadLockScope.Scope(this))
            {
                IsEmpty = false;
                _value.Add(Current);
            }
        }

        /// <summary>
        ///   出栈
        /// </summary>
        public T Pop()
        {
            using (ThreadLockScope.Scope(this))
            {
                if (_value.Count == 0)
                {
                    return FixValue;
                }
                _value.RemoveAt(_value.Count - 1);
                IsEmpty = _value.Count == 0;
                return Current = IsEmpty
                    ? FixValue
                    : _value[_value.Count - 1];
            }
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="value"></param>
        public void Remove(T value)
        {
            using (ThreadLockScope.Scope(this))
            {
                _value?.Remove(value);
            }
        }

        /// <summary>
        /// 直接操作Stack后的更新
        /// </summary>
        public void Refresh()
        {
            using (ThreadLockScope.Scope(this))
            {
                IsEmpty = _value.Count == 0;
                Current = IsEmpty
                    ? FixValue
                    : _value[_value.Count - 1];
            }
        }
    }
}
