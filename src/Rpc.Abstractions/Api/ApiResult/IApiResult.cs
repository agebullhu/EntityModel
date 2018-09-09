using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
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