using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    /// 页面配置
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class PageConfig
    {
        /// <summary>
        /// 系统内部的类名
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemType;
        /// <summary>
        /// 是否显示
        /// </summary>
        [JsonProperty("hide", NullValueHandling = NullValueHandling.Ignore)]
        public bool Hide;
        /// <summary>
        /// 是否审批对象
        /// </summary>
        [JsonProperty("audit", NullValueHandling = NullValueHandling.Ignore)]
        public bool Audit;

        /// <summary>
        /// 能否逐级审批
        /// </summary>
        [JsonProperty("level_audit", NullValueHandling = NullValueHandling.Ignore)]
        public bool LevelAudit;

        /// <summary>
        /// 审批页面
        /// </summary>
        [JsonProperty("audit_page", NullValueHandling = NullValueHandling.Ignore)]
        public int AuditPage;

        /// <summary>
        /// 主页面
        /// </summary>
        [JsonProperty("master_page", NullValueHandling = NullValueHandling.Ignore)]
        public int MasterPage;

        /// <summary>
        /// 是否数据管理
        /// </summary>
        [JsonProperty("data_state", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataState;

        /// <summary>
        /// 标准编辑
        /// </summary>
        [JsonProperty("edit", NullValueHandling = NullValueHandling.Ignore)]
        public bool Edit;

    }
}