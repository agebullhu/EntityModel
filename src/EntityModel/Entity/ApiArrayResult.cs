using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     API返回数组泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiArrayResult<TData> : ApiResult, IApiResult<List<TData>>
    {
        /// <summary>
        ///     返回列表
        /// </summary>
        public List<TData> Data => ResultData;

        /// <summary>
        ///     返回列表
        /// </summary>
        [JsonProperty("data")]
        public List<TData> ResultData { get; set; }
    }
    /// <summary>
    ///     API返回数组泛型类
    /// </summary>
    public class ApiPageResult : ApiResult<ApiPage>
    {
    }

    /// <summary>
    ///     API返回数组泛型类
    /// </summary>
    public class ApiPageResult<TData> : ApiResult<ApiPageData<TData>>
    {
    }
}