// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     编辑支持的实体对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class EditDataObject<TStatus> : StatusDataObject<TStatus>, IEditObject
        where TStatus : IndexEditStatus, new()
    {
        #region 复制支持

        /// <summary>
        ///     复制修改状态
        /// </summary>
        /// <param name="target">要复制的源</param>
        protected void CopyState(EditDataObject target)
        {
            __EntityStatus.CopyState(target.__EntityStatus);
        }

        #endregion

        #region 仅可重载方法

        /// <summary>
        ///     构建状态对象
        /// </summary>
        protected override TStatus CreateStatus()
        {
            var status = new TStatus();
            status.Initialize(this);
            return status;
        }

        /// <summary>
        ///     接受修改
        /// </summary>
        public void AcceptChanged()
        {
            AcceptChangedInner();
            OnStatusChanged(NotificationStatusType.Modified);
        }


        /// <summary>
        ///     回退修改
        /// </summary>
        public void RejectChanged()
        {
            RejectChangedInner();
            if (__status != null && __status.IsNew)
            {
                return;
            }
            __status = null;
        }

        /// <summary>
        ///     接受修改(只可重写,不可调用)
        /// </summary>
        protected virtual void AcceptChangedInner()
        {
            __EntityStatus.AcceptChanged();
        }

        /// <summary>
        ///     回退修改(只可重写,不可调用)
        /// </summary>
        protected virtual void RejectChangedInner()
        {
        }

        /// <summary>
        ///     已被修改(不可调用,只可重写)
        /// </summary>
        protected internal virtual void OnModified(int field)
        {
        }

        #endregion

        #region 修改状态

        /// <summary>
        ///     是否修改
        /// </summary>
        bool IEditObject.IsModified => __status != null && __status.IsModified;

        /// <summary>
        ///     是否新增
        /// </summary>
        bool IEditObject.IsAdd => __status != null && __status.IsNew;

        /// <summary>
        ///     是否修改
        /// </summary>
        public bool FieldIsModified(int propertyIndex)
        {
            return !IsReadOnly && __EntityStatus.FieldIsModified(propertyIndex);
        }

        /// <summary>
        ///     设置为非改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        public virtual void SetUnModify(int propertyIndex)
        {
            if (IsReadOnly)
            {
                return;
            }
            __EntityStatus.SetUnModify(propertyIndex);
        }

        /// <inheritdoc />
        /// <summary>
        ///     设置为改变
        /// </summary>
        /// <param name="propertyIndex"> 字段的名字 </param>
        public void SetModify(int propertyIndex)
        {
            RecordModifiedInner(propertyIndex);
        }


        /// <summary>
        ///     记录属性修改
        /// </summary>
        /// <param name="propertyIndex">属性</param>
        protected sealed override void RecordModifiedInner(int propertyIndex)
        {
            if (!IsReadOnly && !__EntityStatus.Arrest.HasFlag(EditArrestMode.RecordChanged))
            {
                __EntityStatus.RecordModified(propertyIndex);
            }
        }

        /// <summary>
        ///     状态变化处理
        /// </summary>
        /// <param name="status">状态</param>
        protected sealed override void StatusChangedInner(NotificationStatusType status)
        {
            if (__status != null && __status.Arrest.HasFlag(EditArrestMode.RecordChanged))
            {
                return;
            }
            OnStatusChangedInner(status);
        }

        /// <summary>
        ///     发出状态变化事件
        /// </summary>
        /// <param name="status">状态</param>
        protected sealed override void OnStatusChanged(string status)
        {
            if (__status != null && __status.Arrest.HasFlag(EditArrestMode.RecordChanged))
            {
                return;
            }
            OnStatusChangedInner(status);
        }

        #endregion

        #region 编辑方法

        /// <inheritdoc />
        /// <summary>
        ///     是否已删除
        /// </summary>
        /// <returns></returns>
        bool IEditObject.IsDelete => __EntityStatus.IsDelete;

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
            OnLaterPeriodBySignleModified(__EntityStatus.Subsist, __EntityStatus.ModifiedProperties);
            OnLaterPeriodByModified(__EntityStatus.Subsist, __EntityStatus.ModifiedProperties);
        }

        /// <summary>
        ///     以全部数据被修改的状态,执行后期操作
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void LaterPeriodByModify(EntitySubsist subsist)
        {
            OnLaterPeriodBySignleModified(subsist, __EntityStatus.ModifiedProperties);
        }

        /// <summary>
        ///     以全部数据被修改的状态,执行后期操作
        /// </summary>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public void DoLaterPeriodByAllModified()
        {
            OnLaterPeriodBySignleModified(EntitySubsist.Added, __status?.modifiedProperties);
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
        }

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
        }

        #endregion

        #region 属性

        /// <summary>
        /// 数据是否被用户处理过
        /// </summary>
        [IgnoreDataMember, Browsable(false), JsonIgnore] int _x_entity_state_xx_;

        /// <summary>
        /// 数据是否被用户处理过
        /// </summary>
        [IgnoreDataMember, Browsable(false), JsonIgnore]
        public bool __IsFromUser
        {
            get { return (_x_entity_state_xx_ & 0x1) == 0x1; }
            set
            {
                if (value)
                    _x_entity_state_xx_ |= 0x1;
                else
                    _x_entity_state_xx_ &= ~0x1;
            }
        }

        /// <summary>
        ///     属性总量
        /// </summary>
        [IgnoreDataMember, Browsable(false), JsonIgnore]
        public int __PropertyCount
        {
            get { return __Struct.Properties.Count; }
        }

        private Dictionary<string, string> _properties;

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> __ColumnMap
        {
            get { return _properties ?? (_properties = __Struct.Properties.Values.ToDictionary(p => p.PropertyName, p => p.ColumnName)); }
        }

        /// <summary>
        ///     实体结构
        /// </summary>
        [IgnoreDataMember, Browsable(false), JsonIgnore]
        public abstract EntitySturct __Struct { get; }

        #endregion

        #region 本地操作支持

#if CLIENT
    /// <summary>
    ///     保存时间戳
    /// </summary>
        [JsonIgnore]
        private DateTime _timeStamp;

        /// <summary>
        ///     保存时间戳
        /// </summary>
        [JsonIgnore]
        public DateTime timeStamp
        {
            get { return _timeStamp; }
            set
            {
                timeStamp2 = _timeStamp;
                _timeStamp = value;
            }
        }

        /// <summary>
        ///     保存时间戳
        /// </summary>
        [JsonIgnore]
        public DateTime timeStamp2 { get; set; }

        /// <summary>
        ///     是否来自本地缓存
        /// </summary>
        [JsonIgnore]
        public bool fromCache { get; set; }
#endif

        #endregion

        #region 二进制序列化

#if Binary
        /// <summary>
        ///     序列化成BYTE
        /// </summary>
        /// <returns>BYTE</returns>
        public byte[] SerializableToBytes()
        {
            using (var mem = new MemoryStream(1024))
            {
                using (var writer = new BinaryWriter(mem))
                {
                    WriteBinaryValue(writer);
                    //writer.Write((byte)0xFF);
                    //writer.Write(DateTime.Now.Ticks);
                }
                return mem.ToArray();
            }
        }

        /// <summary>
        ///     写入二进制流
        /// </summary>
        /// <param name="writer">流对象</param>
        public virtual void WriteBinaryValue(BinaryWriter writer)
        {
        }


        /// <summary>
        ///     从BYTE反序列化成
        /// </summary>
        public void DeSerializableFromBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return;
            }
            using (new EditScope(__EntityStatus, EditArrestMode.All, false))
            {
                using (var mem = new MemoryStream(buffer))
                {
                    using (var reader = new BinaryReader(mem))
                    {
                        var ver = reader.ReadByte();
                        ReadBinaryValue(reader, ver);
                        //if (mem.Position + 8 < reader.BaseStream.Length)
                        //    timeStamp = new DateTime(reader.ReadInt64());
                    }
                }
            }
        }

        /// <summary>
        ///     读取二进制值
        /// </summary>
        /// <param name="reader">流对象</param>
        /// <param name="ver">数据版本号</param>
        public virtual void ReadBinaryValue(BinaryReader reader, int ver)
        {
        }
#endif

        #endregion
    }

    /// <summary>
    ///     编辑支持的实体对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class EditDataObject : EditDataObject<IndexEditStatus>
    {
    }
}