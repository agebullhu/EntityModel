/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/9/16 22:24:35*/
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
    /// 人员职位设置
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PositionPersonnelData : IStateData , IHistoryData , IAuditData , IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public PositionPersonnelData()
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
        /// 员工标识:员工标识的实时记录顺序
        /// </summary>
        internal const int Real_PersonnelId = 1;

        /// <summary>
        /// 员工标识:员工标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _personnelid;

        partial void OnPersonnelIdGet();

        partial void OnPersonnelIdSet(ref int value);

        partial void OnPersonnelIdSeted();

        /// <summary>
        /// 员工标识:员工标识
        /// </summary>
        /// <remarks>
        /// 员工标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("PersonnelId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"员工标识")]
        public  int PersonnelId
        {
            get
            {
                OnPersonnelIdGet();
                return this._personnelid;
            }
            set
            {
                if(this._personnelid == value)
                    return;
                OnPersonnelIdSet(ref value);
                this._personnelid = value;
                OnPersonnelIdSeted();
                this.OnPropertyChanged(Real_PersonnelId);
            }
        }
        /// <summary>
        /// 职员:姓名的实时记录顺序
        /// </summary>
        internal const int Real_Personnel = 2;

        /// <summary>
        /// 职员:姓名
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _personnel;

        /// <summary>
        /// 职员:姓名
        /// </summary>
        /// <remarks>
        /// 姓名
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Personnel", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"职员")]
        public  string Personnel
        {
            get
            {
                return this._personnel;
            }
            set
            {
                this._personnel = value;
            }
        }
        /// <summary>
        /// 称谓:称谓的实时记录顺序
        /// </summary>
        internal const int Real_Appellation = 3;

        /// <summary>
        /// 称谓:称谓
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _appellation;

        partial void OnAppellationGet();

        partial void OnAppellationSet(ref string value);

        partial void OnAppellationSeted();

        /// <summary>
        /// 称谓:称谓
        /// </summary>
        /// <remarks>
        /// 称谓
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Appellation", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"称谓")]
        public  string Appellation
        {
            get
            {
                OnAppellationGet();
                return this._appellation;
            }
            set
            {
                if(this._appellation == value)
                    return;
                OnAppellationSet(ref value);
                this._appellation = value;
                OnAppellationSeted();
                this.OnPropertyChanged(Real_Appellation);
            }
        }
        /// <summary>
        /// 职位标识:职位标识的实时记录顺序
        /// </summary>
        internal const int Real_OrganizePositionId = 4;

        /// <summary>
        /// 职位标识:职位标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _organizepositionid;

        partial void OnOrganizePositionIdGet();

        partial void OnOrganizePositionIdSet(ref int value);

        partial void OnOrganizePositionIdSeted();

        /// <summary>
        /// 职位标识:职位标识
        /// </summary>
        /// <remarks>
        /// 职位标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrganizePositionId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"职位标识")]
        public  int OrganizePositionId
        {
            get
            {
                OnOrganizePositionIdGet();
                return this._organizepositionid;
            }
            set
            {
                if(this._organizepositionid == value)
                    return;
                OnOrganizePositionIdSet(ref value);
                this._organizepositionid = value;
                OnOrganizePositionIdSeted();
                this.OnPropertyChanged(Real_OrganizePositionId);
            }
        }
        /// <summary>
        /// 职位:称谓的实时记录顺序
        /// </summary>
        internal const int Real_Position = 5;

        /// <summary>
        /// 职位:称谓
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _position;

        /// <summary>
        /// 职位:称谓
        /// </summary>
        /// <remarks>
        /// 称谓
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Position", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"职位")]
        public  string Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }
        /// <summary>
        /// 角色标识:角色标识的实时记录顺序
        /// </summary>
        internal const int Real_RoleId = 6;

        /// <summary>
        /// 角色标识:角色标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _roleid;

        /// <summary>
        /// 角色标识:角色标识
        /// </summary>
        /// <remarks>
        /// 角色标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("RoleId", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"角色标识")]
        public  int RoleId
        {
            get
            {
                return this._roleid;
            }
            set
            {
                this._roleid = value;
            }
        }
        /// <summary>
        /// 角色:角色的实时记录顺序
        /// </summary>
        internal const int Real_Role = 7;

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
        /// 性别:性别的实时记录顺序
        /// </summary>
        internal const int Real_Six = 8;

        /// <summary>
        /// 性别:性别
        /// </summary>
        [DataMember,JsonIgnore]
        internal bool _six;

        /// <summary>
        /// 性别:性别
        /// </summary>
        /// <remarks>
        /// 性别
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Six", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"性别")]
        public  bool Six
        {
            get
            {
                return this._six;
            }
            set
            {
                this._six = value;
            }
        }
        /// <summary>
        /// 生日:生日的实时记录顺序
        /// </summary>
        internal const int Real_Birthday = 9;

        /// <summary>
        /// 生日:生日
        /// </summary>
        [DataMember,JsonIgnore]
        internal DateTime _birthday;

        /// <summary>
        /// 生日:生日
        /// </summary>
        /// <remarks>
        /// 生日
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Birthday", NullValueHandling = NullValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , ReadOnly(true) , DisplayName(@"生日")]
        public  DateTime Birthday
        {
            get
            {
                return this._birthday;
            }
            set
            {
                this._birthday = value;
            }
        }
        /// <summary>
        /// 电话:电话的实时记录顺序
        /// </summary>
        internal const int Real_Tel = 10;

        /// <summary>
        /// 电话:电话
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _tel;

        /// <summary>
        /// 电话:电话
        /// </summary>
        /// <remarks>
        /// 电话
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Tel", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"电话")]
        public  string Tel
        {
            get
            {
                return this._tel;
            }
            set
            {
                this._tel = value;
            }
        }
        /// <summary>
        /// 手机:手机的实时记录顺序
        /// </summary>
        internal const int Real_Mobile = 11;

        /// <summary>
        /// 手机:手机
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _mobile;

        /// <summary>
        /// 手机:手机
        /// </summary>
        /// <remarks>
        /// 手机
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Mobile", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"手机")]
        public  string Mobile
        {
            get
            {
                return this._mobile;
            }
            set
            {
                this._mobile = value;
            }
        }
        /// <summary>
        /// 机构标识:机构标识的实时记录顺序
        /// </summary>
        internal const int Real_OrganizationId = 12;

        /// <summary>
        /// 机构标识:机构标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _organizationid;

        /// <summary>
        /// 机构标识:机构标识
        /// </summary>
        /// <remarks>
        /// 机构标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("OrganizationId", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"机构标识")]
        public  int OrganizationId
        {
            get
            {
                return this._organizationid;
            }
            set
            {
                this._organizationid = value;
            }
        }
        /// <summary>
        /// 所在机构:所在机构的实时记录顺序
        /// </summary>
        internal const int Real_Organization = 13;

        /// <summary>
        /// 所在机构:所在机构
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _organization;

        /// <summary>
        /// 所在机构:所在机构
        /// </summary>
        /// <remarks>
        /// 所在机构
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Organization", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"所在机构")]
        public  string Organization
        {
            get
            {
                return this._organization;
            }
            set
            {
                this._organization = value;
            }
        }
        /// <summary>
        /// 部门外键:部门外键的实时记录顺序
        /// </summary>
        internal const int Real_DepartmentId = 14;

        /// <summary>
        /// 部门外键:部门外键
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _departmentid;

        /// <summary>
        /// 部门外键:部门外键
        /// </summary>
        /// <remarks>
        /// 部门外键
        /// </remarks>
        [IgnoreDataMember , JsonProperty("DepartmentId", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"部门外键")]
        public  int DepartmentId
        {
            get
            {
                return this._departmentid;
            }
            set
            {
                this._departmentid = value;
            }
        }
        /// <summary>
        /// 部门:部门的实时记录顺序
        /// </summary>
        internal const int Real_Department = 15;

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
        /// 备注:备注的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 16;

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
        internal const int Real_DataState = 17;

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
        internal const int Real_IsFreeze = 18;

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
        internal const int Real_AuthorID = 19;

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
        internal const int Real_AddDate = 20;

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
        internal const int Real_LastReviserID = 21;

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
        internal const int Real_LastModifyDate = 22;

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
        internal const int Real_AuditState = 23;

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
        internal const int Real_AuditorId = 24;

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
        internal const int Real_AuditDate = 25;

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
        /// <summary>
        /// 系统用户外键:标识的实时记录顺序
        /// </summary>
        internal const int Real_UserId = 26;

        /// <summary>
        /// 系统用户外键:标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _userid;

        /// <summary>
        /// 系统用户外键:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("UserId", NullValueHandling = NullValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"系统用户外键")]
        public  int UserId
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }
        /// <summary>
        /// 级别:级别的实时记录顺序
        /// </summary>
        internal const int Real_OrgLevel = 27;

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
        /// master_id:master_id
        /// </summary>
        /// <remarks>
        /// 仅限用于查询的Lambda表达式使用
        /// </remarks>
        [IgnoreDataMember , Browsable(false),JsonIgnore]
        public int master_id
        {
            get
            {
                throw new Exception("master_id:master_id属性仅限用于查询的Lambda表达式使用");
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
            case "personnelid":
                this.PersonnelId = (int)Convert.ToDecimal(value);
                return;
            case "personnel":
                this.Personnel = value == null ? null : value.ToString();
                return;
            case "appellation":
                this.Appellation = value == null ? null : value.ToString();
                return;
            case "organizepositionid":
                this.OrganizePositionId = (int)Convert.ToDecimal(value);
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
            case "six":
                if (value != null)
                {
                    int vl;
                    if (int.TryParse(value.ToString(), out vl))
                    {
                        this.Six = vl != 0;
                    }
                    else
                    {
                        this.Six = Convert.ToBoolean(value);
                    }
                }
                return;
            case "birthday":
                this.Birthday = Convert.ToDateTime(value);
                return;
            case "tel":
                this.Tel = value == null ? null : value.ToString();
                return;
            case "mobile":
                this.Mobile = value == null ? null : value.ToString();
                return;
            case "organizationid":
                this.OrganizationId = (int)Convert.ToDecimal(value);
                return;
            case "organization":
                this.Organization = value == null ? null : value.ToString();
                return;
            case "departmentid":
                this.DepartmentId = (int)Convert.ToDecimal(value);
                return;
            case "department":
                this.Department = value == null ? null : value.ToString();
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
            case "userid":
                this.UserId = (int)Convert.ToDecimal(value);
                return;
            case "orglevel":
                this.OrgLevel = (int)Convert.ToDecimal(value);
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
            case Index_PersonnelId:
                this.PersonnelId = Convert.ToInt32(value);
                return;
            case Index_Personnel:
                this.Personnel = value == null ? null : value.ToString();
                return;
            case Index_Appellation:
                this.Appellation = value == null ? null : value.ToString();
                return;
            case Index_OrganizePositionId:
                this.OrganizePositionId = Convert.ToInt32(value);
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
            case Index_Six:
                this.Six = Convert.ToBoolean(value);
                return;
            case Index_Birthday:
                this.Birthday = Convert.ToDateTime(value);
                return;
            case Index_Tel:
                this.Tel = value == null ? null : value.ToString();
                return;
            case Index_Mobile:
                this.Mobile = value == null ? null : value.ToString();
                return;
            case Index_OrganizationId:
                this.OrganizationId = Convert.ToInt32(value);
                return;
            case Index_Organization:
                this.Organization = value == null ? null : value.ToString();
                return;
            case Index_DepartmentId:
                this.DepartmentId = Convert.ToInt32(value);
                return;
            case Index_Department:
                this.Department = value == null ? null : value.ToString();
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
            case Index_UserId:
                this.UserId = Convert.ToInt32(value);
                return;
            case Index_OrgLevel:
                this.OrgLevel = Convert.ToInt32(value);
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
            case "PersonnelId":
                return this.PersonnelId;
            case "Personnel":
                return this.Personnel;
            case "Appellation":
                return this.Appellation;
            case "OrganizePositionId":
                return this.OrganizePositionId;
            case "Position":
                return this.Position;
            case "RoleId":
                return this.RoleId;
            case "Role":
                return this.Role;
            case "Six":
                return this.Six;
            case "Birthday":
                return this.Birthday;
            case "Tel":
                return this.Tel;
            case "Mobile":
                return this.Mobile;
            case "OrganizationId":
                return this.OrganizationId;
            case "Organization":
                return this.Organization;
            case "DepartmentId":
                return this.DepartmentId;
            case "Department":
                return this.Department;
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
            case "UserId":
                return this.UserId;
            case "OrgLevel":
                return this.OrgLevel;
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
                case Index_PersonnelId:
                    return this.PersonnelId;
                case Index_Personnel:
                    return this.Personnel;
                case Index_Appellation:
                    return this.Appellation;
                case Index_OrganizePositionId:
                    return this.OrganizePositionId;
                case Index_Position:
                    return this.Position;
                case Index_RoleId:
                    return this.RoleId;
                case Index_Role:
                    return this.Role;
                case Index_Six:
                    return this.Six;
                case Index_Birthday:
                    return this.Birthday;
                case Index_Tel:
                    return this.Tel;
                case Index_Mobile:
                    return this.Mobile;
                case Index_OrganizationId:
                    return this.OrganizationId;
                case Index_Organization:
                    return this.Organization;
                case Index_DepartmentId:
                    return this.DepartmentId;
                case Index_Department:
                    return this.Department;
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
                case Index_UserId:
                    return this.UserId;
                case Index_OrgLevel:
                    return this.OrgLevel;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(PositionPersonnelData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as PositionPersonnelData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._personnelid = sourceEntity._personnelid;
            this._personnel = sourceEntity._personnel;
            this._appellation = sourceEntity._appellation;
            this._organizepositionid = sourceEntity._organizepositionid;
            this._position = sourceEntity._position;
            this._roleid = sourceEntity._roleid;
            this._role = sourceEntity._role;
            this._six = sourceEntity._six;
            this._birthday = sourceEntity._birthday;
            this._tel = sourceEntity._tel;
            this._mobile = sourceEntity._mobile;
            this._organizationid = sourceEntity._organizationid;
            this._organization = sourceEntity._organization;
            this._departmentid = sourceEntity._departmentid;
            this._department = sourceEntity._department;
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
            this._userid = sourceEntity._userid;
            this._orglevel = sourceEntity._orglevel;
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(PositionPersonnelData source)
        {
                this.Id = source.Id;
                this.PersonnelId = source.PersonnelId;
                this.Personnel = source.Personnel;
                this.Appellation = source.Appellation;
                this.OrganizePositionId = source.OrganizePositionId;
                this.Position = source.Position;
                this.RoleId = source.RoleId;
                this.Role = source.Role;
                this.Six = source.Six;
                this.Birthday = source.Birthday;
                this.Tel = source.Tel;
                this.Mobile = source.Mobile;
                this.OrganizationId = source.OrganizationId;
                this.Organization = source.Organization;
                this.DepartmentId = source.DepartmentId;
                this.Department = source.Department;
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
                this.UserId = source.UserId;
                this.OrgLevel = source.OrgLevel;
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
                OnPersonnelIdModified(subsist,false);
                OnPersonnelModified(subsist,false);
                OnAppellationModified(subsist,false);
                OnOrganizePositionIdModified(subsist,false);
                OnPositionModified(subsist,false);
                OnRoleIdModified(subsist,false);
                OnRoleModified(subsist,false);
                OnSixModified(subsist,false);
                OnBirthdayModified(subsist,false);
                OnTelModified(subsist,false);
                OnMobileModified(subsist,false);
                OnOrganizationIdModified(subsist,false);
                OnOrganizationModified(subsist,false);
                OnDepartmentIdModified(subsist,false);
                OnDepartmentModified(subsist,false);
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
                OnUserIdModified(subsist,false);
                OnOrgLevelModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIdModified(subsist,true);
                OnPersonnelIdModified(subsist,true);
                OnPersonnelModified(subsist,true);
                OnAppellationModified(subsist,true);
                OnOrganizePositionIdModified(subsist,true);
                OnPositionModified(subsist,true);
                OnRoleIdModified(subsist,true);
                OnRoleModified(subsist,true);
                OnSixModified(subsist,true);
                OnBirthdayModified(subsist,true);
                OnTelModified(subsist,true);
                OnMobileModified(subsist,true);
                OnOrganizationIdModified(subsist,true);
                OnOrganizationModified(subsist,true);
                OnDepartmentIdModified(subsist,true);
                OnDepartmentModified(subsist,true);
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
                OnUserIdModified(subsist,true);
                OnOrgLevelModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[28] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnPersonnelIdModified(subsist,modifieds[Real_PersonnelId] == 1);
                OnPersonnelModified(subsist,modifieds[Real_Personnel] == 1);
                OnAppellationModified(subsist,modifieds[Real_Appellation] == 1);
                OnOrganizePositionIdModified(subsist,modifieds[Real_OrganizePositionId] == 1);
                OnPositionModified(subsist,modifieds[Real_Position] == 1);
                OnRoleIdModified(subsist,modifieds[Real_RoleId] == 1);
                OnRoleModified(subsist,modifieds[Real_Role] == 1);
                OnSixModified(subsist,modifieds[Real_Six] == 1);
                OnBirthdayModified(subsist,modifieds[Real_Birthday] == 1);
                OnTelModified(subsist,modifieds[Real_Tel] == 1);
                OnMobileModified(subsist,modifieds[Real_Mobile] == 1);
                OnOrganizationIdModified(subsist,modifieds[Real_OrganizationId] == 1);
                OnOrganizationModified(subsist,modifieds[Real_Organization] == 1);
                OnDepartmentIdModified(subsist,modifieds[Real_DepartmentId] == 1);
                OnDepartmentModified(subsist,modifieds[Real_Department] == 1);
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
                OnUserIdModified(subsist,modifieds[Real_UserId] == 1);
                OnOrgLevelModified(subsist,modifieds[Real_OrgLevel] == 1);
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
        /// 员工标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPersonnelIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 职员修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPersonnelModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 称谓修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnAppellationModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 职位标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrganizePositionIdModified(EntitySubsist subsist,bool isModified);

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
        /// 性别修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnSixModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 生日修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnBirthdayModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 电话修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnTelModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 手机修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMobileModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 机构标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrganizationIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 所在机构修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrganizationModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 部门外键修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDepartmentIdModified(EntitySubsist subsist,bool isModified);

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

        /// <summary>
        /// 系统用户外键修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnUserIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 级别修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnOrgLevelModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion

        #region 实体结构

        
        public const byte Index_Id = 1;
        public const byte Index_PersonnelId = 2;
        public const byte Index_Personnel = 3;
        public const byte Index_Appellation = 4;
        public const byte Index_OrganizePositionId = 5;
        public const byte Index_Position = 6;
        public const byte Index_RoleId = 7;
        public const byte Index_Role = 8;
        public const byte Index_Six = 9;
        public const byte Index_Birthday = 10;
        public const byte Index_Tel = 11;
        public const byte Index_Mobile = 12;
        public const byte Index_OrganizationId = 13;
        public const byte Index_Organization = 14;
        public const byte Index_DepartmentId = 15;
        public const byte Index_Department = 16;
        public const byte Index_Memo = 17;
        public const byte Index_DataState = 18;
        public const byte Index_IsFreeze = 19;
        public const byte Index_AuthorID = 20;
        public const byte Index_AddDate = 21;
        public const byte Index_LastReviserID = 22;
        public const byte Index_LastModifyDate = 23;
        public const byte Index_AuditState = 24;
        public const byte Index_AuditorId = 25;
        public const byte Index_AuditDate = 26;
        public const byte Index_UserId = 28;
        public const byte Index_OrgLevel = 29;

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
            EntityName = "PositionPersonnel",
            PrimaryKey = "Id",
            EntityType = 0x50006,
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
                    Real_PersonnelId,
                    new PropertySturct
                    {
                        Index = Index_PersonnelId,
                        Name = "PersonnelId",
                        Title = "员工标识",
                        ColumnName = "personnel_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_Personnel,
                    new PropertySturct
                    {
                        Index = Index_Personnel,
                        Name = "Personnel",
                        Title = "职员",
                        ColumnName = "personnel",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Appellation,
                    new PropertySturct
                    {
                        Index = Index_Appellation,
                        Name = "Appellation",
                        Title = "称谓",
                        ColumnName = "appellation",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_OrganizePositionId,
                    new PropertySturct
                    {
                        Index = Index_OrganizePositionId,
                        Name = "OrganizePositionId",
                        Title = "职位标识",
                        ColumnName = "organize_position_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_Position,
                    new PropertySturct
                    {
                        Index = Index_Position,
                        Name = "Position",
                        Title = "职位",
                        ColumnName = "position",
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
                    Real_Six,
                    new PropertySturct
                    {
                        Index = Index_Six,
                        Name = "Six",
                        Title = "性别",
                        ColumnName = "six",
                        PropertyType = typeof(bool),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Birthday,
                    new PropertySturct
                    {
                        Index = Index_Birthday,
                        Name = "Birthday",
                        Title = "生日",
                        ColumnName = "birthday",
                        PropertyType = typeof(DateTime),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Tel,
                    new PropertySturct
                    {
                        Index = Index_Tel,
                        Name = "Tel",
                        Title = "电话",
                        ColumnName = "tel",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Mobile,
                    new PropertySturct
                    {
                        Index = Index_Mobile,
                        Name = "Mobile",
                        Title = "手机",
                        ColumnName = "mobile",
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
                        Title = "机构标识",
                        ColumnName = "organization_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Organization,
                    new PropertySturct
                    {
                        Index = Index_Organization,
                        Name = "Organization",
                        Title = "所在机构",
                        ColumnName = "organization",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_DepartmentId,
                    new PropertySturct
                    {
                        Index = Index_DepartmentId,
                        Name = "DepartmentId",
                        Title = "部门外键",
                        ColumnName = "department_id",
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
                        ColumnName = "department",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
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
                },
                {
                    Real_UserId,
                    new PropertySturct
                    {
                        Index = Index_UserId,
                        Name = "UserId",
                        Title = "系统用户外键",
                        ColumnName = "user_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
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
                }
            }
        };

        #endregion
    }
}