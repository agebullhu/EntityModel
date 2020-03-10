using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiFileResult : ApiResult
    {
        /// <summary>
        ///     文件内容二进制数据
        /// </summary>
        [JsonIgnore]
        public byte[] Data { get; set; }

        /// <summary>
        ///     文件名
        /// </summary>
        public string File => FileName;

        /// <summary>
        ///     文件类型
        /// </summary>
        public string Type => FileType;

        /// <summary>
        ///     文件名
        /// </summary>
        [JsonProperty("file")]
        public string FileName { get; set; }

        /// <summary>
        ///     文件MIME类型
        /// </summary>
        [JsonProperty("mime")]
        public string Mime { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        [JsonProperty("type")]
        public string FileType { get; set; }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ApiFileResult ErrorResult(int errCode, string message = null)
        {
            return new ApiFileResult
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }
    }
}