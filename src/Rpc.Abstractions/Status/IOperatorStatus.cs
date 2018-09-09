using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     操作状态
    /// </summary>
    public interface IOperatorStatus
    {
        /// <summary>
        ///     错误码（系统定义）
        /// </summary>
        [JsonProperty("code")]
        int ErrorCode { get; set; }

        /// <summary>
        ///     对应HTTP错误码（参考）
        /// </summary>
        [JsonProperty("http")]
        string HttpCode { get; set; }

        /// <summary>
        ///     客户端信息
        /// </summary>
        [JsonProperty("msg")]
        string ClientMessage { get; set; }

        /// <summary>
        ///     内部信息
        /// </summary>
        string InnerMessage { get; set; }
    }

    /// <summary>
    ///     API状态返回接口实现
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OperatorStatus : IOperatorStatus
    {
        /// <summary>
        ///     默认构造
        /// </summary>
        public OperatorStatus()
        {
        }

        /// <summary>
        ///     默认构造
        /// </summary>
        public OperatorStatus(int code, string messgae)
        {
            ErrorCode = code;
            ClientMessage = messgae;
        }

        /// <summary>
        ///     错误码
        /// </summary>
        /// <remarks>
        ///     参见 ErrorCode 说明
        /// </remarks>
        /// <example>-1</example>
        [JsonProperty("code")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///     对应HTTP错误码（参考）
        /// </summary>
        /// <example>404</example>
        [JsonProperty("http")]
        public string HttpCode { get; set; } = "200";

        /// <summary>
        ///     提示信息
        /// </summary>
        /// <example>你的数据不正确</example>
        [JsonProperty("msg")]
        public string ClientMessage { get; set; }

        /// <summary>
        ///     内部提示信息
        /// </summary>
        public string InnerMessage { get; set; }
    }
}