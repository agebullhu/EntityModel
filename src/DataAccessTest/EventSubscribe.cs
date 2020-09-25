
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zeroteam.MessageMVC.EventBus
{
    /// <summary>
    /// 事件订阅
    /// </summary>
    /// <remark>
    /// 事件订阅
    /// </remark>
    [DataContract, JsonObject(MemberSerialization.OptIn)]
    public partial class EventSubscribeData
    {

        /// <summary>
        ///  主键
        /// </summary>
        /// <example>
        ///     0
        /// </example>

        [DataMember, JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), ReadOnly(true), DisplayName(@"主键")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        ///  事件标识
        /// </summary>
        /// <example>
        ///     0
        /// </example>

        [DataMember, JsonProperty("EventId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"事件标识")]
        public long EventId
        {
            get;
            set;
        }
        /// <summary>
        ///  所属服务
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>

        [DataMember, JsonProperty("Service", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"所属服务")]
        public string Service
        {
            get;
            set;
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

        [DataMember, JsonProperty("IsLookUp", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"是否查阅服务")]
        public bool IsLookUp
        {
            get;
            set;
        }
        /// <summary>
        ///  接口名称
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>

        [DataMember, JsonProperty("ApiName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"接口名称")]
        public string ApiName
        {
            get;
            set;
        }
        /// <summary>
        ///  订阅备注
        /// </summary>

        [DataMember, JsonProperty("Memo", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"订阅备注")]
        public string Memo
        {
            get;
            set;
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

        [DataMember, JsonProperty("TargetName", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"目标名称")]
        public string TargetName
        {
            get;
            set;
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

        [DataMember, JsonProperty("TargetType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"目标类型")]
        public string TargetType
        {
            get;
            set;
        }
        /// <summary>
        ///  目标说明
        /// </summary>

        [DataMember, JsonProperty("TargetDescription", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"目标说明")]
        public string TargetDescription
        {
            get;
            set;
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

        [DataMember, JsonProperty("isFreeze", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"冻结更新")]
        public bool IsFreeze
        {
            get;
            set;
        }
        /// <summary>
        ///  数据状态
        /// </summary>
        /// <example>
        ///     0
        /// </example>

        [DataMember, JsonProperty("dataState", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"数据状态")]
        public DataStateType DataState
        {
            get;
            set;
        }

        /// <summary>
        ///  制作时间
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>

        [DataMember, JsonProperty("addDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"制作时间")]
        public DateTime AddDate
        {
            get;
            set;
        }
        /// <summary>
        ///  制作人标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>

        [DataMember, JsonIgnore, DisplayName(@"制作人标识")]
        public string AuthorId
        {
            get;
            set;
        }
        /// <summary>
        ///  制作人
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>

        [DataMember, JsonProperty("author", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"制作人")]
        public string Author
        {
            get;
            set;
        }
        /// <summary>
        ///  最后修改日期
        /// </summary>
        /// <example>
        ///     2012-12-21
        /// </example>

        [DataMember, JsonProperty("lastModifyDate", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"最后修改日期")]
        public DateTime LastModifyDate
        {
            get;
            set;
        }
        /// <summary>
        ///  最后修改者标识
        /// </summary>
        /// <value>
        ///     可存储32个字符.合理长度应不大于32.
        /// </value>

        [DataMember, JsonIgnore, DisplayName(@"最后修改者标识")]
        public string LastReviserId
        {
            get;
            set;
        }
        /// <summary>
        ///  最后修改者
        /// </summary>
        /// <value>
        ///     可存储200个字符.合理长度应不大于200.
        /// </value>

        [DataMember, JsonProperty("lastReviser", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore), DisplayName(@"最后修改者")]
        public string LastReviser
        {
            get;
            set;
        }
    }
}