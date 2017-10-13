/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/9/16 22:23:56*/
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
    /// 机构职位设置
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class OrganizePositionData : IStateData , IHistoryData , IAuditData , IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public OrganizePositionData()
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
        /// 职位:称谓的实时记录顺序
        /// </summary>
        internal const int Real_Position = 1;

        /// <summary>
        /// 职位:称谓
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _position;

        partial void OnPositionGet();

        partial void OnPositionSet(ref string value);

        partial void OnPositionSeted();

        /// <summary>
        /// 职位:称谓
        /// </summary>
        /// <remarks>
        /// 称谓
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Position", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"职位")]
        public  string Position
        {
            get
            {
                OnPositionGet();
                return this._position;
            }
            set
            {
                if(this._position == value)
                    return;
                OnPositionSet(ref value);
                this._position = value;
                OnPositionSeted();
                this.OnPropertyChanged(Real_Position);
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
        /// 角色:角色的实时记录顺序
        /// </summary>
        internal const int Real_Role = 3;

        /// <summary>
        /// 角色:角色
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _role;

        /// <summary>
        /// 角色:角色
        /// </summary>
        /// <remarks>
        /// 角色
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Role", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"角色")]
        public  string Role
        {
            get
            {
                return this._role;
            }
            set
            {
                this._role = value;
            }
        }
        /// <summary>
        /// 部门标识:部门标识的实时记录顺序
        /// </summary>
        internal const int Real_OrganizationId = 4;

        /// <summary>
        /// 部门标识:部门标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _organizationid;

        partial void OnOrganizationIdGet();

        partial void OnOrganizationIdSet(ref int value);

        partial void OnOrganizationIdSeted();

        /// <summary>
        /// 部门标识:部门标识
        /// </summary>
        /// <remarks>
        /// 部门标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrganizationId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"部门标识")]
        public  int OrganizationId
        {
            get
            {
                OnOrganizationIdGet();
                return this._organizationid;
            }
            set
            {
                if(this._organizationid == value)
                    return;
                OnOrganizationIdSet(ref value);
                this._organizationid = value;
                OnOrganizationIdSeted();
                this.OnPropertyChanged(Real_OrganizationId);
            }
        }
        /// <summary>
        /// 部门:部门的实时记录顺序
        /// </summary>
        internal const int Real_Department = 5;

        /// <summary>
        /// 部门:部门
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _department;

        /// <summary>
        /// 部门:部门
        /// </summary>
        /// <remarks>
        /// 部门
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Department", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"部门")]
        public  string Department
        {
            get
            {
                return this._department;
            }
            set
            {
                this._department = value;
            }
        }
        /// <summary>
        /// 级别:级别的实时记录顺序
        /// </summary>
        internal const int Real_OrgLevel = 6;

        /// <summary>
        /// 级别:级别
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _orglevel;

        /// <summary>
        /// 级别:级别
        /// </summary>
        /// <remarks>
        /// 级别
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrgLevel", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"级别")]
        public  int OrgLevel
        {
            get
            {
                return this._orglevel;
            }
            set
            {
                this._orglevel = value;
            }
        }
        /// <summary>
        /// 备注:备注的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 7;

        /// <summary>
        /// 备注:备注
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _memo;

        partial void OnMemoGet();

        partial void OnMemoSet(ref string value);

        partial void OnMemoSeted();

        /// <summary>
        /// 备注:备注
        /// </summary>
        /// <remarks>
        /// 备注
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Memo", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"备注")]
        public  string Memo
        {
            get
            {
                OnMemoGet();
                return this._memo;
            }
            set
            {
                if(this._memo == value)
                    return;
                OnMemoSet(ref value);
                this._memo = value;
                OnMemoSeted();
                this.OnPropertyChanged(Real_Memo);
            }
        }
        /// <summary>
        /// 数据状态:数据状态的实时记录顺序
        /// </summary>
        internal const int Real_DataState = 8;

        /// <summary>
        /// 数据状态:数据状态
        /// </summary>
        [DataMember,JsonIgnore]
        internal DataStateType _datastate;

        partial void OnDataStateGet();

        partial void OnDataStateSet(ref DataStateType value);

        partial void OnDataStateSeted();

        /// <summary>
        /// 数据状态:数据状态
        /// </summary>
        /// <remarks>
        /// 数据状态
        /// </remarks>
        [IgnoreDataMember , JsonProperty("DataState", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"数据状态")]
        public  DataStateType DataState
        {
            get
            {
                OnDataStateGet();
                return this._datastate;
            }
            set
            {
                if(this._datastate == value)
                    return;
                OnDataStateSet(ref value);
                this._datastate = value;
                OnDataStateSeted();
                this.OnPropertyChanged(Real_DataState);
            }
        }
        /// <summary>
        /// 数据是否已冻结:数据是否已冻结的实时记录顺序
        /// </summary>
        internal const int Real_IsFreeze = 9;

        /// <summary>
        /// 数据是否已冻结:数据是否已冻结
        /// </summary>
        [DataMember,JsonIgnore]
        internal bool _isfreeze;

        partial void OnIsFreezeGet();

        partial void OnIsFreezeSet(ref bool value);

        partial void OnIsFreezeSeted();

        /// <summary>
        /// 数据是否已冻结:数据是否已冻结
        /// </summary>
        /// <remarks>
        /// 数据是否已冻结
        /// </remarks>
        [IgnoreDataMember , JsonProperty("IsFreeze", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"数据是否已冻结")]
        public  bool IsFreeze
        {
            get
            {
                OnIsFreezeGet();
                return this._isfreeze;
            }
            set
            {
                if(this._isfreeze == value)
                    return;
                OnIsFreezeSet(ref value);
                this._isfreeze = value;
                OnIsFreezeSeted();
                this.OnPropertyChanged(Real_IsFreeze);
            }
        }
        /// <summary>
        /// 制作人:制作人的实时记录顺序
        /// </summary>
        internal const int Real_AuthorID = 10;

        /// <summary>
        /// 制作人:制作人
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _authorid;

        partial void OnAuthorIDGet();

        partial void OnAuthorIDSet(ref int value);

        partial void OnAuthorIDSeted();

        /// <summary>
        /// 制作人:制作人
        /// </summary>
        /// <remarks>
        /// 制作人
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AuthorID", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"制作人")]
        public  int AuthorID
        {
            get
            {
                OnAuthorIDGet();
                return this._authorid;
            }
            set
            {
                if(this._authorid == value)
                    return;
                OnAuthorIDSet(ref value);
                this._authorid = value;
                OnAuthorIDSeted();
                this.OnPropertyChanged(Real_AuthorID);
            }
        }
        /// <summary>
        /// 制作时间:制作时间的实时记录顺序
        /// </summary>
        internal const int Real_AddDate = 11;

        /// <summary>
        /// 制作时间:制作时间
        /// </summary>
        [DataMember,JsonIgnore]
        internal DateTime _adddate;

        partial void OnAddDateGet();

        partial void OnAddDateSet(ref DateTime value);

        partial void OnAddDateSeted();

        /// <summary>
        /// 制作时间:制作时间
        /// </summary>
        /// <remarks>
        /// 制作时间
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AddDate", NullValueHandling = NullValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"制作时间")]
        public  DateTime AddDate
        {
            get
            {
                OnAddDateGet();
                return this._adddate;
            }
            set
            {
                if(this._adddate == value)
                    return;
                OnAddDateSet(ref value);
                this._adddate = value;
                OnAddDateSeted();
                this.OnPropertyChanged(Real_AddDate);
            }
        }
        /// <summary>
        /// 最后修改者:最后修改者的实时记录顺序
        /// </summary>
        internal const int Real_LastReviserID = 12;

        /// <summary>
        /// 最后修改者:最后修改者
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _lastreviserid;

        partial void OnLastReviserIDGet();

        partial void OnLastReviserIDSet(ref int value);

        partial void OnLastReviserIDSeted();

        /// <summary>
        /// 最后修改者:最后修改者
        /// </summary>
        /// <remarks>
        /// 最后修改者
        /// </remarks>
        [IgnoreDataMember , JsonProperty("LastReviserID", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"最后修改者")]
        public  int LastReviserID
        {
            get
            {
                OnLastReviserIDGet();
                return this._lastreviserid;
            }
            set
            {
                if(this._lastreviserid == value)
                    return;
                OnLastReviserIDSet(ref value);
                this._lastreviserid = value;
                OnLastReviserIDSeted();
                this.OnPropertyChanged(Real_LastReviserID);
            }
        }
        /// <summary>
        /// 最后修改日期:最后修改日期的实时记录顺序
        /// </summary>
        internal const int Real_LastModifyDate = 13;

        /// <summary>
        /// 最后修改日期:最后修改日期
        /// </summary>
        [DataMember,JsonIgnore]
        internal DateTime _lastmodifydate;

        partial void OnLastModifyDateGet();

        partial void OnLastModifyDateSet(ref DateTime value);

        partial void OnLastModifyDateSeted();

        /// <summary>
        /// 最后修改日期:最后修改日期
        /// </summary>
        /// <remarks>
        /// 最后修改日期
        /// </remarks>
        [IgnoreDataMember , JsonProperty("LastModifyDate", NullValueHandling = NullValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"最后修改日期")]
        public  DateTime LastModifyDate
        {
            get
            {
                OnLastModifyDateGet();
                return this._lastmodifydate;
            }
            set
            {
                if(this._lastmodifydate == value)
                    return;
                OnLastModifyDateSet(ref value);
                this._lastmodifydate = value;
                OnLastModifyDateSeted();
                this.OnPropertyChanged(Real_LastModifyDate);
            }
        }
        /// <summary>
        /// 审核状态:审核状态的实时记录顺序
        /// </summary>
        internal const int Real_AuditState = 14;

        /// <summary>
        /// 审核状态:审核状态
        /// </summary>
        [DataMember,JsonIgnore]
        internal AuditStateType _auditstate;

        partial void OnAuditStateGet();

        partial void OnAuditStateSet(ref AuditStateType value);

        partial void OnAuditStateSeted();

        /// <summary>
        /// 审核状态:审核状态
        /// </summary>
        /// <remarks>
        /// 审核状态
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AuditState", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"审核状态")]
        public  AuditStateType AuditState
        {
            get
            {
                OnAuditStateGet();
                return this._auditstate;
            }
            set
            {
                if(this._auditstate == value)
                    return;
                OnAuditStateSet(ref value);
                this._auditstate = value;
                OnAuditStateSeted();
                this.OnPropertyChanged(Real_AuditState);
            }
        }
        /// <summary>
        /// 审核人:审核人的实时记录顺序
        /// </summary>
        internal const int Real_AuditorId = 15;

        /// <summary>
        /// 审核人:审核人
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _auditorid;

        partial void OnAuditorIdGet();

        partial void OnAuditorIdSet(ref int value);

        partial void OnAuditorIdSeted();

        /// <summary>
        /// 审核人:审核人
        /// </summary>
        /// <remarks>
        /// 审核人
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AuditorId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"审核人")]
        public  int AuditorId
        {
            get
            {
                OnAuditorIdGet();
                return this._auditorid;
            }
            set
            {
                if(this._auditorid == value)
                    return;
                OnAuditorIdSet(ref value);
                this._auditorid = value;
                OnAuditorIdSeted();
                this.OnPropertyChanged(Real_AuditorId);
            }
        }
        /// <summary>
        /// 审核时间:审核时间的实时记录顺序
        /// </summary>
        internal const int Real_AuditDate = 16;

        /// <summary>
        /// 审核时间:审核时间
        /// </summary>
        [DataMember,JsonIgnore]
        internal DateTime _auditdate;

        partial void OnAuditDateGet();

        partial void OnAuditDateSet(ref DateTime value);

        partial void OnAuditDateSeted();

        /// <summary>
        /// 审核时间:审核时间
        /// </summary>
        /// <remarks>
        /// 审核时间
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AuditDate", NullValueHandling = NullValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"审核时间")]
        public  DateTime AuditDate
        {
            get
            {
                OnAuditDateGet();
                return this._auditdate;
            }
            set
            {
                if(this._auditdate == value)
                    return;
                OnAuditDateSet(ref value);
                this._auditdate = value;
                OnAuditDateSeted();
                this.OnPropertyChanged(Real_AuditDate);
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
            case "position":
                this.Position = value == null ? null : value.ToString();
                return;
            case "roleid":
                this.RoleId = (int)Convert.ToDecimal(value);
                return;
            case "role":
                this.Role = value == null ? null : value.ToString();
                return;
            case "organizationid":
                this.OrganizationId = (int)Convert.ToDecimal(value);
                return;
            case "department":
                this.Department = value == null ? null : value.ToString();
                return;
            case "orglevel":
                this.OrgLevel = (int)Convert.ToDecimal(value);
                return;
            case "memo":
                this.Memo = value == null ? null : value.ToString();
                return;
            case "datastate":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.DataState = (DataStateType)(int)value;
                    }
                    else if(value is DataStateType)
                    {
                        this.DataState = (DataStateType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        DataStateType val;
                        if (DataStateType.TryParse(str, out val))
                        {
                            this.DataState = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.DataState = (DataStateType)vl;
                            }
                        }
                    }
                }
                return;
            case "isfreeze":
                if (value != null)
                {
                    int vl;
                    if (int.TryParse(value.ToString(), out vl))
                    {
                        this.IsFreeze = vl != 0;
                    }
                    else
                    {
                        this.IsFreeze = Convert.ToBoolean(value);
                    }
                }
                return;
            case "authorid":
                this.AuthorID = (int)Convert.ToDecimal(value);
                return;
            case "adddate":
                this.AddDate = Convert.ToDateTime(value);
                return;
            case "lastreviserid":
                this.LastReviserID = (int)Convert.ToDecimal(value);
                return;
            case "lastmodifydate":
                this.LastModifyDate = Convert.ToDateTime(value);
                return;
            case "auditstate":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.AuditState = (AuditStateType)(int)value;
                    }
                    else if(value is AuditStateType)
                    {
                        this.AuditState = (AuditStateType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        AuditStateType val;
                        if (AuditStateType.TryParse(str, out val))
                        {
                            this.AuditState = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.AuditState = (AuditStateType)vl;
                            }
                        }
                    }
                }
                return;
            case "auditorid":
                this.AuditorId = (int)Convert.ToDecimal(value);
                return;
            case "auditdate":
                this.AuditDate = Convert.ToDateTime(value);
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
            case Index_Position:
                this.Position = value == null ? null : value.ToString();
                return;
            case Index_RoleId:
                this.RoleId = Convert.ToInt32(value);
                return;
            case Index_Role:
                this.Role = value == null ? null : value.ToString();
                return;
            case Index_OrganizationId:
                this.OrganizationId = Convert.ToInt32(value);
                return;
            case Index_Department:
                this.Department = value == null ? null : value.ToString();
                return;
            case Index_OrgLevel:
                this.OrgLevel = Convert.ToInt32(value);
                return;
            case Index_Memo:
                this.Memo = value == null ? null : value.ToString();
                return;
            case Index_DataState:
                this.DataState = (DataStateType)value;
                return;
            case Index_IsFreeze:
                this.IsFreeze = Convert.ToBoolean(value);
                return;
            case Index_AuthorID:
                this.AuthorID = Convert.ToInt32(value);
                return;
            case Index_AddDate:
                this.AddDate = Convert.ToDateTime(value);
                return;
            case Index_LastReviserID:
                this.LastReviserID = Convert.ToInt32(value);
                return;
            case Index_LastModifyDate:
                this.LastModifyDate = Convert.ToDateTime(value);
                return;
            case Index_AuditState:
                this.AuditState = (AuditStateType)value;
                return;
            case Index_AuditorId:
                this.AuditorId = Convert.ToInt32(value);
                return;
            case Index_AuditDate:
                this.AuditDate = Convert.ToDateTime(value);
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
            case "Position":
                return this.Position;
            case "RoleId":
                return this.RoleId;
            case "Role":
                return this.Role;
            case "OrganizationId":
                return this.OrganizationId;
            case "Department":
                return this.Department;
            case "OrgLevel":
                return this.OrgLevel;
            case "Memo":
                return this.Memo;
            case "DataState":
                return this.DataState;
            case "IsFreeze":
                return this.IsFreeze;
            case "AuthorID":
                return this.AuthorID;
            case "AddDate":
                return this.AddDate;
            case "LastReviserID":
                return this.LastReviserID;
            case "LastModifyDate":
                return this.LastModifyDate;
            case "AuditState":
                return this.AuditState;
            case "AuditorId":
                return this.AuditorId;
            case "AuditDate":
                return this.AuditDate;
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
                case Index_Position:
                    return this.Position;
                case Index_RoleId:
                    return this.RoleId;
                case Index_Role:
                    return this.Role;
                case Index_OrganizationId:
                    return this.OrganizationId;
                case Index_Department:
                    return this.Department;
                case Index_OrgLevel:
                    return this.OrgLevel;
                case Index_Memo:
                    return this.Memo;
                case Index_DataState:
                    return this.DataState;
                case Index_IsFreeze:
                    return this.IsFreeze;
                case Index_AuthorID:
                    return this.AuthorID;
                case Index_AddDate:
                    return this.AddDate;
                case Index_LastReviserID:
                    return this.LastReviserID;
                case Index_LastModifyDate:
                    return this.LastModifyDate;
                case Index_AuditState:
                    return this.AuditState;
                case Index_AuditorId:
                    return this.AuditorId;
                case Index_AuditDate:
                    return this.AuditDate;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(OrganizePositionData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as OrganizePositionData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._position = sourceEntity._position;
            this._roleid = sourceEntity._roleid;
            this._role = sourceEntity._role;
            this._organizationid = sourceEntity._organizationid;
            this._department = sourceEntity._department;
            this._orglevel = sourceEntity._orglevel;
            this._memo = sourceEntity._memo;
            this._datastate = sourceEntity._datastate;
            this._isfreeze = sourceEntity._isfreeze;
            this._authorid = sourceEntity._authorid;
            this._adddate = sourceEntity._adddate;
            this._lastreviserid = sourceEntity._lastreviserid;
            this._lastmodifydate = sourceEntity._lastmodifydate;
            this._auditstate = sourceEntity._auditstate;
            this._auditorid = sourceEntity._auditorid;
            this._auditdate = sourceEntity._auditdate;
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(OrganizePositionData source)
        {
                this.Id = source.Id;
                this.Position = source.Position;
                this.RoleId = source.RoleId;
                this.Role = source.Role;
                this.OrganizationId = source.OrganizationId;
                this.Department = source.Department;
                this.OrgLevel = source.OrgLevel;
                this.Memo = source.Memo;
                this.DataState = source.DataState;
                this.IsFreeze = source.IsFreeze;
                this.AuthorID = source.AuthorID;
                this.AddDate = source.AddDate;
                this.LastReviserID = source.LastReviserID;
                this.LastModifyDate = source.LastModifyDate;
                this.AuditState = source.AuditState;
                this.AuditorId = source.AuditorId;
                this.AuditDate = source.AuditDate;
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
                OnPositionModified(subsist,false);
                OnRoleIdModified(subsist,false);
                OnRoleModified(subsist,false);
                OnOrganizationIdModified(subsist,false);
                OnDepartmentModified(subsist,false);
                OnOrgLevelModified(subsist,false);
                OnMemoModified(subsist,false);
                OnDataStateModified(subsist,false);
                OnIsFreezeModified(subsist,false);
                OnAuthorIDModified(subsist,false);
                OnAddDateModified(subsist,false);
                OnLastReviserIDModified(subsist,false);
                OnLastModifyDateModified(subsist,false);
                OnAuditStateModified(subsist,false);
                OnAuditorIdModified(subsist,false);
                OnAuditDateModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIdModified(subsist,true);
                OnPositionModified(subsist,true);
                OnRoleIdModified(subsist,true);
                OnRoleModified(subsist,true);
                OnOrganizationIdModified(subsist,true);
                OnDepartmentModified(subsist,true);
                OnOrgLevelModified(subsist,true);
                OnMemoModified(subsist,true);
                OnDataStateModified(subsist,true);
                OnIsFreezeModified(subsist,true);
                OnAuthorIDModified(subsist,true);
                OnAddDateModified(subsist,true);
                OnLastReviserIDModified(subsist,true);
                OnLastModifyDateModified(subsist,true);
                OnAuditStateModified(subsist,true);
                OnAuditorIdModified(subsist,true);
                OnAuditDateModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[17] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnPositionModified(subsist,modifieds[Real_Position] == 1);
                OnRoleIdModified(subsist,modifieds[Real_RoleId] == 1);
                OnRoleModified(subsist,modifieds[Real_Role] == 1);
                OnOrganizationIdModified(subsist,modifieds[Real_OrganizationId] == 1);
                OnDepartmentModified(subsist,modifieds[Real_Department] == 1);
                OnOrgLevelModified(subsist,modifieds[Real_OrgLevel] == 1);
                OnMemoModified(subsist,modifieds[Real_Memo] == 1);
                OnDataStateModified(subsist,modifieds[Real_DataState] == 1);
                OnIsFreezeModified(subsist,modifieds[Real_IsFreeze] == 1);
                OnAuthorIDModified(subsist,modifieds[Real_AuthorID] == 1);
                OnAddDateModified(subsist,modifieds[Real_AddDate] == 1);
                OnLastReviserIDModified(subsist,modifieds[Real_LastReviserID] == 1);
                OnLastModifyDateModified(subsist,modifieds[Real_LastModifyDate] == 1);
                OnAuditStateModified(subsist,modifieds[Real_AuditState] == 1);
                OnAuditorIdModified(subsist,modifieds[Real_AuditorId] == 1);
                OnAuditDateModified(subsist,modifieds[Real_AuditDate] == 1);
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
        /// 职位修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPositionModified(EntitySubsist subsist,bool isModified);

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
        /// 角色修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnRoleModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 部门标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrganizationIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 部门修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDepartmentModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 级别修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrgLevelModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 备注修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMemoModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 数据状态修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDataStateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 数据是否已冻结修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnIsFreezeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 制作人修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAuthorIDModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 制作时间修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAddDateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 最后修改者修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnLastReviserIDModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 最后修改日期修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnLastModifyDateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 审核状态修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAuditStateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 审核人修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAuditorIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 审核时间修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAuditDateModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion

        #region 实体结构

        
        public const byte Index_Id = 1;
        public const byte Index_Position = 2;
        public const byte Index_RoleId = 3;
        public const byte Index_Role = 4;
        public const byte Index_OrganizationId = 5;
        public const byte Index_Department = 6;
        public const byte Index_OrgLevel = 7;
        public const byte Index_Memo = 8;
        public const byte Index_DataState = 9;
        public const byte Index_IsFreeze = 10;
        public const byte Index_AuthorID = 11;
        public const byte Index_AddDate = 12;
        public const byte Index_LastReviserID = 13;
        public const byte Index_LastModifyDate = 14;
        public const byte Index_AuditState = 15;
        public const byte Index_AuditorId = 16;
        public const byte Index_AuditDate = 17;

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
            EntityName = "OrganizePosition",
            PrimaryKey = "Id",
            EntityType = 0x50004,
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
                    Real_Position,
                    new PropertySturct
                    {
                        Index = Index_Position,
                        Name = "Position",
                        Title = "职位",
                        ColumnName = "appellation",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
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
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_Role,
                    new PropertySturct
                    {
                        Index = Index_Role,
                        Name = "Role",
                        Title = "角色",
                        ColumnName = "role",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_OrganizationId,
                    new PropertySturct
                    {
                        Index = Index_OrganizationId,
                        Name = "OrganizationId",
                        Title = "部门标识",
                        ColumnName = "organization_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_Department,
                    new PropertySturct
                    {
                        Index = Index_Department,
                        Name = "Department",
                        Title = "部门",
                        ColumnName = "organization",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_OrgLevel,
                    new PropertySturct
                    {
                        Index = Index_OrgLevel,
                        Name = "OrgLevel",
                        Title = "级别",
                        ColumnName = "org_level",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_Memo,
                    new PropertySturct
                    {
                        Index = Index_Memo,
                        Name = "Memo",
                        Title = "备注",
                        ColumnName = "memo",
                        PropertyType = typeof(string),
                        CanNull = true,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_DataState,
                    new PropertySturct
                    {
                        Index = Index_DataState,
                        Name = "DataState",
                        Title = "数据状态",
                        ColumnName = "data_state",
                        PropertyType = typeof(DataStateType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_IsFreeze,
                    new PropertySturct
                    {
                        Index = Index_IsFreeze,
                        Name = "IsFreeze",
                        Title = "数据是否已冻结",
                        ColumnName = "is_freeze",
                        PropertyType = typeof(bool),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_AuthorID,
                    new PropertySturct
                    {
                        Index = Index_AuthorID,
                        Name = "AuthorID",
                        Title = "制作人",
                        ColumnName = "author_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_AddDate,
                    new PropertySturct
                    {
                        Index = Index_AddDate,
                        Name = "AddDate",
                        Title = "制作时间",
                        ColumnName = "add_date",
                        PropertyType = typeof(DateTime),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_LastReviserID,
                    new PropertySturct
                    {
                        Index = Index_LastReviserID,
                        Name = "LastReviserID",
                        Title = "最后修改者",
                        ColumnName = "last_reviser_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_LastModifyDate,
                    new PropertySturct
                    {
                        Index = Index_LastModifyDate,
                        Name = "LastModifyDate",
                        Title = "最后修改日期",
                        ColumnName = "last_modify_date",
                        PropertyType = typeof(DateTime),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_AuditState,
                    new PropertySturct
                    {
                        Index = Index_AuditState,
                        Name = "AuditState",
                        Title = "审核状态",
                        ColumnName = "audit_state",
                        PropertyType = typeof(AuditStateType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_AuditorId,
                    new PropertySturct
                    {
                        Index = Index_AuditorId,
                        Name = "AuditorId",
                        Title = "审核人",
                        ColumnName = "auditor_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_AuditDate,
                    new PropertySturct
                    {
                        Index = Index_AuditDate,
                        Name = "AuditDate",
                        Title = "审核时间",
                        ColumnName = "audit_date",
                        PropertyType = typeof(DateTime),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                }
            }
        };

        #endregion
    }
}