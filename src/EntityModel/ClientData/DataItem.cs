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
        /// 名称
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }
}