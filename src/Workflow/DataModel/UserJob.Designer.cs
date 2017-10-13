/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/10/7 10:47:08*/
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

using Gboxt.Common.Workflow.DataAccess;
using Gboxt.Common.DataModel.MySql;
using Newtonsoft.Json;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class UserJobData : IStateData , IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public UserJobData()
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
        /// 标题:标题的实时记录顺序
        /// </summary>
        internal const int Real_Title = 1;

        /// <summary>
        /// 标题:标题
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _title;

        partial void OnTitleGet();

        partial void OnTitleSet(ref string value);

        partial void OnTitleSeted();

        /// <summary>
        /// 标题:标题
        /// </summary>
        /// <remarks>
        /// 标题
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Title", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"标题")]
        public  string Title
        {
            get
            {
                OnTitleGet();
                return this._title;
            }
            set
            {
                if(this._title == value)
                    return;
                OnTitleSet(ref value);
                this._title = value;
                OnTitleSeted();
                this.OnPropertyChanged(Real_Title);
            }
        }
        /// <summary>
        /// 发生日期:发生日期的实时记录顺序
        /// </summary>
        internal const int Real_Date = 2;

        /// <summary>
        /// 发生日期:发生日期
        /// </summary>
        [DataMember,JsonIgnore]
        internal DateTime _date;

        partial void OnDateGet();

        partial void OnDateSet(ref DateTime value);

        partial void OnDateSeted();

        /// <summary>
        /// 发生日期:发生日期
        /// </summary>
        /// <remarks>
        /// 发生日期
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Date", NullValueHandling = NullValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"发生日期")]
        public  DateTime Date
        {
            get
            {
                OnDateGet();
                return this._date;
            }
            set
            {
                if(this._date == value)
                    return;
                OnDateSet(ref value);
                this._date = value;
                OnDateSeted();
                this.OnPropertyChanged(Real_Date);
            }
        }
        /// <summary>
        /// 工作消息:工作消息的实时记录顺序
        /// </summary>
        internal const int Real_Message = 3;

        /// <summary>
        /// 工作消息:工作消息
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _message;

        partial void OnMessageGet();

        partial void OnMessageSet(ref string value);

        partial void OnMessageSeted();

        /// <summary>
        /// 工作消息:工作消息
        /// </summary>
        /// <remarks>
        /// 工作消息
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Message", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"工作消息")]
        public  string Message
        {
            get
            {
                OnMessageGet();
                return this._message;
            }
            set
            {
                if(this._message == value)
                    return;
                OnMessageSet(ref value);
                this._message = value;
                OnMessageSeted();
                this.OnPropertyChanged(Real_Message);
            }
        }
        /// <summary>
        /// 任务分类:任务分类的实时记录顺序
        /// </summary>
        internal const int Real_JobType = 4;

        /// <summary>
        /// 任务分类:任务分类
        /// </summary>
        [DataMember,JsonIgnore]
        internal UserJobType _jobtype;

        partial void OnJobTypeGet();

        partial void OnJobTypeSet(ref UserJobType value);

        partial void OnJobTypeSeted();

        /// <summary>
        /// 任务分类:任务分类
        /// </summary>
        /// <remarks>
        /// 任务分类
        /// </remarks>
        [IgnoreDataMember , JsonProperty("JobType", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"任务分类")]
        public  UserJobType JobType
        {
            get
            {
                OnJobTypeGet();
                return this._jobtype;
            }
            set
            {
                if(this._jobtype == value)
                    return;
                OnJobTypeSet(ref value);
                this._jobtype = value;
                OnJobTypeSeted();
                this.OnPropertyChanged(Real_JobType);
            }
        }
        /// <summary>
        /// 工作状态:工作状态的实时记录顺序
        /// </summary>
        internal const int Real_JobStatus = 5;

        /// <summary>
        /// 工作状态:工作状态
        /// </summary>
        [DataMember,JsonIgnore]
        internal JobStatusType _jobstatus;

        partial void OnJobStatusGet();

        partial void OnJobStatusSet(ref JobStatusType value);

        partial void OnJobStatusSeted();

        /// <summary>
        /// 工作状态:工作状态
        /// </summary>
        /// <remarks>
        /// 工作状态
        /// </remarks>
        [IgnoreDataMember , JsonProperty("JobStatus", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"工作状态")]
        public  JobStatusType JobStatus
        {
            get
            {
                OnJobStatusGet();
                return this._jobstatus;
            }
            set
            {
                if(this._jobstatus == value)
                    return;
                OnJobStatusSet(ref value);
                this._jobstatus = value;
                OnJobStatusSeted();
                this.OnPropertyChanged(Real_JobStatus);
            }
        }
        /// <summary>
        /// 命令类型:命令类型的实时记录顺序
        /// </summary>
        internal const int Real_CommandType = 6;

        /// <summary>
        /// 命令类型:命令类型
        /// </summary>
        [DataMember,JsonIgnore]
        internal JobCommandType _commandtype;

        partial void OnCommandTypeGet();

        partial void OnCommandTypeSet(ref JobCommandType value);

        partial void OnCommandTypeSeted();

        /// <summary>
        /// 命令类型:命令类型
        /// </summary>
        /// <remarks>
        /// 命令类型
        /// </remarks>
        [IgnoreDataMember , JsonProperty("CommandType", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"命令类型")]
        public  JobCommandType CommandType
        {
            get
            {
                OnCommandTypeGet();
                return this._commandtype;
            }
            set
            {
                if(this._commandtype == value)
                    return;
                OnCommandTypeSet(ref value);
                this._commandtype = value;
                OnCommandTypeSeted();
                this.OnPropertyChanged(Real_CommandType);
            }
        }
        /// <summary>
        /// 关联标识:关联标识的实时记录顺序
        /// </summary>
        internal const int Real_LinkId = 7;

        /// <summary>
        /// 关联标识:关联标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _linkid;

        partial void OnLinkIdGet();

        partial void OnLinkIdSet(ref int value);

        partial void OnLinkIdSeted();

        /// <summary>
        /// 关联标识:关联标识
        /// </summary>
        /// <remarks>
        /// 关联标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("LinkId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"关联标识")]
        public  int LinkId
        {
            get
            {
                OnLinkIdGet();
                return this._linkid;
            }
            set
            {
                if(this._linkid == value)
                    return;
                OnLinkIdSet(ref value);
                this._linkid = value;
                OnLinkIdSeted();
                this.OnPropertyChanged(Real_LinkId);
            }
        }
        /// <summary>
        /// 关联标识:关联标识
        /// </summary>
        /// <remarks>
        /// 关联标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("DataId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"关联标识")]
        public int DataId
        {
            get
            {
                return this.LinkId;
            }
            set
            {
                this.LinkId = value;
            }
        }
        /// <summary>
        /// 连接类型:连接类型的实时记录顺序
        /// </summary>
        internal const int Real_EntityType = 8;

        /// <summary>
        /// 连接类型:连接类型
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _entitytype;

        partial void OnEntityTypeGet();

        partial void OnEntityTypeSet(ref int value);

        partial void OnEntityTypeSeted();

        /// <summary>
        /// 连接类型:连接类型
        /// </summary>
        /// <remarks>
        /// 连接类型
        /// </remarks>
        [IgnoreDataMember , JsonProperty("EntityType", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"连接类型")]
        public  int EntityType
        {
            get
            {
                OnEntityTypeGet();
                return this._entitytype;
            }
            set
            {
                if(this._entitytype == value)
                    return;
                OnEntityTypeSet(ref value);
                this._entitytype = value;
                OnEntityTypeSeted();
                this.OnPropertyChanged(Real_EntityType);
            }
        }
        /// <summary>
        /// 目标用户标识:关联用户标识的实时记录顺序
        /// </summary>
        internal const int Real_ToUserId = 9;

        /// <summary>
        /// 目标用户标识:关联用户标识
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _touserid;

        partial void OnToUserIdGet();

        partial void OnToUserIdSet(ref int value);

        partial void OnToUserIdSeted();

        /// <summary>
        /// 目标用户标识:关联用户标识
        /// </summary>
        /// <remarks>
        /// 关联用户标识
        /// </remarks>
        [IgnoreDataMember , JsonProperty("ToUserId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"目标用户标识")]
        public  int ToUserId
        {
            get
            {
                OnToUserIdGet();
                return this._touserid;
            }
            set
            {
                if(this._touserid == value)
                    return;
                OnToUserIdSet(ref value);
                this._touserid = value;
                OnToUserIdSeted();
                this.OnPropertyChanged(Real_ToUserId);
            }
        }
        /// <summary>
        /// 目标用户名字:接收用户名字的实时记录顺序
        /// </summary>
        internal const int Real_ToUserName = 10;

        /// <summary>
        /// 目标用户名字:接收用户名字
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _tousername;

        partial void OnToUserNameGet();

        partial void OnToUserNameSet(ref string value);

        partial void OnToUserNameSeted();

        /// <summary>
        /// 目标用户名字:接收用户名字
        /// </summary>
        /// <remarks>
        /// 接收用户名字
        /// </remarks>
        [IgnoreDataMember , JsonProperty("ToUserName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"目标用户名字")]
        public  string ToUserName
        {
            get
            {
                OnToUserNameGet();
                return this._tousername;
            }
            set
            {
                if(this._tousername == value)
                    return;
                OnToUserNameSet(ref value);
                this._tousername = value;
                OnToUserNameSeted();
                this.OnPropertyChanged(Real_ToUserName);
            }
        }
        /// <summary>
        /// 来源用户标识:来源用户的实时记录顺序
        /// </summary>
        internal const int Real_FromUserId = 11;

        /// <summary>
        /// 来源用户标识:来源用户
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _fromuserid;

        partial void OnFromUserIdGet();

        partial void OnFromUserIdSet(ref int value);

        partial void OnFromUserIdSeted();

        /// <summary>
        /// 来源用户标识:来源用户
        /// </summary>
        /// <remarks>
        /// 来源用户
        /// </remarks>
        [IgnoreDataMember , JsonProperty("FromUserId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"来源用户标识")]
        public  int FromUserId
        {
            get
            {
                OnFromUserIdGet();
                return this._fromuserid;
            }
            set
            {
                if(this._fromuserid == value)
                    return;
                OnFromUserIdSet(ref value);
                this._fromuserid = value;
                OnFromUserIdSeted();
                this.OnPropertyChanged(Real_FromUserId);
            }
        }
        /// <summary>
        /// 来源用户名字:来源用户名字的实时记录顺序
        /// </summary>
        internal const int Real_FromUserName = 12;

        /// <summary>
        /// 来源用户名字:来源用户名字
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _fromusername;

        partial void OnFromUserNameGet();

        partial void OnFromUserNameSet(ref string value);

        partial void OnFromUserNameSeted();

        /// <summary>
        /// 来源用户名字:来源用户名字
        /// </summary>
        /// <remarks>
        /// 来源用户名字
        /// </remarks>
        [IgnoreDataMember , JsonProperty("FromUserName", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"来源用户名字")]
        public  string FromUserName
        {
            get
            {
                OnFromUserNameGet();
                return this._fromusername;
            }
            set
            {
                if(this._fromusername == value)
                    return;
                OnFromUserNameSet(ref value);
                this._fromusername = value;
                OnFromUserNameSeted();
                this.OnPropertyChanged(Real_FromUserName);
            }
        }
        /// <summary>
        /// 其它参数:其它参数，使用时的其它参数，JSON格式表示的实时记录顺序
        /// </summary>
        internal const int Real_Argument = 13;

        /// <summary>
        /// 其它参数:其它参数，使用时的其它参数，JSON格式表示
        /// </summary>
        [DataMember,JsonIgnore]
        internal string _argument;

        partial void OnArgumentGet();

        partial void OnArgumentSet(ref string value);

        partial void OnArgumentSeted();

        /// <summary>
        /// 其它参数:其它参数，使用时的其它参数，JSON格式表示
        /// </summary>
        /// <remarks>
        /// 其它参数，使用时的其它参数，JSON格式表示
        /// </remarks>
        [IgnoreDataMember , JsonProperty("Argument", NullValueHandling = NullValueHandling.Ignore) , Browsable(false) , DisplayName(@"其它参数")]
        public  string Argument
        {
            get
            {
                OnArgumentGet();
                return this._argument;
            }
            set
            {
                if(this._argument == value)
                    return;
                OnArgumentSet(ref value);
                this._argument = value;
                OnArgumentSeted();
                this.OnPropertyChanged(Real_Argument);
            }
        }
        /// <summary>
        /// 数据状态:数据状态的实时记录顺序
        /// </summary>
        internal const int Real_DataState = 14;

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
        internal const int Real_IsFreeze = 15;

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
        /// 接收者部门:接收者部门标识, 作为行级权限使用的实时记录顺序
        /// </summary>
        internal const int Real_DepartmentId = 16;

        /// <summary>
        /// 接收者部门:接收者部门标识, 作为行级权限使用
        /// </summary>
        [DataMember,JsonIgnore]
        internal int _departmentid;

        partial void OnDepartmentIdGet();

        partial void OnDepartmentIdSet(ref int value);

        partial void OnDepartmentIdSeted();

        /// <summary>
        /// 接收者部门:接收者部门标识, 作为行级权限使用
        /// </summary>
        /// <remarks>
        /// 接收者部门标识, 作为行级权限使用
        /// </remarks>
        [IgnoreDataMember , JsonProperty("DepartmentId", NullValueHandling = NullValueHandling.Ignore) , DisplayName(@"接收者部门")]
        public  int DepartmentId
        {
            get
            {
                OnDepartmentIdGet();
                return this._departmentid;
            }
            set
            {
                if(this._departmentid == value)
                    return;
                OnDepartmentIdSet(ref value);
                this._departmentid = value;
                OnDepartmentIdSeted();
                this.OnPropertyChanged(Real_DepartmentId);
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
            case "title":
                this.Title = value == null ? null : value.ToString();
                return;
            case "date":
                this.Date = Convert.ToDateTime(value);
                return;
            case "message":
                this.Message = value == null ? null : value.ToString();
                return;
            case "jobtype":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.JobType = (UserJobType)(int)value;
                    }
                    else if(value is UserJobType)
                    {
                        this.JobType = (UserJobType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        UserJobType val;
                        if (UserJobType.TryParse(str, out val))
                        {
                            this.JobType = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.JobType = (UserJobType)vl;
                            }
                        }
                    }
                }
                return;
            case "jobstatus":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.JobStatus = (JobStatusType)(int)value;
                    }
                    else if(value is JobStatusType)
                    {
                        this.JobStatus = (JobStatusType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        JobStatusType val;
                        if (JobStatusType.TryParse(str, out val))
                        {
                            this.JobStatus = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.JobStatus = (JobStatusType)vl;
                            }
                        }
                    }
                }
                return;
            case "commandtype":
                if (value != null)
                {
                    if(value is int)
                    {
                        this.CommandType = (JobCommandType)(int)value;
                    }
                    else if(value is JobCommandType)
                    {
                        this.CommandType = (JobCommandType)value;
                    }
                    else
                    {
                        var str = value.ToString();
                        JobCommandType val;
                        if (JobCommandType.TryParse(str, out val))
                        {
                            this.CommandType = val;
                        }
                        else
                        {
                            int vl;
                            if (int.TryParse(str, out vl))
                            {
                                this.CommandType = (JobCommandType)vl;
                            }
                        }
                    }
                }
                return;
            case "dataid":
            case "linkid":
                this.LinkId = (int)Convert.ToDecimal(value);
                return;
            case "entitytype":
                this.EntityType = (int)Convert.ToDecimal(value);
                return;
            case "touserid":
                this.ToUserId = (int)Convert.ToDecimal(value);
                return;
            case "tousername":
                this.ToUserName = value == null ? null : value.ToString();
                return;
            case "fromuserid":
                this.FromUserId = (int)Convert.ToDecimal(value);
                return;
            case "fromusername":
                this.FromUserName = value == null ? null : value.ToString();
                return;
            case "argument":
                this.Argument = value == null ? null : value.ToString();
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
            case "departmentid":
                this.DepartmentId = (int)Convert.ToDecimal(value);
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
            case Index_Title:
                this.Title = value == null ? null : value.ToString();
                return;
            case Index_Date:
                this.Date = Convert.ToDateTime(value);
                return;
            case Index_Message:
                this.Message = value == null ? null : value.ToString();
                return;
            case Index_JobType:
                this.JobType = (UserJobType)value;
                return;
            case Index_JobStatus:
                this.JobStatus = (JobStatusType)value;
                return;
            case Index_CommandType:
                this.CommandType = (JobCommandType)value;
                return;
            case Index_LinkId:
                this.LinkId = Convert.ToInt32(value);
                return;
            case Index_EntityType:
                this.EntityType = Convert.ToInt32(value);
                return;
            case Index_ToUserId:
                this.ToUserId = Convert.ToInt32(value);
                return;
            case Index_ToUserName:
                this.ToUserName = value == null ? null : value.ToString();
                return;
            case Index_FromUserId:
                this.FromUserId = Convert.ToInt32(value);
                return;
            case Index_FromUserName:
                this.FromUserName = value == null ? null : value.ToString();
                return;
            case Index_Argument:
                this.Argument = value == null ? null : value.ToString();
                return;
            case Index_DataState:
                this.DataState = (DataStateType)value;
                return;
            case Index_IsFreeze:
                this.IsFreeze = Convert.ToBoolean(value);
                return;
            case Index_DepartmentId:
                this.DepartmentId = Convert.ToInt32(value);
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
            case "Title":
                return this.Title;
            case "Date":
                return this.Date;
            case "Message":
                return this.Message;
            case "JobType":
                return this.JobType;
            case "JobStatus":
                return this.JobStatus;
            case "CommandType":
                return this.CommandType;
            case "DataId":
            case "LinkId":
                return this.LinkId;
            case "EntityType":
                return this.EntityType;
            case "ToUserId":
                return this.ToUserId;
            case "ToUserName":
                return this.ToUserName;
            case "FromUserId":
                return this.FromUserId;
            case "FromUserName":
                return this.FromUserName;
            case "Argument":
                return this.Argument;
            case "DataState":
                return this.DataState;
            case "IsFreeze":
                return this.IsFreeze;
            case "DepartmentId":
                return this.DepartmentId;
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
                case Index_Title:
                    return this.Title;
                case Index_Date:
                    return this.Date;
                case Index_Message:
                    return this.Message;
                case Index_JobType:
                    return this.JobType;
                case Index_JobStatus:
                    return this.JobStatus;
                case Index_CommandType:
                    return this.CommandType;
                case Index_LinkId:
                    return this.LinkId;
                case Index_EntityType:
                    return this.EntityType;
                case Index_ToUserId:
                    return this.ToUserId;
                case Index_ToUserName:
                    return this.ToUserName;
                case Index_FromUserId:
                    return this.FromUserId;
                case Index_FromUserName:
                    return this.FromUserName;
                case Index_Argument:
                    return this.Argument;
                case Index_DataState:
                    return this.DataState;
                case Index_IsFreeze:
                    return this.IsFreeze;
                case Index_DepartmentId:
                    return this.DepartmentId;
            }*/

            return null;
        }

        #endregion

        #region 关联

        #endregion

        #region 复制


        partial void CopyExtendValue(UserJobData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as UserJobData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._title = sourceEntity._title;
            this._date = sourceEntity._date;
            this._message = sourceEntity._message;
            this._jobtype = sourceEntity._jobtype;
            this._jobstatus = sourceEntity._jobstatus;
            this._commandtype = sourceEntity._commandtype;
            this._linkid = sourceEntity._linkid;
            this._entitytype = sourceEntity._entitytype;
            this._touserid = sourceEntity._touserid;
            this._tousername = sourceEntity._tousername;
            this._fromuserid = sourceEntity._fromuserid;
            this._fromusername = sourceEntity._fromusername;
            this._argument = sourceEntity._argument;
            this._datastate = sourceEntity._datastate;
            this._isfreeze = sourceEntity._isfreeze;
            this._departmentid = sourceEntity._departmentid;
            CopyExtendValue(sourceEntity);
            this.__EntityStatus.SetModified();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(UserJobData source)
        {
                this.Id = source.Id;
                this.Title = source.Title;
                this.Date = source.Date;
                this.Message = source.Message;
                this.JobType = source.JobType;
                this.JobStatus = source.JobStatus;
                this.CommandType = source.CommandType;
                this.LinkId = source.LinkId;
                this.EntityType = source.EntityType;
                this.ToUserId = source.ToUserId;
                this.ToUserName = source.ToUserName;
                this.FromUserId = source.FromUserId;
                this.FromUserName = source.FromUserName;
                this.Argument = source.Argument;
                this.DataState = source.DataState;
                this.IsFreeze = source.IsFreeze;
                this.DepartmentId = source.DepartmentId;
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
                OnTitleModified(subsist,false);
                OnDateModified(subsist,false);
                OnMessageModified(subsist,false);
                OnJobTypeModified(subsist,false);
                OnJobStatusModified(subsist,false);
                OnCommandTypeModified(subsist,false);
                OnLinkIdModified(subsist,false);
                OnEntityTypeModified(subsist,false);
                OnToUserIdModified(subsist,false);
                OnToUserNameModified(subsist,false);
                OnFromUserIdModified(subsist,false);
                OnFromUserNameModified(subsist,false);
                OnArgumentModified(subsist,false);
                OnDataStateModified(subsist,false);
                OnIsFreezeModified(subsist,false);
                OnDepartmentIdModified(subsist,false);
                return;
            }
            else if (subsist == EntitySubsist.Adding || subsist == EntitySubsist.Added)
            {
                OnIdModified(subsist,true);
                OnTitleModified(subsist,true);
                OnDateModified(subsist,true);
                OnMessageModified(subsist,true);
                OnJobTypeModified(subsist,true);
                OnJobStatusModified(subsist,true);
                OnCommandTypeModified(subsist,true);
                OnLinkIdModified(subsist,true);
                OnEntityTypeModified(subsist,true);
                OnToUserIdModified(subsist,true);
                OnToUserNameModified(subsist,true);
                OnFromUserIdModified(subsist,true);
                OnFromUserNameModified(subsist,true);
                OnArgumentModified(subsist,true);
                OnDataStateModified(subsist,true);
                OnIsFreezeModified(subsist,true);
                OnDepartmentIdModified(subsist,true);
                return;
            }
            else if(modifieds != null && modifieds[17] > 0)
            {
                OnIdModified(subsist,modifieds[Real_Id] == 1);
                OnTitleModified(subsist,modifieds[Real_Title] == 1);
                OnDateModified(subsist,modifieds[Real_Date] == 1);
                OnMessageModified(subsist,modifieds[Real_Message] == 1);
                OnJobTypeModified(subsist,modifieds[Real_JobType] == 1);
                OnJobStatusModified(subsist,modifieds[Real_JobStatus] == 1);
                OnCommandTypeModified(subsist,modifieds[Real_CommandType] == 1);
                OnLinkIdModified(subsist,modifieds[Real_LinkId] == 1);
                OnEntityTypeModified(subsist,modifieds[Real_EntityType] == 1);
                OnToUserIdModified(subsist,modifieds[Real_ToUserId] == 1);
                OnToUserNameModified(subsist,modifieds[Real_ToUserName] == 1);
                OnFromUserIdModified(subsist,modifieds[Real_FromUserId] == 1);
                OnFromUserNameModified(subsist,modifieds[Real_FromUserName] == 1);
                OnArgumentModified(subsist,modifieds[Real_Argument] == 1);
                OnDataStateModified(subsist,modifieds[Real_DataState] == 1);
                OnIsFreezeModified(subsist,modifieds[Real_IsFreeze] == 1);
                OnDepartmentIdModified(subsist,modifieds[Real_DepartmentId] == 1);
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
        /// 标题修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnTitleModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 发生日期修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDateModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 工作消息修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnMessageModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 任务分类修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnJobTypeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 工作状态修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnJobStatusModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 命令类型修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnCommandTypeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 关联标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnLinkIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 连接类型修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnEntityTypeModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 目标用户标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnToUserIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 目标用户名字修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnToUserNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 来源用户标识修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnFromUserIdModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 来源用户名字修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnFromUserNameModified(EntitySubsist subsist,bool isModified);

        /// <summary>
        /// 其它参数修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnArgumentModified(EntitySubsist subsist,bool isModified);

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
        /// 接收者部门修改的后期处理(保存前)
        /// </summary>
        /// <param name="subsist">当前对象状态</param>
        /// <param name="isModified">是否被修改</param>
        /// <remarks>
        /// 对关联的属性的更改,请自行保存,否则可能丢失
        /// </remarks>
        partial void OnDepartmentIdModified(EntitySubsist subsist,bool isModified);

        #endregion

        #endregion

        #region 实体结构

        
        public const byte Index_Id = 2;
        public const byte Index_Title = 3;
        public const byte Index_Date = 4;
        public const byte Index_Message = 5;
        public const byte Index_JobType = 6;
        public const byte Index_JobStatus = 7;
        public const byte Index_CommandType = 8;
        public const byte Index_LinkId = 9;
        public const byte Index_EntityType = 10;
        public const byte Index_ToUserId = 11;
        public const byte Index_ToUserName = 12;
        public const byte Index_FromUserId = 13;
        public const byte Index_FromUserName = 14;
        public const byte Index_Argument = 15;
        public const byte Index_DataState = 16;
        public const byte Index_IsFreeze = 17;
        public const byte Index_DepartmentId = 18;

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
            EntityName = "UserJob",
            PrimaryKey = "Id",
            EntityType = 0x70001,
            Properties = new Dictionary<int, PropertySturct>
            {
                {
                    Real_Id,
                    new PropertySturct
                    {
                        Index = Index_Id,
                        Name = "Id",
                        Title = "标识",
                        ColumnName = "user_work_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Title,
                    new PropertySturct
                    {
                        Index = Index_Title,
                        Name = "Title",
                        Title = "标题",
                        ColumnName = "title",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Date,
                    new PropertySturct
                    {
                        Index = Index_Date,
                        Name = "Date",
                        Title = "发生日期",
                        ColumnName = "date",
                        PropertyType = typeof(DateTime),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Message,
                    new PropertySturct
                    {
                        Index = Index_Message,
                        Name = "Message",
                        Title = "工作消息",
                        ColumnName = "message",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_JobType,
                    new PropertySturct
                    {
                        Index = Index_JobType,
                        Name = "JobType",
                        Title = "任务分类",
                        ColumnName = "job_type",
                        PropertyType = typeof(UserJobType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_JobStatus,
                    new PropertySturct
                    {
                        Index = Index_JobStatus,
                        Name = "JobStatus",
                        Title = "工作状态",
                        ColumnName = "job_status",
                        PropertyType = typeof(JobStatusType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_CommandType,
                    new PropertySturct
                    {
                        Index = Index_CommandType,
                        Name = "CommandType",
                        Title = "命令类型",
                        ColumnName = "command_type",
                        PropertyType = typeof(JobCommandType),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_LinkId,
                    new PropertySturct
                    {
                        Index = Index_LinkId,
                        Name = "LinkId",
                        Title = "关联标识",
                        ColumnName = "link_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_EntityType,
                    new PropertySturct
                    {
                        Index = Index_EntityType,
                        Name = "EntityType",
                        Title = "连接类型",
                        ColumnName = "entity_type",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_ToUserId,
                    new PropertySturct
                    {
                        Index = Index_ToUserId,
                        Name = "ToUserId",
                        Title = "目标用户标识",
                        ColumnName = "to_user_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = false,
                        CanExport = false
                    }
                },
                {
                    Real_ToUserName,
                    new PropertySturct
                    {
                        Index = Index_ToUserName,
                        Name = "ToUserName",
                        Title = "目标用户名字",
                        ColumnName = "to_user_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_FromUserId,
                    new PropertySturct
                    {
                        Index = Index_FromUserId,
                        Name = "FromUserId",
                        Title = "来源用户标识",
                        ColumnName = "from_user_id",
                        PropertyType = typeof(int),
                        CanNull = false,
                        ValueType = PropertyValueType.Value,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_FromUserName,
                    new PropertySturct
                    {
                        Index = Index_FromUserName,
                        Name = "FromUserName",
                        Title = "来源用户名字",
                        ColumnName = "from_user_name",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = true,
                        CanExport = true
                    }
                },
                {
                    Real_Argument,
                    new PropertySturct
                    {
                        Index = Index_Argument,
                        Name = "Argument",
                        Title = "其它参数",
                        ColumnName = "argument",
                        PropertyType = typeof(string),
                        CanNull = false,
                        ValueType = PropertyValueType.String,
                        CanImport = false,
                        CanExport = false
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
                    Real_DepartmentId,
                    new PropertySturct
                    {
                        Index = Index_DepartmentId,
                        Name = "DepartmentId",
                        Title = "接收者部门",
                        ColumnName = "department_id",
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