using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 表示API返回的列表
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal class ApiList<TData> : List<TData>, IApiResultData
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="list"></param>
        public ApiList(IList<TData> list)
        {
            if (list == null)
                return;
            AddRange(list);
        }
    }

    /// <summary>
    /// API返回数组泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiArrayResult<TData> : ApiResult, IApiResult<ApiList<TData>>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [JsonIgnore]
        ApiList<TData> IApiResult<ApiList<TData>>.ResultData => new ApiList<TData>(ResultData);

        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty("data")]
        public List<TData> ResultData { get; set; }

    }
}