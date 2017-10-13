
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Agebull.ProjectDeveloper.WebDomain.Models;
using Gboxt.Common.DataModel;
using Gboxt.Common.Workflow;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 职位组织关联
    /// </summary>
    [DataContract]
    sealed partial class OrganizePositionData : AutoJobEntity, IWorkflowData
    {
        /// <summary>
        ///     标题
        /// </summary>
        /// <value>int</value>
        string ITitle.Title => $"职位设置：{Position}";
    }
}