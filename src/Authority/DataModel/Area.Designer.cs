/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 17:34:53*/
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
    /// 区域,组织机构的分视图
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class AreaData : IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public AreaData()
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
        public void ChangePrimaryKey(int areaid)
        {
            _areaid = areaid;
        }
        
        /// <summary>
        /// 区域标识:区域标识的实时记录顺序
        /// </summary>
        internal const int Real_AreaId = 0;

        /// <summary>
        /// 区域标识:区域标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _areaid;

        partial void OnAreaIdGet();

        partial void OnAreaIdSet(ref int value);

        partial void OnAreaIdLoad(ref int value);

        partial void OnAreaIdSeted();

        /// <summary>
        /// 区域标识:区域标识
        /// </summary>
        /// <remarks>
        /// 区域标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("AreaId", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"区域标识")]
        public int AreaId
        {
            get
            {
                OnAreaIdGet();
                return this._areaid;
            }
            set
            {
                if(this._areaid == value)
                    return;
                //if(this._areaid > 0)
                //    throw new Exception("主键一旦设置就不可以修改");
                OnAreaIdSet(ref value);
                this._areaid = value;
                this.OnPropertyChanged(Real_AreaId);
                OnAreaIdSeted();
            }
        }
        /// <summary>
        /// 机构类型:类型的实时记录顺序
        /// </summary>
        internal const int Real_Type = 1;

        /// <summary>
        /// 机构类型:类型
        /// </summary>
        [DataMember,JsonIgnore]
        internal OrganizationType _type;

        partial void OnTypeGet();

        partial void OnTypeSet(ref OrganizationType value);

        partial void OnTypeSeted();

        /// <summary>
        /// 机构类型:类型
        /// </summary>
        /// <remarks>
        /// 类型
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"机构类型")]
        public  OrganizationType Type
        {
            get
            {
                OnTypeGet();
                return this._type;
            }
            set
            {
                if(this._type == value)
                    return;
                OnTypeSet(ref value);
                this._type = value;
                OnTypeSeted();
                this.OnPropertyChanged(Real_Type);
            }
        }
        /// <summary>
        /// 机构类型的可读内容
        /// </summary>
        [IgnoreDataMember,JsonIgnore,DisplayName("机构类型")]
        public string Type_Content
        {
            get
            {
                switch(Type)
                {
                case OrganizationType.None:
                    return @"未确定";
                case OrganizationType.Area:
                    return @"行政区域";
                case OrganizationType.Organization:
                    return @"机构";
                case OrganizationType.Department:
                    return @"部门";
                default:
                    return null;
                }
            }
        }
        /// <summary>
        /// 机构类型的数字属性
        /// </summary>
        [IgnoreDataMember,JsonIgnore]
        public  int Type_Number
        {
            get
            {
                return (int)this.Type;
            }
            set
            {
                this.Type = (OrganizationType)value;
            }
        }
        /// <summary>
        /// 编码:代码的实时记录顺序
        /// </summary>
        internal const int Real_Code = 2;

        /// <summary>
        /// 编码:代码
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _code;

        partial void OnCodeGet();

        partial void OnCodeSet(ref string value);

        partial void OnCodeSeted();

        /// <summary>
        /// 编码:代码
        /// </summary>
        /// <remarks>
        /// 代码
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Code", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"编码")]
        public  string Code
        {
            get
            {
                OnCodeGet();
                return this._code;
            }
            set
            {
                if(this._code == value)
                    return;
                OnCodeSet(ref value);
                this._code = value;
                OnCodeSeted();
                this.OnPropertyChanged(Real_Code);
            }
        }
        /// <summary>
        /// 全称:全称的实时记录顺序
        /// </summary>
        internal const int Real_FullName = 3;

        /// <summary>
        /// 全称:全称
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _fullname;

        partial void OnFullNameGet();

        partial void OnFullNameSet(ref string value);

        partial void OnFullNameSeted();

        /// <summary>
        /// 全称:全称
        /// </summary>
        /// <remarks>
        /// 全称
        /// </remarks>
        [IgnoreDataMember , JsonProperty("FullName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"全称")]
        public  string FullName
        {
            get
            {
                OnFullNameGet();
                return this._fullname;
            }
            set
            {
                if(this._fullname == value)
                    return;
                OnFullNameSet(ref value);
                this._fullname = value;
                OnFullNameSeted();
                this.OnPropertyChanged(Real_FullName);
            }
        }
        /// <summary>
        /// 简称:短名称的实时记录顺序
        /// </summary>
        internal const int Real_ShortName = 4;

        /// <summary>
        /// 简称:短名称
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _shortname;

        partial void OnShortNameGet();

        partial void OnShortNameSet(ref string value);

        partial void OnShortNameSeted();

        /// <summary>
        /// 简称:短名称
        /// </summary>
        /// <remarks>
        /// 短名称
        /// </remarks>
        [IgnoreDataMember , JsonProperty("ShortName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"简称")]
        public  string ShortName
        {
            get
            {
                OnShortNameGet();
                return this._shortname;
            }
            set
            {
                if(this._shortname == value)
                    return;
                OnShortNameSet(ref value);
                this._shortname = value;
                OnShortNameSeted();
                this.OnPropertyChanged(Real_ShortName);
            }
        }
        /// <summary>
        /// 树形名称:树的名字的实时记录顺序
        /// </summary>
        internal const int Real_TreeName = 5;

        /// <summary>
        /// 树形名称:树的名字
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _treename;

        partial void OnTreeNameGet();

        partial void OnTreeNameSet(ref string value);

        partial void OnTreeNameSeted();

        /// <summary>
        /// 树形名称:树的名字
        /// </summary>
        /// <remarks>
        /// 树的名字
        /// </remarks>
        [IgnoreDataMember , JsonProperty("TreeName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"树形名称")]
        public  string TreeName
        {
            get
            {
                OnTreeNameGet();
                return this._treename;
            }
            set
            {
                if(this._treename == value)
                    return;
                OnTreeNameSet(ref value);
                this._treename = value;
                OnTreeNameSeted();
                this.OnPropertyChanged(Real_TreeName);
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

        partial void OnOrgLevelGet();

        partial void OnOrgLevelSet(ref int value);

        partial void OnOrgLevelSeted();

        /// <summary>
        /// 级别:级别
        /// </summary>
        /// <remarks>
        /// 级别
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrgLevel", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"级别")]
        public  int OrgLevel
        {
            get
            {
                OnOrgLevelGet();
                return this._orglevel;
            }
            set
            {
                if(this._orglevel == value)
                    return;
                OnOrgLevelSet(ref value);
                this._orglevel = value;
                OnOrgLevelSeted();
                this.OnPropertyChanged(Real_OrgLevel);
            }
        }
        /// <summary>
        /// 上级标识:上级标识的实时记录顺序
        /// </summary>
        internal const int Real_ParentId = 7;

        /// <summary>
        /// 上级标识:上级标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _parentid;

        partial void OnParentIdGet();

        partial void OnParentIdSet(ref int value);

        partial void OnParentIdSeted();

        /// <summary>
        /// 上级标识:上级标识
        /// </summary>
        /// <remarks>
        /// 上级标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("ParentId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"上级标识")]
        public  int ParentId
        {
            get
            {
                OnParentIdGet();
                return this._parentid;
            }
            set
            {
                if(this._parentid == value)
                    return;
                OnParentIdSet(ref value);
                this._parentid = value;
                OnParentIdSeted();
                this.OnPropertyChanged(Real_ParentId);
            }
        }
        /// <summary>
        /// 机构标识:机构标识的实时记录顺序
        /// </summary>
        internal const int Real_OrgId = 8;

        /// <summary>
        /// 机构标识:机构标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _orgid;

        partial void OnOrgIdGet();

        partial void OnOrgIdSet(ref int value);

        partial void OnOrgIdSeted();

        /// <summary>
        /// 机构标识:机构标识
        /// </summary>
        /// <remarks>
        /// 机构标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrgId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"机构标识")]
        public  int OrgId
        {
            get
            {
                OnOrgIdGet();
                return this._orgid;
            }
            set
            {
                if(this._orgid == value)
                    return;
                OnOrgIdSet(ref value);
                this._orgid = value;
                OnOrgIdSeted();
                this.OnPropertyChanged(Real_OrgId);
            }
        }
        /// <summary>
        /// 备注:备忘录的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 9;

        /// <summary>
        /// 备注:备忘录
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _memo;

        partial void OnMemoGet();

        partial void OnMemoSet(ref string value);

        partial void OnMemoSeted();

        /// <summary>
        /// 备注:备忘录
        /// </summary>
        /// <remarks>
        /// 备忘录
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
        internal const int Real_DataState = 10;

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
        internal const int Real_IsFreeze = 11;

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
        internal const int Real_AuthorID = 12;

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
        internal const int Real_AddDate = 13;

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
        internal const int Real_LastReviserID = 14;

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
        internal const int Real_LastModifyDate = 15;

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
        internal const int Real_AuditState = 16;

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
        internal const int Real_AuditorId = 17;

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
        internal const int Real_AuditDate = 18;

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
        /// 对象标识
        /// </summary>
        [IgnoreDataMember,Browsable(false)]
        public int Id
        {
            get
            {
                return this.AreaId;
            }
            set
            {
                this.AreaId = value;
            }
        }

        /// <summary>
        /// Id键
        /// </summary>
        int IIdentityData.Id
        {
            get
            {
                return (int)this.AreaId;
            }
            set
            {
                this.AreaId = value;
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
            case "areaid":
                this.AreaId = (int)Convert.ToDecimal(value);
                return;
            case "type":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.Type = (OrganizationType)(int)value;
                    }
                    else if(value is OrganizationType)
                    {
                        this.Type = (OrganizationType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        OrganizationType val;
                        if (OrganizationType.TryParse(str, out val))
                        {
                            this.Type = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.Type = (OrganizationType)vl;
                            }
                        }
                    }
                }
                return;
            case "code":
                this.Code = value == null ? null : value.ToString();
                return;
            case "fullname":
                this.FullName = value == null ? null : value.ToString();
                return;
            case "shortname":
                this.ShortName = value == null ? null : value.ToString();
                return;
            case "treename":
                this.TreeName = value == null ? null : value.ToString();
                return;
            case "orglevel":
                this.OrgLevel = (int)Convert.ToDecimal(value);
                return;
            case "parentid":
                this.ParentId = (int)Convert.ToDecimal(value);
                return;
            case "orgid":
                this.OrgId = (int)Convert.ToDecimal(value);
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
            case Index_AreaId:
                this.AreaId = Convert.ToInt32(value);
                return;
            case Index_Type:
                this.Type = (OrganizationType)value;
                return;
            case Index_Code:
                this.Code = value == null ? null : value.ToString();
                return;
            case Index_FullName:
                this.FullName = value == null ? null : value.ToString();
                return;
            case Index_ShortName:
                this.ShortName = value == null ? null : value.ToString();
                return;
            case Index_TreeName:
                this.TreeName = value == null ? null : value.ToString();
                return;
            case Index_OrgLevel:
                this.OrgLevel = Convert.ToInt32(value);
                return;
            case Index_ParentId:
                this.ParentId = Convert.ToInt32(value);
                return;
            case Index_OrgId:
                this.OrgId = Convert.ToInt32(value);
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
            case "AreaId":
                return this.AreaId;
            case "Type":
                return this.Type.ToCaption();
            case "Code":
                return this.Code;
            case "FullName":
                return this.FullName;
            case "ShortName":
                return this.ShortName;
            case "TreeName":
                return this.TreeName;
            case "OrgLevel":
                return this.OrgLevel;
            case "ParentId":
                return this.ParentId;
            case "OrgId":
                return this.OrgId;
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
                case Index_AreaId:
                    return this.AreaId;
                case Index_Type:
                    return this.Type;
                case Index_Code:
                    return this.Code;
                case Index_FullName:
                    return this.FullName;
                case Index_ShortName:
                    return this.ShortName;
                case Index_TreeName:
                    return this.TreeName;
                case Index_OrgLevel:
                    return this.OrgLevel;
                case Index_ParentId:
                    return this.ParentId;
                case Index_OrgId:
                    return this.OrgId;
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


        partial void CopyExtendValue(AreaData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as AreaData;
            if(sourceEntity == null)
                return;
            this._areaid = sourceEntity._areaid;
            this._type = sourceEntity._type;
            this._code = sourceEntity._code;
            this._fullname = sourceEntity._fullname;
            this._shortname = sourceEntity._shortname;
            this._treename = sourceEntity._treename;
            this._orglevel = sourceEntity._orglevel;
            this._parentid = sourceEntity._parentid;
            this._orgid = sourceEntity._orgid;
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
        public void Copy(AreaData source)
        {
                this.AreaId = source.AreaId;
                this.Type = source.Type;
                this.Code = source.Code;
                this.FullName = source.FullName;
                this.ShortName = source.ShortName;
                this.TreeName = source.TreeName;
                this.OrgLevel = source.OrgLevel;
                this.ParentId = source.ParentId;
                this.OrgId = source.OrgId;
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
                OnAreaIdModified(subsist,false);
                OnTypeModified(subsist,false);
                OnCodeModified(subsist,false);
                OnFullNameModified(subsist,false);
                OnShortNameModified(subsist,false);
                OnTreeNameModified(subsist,false);
                OnOrgLevelModified(subsist,false);
                OnParentIdModified(subsist,false);
                OnOrgIdModified(subsist,false);
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
                OnAreaIdModified(subsist,true);
                OnTypeModified(subsist,true);
                OnCodeModified(subsist,true);
                OnFullNameModified(subsist,true);
                OnShortNameModified(subsist,true);
                OnTreeNameModified(subsist,true);
                OnOrgLevelModified(subsist,true);
                OnParentIdModified(subsist,true);
                OnOrgIdModified(subsist,true);
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
            else if(modifieds != null && modifieds[19] > 0)
            {
                OnAreaIdModified(subsist,modifieds[Real_AreaId] == 1);
                OnTypeModified(subsist,modifieds[Real_Type] == 1);
                OnCodeModified(subsist,modifieds[Real_Code] == 1);
                OnFullNameModified(subsist,modifieds[Real_FullName] == 1);
                OnShortNameModified(subsist,modifieds[Real_ShortName] == 1);
                OnTreeNameModified(subsist,modifieds[Real_TreeName] == 1);
                OnOrgLevelModified(subsist,modifieds[Real_OrgLevel] == 1);
                OnParentIdModified(subsist,modifieds[Real_ParentId] == 1);
                OnOrgIdModified(subsist,modifieds[Real_OrgId] == 1);
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
        /// 区域标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAreaIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 机构类型修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnTypeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 编码修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnCodeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 全称修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnFullNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 简称修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnShortNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 树形名称修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnTreeNameModified(EntitySubsist subsist,bool isModified);

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
        /// 上级标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnParentIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 机构标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrgIdModified(EntitySubsist subsist,bool isModified);

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

        
        public const byte Index_AreaId = 1;
        public const byte Index_Type = 3;
        public const byte Index_Code = 4;
        public const byte Index_FullName = 5;
        public const byte Index_ShortName = 6;
        public const byte Index_TreeName = 7;
        public const byte Index_OrgLevel = 8;
        public const byte Index_ParentId = 9;
        public const byte Index_OrgId = 10;
        public const byte Index_Memo = 11;
        public const byte Index_DataState = 62;
        public const byte Index_IsFreeze = 63;
        public const byte Index_AuthorID = 65;
        public const byte Index_AddDate = 67;
        public const byte Index_LastReviserID = 69;
        public const byte Index_LastModifyDate = 71;
        public const byte Index_AuditState = 73;
        public const byte Index_AuditorId = 75;
        public const byte Index_AuditDate = 77;

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
            EntityName = "Area",
            PrimaryKey = "AreaId",
            EntityType = 0x5000A,
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_AreaId,
                    new PropertySturct
                    {
                        Index = Index_AreaId,
                        Name = "AreaId",
                        Title = "区域标识",
                        ColumnName = "id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Type,
                    new PropertySturct
                    {
                        Index = Index_Type,
                        Name = "Type",
                        Title = "机构类型",
                        ColumnName = "type",
                        PropertyType = typeof(OrganizationType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Code,
                    new PropertySturct
                    {
                        Index = Index_Code,
                        Name = "Code",
                        Title = "编码",
                        ColumnName = "code",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_FullName,
                    new PropertySturct
                    {
                        Index = Index_FullName,
                        Name = "FullName",
                        Title = "全称",
                        ColumnName = "full_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_ShortName,
                    new PropertySturct
                    {
                        Index = Index_ShortName,
                        Name = "ShortName",
                        Title = "简称",
                        ColumnName = "short_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_TreeName,
                    new PropertySturct
                    {
                        Index = Index_TreeName,
                        Name = "TreeName",
                        Title = "树形名称",
                        ColumnName = "tree_name",
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
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_ParentId,
                    new PropertySturct
                    {
                        Index = Index_ParentId,
                        Name = "ParentId",
                        Title = "上级标识",
                        ColumnName = "parent_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_OrgId,
                    new PropertySturct
                    {
                        Index = Index_OrgId,
                        Name = "OrgId",
                        Title = "机构标识",
                        ColumnName = "org_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
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
                        CanNull = false,
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