using System.Collections.Generic;
using Agebull.Common;
using Agebull.Common.Context;
using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     API返回数组泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiArrayResult<TData> : ApiResult, IApiResult<List<TData>>
    {
        /// <inheritdoc />
        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("data")]
        public List<TData> ResultData { get; set; }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error(int errCode)
        {
            return new ApiArrayResult<TData>
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error(int errCode, string message)
        {
            return new ApiArrayResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error(int errCode, string message, string innerMessage)
        {
            return new ApiArrayResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage
                }
            };
        }
        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error(int errCode, string message, string innerMessage, string guide, string describe)
        {
            return new ApiArrayResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error(int errCode, string message, string innerMessage, string point, string guide, string describe)
        {
            return new ApiArrayResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    Point = point,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiArrayResult<TData> Succees(List<TData> data, string message = null)
        {
            return message == null
                ? new ApiArrayResult<TData>
                {
                    Success = true,
                    ResultData = data
                }
                : new ApiArrayResult<TData>
                {
                    Success = true,
                    ResultData = data,
                    Status = new OperatorStatus
                    {
                        ClientMessage = message
                    }
                };
        }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public new static ApiArrayResult<TData> Error()
        {
            return new ApiArrayResult<TData>
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

    }
}