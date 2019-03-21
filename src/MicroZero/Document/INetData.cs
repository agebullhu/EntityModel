using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示网络数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public interface INetData
    {
        /// <summary>
        ///     机器
        /// </summary>
        [DataMember]
        [JsonProperty("machine")] string Machine { get; set; }

        /// <summary>
        ///     用户
        /// </summary>
        [DataMember]
        [JsonProperty("user")] string User { get; set; }

        /// <summary>
        ///     请求ID
        /// </summary>
        [DataMember]
        [JsonProperty("requestId")]
        string RequestId { get; set; }
    }
}