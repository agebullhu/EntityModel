/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/2 2:12:46*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;


#endregion

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件定义
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class EventDefaultEntity  : IEditStatus , IIdentityData<long> , IStateData , IHistoryData , IAuthorData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public EventDefaultEntity()
        {
            Initialize();
            InitEntityEditStatus();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();
        partial void InitEntityEditStatus();

        #endregion

        #region 基本属性


        /// <summary>
        /// 修改主键
        /// </summary>
        public void ChangePrimaryKey(long id)
        {
            _id = id;
        }
        /// <summary>
        /// 主键
        /// </summary>
        [IgnoreDataMember,JsonIgnore]
        public long _id;

        
        /// <summary>
        ///  主键
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("Id",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public long Id
        {
            get => this._id;
            set
            {
                if(this._id == value)
                    return;
                this._id = value;
                this.OnSeted(nameof(Id));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _eventName;

        
        /// <summary>
        ///  事件名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("EventName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string EventName
        {
            get => this._eventName;
            set
            {
                if(this._eventName == value)
                    return;
                this._eventName = value;
                this.OnSeted(nameof(EventName));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _eventCode;

        
        /// <summary>
        ///  事件编码
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("EventCode",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string EventCode
        {
            get => this._eventCode;
            set
            {
                if(this._eventCode == value)
                    return;
                this._eventCode = value;
                this.OnSeted(nameof(EventCode));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _version;

        
        /// <summary>
        ///  版本号
        /// </summary>
        /// <value>
        ///     可存储16个字符.合理长度应不大于16.
        /// </value>
        [JsonProperty("Version",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Version
        {
            get => this._version;
            set
            {
                if(this._version == value)
                    return;
                this._version = value;
                this.OnSeted(nameof(Version));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public RegionType _region;

        
        /// <summary>
        ///  领域范围
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("Region",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  RegionType Region
        {
            get => this._region;
            set
            {
                if(this._region == value)
                    return;
                this._region = value;
                this.OnSeted(nameof(Region));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public EventType _eventType;

        
        /// <summary>
        ///  事件类型
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("EventType",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  EventType EventType
        {
            get => this._eventType;
            set
            {
                if(this._eventType == value)
                    return;
                this._eventType = value;
                this.OnSeted(nameof(EventType));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public ResultOptionType _resultOption;

        
        /// <summary>
        ///  处理结果
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("ResultOption",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  ResultOptionType ResultOption
        {
            get => this._resultOption;
            set
            {
                if(this._resultOption == value)
                    return;
                this._resultOption = value;
                this.OnSeted(nameof(ResultOption));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public SuccessOptionType _successOption;

        
        /// <summary>
        ///  成功判断
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("SuccessOption",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  SuccessOptionType SuccessOption
        {
            get => this._successOption;
            set
            {
                if(this._successOption == value)
                    return;
                this._successOption = value;
                this.OnSeted(nameof(SuccessOption));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _app;

        
        /// <summary>
        ///  所属应用
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("App",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string App
        {
            get => this._app;
            set
            {
                if(this._app == value)
                    return;
                this._app = value;
                this.OnSeted(nameof(App));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _classify;

        
        /// <summary>
        ///  事件分类
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("Classify",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Classify
        {
            get => this._classify;
            set
            {
                if(this._classify == value)
                    return;
                this._classify = value;
                this.OnSeted(nameof(Classify));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _tag;

        
        /// <summary>
        ///  事件标签
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("Tag",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Tag
        {
            get => this._tag;
            set
            {
                if(this._tag == value)
                    return;
                this._tag = value;
                this.OnSeted(nameof(Tag));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _memo;

        
        /// <summary>
        ///  事件备注
        /// </summary>
        [JsonProperty("Memo",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Memo
        {
            get => this._memo;
            set
            {
                if(this._memo == value)
                    return;
                this._memo = value;
                this.OnSeted(nameof(Memo));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _targetType;

        
        /// <summary>
        ///  目标类型
        /// </summary>
        /// <remarks>
        ///     *表示所有类型
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("TargetType",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string TargetType
        {
            get => this._targetType;
            set
            {
                if(this._targetType == value)
                    return;
                this._targetType = value;
                this.OnSeted(nameof(TargetType));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _targetName;

        
        /// <summary>
        ///  目标名称
        /// </summary>
        /// <remarks>
        ///     *表示所有目标
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("TargetName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string TargetName
        {
            get => this._targetName;
            set
            {
                if(this._targetName == value)
                    return;
                this._targetName = value;
                this.OnSeted(nameof(TargetName));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _targetDescription;

        
        /// <summary>
        ///  目标说明
        /// </summary>
        [JsonProperty("TargetDescription",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string TargetDescription
        {
            get => this._targetDescription;
            set
            {
                if(this._targetDescription == value)
                    return;
                this._targetDescription = value;
                this.OnSeted(nameof(TargetDescription));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public bool _isFreeze;

        
        /// <summary>
        ///  冻结更新
        /// </summary>
        /// <remarks>
        ///     无论在什么数据状态,一旦设置且保存后,数据将不再允许执行Update的操作,作为Update的统一开关.取消的方法是单独设置这个字段的值
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [JsonProperty("isFreeze",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  bool IsFreeze
        {
            get => this._isFreeze;
            set
            {
                if(this._isFreeze == value)
                    return;
                this._isFreeze = value;
                this.OnSeted(nameof(IsFreeze));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public DataStateType _dataState;

        
        /// <summary>
        ///  数据状态
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [JsonProperty("dataState",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  DataStateType DataState
        {
            get => this._dataState;
            set
            {
                if(this._dataState == value)
                    return;
                this._dataState = value;
                this.OnSeted(nameof(DataState));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _authorId;

        
        /// <summary>
        ///  制作人标识
        /// </summary>
        /// <value>
        ///     用户提交时不能为空,后台保存时不能为空,
        /// </value>
        [JsonProperty("authorId",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string AuthorId
        {
            get => this._authorId;
            set
            {
                if(this._authorId == value)
                    return;
                this._authorId = value;
                this.OnSeted(nameof(AuthorId));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _author;

        
        /// <summary>
        ///  制作人
        /// </summary>
        /// <value>
        ///     用户提交时不能为空,后台保存时不能为空,可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("author",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Author
        {
            get => this._author;
            set
            {
                if(this._author == value)
                    return;
                this._author = value;
                this.OnSeted(nameof(Author));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public DateTime _lastModifyDate;

        
        /// <summary>
        ///  最后修改日期
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [JsonProperty("lastModifyDate",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter))]
        public  DateTime LastModifyDate
        {
            get => this._lastModifyDate;
            set
            {
                if(this._lastModifyDate == value)
                    return;
                this._lastModifyDate = value;
                this.OnSeted(nameof(LastModifyDate));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public DateTime _addDate;

        
        /// <summary>
        ///  制作时间
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [JsonProperty("addDate",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter))]
        public  DateTime AddDate
        {
            get => this._addDate;
            set
            {
                if(this._addDate == value)
                    return;
                this._addDate = value;
                this.OnSeted(nameof(AddDate));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _lastReviserId;

        
        /// <summary>
        ///  最后修改者标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [JsonProperty("lastReviserId",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string LastReviserId
        {
            get => this._lastReviserId;
            set
            {
                if(this._lastReviserId == value)
                    return;
                this._lastReviserId = value;
                this.OnSeted(nameof(LastReviserId));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _lastReviser;

        
        /// <summary>
        ///  最后修改者
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("lastReviser",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string LastReviser
        {
            get => this._lastReviser;
            set
            {
                if(this._lastReviser == value)
                    return;
                this._lastReviser = value;
                this.OnSeted(nameof(LastReviser));
            }
        }
        #endregion


        #region 接口属性


        /// <summary>
        /// 对象标识
        /// </summary>
        [IgnoreDataMember,Browsable(false)]
        long IIdentityData<long>.Id
        {
            get => this.Id;
            set => this.Id = value;
        }
        #endregion


        #region 修改记录

        [DataMember, JsonProperty("editStatusRedorder",  DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        EntityEditStatus _editStatusRedorder;

        /// <summary>
        /// 修改状态
        /// </summary>
        EntityEditStatus IEditStatus.EditStatusRedorder { get => _editStatusRedorder; set=>_editStatusRedorder = value; }

        /// <summary>
        /// 发出标准属性修改事件
        /// </summary>
        [Conditional("StandardPropertyChanged")]
        partial void InitEntityEditStatus()
        {
            _editStatusRedorder = new EntityEditStatus();
        }

        /// <summary>
        /// 发出标准属性修改事件
        /// </summary>
        [Conditional("StandardPropertyChanged")]
        void OnSeted(string name)
        {
            _editStatusRedorder.SetModified(name);
        }
        #endregion

    }
}