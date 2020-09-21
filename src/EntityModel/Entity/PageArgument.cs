using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     分页请求的参数
    /// </summary>
    public class PageArgument : IApiArgument
    {
        /// <summary>
        ///     页号
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int PageIndex { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        [JsonProperty("pageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }


        /// <summary>
        ///     反序
        /// </summary>
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public bool Desc { get; set; }


        /// <summary>
        ///     数据校验
        /// </summary>
        /// <param name="status">检查状态</param>
        /// <returns>成功则返回真</returns>
        public virtual bool Validate(out IOperatorStatus status)
        {
            status = ApiResultHelper.Succees(); 
            var msg = new StringBuilder();
            status.Success = true;
            if (PageIndex < 0)
            {
                status.Success = false;
                msg.Append("页号必须大于或等于0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                status.Success = false;
                msg.Append("行数必须大于0且小于100");
            }
            if (status.Success)
            {
                return true;
            }
            status.Message= msg.ToString();
            status.Code = OperatorStatusCode.ArgumentError;
            return status.Success;
        }
    }

    /// <summary>
    ///     API返回分页信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
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
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiPageData<TEntity> : ApiPage
    {
        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("rows")]
        public List<TEntity> Rows { get; set; }
    }
}