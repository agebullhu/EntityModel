using Newtonsoft.Json;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    ///     API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiFileResult : ApiResult
    {
        /// <summary>
        ///     文件内容二进制数据
        /// </summary>
        public byte[] Bytes { get; set; }

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
        [JsonProperty("file", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FileName { get; set; }

        /// <summary>
        ///     文件MIME类型
        /// </summary>
        [JsonProperty("mime", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Mime { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string FileType { get; set; }

    }
}