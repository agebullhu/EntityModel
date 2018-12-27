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

namespace Agebull.Common.DataModel
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
        long AuditorId { get; set; }


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

    /// <summary>
    ///     审核状态类型
    /// </summary>
    public enum AuditStateType
    {
        /// <summary>
        ///     草稿
        /// </summary>
        None,

        /// <summary>
        ///     反审核
        /// </summary>
        Again,

        /// <summary>
        ///     提交审核
        /// </summary>
        Submit,

        /// <summary>
        ///     审核不通过
        /// </summary>
        Deny,

        /// <summary>
        ///     审核通过
        /// </summary>
        Pass,

        /// <summary>
        ///     结束
        /// </summary>
        End,
        /// <summary>
        /// 不正确的状态
        /// </summary>
        Error
    }
    /// <summary>
    /// 审核状态扩展类
    /// </summary>
    public static class AuditStateTypeHelper
    {
        /// <summary>
        ///     到中文名称
        /// </summary>
        /// <param name="state">审核状态类型</param>
        /// <returns>中文名称</returns>
        public static string ToCaption(this AuditStateType state)
        {
            switch (state)
            {
                case AuditStateType.Submit:
                    return "提交审核";
                case AuditStateType.Deny:
                    return "已否决";
                case AuditStateType.Pass:
                    return "已通过";
                case AuditStateType.Again:
                    return "反审核";
                case AuditStateType.End:
                    return "结束";
                case AuditStateType.None:
                    return "草稿";
                default:
                    return "错误";
            }
        }
        /// <summary>
        ///     到中文名称
        /// </summary>
        /// <param name="state">审核状态类型</param>
        /// <returns>中文名称</returns>
        public static string ToChiness(this AuditStateType state)
        {
            switch (state)
            {
                case AuditStateType.Submit:
                    return "提交审核";
                case AuditStateType.Deny:
                    return "已否决";
                case AuditStateType.Pass:
                    return "已通过";
                case AuditStateType.Again:
                    return "反审核";
                case AuditStateType.End:
                    return "结束";
                case AuditStateType.None:
                    return "草稿";
                default:
                    return "错误";
            }
        }

        /// <summary>
        ///     是否可以审核
        /// </summary>
        public static bool CanAudit(this IAuditData data)
        {
            switch (data.AuditState)
            {
                case AuditStateType.None:
                case AuditStateType.Again:
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     是否可以反审核
        /// </summary>
        public static bool CanAgainAudit(this IAuditData data)
        {
            switch (data.AuditState)
            {
                case AuditStateType.Pass:
                    return true;
            }
            return false;
        }
    }
}