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
    /// 事件订阅
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class EventSubscribeData : IEditStatus, IIdentityData<long>, IStateData, IHistoryData
    {
        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        public EventSubscribeData()
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
        [IgnoreDataMember, JsonIgnore]
        public long _id;


        /// <summary>
        ///  主键
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [
        DataRule(CanNull = true), JsonProperty("id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id
        {
            get => this._id;
            set
            {
                if (this._id == value)
                    return;
                this._id = value;
                this.OnSeted(nameof(Id));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public long _eventId;


        /// <summary>
        ///  事件标识
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [
        DataRule(CanNull = true), JsonProperty("eventId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long EventId
        {
            get => this._eventId;
            set
            {
                if (this._eventId == value)
                    return;
                this._eventId = value;
                this.OnSeted(nameof(EventId));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _service;


        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("service", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Service
        {
            get => this._service;
            set
            {
                if (this._service == value)
                    return;
                this._service = value;
                this.OnSeted(nameof(Service));
            }
        }
        [IgnoreDataMember, JsonIgnore]
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
        [
        DataRule(CanNull = true), JsonProperty("isLookUp", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLookUp
        {
            get => this._isLookUp;
            set
            {
                if (this._isLookUp == value)
                    return;
                this._isLookUp = value;
                this.OnSeted(nameof(IsLookUp));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _apiName;


        /// <summary>
        ///  接口名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("apiName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ApiName
        {
            get => this._apiName;
            set
            {
                if (this._apiName == value)
                    return;
                this._apiName = value;
                this.OnSeted(nameof(ApiName));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _memo;


        /// <summary>
        ///  订阅备注
        /// </summary>
        [
        DataRule(CanNull = true), JsonProperty("memo", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Memo
        {
            get => this._memo;
            set
            {
                if (this._memo == value)
                    return;
                this._memo = value;
                this.OnSeted(nameof(Memo));
            }
        }
        [IgnoreDataMember, JsonIgnore]
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
        [
        DataRule(CanNull = true), JsonProperty("targetName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetName
        {
            get => this._targetName;
            set
            {
                if (this._targetName == value)
                    return;
                this._targetName = value;
                this.OnSeted(nameof(TargetName));
            }
        }
        [IgnoreDataMember, JsonIgnore]
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
        [
        DataRule(CanNull = true), JsonProperty("targetType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetType
        {
            get => this._targetType;
            set
            {
                if (this._targetType == value)
                    return;
                this._targetType = value;
                this.OnSeted(nameof(TargetType));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _targetDescription;


        /// <summary>
        ///  目标说明
        /// </summary>
        [
        DataRule(CanNull = true), JsonProperty("targetDescription", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetDescription
        {
            get => this._targetDescription;
            set
            {
                if (this._targetDescription == value)
                    return;
                this._targetDescription = value;
                this.OnSeted(nameof(TargetDescription));
            }
        }
        [IgnoreDataMember, JsonIgnore]
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
        [
        DataRule(CanNull = true), JsonProperty("isFreeze", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsFreeze
        {
            get => this._isFreeze;
            set
            {
                if (this._isFreeze == value)
                    return;
                this._isFreeze = value;
                this.OnSeted(nameof(IsFreeze));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public DataStateType _dataState;


        /// <summary>
        ///  数据状态
        /// </summary>
        /// <example>
        ///     0
        /// </example>
        [
        DataRule(CanNull = true), JsonProperty("dataState", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DataStateType DataState
        {
            get => this._dataState;
            set
            {
                if (this._dataState == value)
                    return;
                this._dataState = value;
                this.OnSeted(nameof(DataState));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public DateTime _addDate;


        /// <summary>
        ///  制作时间
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [
        DataRule(CanNull = true), JsonProperty("addDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(MyDateTimeConverter))]
        public DateTime AddDate
        {
            get => this._addDate;
            set
            {
                if (this._addDate == value)
                    return;
                this._addDate = value;
                this.OnSeted(nameof(AddDate));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _authorId;


        /// <summary>
        ///  制作人标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("authorId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AuthorId
        {
            get => this._authorId;
            set
            {
                if (this._authorId == value)
                    return;
                this._authorId = value;
                this.OnSeted(nameof(AuthorId));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _author;


        /// <summary>
        ///  制作人
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("author", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Author
        {
            get => this._author;
            set
            {
                if (this._author == value)
                    return;
                this._author = value;
                this.OnSeted(nameof(Author));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public DateTime _lastModifyDate;


        /// <summary>
        ///  最后修改日期
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>
        [
        DataRule(CanNull = true), JsonProperty("lastModifyDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(MyDateTimeConverter))]
        public DateTime LastModifyDate
        {
            get => this._lastModifyDate;
            set
            {
                if (this._lastModifyDate == value)
                    return;
                this._lastModifyDate = value;
                this.OnSeted(nameof(LastModifyDate));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _lastReviserId;


        /// <summary>
        ///  最后修改者标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("lastReviserId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastReviserId
        {
            get => this._lastReviserId;
            set
            {
                if (this._lastReviserId == value)
                    return;
                this._lastReviserId = value;
                this.OnSeted(nameof(LastReviserId));
            }
        }
        [IgnoreDataMember, JsonIgnore]
        public string _lastReviser;


        /// <summary>
        ///  最后修改者
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>
        [
        DataRule(CanNull = true), JsonProperty("lastReviser", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastReviser
        {
            get => this._lastReviser;
            set
            {
                if (this._lastReviser == value)
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
        [IgnoreDataMember, Browsable(false)]
        long IIdentityData<long>.Id
        {
            get => this.Id;
            set => this.Id = value;
        }
        #endregion


        #region 修改记录

        [DataMember, JsonProperty("editStatusRedorder", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        EntityEditStatus _editStatusRedorder;

        /// <summary>
        /// 修改状态
        /// </summary>
        EntityEditStatus IEditStatus.EditStatusRedorder { get => _editStatusRedorder; set => _editStatusRedorder = value; }

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