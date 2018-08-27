using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// API返回数组泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiArrayResult<TData> : ApiResult, IApiResult<List<TData>>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty("data")]
        public List<TData> ResultData { get; set; }

    }
}