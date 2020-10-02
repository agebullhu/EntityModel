/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 4:55:37*/
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
    public partial class EventDefaultModel  : IEditStatus , IIdentityData<long>
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public EventDefaultModel()
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
        [JsonProperty("id",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        public string _service;

        
        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("service",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string Service
        {
            get => this._service;
            set
            {
                if(this._service == value)
                    return;
                this._service = value;
                this.OnSeted(nameof(Service));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public bool _isLookUp;

        
        /// <summary>
        ///  是否查阅服务
        /// </summary>
        /// <remarks>
        ///     如为查阅服务，则发送后不处理与等待结果
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [JsonProperty("isLookUp",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  bool IsLookUp
        {
            get => this._isLookUp;
            set
            {
                if(this._isLookUp == value)
                    return;
                this._isLookUp = value;
                this.OnSeted(nameof(IsLookUp));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _apiName;

        
        /// <summary>
        ///  接口名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("apiName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string ApiName
        {
            get => this._apiName;
            set
            {
                if(this._apiName == value)
                    return;
                this._apiName = value;
                this.OnSeted(nameof(ApiName));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _eventSubscribeMemo;

        
        /// <summary>
        ///  订阅备注
        /// </summary>
        [JsonProperty("eventSubscribeMemo",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string EventSubscribeMemo
        {
            get => this._eventSubscribeMemo;
            set
            {
                if(this._eventSubscribeMemo == value)
                    return;
                this._eventSubscribeMemo = value;
                this.OnSeted(nameof(EventSubscribeMemo));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _eventSubscribeTargetName;

        
        /// <summary>
        ///  目标名称
        /// </summary>
        /// <remarks>
        ///     *表示所有目标
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("eventSubscribeTargetName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string EventSubscribeTargetName
        {
            get => this._eventSubscribeTargetName;
            set
            {
                if(this._eventSubscribeTargetName == value)
                    return;
                this._eventSubscribeTargetName = value;
                this.OnSeted(nameof(EventSubscribeTargetName));
            }
        }
        [IgnoreDataMember , JsonIgnore]
        public string _eventSubscribeTargetType;

        
        /// <summary>
        ///  目标类型
        /// </summary>
        /// <remarks>
        ///     *表示所有类型
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [JsonProperty("eventSubscribeTargetType",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
        public  string EventSubscribeTargetType
        {
            get => this._eventSubscribeTargetType;
            set
            {
                if(this._eventSubscribeTargetType == value)
                    return;
                this._eventSubscribeTargetType = value;
                this.OnSeted(nameof(EventSubscribeTargetType));
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
        [JsonProperty("eventName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("eventCode",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("version",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("region",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("eventType",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("resultOption",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("successOption",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("app",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("classify",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("tag",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("memo",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("targetType",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("targetName",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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
        [JsonProperty("targetDescription",  NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore)]
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