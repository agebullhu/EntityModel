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
    ///     编辑支持的实体对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class EditDataObject : DataObjectBase, IEditObject
    {
        #region 构造

        /// <summary>
        ///     状态对象
        /// </summary>
        [Browsable(false), IgnoreDataMember, JsonIgnore]
        public ObjectStatus __status { get; }

        /// <summary>
        /// 构造
        /// </summary>
        protected EditDataObject()
        {
            __status = new ObjectStatus
            {
                Entity = this
            };
        }

        #endregion

        #region 复制支持

        /// <summary>
        ///     复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            if (!__status.IsReadOnly && source is EditDataObject editData)
                __status.Status.CopyState(editData.__status.Status);
            base.CopyValueInner(source);
        }
        #endregion

        #region 仅可重载方法


        /// <summary>
        ///     接受修改(只可重写,不可调用)
        /// </summary>
        protected internal virtual void AcceptChangedInner()
        {
        }

        /// <summary>
        ///     回退修改(只可重写,不可调用)
        /// </summary>
        protected internal virtual void RejectChangedInner()
        {
        }

        /// <summary>
        ///     已被修改(不可调用,只可重写)
        /// </summary>

        internal void OnModified(int field)
        {
            OnModifiedInner(field);
        }

        /// <summary>
        ///     已被修改(不可调用,只可重写)
        /// </summary>
        protected virtual void OnModifiedInner(int field)
        {
        }

        #endregion

        #region 修改状态

        /// <summary>
        /// 设置为未修改
        /// </summary>
        public void RejectChanged()
        {
            __status.RejectChanged();
        }

        /// <summary>
        /// 设置为未修改
        /// </summary>
        public void AcceptChanged()
        {
            __status.AcceptChanged();
        }

        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        protected sealed override void RecordModifiedInner(int propertyIndex)
        {
            if (__status.IsReadOnly)
                return;
            __status.SetModify(propertyIndex);
        }

        #endregion

        #region 编辑方法

        /// <summary>
        ///     对象删除时的同步处理(只可重写,不可调用)
        /// </summary>
        protected internal virtual void OnDelete(bool isDelete)
        {
        }

        /// <summary>
        ///     对象新增时的同步处理(只可重写,不可调用)
        /// </summary>
        protected internal virtual void OnAdd(bool idAdd)
        {
        }

        #endregion

        #region 后期修改事件

        /// <summary>
        ///     属性修改的后期处理(保存后)
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void LaterPeriodByModify()
        {
            if (__status.IsReadOnly || __status._status == null)
                return;
            //OnLaterPeriodBySignleModified(__status.Subsist, __status._status.modifiedProperties);
            //OnLaterPeriodByModified(__status.Subsist, __status._status.modifiedProperties);
        }

        /*// <summary>
        ///     以全部数据被修改的状态,执行后期操作
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void LaterPeriodByModify(EntitySubsist subsist)
        {
            if (__status.IsReadOnly || __status._status == null)
                return;
            OnLaterPeriodBySignleModified(subsist, __status._status.modifiedProperties);
        }

        /// <summary>
        ///     以全部数据被修改的状态,执行后期操作
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void DoLaterPeriodByAllModified()
        {
            if (__status.IsReadOnly || __status._status == null)
                return;
            OnLaterPeriodBySignleModified(EntitySubsist.Added, __status._status.modifiedProperties);
        }


        /// <summary>
        ///     单个属性修改的后期处理(保存后)
        /// </summary>
        /// <param name="subsist">当前实体生存状态</param>
        /// <param name="modifieds">修改列表</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        protected virtual void OnLaterPeriodBySignleModified(EntitySubsist subsist, byte[] modifieds)
        {
        }

        /// <summary>
        ///     组合属性修改的后期处理(保存后)
        /// </summary>
        /// <param name="subsist">当前实体生存状态</param>
        /// <param name="modifieds">修改列表</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        protected virtual void OnLaterPeriodByModified(EntitySubsist subsist, byte[] modifieds)
        {
        }*/

        #endregion

        #region 校验与重置
        /// <summary>
        ///     数据校验
        /// </summary>
        /// <returns>校验消息,如果正确,返回空</returns>
        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            Validate(result);
            return result;
        }

        /// <summary>
        ///     数据校验
        /// </summary>
        public virtual void Validate(ValidateResult result)
        {
        }

        /// <summary>
        ///     重置
        /// </summary>
        public virtual void Reset()
        {
            __status.Reset();
        }

        bool IEditObject.FieldIsModified(int propertyIndex)
        {
           return __status.FieldIsModified(propertyIndex);
        }

        void IEditObject.SetUnModify(int propertyIndex)
        {
            __status.SetUnModify(propertyIndex);
        }

        void IEditObject.SetModify(int propertyIndex)
        {
            __status.SetModify(propertyIndex);
        }

        bool IEditObject.IsModified => __status.IsModified;

        bool IEditObject.IsDelete => __status.IsDelete;

        bool IEditObject.IsNew => __status.IsNew;

        #endregion

        #region 属性

    }

}
