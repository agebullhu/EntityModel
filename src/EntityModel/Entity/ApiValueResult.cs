using Newtonsoft.Json;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiValueResult : ApiResult
    {
        /// <summary>
        ///     返回值
        /// </summary>
        public string Data => ResultData;

        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("data")]
        public string ResultData { get; set; }
    }


    /// <summary>
    ///     API返回单数据数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiValueResult<TData> : ApiResult
    {
        /// <summary>
        ///     返回值
        /// </summary>
        /// <example>0</example>
        [JsonProperty("data")]
        public TData ResultData { get; set; }

    }
}