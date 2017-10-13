
using System;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;
using Gboxt.Common.Workflow;
using Newtonsoft.Json;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 员工职位关联
    /// </summary>
    [DataContract, Serializable]
    sealed partial class PositionPersonnelData : AutoJobEntity, IStateData, IWorkflowData
    {
        /// <summary>
        ///     标题
        /// </summary>
        /// <value>int</value>
        string ITitle.Title => $"职位分配：{Personnel}({Department}{Position})";
        
        /// <summary>
        /// 职位全称
        /// </summary>
        [JsonProperty("OrganizationPosition", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationPosition => Department + Appellation;
    }
}