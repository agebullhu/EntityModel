using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     API返回基类
    /// </summary>
    public interface IApiResult
    {
        /// <summary>
        ///     成功或失败标记
        /// </summary>
        [JsonProperty("success")]
        bool Success { get; set; }

        /// <summary>
        ///     API执行状态（为空表示状态正常）
        /// </summary>
        [JsonProperty("status")]
        IOperatorStatus Status { get; set; }

        /// <summary>
        ///     API请求标识
        /// </summary>
        [JsonProperty("requestId")]
        string RequestId { get; set; }


        /// <summary>
        ///     API操作标识
        /// </summary>
        [JsonProperty("operatorId")]
        string OperatorId { get; set; }

        /// <summary>
        ///     错误码（系统定义）
        /// </summary>
        [JsonProperty("code")]
        int ErrorCode { get; }

        /// <summary>
        ///     客户端信息
        /// </summary>
        [JsonProperty("msg")]
        string Message { get; }
    }

    /// <summary>
    ///     API返回基类
    /// </summary>
    public interface IApiResult<out TData> : IApiResult
    {
        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("data")]
        TData ResultData { get; }
    }
}