using Newtonsoft.Json;

namespace Agebull.EntityModel
{
    /// <summary>
    ///     实体信息
    /// </summary>
    [JsonObject]
    public class EntityInfo
    {
        /// <summary>
        /// 类型标识
        /// </summary>
        [JsonProperty("entityType")] public int EntityType { get; set; }
        /// <summary>
        /// 页面标识
        /// </summary>
        [JsonProperty("pageId")] public long PageId { get; set; }
    }
}