// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     对象状态基类
    /// </summary>
    [DataContract, Serializable]
    public class ObjectStatus
    {
        /// <summary>
        ///     是否只读,用于在访问前__EntityStatus前防止构造__EntityStatus消耗内存
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool IsReadOnly { get; set; }

        /// <summary>
        ///     状态对象为空
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public bool EntityStatusNull => Status == null;

        /// <summary>
        ///     状态对象
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        internal EditDataObject Entity { get; set; }

        /// <summary>
        ///     状态对象
        /// </summary>
        internal IndexEditStatus _status;

        /// <summary>
        ///     状态对象
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public IndexEditStatus Status => IsReadOnly ? null : (_status ??= new IndexEditStatus(Entity));


        /// <summary>
        ///     重置
        /// </summary>
        public void Reset(sbyte type = Exist)
        {
            _status = null;
            IsReadOnly = false;
            _state = type;
        }

        /// <summary>
        ///     接受修改
        /// </summary>
        public void AcceptChanged()
        {
            if (IsReadOnly)
                return;
            _state = !IsDelete ? Exist : Deleted;
            Status.SetUnModify();
            Entity.AcceptChangedInner();
            Entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     回退修改
        /// </summary>
        public void RejectChanged()
        {
            if (IsReadOnly)
                return;
            IsModified = false;
            Entity.RejectChangedInner();
            Entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        /// <summary>
        ///     是否修改
        /// </summary>
        public bool FieldIsModified(int propertyIndex)
        {
            return !IsReadOnly && _status != null && _status.FieldIsModified(propertyIndex);
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        public void SetUnModify(int propertyIndex)
        {
            if (IsReadOnly)
                return;
            SetModified(Status.SetUnModify(propertyIndex));
        }

        /// <summary>
        ///     设置为改变
        /// </summary>
        /// <param name="property"> 字段的名字 </param>
        public void SetModify(string property)
        {
            if (IsReadOnly)
                return;
            Status.RecordModified(propertyIndex);
            SetModified(true);
        }


        #region 对象生态

        private const sbyte Added = 0x1;

        private const sbyte Exist = 0x2;

        private const sbyte Deleted = 0x4;

        private const sbyte Shadow = 0x8;

        private const sbyte Modify = 0x10;

        private const sbyte User = 0x20;


        private sbyte _state;

        /*// <summary>
        ///     对象生态状态
        /// </summary>
        [ReadOnly(true), DisplayName("对象生态状态"), Category("运行时")]
        public EntitySubsist Subsist
        {
            get
            {
                switch (_state & 0xF)
                {
                    case Added:
                        return EntitySubsist.Added;
                    case Added | Modify:
                        return EntitySubsist.Adding;
                    case Deleted:
                        return EntitySubsist.Deleted;
                    case Deleted | Modify:
                        return EntitySubsist.Deleting;
                    case Exist:
                        return EntitySubsist.Exist;
                    case Exist | Modify:
                        return EntitySubsist.Modified;
                    default:
                        return EntitySubsist.None;
                }
            }
        }*/

        /// <summary>
        ///     是否已修改
        /// </summary>
        [ReadOnly(true), DisplayName("是否修改"), Category("运行时")]
        public bool IsModified
        {
            get => (_state & Modify) == Modify;
            set
            {
                if (value)
                {
                    _state |= Modify;
                }
                else
                {
                    _state &= ~Modify;
                }

                if (IsReadOnly)
                {
                    if ((_state & Modify) == Modify)
                    {
                        Status.SetModified();
                    }
                    else
                    {
                        Status.SetUnModify();
                    }
                }
                Entity.OnStatusChanged(NotificationStatusType.Modified);
            }
        }
        void SetModified(bool modify)
        {
            if (modify)
            {
                _state |= Modify;
            }
            else
            {
                _state &= ~Modify;
            }
            Entity.OnStatusChanged(NotificationStatusType.Modified);
        }
        /// <summary>
        ///     是否已存在
        /// </summary>
        [ReadOnly(true), DisplayName("是否已存在"), Category("运行时")]
        public bool IsExist
        {
            get => (_state & Exist) == Exist;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsNew:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Exist;
                }
                else
                {
                    _state &= ~Exist;
                }
            }
        }

        /// <summary>
        ///     是否新增加的
        /// </summary>
        [ReadOnly(true), DisplayName("是否修改"), Category("运行时")]
        public bool IsNew
        {
            get => (_state & Added) == Added;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsNew:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Added;
                }
                else
                {
                    _state &= ~Added;
                }
                Entity.OnAdd(value);
                Entity.OnStatusChanged(NotificationStatusType.Added);
            }
        }

        /// <summary>
        ///     是否已删除
        /// </summary>
        /// <returns></returns>
        [ReadOnly(true), DisplayName("是否已删除"), Category("运行时")]
        public bool IsDelete
        {
            get => (_state & Deleted) == Deleted;
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsDelete:{0}", value, Entity.GetValue(Entity.__Struct.PrimaryKey)), Entity.GetType().Name);
                if (value)
                {
                    _state |= Deleted;
                }
                else
                {
                    _state &= ~Deleted;
                }
                Entity.OnDelete(value);
                Entity.OnStatusChanged(NotificationStatusType.Deleted);
            }
        }

        /// <summary>
        ///     是否来自编辑客户端
        /// </summary>
        public bool IsFromClient
        {
            get => (_state & User) == User;
            set
            {
                if (value)
                {
                    _state |= User;
                }
                else
                {
                    _state &= ~User;
                }
            }
        }

        /// <summary>
        ///     是否影子复制
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     影子复制是为节省内在而设置的,有已下特性
        ///     1 从一个长生存期(进程相同)的对象复制而来
        ///     2 它有可能被修改
        ///     3 当它被修改时,使用已修改的新拷贝并独立存储
        ///     4 当它未修改时,使用源对象的内容且不进行存储
        ///     属性说明
        ///     1 当设置为真时,它是一种特殊的新增对象(即设置新增标记)
        ///     2 当设置为假时,它是普通的新增对象(即只有新增标记)
        ///     3 无论设置为真假,新增标记将保留
        ///     4 由于需要尽量减少不必要的保存,它不调用新增方法(OnAdd)
        /// </remarks>
        [ReadOnly(true), DisplayName("是否已删除"), Category("运行时")]
        public bool IsShadow
        {
            get => (_state & Shadow) == Shadow;
            set
            {
                if (value)
                {
                    _state |= Shadow;
                }
                else
                {
                    _state &= ~Shadow;
                }
            }
        }

        #endregion
    }
}