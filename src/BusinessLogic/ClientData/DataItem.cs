using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Agebull.EntityModel.Vue
{
    /// <summary>
    /// 简单的数据节点
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <example>1</example>
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// <example>名称</example>
        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }
}