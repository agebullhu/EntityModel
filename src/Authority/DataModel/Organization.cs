using System.Runtime.Serialization;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Gboxt.Common.DataModel;
using Gboxt.Common.Workflow;
using Newtonsoft.Json;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [DataContract]
    sealed partial class OrganizationData : AutoJobEntity, IWorkflowData
    {
        /// <summary>
        ///     标题
        /// </summary>
        /// <value>int</value>
        string ITitle.Title => $"组织机构：{FullName}";
        /// <summary>
        ///     是否已选择
        /// </summary>
        [JsonProperty("checked")]
        public bool IsSelect { get; set; }

        /// <summary>
        ///     表格树关闭状态
        /// </summary>
        [JsonProperty("state")]
        public string TreeState
        {
            get { return "open"; }
        }

        /// <summary>
        ///     是否展开
        /// </summary>
        [JsonProperty("IsOpen")]
        public bool IsOpen { get; set; }

        //[JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        //public List<OrganizationData> Childrens { get; set; } = new List<OrganizationData>();
        

        /// <summary>
        ///     子级
        /// </summary>
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public OrganizationData[] Children { get; set; }

        public EasyUiTreeNode ToEasyUiTreeNode()
        {
            return new EasyUiTreeNode
            {
                ID = Id,
                Icon = Type == OrganizationType.Organization ? "icon-com" : "icon-bm",
                IsOpen = Type != OrganizationType.Organization,
                Attributes = "org",
                Text = FullName
            }; 
        }
    }
}