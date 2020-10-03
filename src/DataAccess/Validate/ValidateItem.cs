using Newtonsoft.Json;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 校验节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ValidateItem
    {
        /// <summary>
        /// 正确
        /// </summary>
        [JsonProperty("succeed", NullValueHandling = NullValueHandling.Ignore)]
        public bool Succeed { get; set; }

        /// <summary>
        /// 1警告
        /// </summary>
        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public bool Warning { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// 字段标题目
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}