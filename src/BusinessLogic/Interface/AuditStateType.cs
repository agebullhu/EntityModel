﻿/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立:2016-06-07
修改: -
*****************************************************/

#region 引用


#endregion

namespace Agebull.EntityModel.Common
{
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
        ///     重新处理
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
        State
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
                case AuditStateType.End:
                    return "终审通过";
                case AuditStateType.Again:
                    return "重新处理";
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