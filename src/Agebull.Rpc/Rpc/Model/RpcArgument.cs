
using System;
using Newtonsoft.Json;

namespace Agebull.Zmq.Rpc
{


    /// <summary>
    /// 命令调用参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RpcArgument
    {
        /// <summary>
        /// 请求者标识（如果需要回发，则不能为空，因为这是回发接收者的全局标识）
        /// </summary>
        [JsonProperty("ClientKey", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientKey { get; set; }

        /// <summary>
        /// 请求标识（不可为空）
        /// </summary>
        [JsonProperty("RequestId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid RequestId { get; set; }

        /// <summary>
        /// 请求用户信息（可为空）
        /// </summary>
        [JsonProperty("UserContext", NullValueHandling = NullValueHandling.Ignore)]
        public UserContext UserInfo { get; set; }

        /// <summary>
        /// 命令参数标识，为0表示无参数，其实应使用RpcArgument
        /// </summary>
        [JsonProperty("ArgumentTypeId", NullValueHandling = NullValueHandling.Ignore)]
        public int ArgumentTypeId { get; set; }

    }

    /// <summary>
    /// 命令调用参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class RpcArgument<T> : RpcArgument
    {
        /// <summary>
        /// 参数
        /// </summary>
        [JsonProperty("Argument", NullValueHandling = NullValueHandling.Ignore)]
        public T Argument { get; set; }
    }

    /// <summary>
    /// 用户请求的上下文信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UserContext
    {
        /// <summary>
        /// App设备标识（可为空）
        /// </summary>
        [JsonProperty("DeviceKey", NullValueHandling = NullValueHandling.Ignore)]
        public Guid DeviceKey { get; set; } = Guid.Empty;
        /// <summary>
        /// 请求用户标识（可为空）
        /// </summary>
        [JsonProperty("UserId", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }
        /// <summary>
        /// 用户请求SessionId（可为空）
        /// </summary>
        [JsonProperty("SessionId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid SessionId { get; set; }
    }
}
