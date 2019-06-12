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
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     编辑支持的实体对象
    /// </summary>
    [DataContract, Serializable]
    public abstract class EditDataObject : DataObjectBase//, IEditObject
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
            OnLaterPeriodBySignleModified(__status.Subsist, __status._status.modifiedProperties);
            OnLaterPeriodByModified(__status.Subsist, __status._status.modifiedProperties);
        }

        /// <summary>
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

}

/*
 

 */
