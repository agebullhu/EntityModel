using System;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// 内部服务调用上下文（跨系统边界传递）
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InternalCallContext
    {
        /// <summary>
        /// 请求服务身份
        /// </summary>
        [JsonProperty("s", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Guid ServiceKey { get; set; }

        /// <summary>
        /// 全局请求标识（源头为用户请求）
        /// </summary>
        [JsonProperty("r", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Guid RequestId { get; set; }

        /// <summary>
        /// 当前请求的用户标识
        /// </summary>
        [JsonProperty("u", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public long UserId { get; set; }

        /// <summary>
        /// 线程标识
        /// </summary>
        [JsonIgnore]
        public long ThreadId { get; set; }
        /*
        /// <summary>
        /// 当前请求原始参数（来自URL或FORM）
        /// </summary>
        [JsonProperty("a", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string RequestArgument { get; set; }*/
    }
}