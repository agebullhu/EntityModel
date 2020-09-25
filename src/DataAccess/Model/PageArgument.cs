using System.Collections.Generic;
using System.Text;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     分页请求的参数
    /// </summary>
    public class PageArgument //: IApiArgument
    {
        /// <summary>
        ///     页号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     每页行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public string Order { get; set; }


        /// <summary>
        ///     反序
        /// </summary>
        public bool Desc { get; set; }


        /// <summary>
        ///     数据校验
        /// </summary>
        /// <param name="message">检查状态</param>
        /// <returns>成功则返回真</returns>
        public bool Validate(out string message)
        {
            var msg = new StringBuilder();
            var success = true;
            if (PageIndex < 0)
            {
                success = false;
                msg.Append("页号必须大于或等于0");
            }

            if (PageSize <= 0 || PageSize > 100)
            {
                success = false;
                msg.Append("行数必须大于0且小于100");
            }
            message = success ? null : msg.ToString();
            return success;
        }
    }

    /// <summary>
    ///     API返回分页信息
    /// </summary>
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
        public int PageIndex { get; set; }

        /// <summary>
        ///     一页行数
        /// </summary>
        /// <example>16</example>
        public int PageSize { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        /// <example>999</example>
        public long PageCount { get; set; }

        /// <summary>
        ///     总行数
        /// </summary>
        /// <example>9999</example>
        public long RowCount { get; set; }

    }

    /// <summary>
    ///     API返回分布页数据
    /// </summary>
    public class ApiPageData<TEntity> : ApiPage
    {
        /// <summary>
        ///     返回值
        /// </summary>
        public List<TEntity> Rows { get; set; }
    }
}