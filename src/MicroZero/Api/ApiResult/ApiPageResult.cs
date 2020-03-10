using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     API返回分页信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPage
    {
        /// <summary>
        ///     当前页号（从1开始）
        /// </summary>
        /// <example>1</example>
        public int Page => PageIndex;

        /// <summary>
        ///     当前页号（从1开始）
        /// </summary>
        /// <example>1</example>
        [JsonProperty("page")]
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页行数
        /// </summary>
        /// <example>16</example>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        /// <example>999</example>
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        /// <summary>
        ///     总行数
        /// </summary>
        /// <example>9999</example>
        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        /// <summary>
        ///     总行数
        /// </summary>
        /// <example>9999</example>
        [JsonProperty("total", Required = Required.Always)]
        public int Total => RowCount;
    }

    /// <summary>
    ///     API返回分布页数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPageData<TData> : ApiPage
    {
        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("rows")]
        public List<TData> Rows { get; set; }
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