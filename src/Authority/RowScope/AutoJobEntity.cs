using System;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// 表示为逐级审批的数据
    /// </summary>
    public interface ILevelAuditData : IAuditData
    {
        /// <summary>
        /// 当前审批级别
        /// </summary>
        int DepartmentLevel { get; set; }

        /// <summary>
        /// 终审级别
        /// </summary>
        int LastLevel { get; }
    }

    /// <summary>
    ///     表示这条数据支持审核
    /// </summary>
    [DataContract, Serializable]
    public abstract class AutoJobEntity : EditDataObject
    {
        /// <summary>
        ///     审核人
        /// </summary>
        /// <value>string</value>
        [DataMember]
        public string ToUsers { get; set; }

        /// <summary>
        ///     审核人
        /// </summary>
        /// <value>string</value>
        [DataMember]
        public bool CanAudit { get; set; }
    }
}