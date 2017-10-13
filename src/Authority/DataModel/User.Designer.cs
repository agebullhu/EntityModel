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
    /// 系统用户
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class UserData : IStateData , IHistoryData , IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public UserData()
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
        /// 姓名:姓名的实时记录顺序
        /// </summary>
        internal const int Real_RealName = 1;

        /// <summary>
        /// 姓名:姓名
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _realname;

        partial void OnRealNameGet();

        partial void OnRealNameSet(ref string value);

        partial void OnRealNameSeted();

        /// <summary>
        /// 姓名:姓名
        /// </summary>
        /// <remarks>
        /// 姓名
        /// </remarks>
        [IgnoreDataMember , JsonProperty("RealName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"姓名")]
        public  string RealName
        {
            get
            {
                OnRealNameGet();
                return this._realname;
            }
            set
            {
                if(this._realname == value)
                    return;
                OnRealNameSet(ref value);
                this._realname = value;
                OnRealNameSeted();
                this.OnPropertyChanged(Real_RealName);
            }
        }
        /// <summary>
        /// 用户名:用户名的实时记录顺序
        /// </summary>
        internal const int Real_UserName = 2;

        /// <summary>
        /// 用户名:用户名
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _username;

        partial void OnUserNameGet();

        partial void OnUserNameSet(ref string value);

        partial void OnUserNameSeted();

        /// <summary>
        /// 用户名:用户名
        /// </summary>
        /// <remarks>
        /// 用户名
        /// </remarks>
        [IgnoreDataMember , JsonProperty("UserName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"用户名")]
        public  string UserName
        {
            get
            {
                OnUserNameGet();
                return this._username;
            }
            set
            {
                if(this._username == value)
                    return;
                OnUserNameSet(ref value);
                this._username = value;
                OnUserNameSeted();
                this.OnPropertyChanged(Real_UserName);
            }
        }
        /// <summary>
        /// 密码:密码的实时记录顺序
        /// </summary>
        internal const int Real_PassWord = 3;

        /// <summary>
        /// 密码:密码
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _password;

        partial void OnPassWordGet();

        partial void OnPassWordSet(ref string value);

        partial void OnPassWordSeted();

        /// <summary>
        /// 密码:密码
        /// </summary>
        /// <remarks>
        /// 密码
        /// </remarks>
        [IgnoreDataMember , JsonProperty("PassWord", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"密码")]
        public  string PassWord
        {
            get
            {
                OnPassWordGet();
                return this._password;
            }
            set
            {
                if(this._password == value)
                    return;
                OnPassWordSet(ref value);
                this._password = value;
                OnPassWordSeted();
                this.OnPropertyChanged(Real_PassWord);
            }
        }
        /// <summary>
        /// 标识:标识的实时记录顺序
        /// </summary>
        internal const int Real_RoleId = 4;

        /// <summary>
        /// 标识:标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _roleid;

        partial void OnRoleIdGet();

        partial void OnRoleIdSet(ref int value);

        partial void OnRoleIdSeted();

        /// <summary>
        /// 标识:标识
        /// </summary>
        /// <remarks>
        /// 标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("RoleId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"标识")]
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
        /// 角色:名称的实时记录顺序
        /// </summary>
        internal const int Real_Role = 5;

        /// <summary>
        /// 角色:名称
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _role;

        /// <summary>
        /// 角色:名称
        /// </summary>
        /// <remarks>
        /// 名称
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
        /// 备注:备注的实时记录顺序
        /// </summary>
        internal const int Real_Memo = 6;

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
        internal const int Real_DataState = 7;

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
        internal const int Real_IsFreeze = 8;

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
        internal const int Real_AuthorID = 9;

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
        internal const int Real_AddDate = 10;

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
        internal const int Real_LastReviserID = 11;

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
        internal const int Real_LastModifyDate = 12;

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
        internal const int Real_AuditState = 13;

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
        internal const int Real_AuditorId = 14;

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
        internal const int Real_AuditDate = 15;

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
            case "realname":
                this.RealName = value == null ? null : value.ToString();
                return;
            case "username":
                this.UserName = value == null ? null : value.ToString();
                return;
            case "password":
                this.PassWord = value == null ? null : value.ToString();
                return;
            case "roleid":
                this.RoleId = (int)Convert.ToDecimal(value);
                return;
            case "role":
                this.Role = value == null ? null : value.ToString();
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
            case Index_RealName:
                this.RealName = value == null ? null : value.ToString();
                return;
            case Index_UserName:
                this.UserName = value == null ? null : value.ToString();
                return;
            case Index_PassWord:
                this.PassWord = value == null ? null : value.ToString();
                return;
            case Index_RoleId:
                this.RoleId = Convert.ToInt32(value);
                return;
            case Index_Role:
                this.Role = value == null ? null : value.ToString();
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
            case "RealName":
                return this.RealName;
            case "UserName":
                return this.UserName;
            case "PassWord":
                return this.PassWord;
            case "RoleId":
                return this.RoleId;
            case "Role":
                return this.Role;
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
                case Index_RealName:
                    return this.RealName;
                case Index_UserName:
                    return this.UserName;
                case Index_PassWord:
                    return this.PassWord;
                case Index_RoleId:
                    return this.RoleId;
                case Index_Role:
                    return this.Role;
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


        partial void CopyExtendValue(UserData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as UserData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._realname = sourceEntity._realname;
            this._username = sourceEntity._username;
            this._password = sourceEntity._password;
            this._roleid = sourceEntity._roleid;
            this._role = sourceEntity._role;
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
        public void Copy(UserData source)
        {
                this.Id = source.Id;
                this.RealName = source.RealName;
                this.UserName = source.UserName;
                this.PassWord = source.PassWord;
                this.RoleId = source.RoleId;
                this.Role = source.Role;
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
                OnRealNameModified(subsist,false);
                OnUserNameModified(subsist,false);
                OnPassWordModified(subsist,false);
                OnRoleIdModified(subsist,false);
                OnRoleModified(subsist,false);
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
                OnRealNameModified(subsist,true);
                OnUserNameModified(subsist,true);
                OnPassWordModified(subsist,true);
                OnRoleIdModified(subsist,true);
                OnRoleModified(subsist,true);
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
            else if(modifieds != null && modifieds[16] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnRealNameModified(subsist,modifieds[Real_RealName] == 1);
                OnUserNameModified(subsist,modifieds[Real_UserName] == 1);
                OnPassWordModified(subsist,modifieds[Real_PassWord] == 1);
                OnRoleIdModified(subsist,modifieds[Real_RoleId] == 1);
                OnRoleModified(subsist,modifieds[Real_Role] == 1);
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
        /// 姓名修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnRealNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 用户名修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnUserNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 密码修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnPassWordModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 标识修改的后期处理(保存前)
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
        public const byte Index_RealName = 3;
        public const byte Index_UserName = 4;
        public const byte Index_PassWord = 5;
        public const byte Index_RoleId = 6;
        public const byte Index_Role = 7;
        public const byte Index_Memo = 8;
        public const byte Index_DataState = 28;
        public const byte Index_IsFreeze = 30;
        public const byte Index_AuthorID = 32;
        public const byte Index_AddDate = 34;
        public const byte Index_LastReviserID = 36;
        public const byte Index_LastModifyDate = 38;
        public const byte Index_AuditState = 40;
        public const byte Index_AuditorId = 42;
        public const byte Index_AuditDate = 44;

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
            EntityName = "User",
            PrimaryKey = "Id",
            EntityType = 0x50008,
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
                    Real_RealName,
                    new PropertySturct
                    {
                        Index = Index_RealName,
                        Name = "RealName",
                        Title = "姓名",
                        ColumnName = "real_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_UserName,
                    new PropertySturct
                    {
                        Index = Index_UserName,
                        Name = "UserName",
                        Title = "用户名",
                        ColumnName = "user_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_PassWord,
                    new PropertySturct
                    {
                        Index = Index_PassWord,
                        Name = "PassWord",
                        Title = "密码",
                        ColumnName = "pass_word",
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
                        Title = "标识",
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
                        ColumnName = "Role",
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