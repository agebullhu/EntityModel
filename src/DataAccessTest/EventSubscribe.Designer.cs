/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/9/16 10:40:07*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;


using Agebull.Common;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;


#endregion

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class EventSubscribeData : IStateData , IHistoryData , IIdentityData
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public EventSubscribeData()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();
        #endregion

        #region 基本属性

        /// <summary>
        /// 发出标准属性修改事件
        /// </summary>
        [Conditional("StandardPropertyChanged")]
        void OnSeted(string name) => OnPropertyChanged(name);



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

        partial void OnIdGet();

        partial void OnIdSet(ref long value);

        partial void OnIdLoad(ref long value);

        partial void OnIdSeted();

        
        /// <summary>
        ///  主键
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , ReadOnly(true) , DisplayName(@"主键")]
        public long Id
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
                OnIdSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_Id);
                this.OnSeted(nameof(Id));
            }
        }
        
        /// <summary>
        ///  事件标识
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public long _eventId;

        partial void OnEventIdGet();

        partial void OnEventIdSet(ref long value);

        partial void OnEventIdSeted();

        
        /// <summary>
        ///  事件标识
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("EventId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"事件标识")]
        public  long EventId
        {
            get
            {
                OnEventIdGet();
                return this._eventId;
            }
            set
            {
                if(this._eventId == value)
                    return;
                OnEventIdSet(ref value);
                this._eventId = value;
                OnEventIdSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_EventId);
                this.OnSeted(nameof(EventId));
            }
        }
        
        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _service;

        partial void OnServiceGet();

        partial void OnServiceSet(ref string value);

        partial void OnServiceSeted();

        
        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("Service", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"所属服务")]
        public  string Service
        {
            get
            {
                OnServiceGet();
                return this._service;
            }
            set
            {
                if(this._service == value)
                    return;
                OnServiceSet(ref value);
                this._service = value;
                OnServiceSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_Service);
                this.OnSeted(nameof(Service));
            }
        }
        
        /// <summary>
        ///  是否查阅服务
        /// </summary>
        /// <remarks>
        ///     如为查阅服务，则发送后不处理与等待结果
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public bool _isLookUp;

        partial void OnIsLookUpGet();

        partial void OnIsLookUpSet(ref bool value);

        partial void OnIsLookUpSeted();

        
        /// <summary>
        ///  是否查阅服务
        /// </summary>
        /// <remarks>
        ///     如为查阅服务，则发送后不处理与等待结果
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("IsLookUp", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"是否查阅服务")]
        public  bool IsLookUp
        {
            get
            {
                OnIsLookUpGet();
                return this._isLookUp;
            }
            set
            {
                if(this._isLookUp == value)
                    return;
                OnIsLookUpSet(ref value);
                this._isLookUp = value;
                OnIsLookUpSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_IsLookUp);
                this.OnSeted(nameof(IsLookUp));
            }
        }
        
        /// <summary>
        ///  接口名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _apiName;

        partial void OnApiNameGet();

        partial void OnApiNameSet(ref string value);

        partial void OnApiNameSeted();

        
        /// <summary>
        ///  接口名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("ApiName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"接口名称")]
        public  string ApiName
        {
            get
            {
                OnApiNameGet();
                return this._apiName;
            }
            set
            {
                if(this._apiName == value)
                    return;
                OnApiNameSet(ref value);
                this._apiName = value;
                OnApiNameSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_ApiName);
                this.OnSeted(nameof(ApiName));
            }
        }
        
        /// <summary>
        ///  目标说明
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _targetDescription;

        partial void OnTargetDescriptionGet();

        partial void OnTargetDescriptionSet(ref string value);

        partial void OnTargetDescriptionSeted();

        
        /// <summary>
        ///  目标说明
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("TargetDescription", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"目标说明")]
        public  string TargetDescription
        {
            get
            {
                OnTargetDescriptionGet();
                return this._targetDescription;
            }
            set
            {
                if(this._targetDescription == value)
                    return;
                OnTargetDescriptionSet(ref value);
                this._targetDescription = value;
                OnTargetDescriptionSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_TargetDescription);
                this.OnSeted(nameof(TargetDescription));
            }
        }
        
        /// <summary>
        ///  目标名称
        /// </summary>
        /// <remarks>
        ///     *表示所有目标
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _targetName;

        partial void OnTargetNameGet();

        partial void OnTargetNameSet(ref string value);

        partial void OnTargetNameSeted();

        
        /// <summary>
        ///  目标名称
        /// </summary>
        /// <remarks>
        ///     *表示所有目标
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("TargetName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"目标名称")]
        public  string TargetName
        {
            get
            {
                OnTargetNameGet();
                return this._targetName;
            }
            set
            {
                if(this._targetName == value)
                    return;
                OnTargetNameSet(ref value);
                this._targetName = value;
                OnTargetNameSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_TargetName);
                this.OnSeted(nameof(TargetName));
            }
        }
        
        /// <summary>
        ///  目标类型
        /// </summary>
        /// <remarks>
        ///     *表示所有类型
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _targetType;

        partial void OnTargetTypeGet();

        partial void OnTargetTypeSet(ref string value);

        partial void OnTargetTypeSeted();

        
        /// <summary>
        ///  目标类型
        /// </summary>
        /// <remarks>
        ///     *表示所有类型
        /// </remarks>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("TargetType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"目标类型")]
        public  string TargetType
        {
            get
            {
                OnTargetTypeGet();
                return this._targetType;
            }
            set
            {
                if(this._targetType == value)
                    return;
                OnTargetTypeSet(ref value);
                this._targetType = value;
                OnTargetTypeSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_TargetType);
                this.OnSeted(nameof(TargetType));
            }
        }
        
        /// <summary>
        ///  备注
        /// </summary>
        [IgnoreDataMember,JsonIgnore]
        public string _memo;

        partial void OnMemoGet();

        partial void OnMemoSet(ref string value);

        partial void OnMemoSeted();

        
        /// <summary>
        ///  备注
        /// </summary>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("Memo", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"备注")]
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
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_Memo);
                this.OnSeted(nameof(Memo));
            }
        }
        
        /// <summary>
        ///  冻结更新
        /// </summary>
        /// <remarks>
        ///     无论在什么数据状态,一旦设置且保存后,数据将不再允许执行Update的操作,作为Update的统一开关.取消的方法是单独设置这个字段的值
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public bool _isFreeze;

        partial void OnIsFreezeGet();

        partial void OnIsFreezeSet(ref bool value);

        partial void OnIsFreezeSeted();

        
        /// <summary>
        ///  冻结更新
        /// </summary>
        /// <remarks>
        ///     无论在什么数据状态,一旦设置且保存后,数据将不再允许执行Update的操作,作为Update的统一开关.取消的方法是单独设置这个字段的值
        /// </remarks>
        /// <example>
        ///     true
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("isFreeze", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"冻结更新")]
        public  bool IsFreeze
        {
            get
            {
                OnIsFreezeGet();
                return this._isFreeze;
            }
            set
            {
                if(this._isFreeze == value)
                    return;
                OnIsFreezeSet(ref value);
                this._isFreeze = value;
                OnIsFreezeSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_IsFreeze);
                this.OnSeted(nameof(IsFreeze));
            }
        }
        
        /// <summary>
        ///  数据状态
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public DataStateType _dataState;

        partial void OnDataStateGet();

        partial void OnDataStateSet(ref DataStateType value);

        partial void OnDataStateSeted();

        
        /// <summary>
        ///  数据状态
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("dataState", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"数据状态")]
        public  DataStateType DataState
        {
            get
            {
                OnDataStateGet();
                return this._dataState;
            }
            set
            {
                if(this._dataState == value)
                    return;
                OnDataStateSet(ref value);
                this._dataState = value;
                OnDataStateSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_DataState);
                this.OnSeted(nameof(DataState));
            }
        }
        /// <summary>
        /// 数据状态的可读内容
        /// </summary>
        [IgnoreDataMember,JsonIgnore,DisplayName("数据状态")]
        public string DataState_Content => DataState.ToCaption();

        /// <summary>
        /// 数据状态的数字属性
        /// </summary>
        [IgnoreDataMember,JsonIgnore]
        public  int DataState_Number
        {
            get => (int)this.DataState;
            set => this.DataState = (DataStateType)value;
        }
        
        /// <summary>
        ///  制作时间
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public DateTime _addDate;

        partial void OnAddDateGet();

        partial void OnAddDateSet(ref DateTime value);

        partial void OnAddDateSeted();

        
        /// <summary>
        ///  制作时间
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("addDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"制作时间")]
        public  DateTime AddDate
        {
            get
            {
                OnAddDateGet();
                return this._addDate;
            }
            set
            {
                if(this._addDate == value)
                    return;
                OnAddDateSet(ref value);
                this._addDate = value;
                OnAddDateSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_AddDate);
                this.OnSeted(nameof(AddDate));
            }
        }
        
        /// <summary>
        ///  制作人标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _authorId;

        partial void OnAuthorIdGet();

        partial void OnAuthorIdSet(ref string value);

        partial void OnAuthorIdSeted();

        
        /// <summary>
        ///  制作人标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("authorId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"制作人标识")]
        public  string AuthorId
        {
            get
            {
                OnAuthorIdGet();
                return this._authorId;
            }
            set
            {
                if(this._authorId == value)
                    return;
                OnAuthorIdSet(ref value);
                this._authorId = value;
                OnAuthorIdSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_AuthorId);
                this.OnSeted(nameof(AuthorId));
            }
        }
        
        /// <summary>
        ///  制作人
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _author;

        partial void OnAuthorGet();

        partial void OnAuthorSet(ref string value);

        partial void OnAuthorSeted();

        
        /// <summary>
        ///  制作人
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("author", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"制作人")]
        public  string Author
        {
            get
            {
                OnAuthorGet();
                return this._author;
            }
            set
            {
                if(this._author == value)
                    return;
                OnAuthorSet(ref value);
                this._author = value;
                OnAuthorSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_Author);
                this.OnSeted(nameof(Author));
            }
        }
        
        /// <summary>
        ///  最后修改日期
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [IgnoreDataMember,JsonIgnore]
        public DateTime _lastModifyDate;

        partial void OnLastModifyDateGet();

        partial void OnLastModifyDateSet(ref DateTime value);

        partial void OnLastModifyDateSeted();

        
        /// <summary>
        ///  最后修改日期
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("lastModifyDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , JsonConverter(typeof(MyDateTimeConverter)) , DisplayName(@"最后修改日期")]
        public  DateTime LastModifyDate
        {
            get
            {
                OnLastModifyDateGet();
                return this._lastModifyDate;
            }
            set
            {
                if(this._lastModifyDate == value)
                    return;
                OnLastModifyDateSet(ref value);
                this._lastModifyDate = value;
                OnLastModifyDateSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_LastModifyDate);
                this.OnSeted(nameof(LastModifyDate));
            }
        }
        
        /// <summary>
        ///  最后修改者标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _lastReviserId;

        partial void OnLastReviserIdGet();

        partial void OnLastReviserIdSet(ref string value);

        partial void OnLastReviserIdSeted();

        
        /// <summary>
        ///  最后修改者标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("lastReviserId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"最后修改者标识")]
        public  string LastReviserId
        {
            get
            {
                OnLastReviserIdGet();
                return this._lastReviserId;
            }
            set
            {
                if(this._lastReviserId == value)
                    return;
                OnLastReviserIdSet(ref value);
                this._lastReviserId = value;
                OnLastReviserIdSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_LastReviserId);
                this.OnSeted(nameof(LastReviserId));
            }
        }
        
        /// <summary>
        ///  最后修改者
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [IgnoreDataMember,JsonIgnore]
        public string _lastReviser;

        partial void OnLastReviserGet();

        partial void OnLastReviserSet(ref string value);

        partial void OnLastReviserSeted();

        
        /// <summary>
        ///  最后修改者
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [DataRule(CanNull = true)]
        [DataMember , JsonProperty("lastReviser", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling= DefaultValueHandling.Ignore) , DisplayName(@"最后修改者")]
        public  string LastReviser
        {
            get
            {
                OnLastReviserGet();
                return this._lastReviser;
            }
            set
            {
                if(this._lastReviser == value)
                    return;
                OnLastReviserSet(ref value);
                this._lastReviser = value;
                OnLastReviserSeted();
                this.OnPropertyChanged(EventSubscribeDataStruct.Real_LastReviser);
                this.OnSeted(nameof(LastReviser));
            }
        }

        #region 接口属性

        #endregion
        #region 扩展属性

        #endregion
        #endregion

        #region 名称的属性操作

    

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected override bool SetValueInner(string property, string value)
        {
            if(property == null) return false;
            switch(property.Trim().ToLower())
            {
            case "id":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (long.TryParse(value, out var vl))
                    {
                        this.Id = vl;
                        return true;
                    }
                }
                return false;
            case "eventid":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (long.TryParse(value, out var vl))
                    {
                        this.EventId = vl;
                        return true;
                    }
                }
                return false;
            case "service":
                this.Service = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "islookup":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (bool.TryParse(value, out var vl))
                    {
                        this.IsLookUp = vl;
                        return true;
                    }
                }
                return false;
            case "apiname":
                this.ApiName = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "targetdescription":
                this.TargetDescription = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "targetname":
                this.TargetName = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "targettype":
                this.TargetType = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "memo":
                this.Memo = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "isfreeze":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (bool.TryParse(value, out var vl))
                    {
                        this.IsFreeze = vl;
                        return true;
                    }
                }
                return false;
            case "datastate":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (DataStateType.TryParse(value, out DataStateType val))
                    {
                        this.DataState = val;
                        return true;
                    }
                    else if (int.TryParse(value, out int vl))
                    {
                        this.DataState = (DataStateType)vl;
                        return true;
                    }
                }
                return false;
            case "adddate":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParse(value, out var vl))
                    {
                        this.AddDate = vl;
                        return true;
                    }
                }
                return false;
            case "authorid":
                this.AuthorId = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "author":
                this.Author = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "lastmodifydate":
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParse(value, out var vl))
                    {
                        this.LastModifyDate = vl;
                        return true;
                    }
                }
                return false;
            case "lastreviserid":
                this.LastReviserId = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            case "lastreviser":
                this.LastReviser = string.IsNullOrWhiteSpace(value) ? null : value;
                return true;
            }
            return false;
        }

    

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected override void SetValueInner(string property, object value)
        {
            if(property == null) return;
            switch(property.Trim().ToLower())
            {
            case "id":
                this.Id = (long)Convert.ToDecimal(value);
                return;
            case "eventid":
                this.EventId = (long)Convert.ToDecimal(value);
                return;
            case "service":
                this.Service = value == null ? null : value.ToString();
                return;
            case "islookup":
                if (value != null)
                {
                    int vl;
                    if (int.TryParse(value.ToString(), out vl))
                    {
                        this.IsLookUp = vl != 0;
                    }
                    else
                    {
                        this.IsLookUp = Convert.ToBoolean(value);
                    }
                }
                return;
            case "apiname":
                this.ApiName = value == null ? null : value.ToString();
                return;
            case "targetdescription":
                this.TargetDescription = value == null ? null : value.ToString();
                return;
            case "targetname":
                this.TargetName = value == null ? null : value.ToString();
                return;
            case "targettype":
                this.TargetType = value == null ? null : value.ToString();
                return;
            case "memo":
                this.Memo = value == null ? null : value.ToString();
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
            case "adddate":
                this.AddDate = Convert.ToDateTime(value);
                return;
            case "authorid":
                this.AuthorId = value == null ? null : value.ToString();
                return;
            case "author":
                this.Author = value == null ? null : value.ToString();
                return;
            case "lastmodifydate":
                this.LastModifyDate = Convert.ToDateTime(value);
                return;
            case "lastreviserid":
                this.LastReviserId = value == null ? null : value.ToString();
                return;
            case "lastreviser":
                this.LastReviser = value == null ? null : value.ToString();
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
            switch(index)
            {
            case EventSubscribeDataStruct.Id:
                this.Id = Convert.ToInt64(value);
                return;
            case EventSubscribeDataStruct.EventId:
                this.EventId = Convert.ToInt64(value);
                return;
            case EventSubscribeDataStruct.Service:
                this.Service = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.IsLookUp:
                this.IsLookUp = Convert.ToBoolean(value);
                return;
            case EventSubscribeDataStruct.ApiName:
                this.ApiName = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.TargetDescription:
                this.TargetDescription = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.TargetName:
                this.TargetName = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.TargetType:
                this.TargetType = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.Memo:
                this.Memo = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.IsFreeze:
                this.IsFreeze = Convert.ToBoolean(value);
                return;
            case EventSubscribeDataStruct.DataState:
                this.DataState = (DataStateType)value;
                return;
            case EventSubscribeDataStruct.AddDate:
                this.AddDate = Convert.ToDateTime(value);
                return;
            case EventSubscribeDataStruct.AuthorId:
                this.AuthorId = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.Author:
                this.Author = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.LastModifyDate:
                this.LastModifyDate = Convert.ToDateTime(value);
                return;
            case EventSubscribeDataStruct.LastReviserId:
                this.LastReviserId = value == null ? null : value.ToString();
                return;
            case EventSubscribeDataStruct.LastReviser:
                this.LastReviser = value == null ? null : value.ToString();
                return;
            }
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="property"></param>
        protected override object GetValueInner(string property)
        {
            switch(property)
            {
            case "id":
                return this.Id;
            case "eventid":
                return this.EventId;
            case "service":
                return this.Service;
            case "islookup":
                return this.IsLookUp;
            case "apiname":
                return this.ApiName;
            case "targetdescription":
                return this.TargetDescription;
            case "targetname":
                return this.TargetName;
            case "targettype":
                return this.TargetType;
            case "memo":
                return this.Memo;
            case "isfreeze":
                return this.IsFreeze;
            case "datastate":
                return this.DataState.ToCaption();
            case "adddate":
                return this.AddDate;
            case "authorid":
                return this.AuthorId;
            case "author":
                return this.Author;
            case "lastmodifydate":
                return this.LastModifyDate;
            case "lastreviserid":
                return this.LastReviserId;
            case "lastreviser":
                return this.LastReviser;
            }

            return null;
        }


        /// <summary>
        ///     读取属性值
        /// </summary>
        /// <param name="index"></param>
        protected override object GetValueInner(int index)
        {
            switch(index)
            {
                case EventSubscribeDataStruct.Id:
                    return this.Id;
                case EventSubscribeDataStruct.EventId:
                    return this.EventId;
                case EventSubscribeDataStruct.Service:
                    return this.Service;
                case EventSubscribeDataStruct.IsLookUp:
                    return this.IsLookUp;
                case EventSubscribeDataStruct.ApiName:
                    return this.ApiName;
                case EventSubscribeDataStruct.TargetDescription:
                    return this.TargetDescription;
                case EventSubscribeDataStruct.TargetName:
                    return this.TargetName;
                case EventSubscribeDataStruct.TargetType:
                    return this.TargetType;
                case EventSubscribeDataStruct.Memo:
                    return this.Memo;
                case EventSubscribeDataStruct.IsFreeze:
                    return this.IsFreeze;
                case EventSubscribeDataStruct.DataState:
                    return this.DataState;
                case EventSubscribeDataStruct.AddDate:
                    return this.AddDate;
                case EventSubscribeDataStruct.AuthorId:
                    return this.AuthorId;
                case EventSubscribeDataStruct.Author:
                    return this.Author;
                case EventSubscribeDataStruct.LastModifyDate:
                    return this.LastModifyDate;
                case EventSubscribeDataStruct.LastReviserId:
                    return this.LastReviserId;
                case EventSubscribeDataStruct.LastReviser:
                    return this.LastReviser;
            }

            return null;
        }

        #endregion

        #region 复制
        

        partial void CopyExtendValue(EventSubscribeData source);

        /// <summary>
        /// 复制值
        /// </summary>
        /// <param name="source">复制的源字段</param>
        protected override void CopyValueInner(DataObjectBase source)
        {
            var sourceEntity = source as EventSubscribeData;
            if(sourceEntity == null)
                return;
            this._id = sourceEntity._id;
            this._eventId = sourceEntity._eventId;
            this._service = sourceEntity._service;
            this._isLookUp = sourceEntity._isLookUp;
            this._apiName = sourceEntity._apiName;
            this._targetDescription = sourceEntity._targetDescription;
            this._targetName = sourceEntity._targetName;
            this._targetType = sourceEntity._targetType;
            this._memo = sourceEntity._memo;
            this._isFreeze = sourceEntity._isFreeze;
            this._dataState = sourceEntity._dataState;
            this._addDate = sourceEntity._addDate;
            this._authorId = sourceEntity._authorId;
            this._author = sourceEntity._author;
            this._lastModifyDate = sourceEntity._lastModifyDate;
            this._lastReviserId = sourceEntity._lastReviserId;
            this._lastReviser = sourceEntity._lastReviser;
            CopyExtendValue(sourceEntity);
            this.__status.IsModified = true;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="source">复制的源字段</param>
        public void Copy(EventSubscribeData source)
        {
                this.Id = source.Id;
                this.EventId = source.EventId;
                this.Service = source.Service;
                this.IsLookUp = source.IsLookUp;
                this.ApiName = source.ApiName;
                this.TargetDescription = source.TargetDescription;
                this.TargetName = source.TargetName;
                this.TargetType = source.TargetType;
                this.Memo = source.Memo;
                this.IsFreeze = source.IsFreeze;
                this.DataState = source.DataState;
                this.AddDate = source.AddDate;
                this.AuthorId = source.AuthorId;
                this.Author = source.Author;
                this.LastModifyDate = source.LastModifyDate;
                this.LastReviserId = source.LastReviserId;
                this.LastReviser = source.LastReviser;
        }
        #endregion

        #region 数据结构

        /// <summary>
        /// 实体结构
        /// </summary>
        [IgnoreDataMember,Browsable (false)]
        public override EntitySturct __Struct
        {
            get
            {
                return EventSubscribeDataStruct.Struct;
            }
        }
        #endregion

    }
}