using Agebull.Common.Rpc;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     API状态返回接口实现
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OperatorStatus : IOperatorStatus
    {
        /// <summary>
        ///     默认构造
        /// </summary>
        public OperatorStatus()
        {
            Point = GlobalContext.ServiceRealName;
        }

        /// <summary>
        ///     默认构造
        /// </summary>
        public OperatorStatus(int code, string messgae)
        {
            ErrorCode = code;
            ClientMessage = messgae;
        }

        /// <summary>
        ///     指导码（系统定义）
        /// </summary>
        /// <remarks>
        /// 指示下一步应如何处理的代码
        /// </remarks>
        /// <example>retry</example>
        [JsonProperty("guide")]
        public string GuideCode { get; set; }

        /// <summary>
        ///     错误说明
        /// </summary>
        /// <remarks>
        /// 详细说明错误内容
        /// </remarks>
        /// <example>系统未就绪</example>
        [JsonProperty("describe")]
        public string Describe { get; set; }

        /// <summary>
        ///     错误点（系统定义）
        /// </summary>
        /// <remarks>
        /// 系统在哪一个节点发生错误的标识
        /// </remarks>
        /// <example>1</example>
        [JsonProperty("point")]
        public string Point { get; set; }

        /// <summary>
        ///     错误码
        /// </summary>
        /// <remarks>
        ///     参见 ErrorCode 说明
        /// </remarks>
        /// <example>-1</example>
        [JsonProperty("code")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///     对应HTTP错误码（参考）
        /// </summary>
        /// <example>404</example>
        [JsonProperty("http")]
        public string HttpCode { get; set; } = "200";

        /// <summary>
        ///     提示信息
        /// </summary>
        /// <example>你的数据不正确</example>
        [JsonProperty("msg")]
        public string ClientMessage { get; set; }

        /// <summary>
        ///     内部提示信息
        /// </summary>
        public string InnerMessage { get; set; }
    }
}