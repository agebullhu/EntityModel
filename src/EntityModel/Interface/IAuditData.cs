// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示这条数据支持审核
    /// </summary>
    public interface IAuditData
    {
        /// <summary>
        ///     审核状态
        /// </summary>
        /// <value>AuditStateType</value>
        AuditStateType AuditState { get; set; }

        /// <summary>
        ///     审核人
        /// </summary>
        /// <value>string</value>
        string AuditorId { get; set; }


        /// <summary>
        ///     审核人
        /// </summary>
        /// <value>string</value>
        string Auditor { get; set; }


        /// <summary>
        ///     审核日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime AuditDate { get; set; }

        ///// <summary>
        /////     审核人(仅用于界面)
        ///// </summary>
        ///// <value>string</value>
        //string ToUsers { get; set; }

        ///// <summary>
        /////     能否审核(仅用于界面)
        ///// </summary>
        ///// <value>bool</value>
        //bool CanAudit { get; set; }
    }
}