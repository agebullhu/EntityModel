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
        /// 上级ID
        /// </summary>
        /// <example>1</example>
        [JsonProperty("pid"), JsonPropertyName("pid")]
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// <example>名称</example>
        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// 简单的数据节点
    /// </summary>
    public class DataItem<T>
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <example>1</example>
        [JsonProperty("id"), JsonPropertyName("id")]
        public T Id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        /// <example>1</example>
        [JsonProperty("pid"), JsonPropertyName("pid")]
        public T ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// <example>名称</example>
        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }
}