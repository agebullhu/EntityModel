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

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     编辑状态
    /// </summary>
    [DataContract, Serializable]
    public class IndexEditStatus : ObjectStatusBase
    {
        #region 还原与接受

        /// <summary>
        ///     接受修改
        /// </summary>
        internal void AcceptChanged()
        {
            _state = !IsDelete ? (byte) 0 : (byte) 0x2;
            if (modifiedProperties != null)
            {
                SetUnModify();
            }
        }

        #endregion

        #region 高级对象

        /// <summary>
        ///     对应的对象
        /// </summary>
        [IgnoreDataMember]
        public EditDataObject<IndexEditStatus> EditObject { get; private set; }

        /// <summary>
        ///     初始化的实现
        /// </summary>
        protected override void InitializeInner()
        {
            base.InitializeInner();
            EditObject = Object as EditDataObject<IndexEditStatus>;
        }

        #endregion

        #region 修改状态

        /// <summary>
        ///     阻止配置
        /// </summary>
        [IgnoreDataMember] private EditArrestMode _arrest;

        /// <summary>
        ///     阻止配置
        /// </summary>
        [ReadOnly(true), DisplayName("阻止配置"), Category("运行时")]
        public EditArrestMode Arrest
        {
            get { return _arrest; }
            set
            {
                if (value == _arrest)
                {
                    return;
                }
                _arrest = value;
                Object.OnStatusChanged(() => Arrest);
            }
        }

        /// <summary>
        ///     修改的属性列表
        /// </summary>
        [DataMember] internal byte[] modifiedProperties;

        /// <summary>
        ///     修改的属性列表
        /// </summary>
        [ReadOnly(true), DisplayName("修改的属性列表"), Category("运行时")]
        public byte[] ModifiedProperties
        {
            get { return modifiedProperties ?? (modifiedProperties = new byte[EditObject.__PropertyCount + 1]); }
        }

        /// <summary>
        ///     是否修改
        /// </summary>
        public bool FieldIsModified(int propertyIndex)
        {
            return modifiedProperties != null && modifiedProperties[propertyIndex] == 1;
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        public void SetUnModify()
        {
            NeedSave = false;
            if (modifiedProperties == null)
            {
                return;
            }
            for (var index = 0; index < modifiedProperties.Length; index++)
            {
                modifiedProperties[index] = 0;
            }
        }

        /// <summary>
        ///     设置为改变
        /// </summary>
        public void SetModified()
        {
            EditObject.IsReadOnly = false;
            for (var index = 0; index < ModifiedProperties.Length - 1; index++)
            {
                ModifiedProperties[index] = 1;
            }
            ModifiedProperties[EditObject.__PropertyCount] = (byte) EditObject.__PropertyCount;
            NeedSave = true;
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        public void SetUnModify(int propertyIndex)
        {
            if (modifiedProperties == null)
            {
                return;
            }
            if (ModifiedProperties[propertyIndex] == 0)
            {
                return;
            }
            ModifiedProperties[propertyIndex] = 0;
            ModifiedProperties[EditObject.__PropertyCount] -= 1;
            if (ModifiedProperties[EditObject.__PropertyCount] == 0)
            {
                NeedSave = false;
            }
        }

        /// <summary>
        ///     设置为改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        public void SetModified(int propertyIndex)
        {
            if (ModifiedProperties[propertyIndex] == 1)
            {
                return;
            }
            ModifiedProperties[propertyIndex] = 1;
            ModifiedProperties[EditObject.__PropertyCount] += 1;
            NeedSave = true;
        }

        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        internal void RecordModified(int propertyIndex)
        {
            if (!Arrest.HasFlag(EditArrestMode.RecordChanged))
            {
                RecordModifiedInner(propertyIndex);
            }
            if (!Arrest.HasFlag(EditArrestMode.InnerLogical))
            {
                EditObject.OnModified(propertyIndex);
            }
        }


        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        private void RecordModifiedInner(int propertyIndex)
        {
            //Trace.WriteLine(string.Format("{1} => Modified:{0}", EditObject.__Struct.Properties[propertyIndex].PropertyName, EditObject.GetValue(EditObject.__Struct.PrimaryKey)),
            //    EditObject.GetType().Name);

            SetModified(propertyIndex);
            if (!Arrest.HasFlag(EditArrestMode.InnerLogical))
            {
                EditObject.OnModified(propertyIndex);
            }
        }

        #endregion

        #region 复制支持

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        internal void CopyState(IndexEditStatus target)
        {
            CopyStateInner(target);
        }

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        private void CopyStateInner(IndexEditStatus target)
        {
            if (target.modifiedProperties == null)
            {
                modifiedProperties = null;
            }
            else
            {
                for (var index = 0; index < target.ModifiedProperties.Length; index++)
                {
                    modifiedProperties[index] = target.ModifiedProperties[index];
                }
            }
        }

        #endregion

        #region 对象生态

        /// <summary>
        ///     对象生态状态
        /// </summary>
        [ReadOnly(true), DisplayName("对象生态状态"), Category("运行时")]
        public EntitySubsist Subsist
        {
            get
            {
                switch (_state)
                {
                    case 0x5:
                        return EntitySubsist.Shadow;
                    case 0x1:
                        return EntitySubsist.Added;
                    case 0xF1:
                    case 0xF5:
                        return EntitySubsist.Adding;
                    case 0x2:
                        return EntitySubsist.Deleted;
                    case 0xF2:
                        return EntitySubsist.Deleting;
                    case 0xF0:
                        return EntitySubsist.Modified;
                    default:
                        return EntitySubsist.Exist;
                }
            }
        }

        private byte _state;

        /// <summary>
        ///     是否已修改
        /// </summary>
        [ReadOnly(true), DisplayName("是否修改"), Category("运行时")]
        public bool IsModified
        {
            get { return modifiedProperties != null && ModifiedProperties[EditObject.__PropertyCount] > 0; }
            set
            {
                if ((modifiedProperties != null && ModifiedProperties[EditObject.__PropertyCount] > 0 == value))
                {
                    return;
                }
                if (value)
                {
                    SetModified();
                }
                else
                {
                    SetUnModify();
                }
                Object.OnStatusChanged(() => IsModified);
            }
        }

        /// <summary>
        ///     是否新增加的
        /// </summary>
        [ReadOnly(true), DisplayName("是否修改"), Category("运行时")]
        public bool IsNew
        {
            get { return (_state & 0x1) == 0x1; }
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsNew:{0}", value, EditObject.GetValue(EditObject.__Struct.PrimaryKey)), EditObject.GetType().Name);
                if (value)
                {
                    _state |= 0x1;
                }
                else if (IsModified)
                {
                    _state &= 0xFE;
                }
                else
                {
                    _state &= 0xE;
                }
                EditObject.OnAdd(value);
                Object.OnStatusChanged(() => IsNew);
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
            get { return (_state & 0x5) == 0x5; }
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsShadow:{0}", value, EditObject.GetValue(EditObject.__Struct.PrimaryKey)), EditObject.GetType().Name);
                if (value)
                {
                    _state |= 0x5;
                }
                else if (IsModified)
                {
                    _state &= 0xFB;
                }
                else
                {
                    _state &= 0xB;
                }
                Object.OnStatusChanged(() => IsShadow);
            }
        }

        /// <summary>
        ///     是否已删除
        /// </summary>
        /// <returns></returns>
        [ReadOnly(true), DisplayName("是否已删除"), Category("运行时")]
        public bool IsDelete
        {
            get { return (_state & 0x2) == 0x2; }
            set
            {
                //Trace.WriteLine(string.Format("{1} => IsDelete:{0}", value, EditObject.GetValue(EditObject.__Struct.PrimaryKey)), EditObject.GetType().Name);
                if (value)
                {
                    _state |= 0xF2;
                }
                else if (IsModified)
                {
                    _state &= 0xFD;
                }
                else
                {
                    _state &= 0xD;
                }
                EditObject.OnDelete(value);
                Object.OnStatusChanged(() => IsDelete);
            }
        }

        /// <summary>
        ///     是否需要保存
        /// </summary>
        /// <returns></returns>
        [ReadOnly(true), DisplayName("是否需要保存"), Category("运行时")]
        public bool NeedSave
        {
            get { return (_state & 0xF0) == 0xF0; }
            set
            {
                //Trace.WriteLine(string.Format("{1} => NeedSave:{0}", value, EditObject.GetValue(EditObject.__Struct.PrimaryKey)), EditObject.GetType().Name);
                if (value)
                {
                    _state |= 0xF0;
                }
                else
                {
                    _state &= 0x0F;
                }
            }
        }

        #endregion
    }
}