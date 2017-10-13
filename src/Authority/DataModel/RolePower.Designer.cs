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
    /// 角色权限
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class RolePowerData : IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public RolePowerData()
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
        internal const int Real_ID = 0;

        /// <summary>
        /// 标识:标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _id;

        partial void OnIDGet();

        partial void OnIDSet(ref int value);

        partial void OnIDLoad(ref int value);

        partial void OnIDSeted();

        /// <summary>
        /// 标识:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"标识")]
        public int ID
        {
            get
            {
                OnIDGet();
                return this._id;
            }
            set
            {
                if(this._id == value)
                    return;
                //if(this._id > 0)
                //    throw new Exception("主键一旦设置就不可以修改");
                OnIDSet(ref value);
                this._id = value;
                this.OnPropertyChanged(Real_ID);
                OnIDSeted();
            }
        }
        /// <summary>
        /// 页面标识:页面标识的实时记录顺序
        /// </summary>
        internal const int Real_PageItemId = 1;

        /// <summary>
        /// 页面标识:页面标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _pageitemid;

        partial void OnPageItemIdGet();

        partial void OnPageItemIdSet(ref int value);

        partial void OnPageItemIdSeted();

        /// <summary>
        /// 页面标识:页面标识
        /// </summary>
        /// <remarks>
        /// 页面标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("PageItemId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"页面标识")]
        public  int PageItemId
        {
            get
            {
                OnPageItemIdGet();
                return this._pageitemid;
            }
            set
            {
                if(this._pageitemid == value)
                    return;
                OnPageItemIdSet(ref value);
                this._pageitemid = value;
                OnPageItemIdSeted();
                this.OnPropertyChanged(Real_PageItemId);
            }
        }
        /// <summary>
        /// 角色标识:角色标识的实时记录顺序
        /// </summary>
        internal const int Real_RoleId = 2;

        /// <summary>
        /// 角色标识:角色标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _roleid;

        partial void OnRoleIdGet();

        partial void OnRoleIdSet(ref int value);

        partial void OnRoleIdSeted();

        /// <summary>
        /// 角色标识:角色标识
        /// </summary>
        /// <remarks>
        /// 角色标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("RoleId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"角色标识")]
        public  int RoleId
        {
            get
            {
                OnRoleIdGet();
                return this._roleid;
            }
            set
            {
                if(this._roleid == value)
                    return;
                OnRoleIdSet(ref value);
                this._roleid = value;
                OnRoleIdSeted();
                this.OnPropertyChanged(Real_RoleId);
            }
        }
        /// <summary>
        /// 权限:权限的实时记录顺序
        /// </summary>
        internal const int Real_Power = 3;

        /// <summary>
        /// 权限:权限
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _power;

        partial void OnPowerGet();

        partial void OnPowerSet(ref int value);

        partial void OnPowerSeted();

        /// <summary>
        /// 权限:权限
        /// </summary>
        /// <remarks>
        /// 权限
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Power", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"权限")]
        public  int Power
        {
            get
            {
                OnPowerGet();
                return this._power;
            }
            set
            {
                if(this._power == value)
                    return;
                OnPowerSet(ref value);
                this._power = value;
                OnPowerSeted();
                this.OnPropertyChanged(Real_Power);
            }
        }
        /// <summary>
        /// 权限范围:权限范围的实时记录顺序
        /// </summary>
        internal const int Real_DataScope = 4;

        /// <summary>
        /// 权限范围:权限范围
        /// </summary>
        [DataMember,JsonIgnore]
        internal PositionDataScopeType _datascope;

        partial void OnDataScopeGet();

        partial void OnDataScopeSet(ref PositionDataScopeType value);

        partial void OnDataScopeSeted();

        /// <summary>
        /// 权限范围:权限范围
        /// </summary>
        /// <remarks>
        /// 权限范围
        /// </remarks>
        [IgnoreDataMember , JsonProperty("DataScope", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"权限范围")]
        public  PositionDataScopeType DataScope
        {
            get
            {
                OnDataScopeGet();
                return this._datascope;
            }
            set
            {
                if(this._datascope == value)
                    return;
                OnDataScopeSet(ref value);
                this._datascope = value;
                OnDataScopeSeted();
                this.OnPropertyChanged(Real_DataScope);
            }
        }
        #endregion

        #region IIdentityData接口


        /// <summary>
        /// 对象标识
        /// </summary>
        [IgnoreDataMember,Browsable(false)]
        public int Id
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }

        /// <summary>
        /// Id键
        /// </summary>
        int IIdentityData.Id
        {
            get
            {
                return (int)this.ID;
            }
            set
            {
                this.ID = value;
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
                this.ID = (int)Convert.ToDecimal(value);
                return;
            case "pageitemid":
                this.PageItemId = (int)Convert.ToDecimal(value);
                return;
            case "roleid":
                this.RoleId = (int)Convert.ToDecimal(value);
                return;
            case "power":
                this.Power = (int)Convert.ToDecimal(value);
                return;
            case "datascope":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.DataScope = (PositionDataScopeType)(int)value;
                    }
                    else if(value is PositionDataScopeType)
                    {
                        this.DataScope = (PositionDataScopeType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        PositionDataScopeType val;
                        if (PositionDataScopeType.TryParse(str, out val))
                        {
                            this.DataScope = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.DataScope = (PositionDataScopeType)vl;
                            }
                        }
                    }
                }
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
            case Index_ID:
                this.ID = Convert.ToInt32(value);
                return;
            case Index_PageItemId:
                this.PageItemId = Convert.ToInt32(value);
                return;
            case Index_RoleId:
                this.RoleId = Convert.ToInt32(value);
                return;
            case Index_Power:
                this.Power = Convert.ToInt32(value);
                return;
            case Index_DataScope:
                this.DataScope = (PositionDataScopeType)value;
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
            case "ID":
                return this.ID;
            case "PageItemId":
                return this.PageItemId;
            case "RoleId":
                return this.RoleId;
            case "Power":
                return this.Power;
            case "DataScope":
                return this.DataScope;
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
                case Index_ID:
                    return this.ID;
                case Index_PageItemId:
                    return this.PageItemId;
                case Index_RoleId:
                    return this.RoleId;
                case Index_Power:
                    return this.Power;
                case Index_DataScope:
                    return this.DataScope;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(RolePowerData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as RolePowerData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._pageitemid = sourceEntity._pageitemid;
            this._roleid = sourceEntity._roleid;
            this._power = sourceEntity._power;
            this._datascope = sourceEntity._datascope;
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(RolePowerData source)
        {
                this.ID = source.ID;
                this.PageItemId = source.PageItemId;
                this.RoleId = source.RoleId;
                this.Power = source.Power;
                this.DataScope = source.DataScope;
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
                OnIDModified(subsist,false);
                OnPageItemIdModified(subsist,false);
                OnRoleIdModified(subsist,false);
                OnPowerModified(subsist,false);
                OnDataScopeModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIDModified(subsist,true);
                OnPageItemIdModified(subsist,true);
                OnRoleIdModified(subsist,true);
                OnPowerModified(subsist,true);
                OnDataScopeModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[5] > 0)
            {
                OnIDModified(subsist,modifieds[Real_ID] == 1);
                OnPageItemIdModified(subsist,modifieds[Real_PageItemId] == 1);
                OnRoleIdModified(subsist,modifieds[Real_RoleId] == 1);
                OnPowerModified(subsist,modifieds[Real_Power] == 1);
                OnDataScopeModified(subsist,modifieds[Real_DataScope] == 1);
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
        partial void OnIDModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 页面标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPageItemIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 角色标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnRoleIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 权限修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPowerModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 权限范围修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDataScopeModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion

        #region 实体结构

        
        public const byte Index_ID = 1;
        public const byte Index_PageItemId = 3;
        public const byte Index_RoleId = 4;
        public const byte Index_Power = 5;
        public const byte Index_DataScope = 6;

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
            EntityName = "RolePower",
            PrimaryKey = "ID",
            EntityType = 0x50007,
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_ID,
                    new PropertySturct
                    {
                        Index = Index_ID,
                        Name = "ID",
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
                    Real_PageItemId,
                    new PropertySturct
                    {
                        Index = Index_PageItemId,
                        Name = "PageItemId",
                        Title = "页面标识",
                        ColumnName = "page_item_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_RoleId,
                    new PropertySturct
                    {
                        Index = Index_RoleId,
                        Name = "RoleId",
                        Title = "角色标识",
                        ColumnName = "role_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Power,
                    new PropertySturct
                    {
                        Index = Index_Power,
                        Name = "Power",
                        Title = "权限",
                        ColumnName = "power",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_DataScope,
                    new PropertySturct
                    {
                        Index = Index_DataScope,
                        Name = "DataScope",
                        Title = "权限范围",
                        ColumnName = "data_scope",
                        PropertyType = typeof(PositionDataScopeType),
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