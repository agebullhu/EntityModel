
using System.Text;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// 表示API请求参数
    /// </summary>
    public interface IApiArgument
    {
        /// <summary>
        /// 转为Form的文本
        /// </summary>
        /// <returns></returns>
        string ToFormString();

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        bool Validate(out string message);
    }

    /// <summary>
    /// 请求参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument<T> : IApiArgument
    {
        /// <summary>
        /// AT
        /// </summary>
        [JsonProperty("v",NullValueHandling = NullValueHandling.Ignore)]
        public T Value { get; set; }



        string IApiArgument.ToFormString()
        {
            StringBuilder code = new StringBuilder();
            code.Append($"Value={Value}");
            return code.ToString();
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
    /// <summary>
    /// 分页请求的参数
    /// </summary>
    public class PageArgument : IApiArgument
    {
        /// <summary>
        /// 页号
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        [JsonProperty("pageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }


        /// <summary>
        /// 反序
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public bool Desc { get; set; }

        
        string IApiArgument.ToFormString()
        {
            return $"Page={Page}&PageSize={PageSize}&Order={Order}&Desc={Desc}";
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
}
