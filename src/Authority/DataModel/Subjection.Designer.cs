/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 17:34:54*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using Agebull.Common;
using Gboxt.Common.DataModel;

using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 行级权限关联
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class SubjectionData : IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public SubjectionData()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();
        #endregion


        #region 属性字义


        /// <summary>
        /// 修改主键
        /// </summary>
        public void ChangePrimaryKey(int id)
        {
            _id = id;
        }
        
        /// <summary>
        /// 标识:标识的实时记录顺序
        /// </summary>
        internal const int Real_Id = 0;

        /// <summary>
        /// 标识:标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _id;

        partial void OnIdGet();

        partial void OnIdSet(ref int value);

        partial void OnIdLoad(ref int value);

        partial void OnIdSeted();

        /// <summary>
        /// 标识:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"标识")]
        public int Id
        {
            get
            {
                OnIdGet();
                return this._id;
            }
            set
            {
                if(this._id == value)
                    return;
                //if(this._id > 0)
                //    throw new Exception("主键一旦设置就不可以修改");
                OnIdSet(ref value);
                this._id = value;
                this.OnPropertyChanged(Real_Id);
                OnIdSeted();
            }
        }
        /// <summary>
        /// 主键:主键的实时记录顺序
        /// </summary>
        internal const int Real_MasterId = 1;

        /// <summary>
        /// 主键:主键
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _masterid;

        partial void OnMasterIdGet();

        partial void OnMasterIdSet(ref int value);

        partial void OnMasterIdSeted();

        /// <summary>
        /// 主键:主键
        /// </summary>
        /// <remarks>
        /// 主键
        /// </remarks>
        [IgnoreDataMember , JsonProperty("MasterId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"主键")]
        public  int MasterId
        {
            get
            {
                OnMasterIdGet();
                return this._masterid;
            }
            set
            {
                if(this._masterid == value)
                    return;
                OnMasterIdSet(ref value);
                this._masterid = value;
                OnMasterIdSeted();
                this.OnPropertyChanged(Real_MasterId);
            }
        }
        /// <summary>
        /// 关联:关联的实时记录顺序
        /// </summary>
        internal const int Real_SlaveId = 2;

        /// <summary>
        /// 关联:关联
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _slaveid;

        partial void OnSlaveIdGet();

        partial void OnSlaveIdSet(ref int value);

        partial void OnSlaveIdSeted();

        /// <summary>
        /// 关联:关联
        /// </summary>
        /// <remarks>
        /// 关联
        /// </remarks>
        [IgnoreDataMember , JsonProperty("SlaveId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"关联")]
        public  int SlaveId
        {
            get
            {
                OnSlaveIdGet();
                return this._slaveid;
            }
            set
            {
                if(this._slaveid == value)
                    return;
                OnSlaveIdSet(ref value);
                this._slaveid = value;
                OnSlaveIdSeted();
                this.OnPropertyChanged(Real_SlaveId);
            }
        }
        #endregion

        #region IIdentityData接口


        /// <summary>
        /// Id键
        /// </summary>
        int IIdentityData.Id
        {
            get
            {
                return (int)this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
        #endregion
        #region 属性扩展


    

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected override void SetValueInner(string property, object value)
        {
            switch(property.Trim().ToLower())
            {
            case "id":
                this.Id = (int)Convert.ToDecimal(value);
                return;
            case "masterid":
                this.MasterId = (int)Convert.ToDecimal(value);
                return;
            case "slaveid":
                this.SlaveId = (int)Convert.ToDecimal(value);
                return;
            }

            //System.Diagnostics.Trace.WriteLine(property + @"=>" + value);

        }

    

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void SetValueInner(int index, object value)
        {
            /*switch(index)
            {
            case Index_Id:
                this.Id = Convert.ToInt32(value);
                return;
            case Index_MasterId:
                this.MasterId = Convert.ToInt32(value);
                return;
            case Index_SlaveId:
                this.SlaveId = Convert.ToInt32(value);
                return;
            }*/
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="property"></param>
        protected override object GetValueInner(string property)
        {
            switch(property)
            {
            case "Id":
                return this.Id;
            case "MasterId":
                return this.MasterId;
            case "SlaveId":
                return this.SlaveId;
            }

            return null;
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="index"></param>
        protected override object GetValueInner(int index)
        {
            /*switch(index)
            {
                case Index_Id:
                    return this.Id;
                case Index_MasterId:
                    return this.MasterId;
                case Index_SlaveId:
                    return this.SlaveId;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(SubjectionData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as SubjectionData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._masterid = sourceEntity._masterid;
            this._slaveid = sourceEntity._slaveid;
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(SubjectionData source)
        {
                this.Id = source.Id;
                this.MasterId = source.MasterId;
                this.SlaveId = source.SlaveId;
        }
        #endregion

        #region 后期处理

        /// <summary>
        /// 单个属性修改的后期处理(保存后)
        /// </summary>
        /// <param name="subsist">当前实体生存状态</param>
        /// <param name="modifieds">修改列表</param>
        /// <remarks>
        /// 对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        protected override void OnLaterPeriodBySignleModified(EntitySubsist subsist,byte[] modifieds)
        {
            if (subsist == EntitySubsist.Deleting)
            {
                OnIdModified(subsist,false);
                OnMasterIdModified(subsist,false);
                OnSlaveIdModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIdModified(subsist,true);
                OnMasterIdModified(subsist,true);
                OnSlaveIdModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[3] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnMasterIdModified(subsist,modifieds[Real_MasterId] == 1);
                OnSlaveIdModified(subsist,modifieds[Real_SlaveId] == 1);
            }
        }

        #region 属性后期修改的分部方法

        /// <summary>
        /// 标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 主键修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMasterIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 关联修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnSlaveIdModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion

        #region 实体结构

        
        public const byte Index_Id = 1;
        public const byte Index_MasterId = 4;
        public const byte Index_SlaveId = 5;

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember,Browsable (false)]
        public override EntitySturct __Struct
        {
            get
            {
                return __struct;
            }
        }

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember]
        static readonly EntitySturct __struct = new EntitySturct
        {
            EntityName = "Subjection",
            PrimaryKey = "Id",
            EntityType = 0x50009,
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_Id,
                    new PropertySturct
                    {
                        Index = Index_Id,
                        Name = "Id",
                        Title = "标识",
                        ColumnName = "id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_MasterId,
                    new PropertySturct
                    {
                        Index = Index_MasterId,
                        Name = "MasterId",
                        Title = "主键",
                        ColumnName = "master_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_SlaveId,
                    new PropertySturct
                    {
                        Index = Index_SlaveId,
                        Name = "SlaveId",
                        Title = "关联",
                        ColumnName = "slave_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                }
            }
        };

        #endregion
    }
}