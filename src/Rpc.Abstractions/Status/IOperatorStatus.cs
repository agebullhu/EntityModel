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
        ///     错误点（系统定义）
        /// </summary>
        [JsonProperty("point")]
        string Point { get; set; }

        /// <summary>
        ///     指导码（系统定义）
        /// </summary>
        [JsonProperty("guide")]
        string GuideCode { get; set; }

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
        ///     错误说明
        /// </summary>
        [JsonProperty("describe")]
        string Describe { get; set; }

        /// <summary>
        ///     内部信息
        /// </summary>
        string InnerMessage { get; set; }
    }
}