using System.ComponentModel;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Agebull.Common.Rpc
{
    /// <summary>
    /// 请求信息
    /// </summary>
    [DataContract, Category("请求信息"), JsonObject(MemberSerialization.OptIn)]
    public class RequestInfo
    {
        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="requestId"></param>
        public void SetValue(string callId, string requestId)
        {
            CallGlobalId = callId;
            RequestId = requestId;
        }
        /// <summary>
        /// 构造
        /// </summary>
        public RequestInfo() : this($"{GlobalContext.ServiceName}-{RandomOperate.Generate(6)}")
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requestId"></param>
        public RequestInfo(string requestId)
        {
            RequestId = requestId;
            Token = "?";
        }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="globalId"></param>
        /// <param name="requestId"></param>
        public RequestInfo(string callId, string globalId, string requestId) : this(requestId)
        {
            CallGlobalId = callId;
            LocalGlobalId = globalId;
            RequestId = requestId;
            Token = "?";
        }
        /// <summary>
        /// 当前的全局标识(本地)
        /// </summary>
        [JsonProperty("localGlobalId", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string LocalGlobalId { get; set; }

        /// <summary>
        /// 请求的全局标识(传递)
        /// </summary>
        [JsonProperty("callGlobalId", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string CallGlobalId { get; set; }

        /// <summary>
        /// 请求服务身份
        /// </summary>
        [JsonProperty("service", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Service => GlobalContext.ServiceName;

        /// <summary>
        /// 全局请求标识（源头为用户请求）
        /// </summary>
        [JsonProperty("requestId", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string RequestId { get; set; }

        /// <summary>
        /// 身份令牌
        /// </summary>
        /// <remarks>
        /// Http : Head中的Authorizaition节
        /// </remarks>
        [JsonProperty("token", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonProperty("argumentType", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public ArgumentType ArgumentType { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonProperty("requestType", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public RequestType RequestType { get; set; }

        /// <summary>
        ///     登录设备的操作系统
        /// </summary>
        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public string Os { get; set; }

        /// <summary>
        ///     当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        [JsonProperty("app", NullValueHandling = NullValueHandling.Ignore)]
        public string App { get; set; }

        /// <summary>
        /// 请求IP
        /// </summary>
        [JsonProperty("ip", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Ip { get; set; }

        /// <summary>
        /// 请求端口号
        /// </summary>
        [JsonProperty("port", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Port { get; set; }

        /// <summary>
        /// HTTP的UserAgent
        /// </summary>
        [JsonProperty("userAgent", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string UserAgent { get; set; }

    }
}