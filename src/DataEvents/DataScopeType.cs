﻿#region 引用

#endregion

namespace Agebull.EntityModel.DataEvents
{
    /// <summary>
    /// 行级权限类型
    /// </summary>
    /// <remark>
    /// 行级权限类型
    /// </remark>
    internal enum DataScopeType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 本人
        /// </summary>
        Person = 0x1,
        /// <summary>
        /// 本级
        /// </summary>
        Home = 0x2,
        /// <summary>
        /// 本人及本级
        /// </summary>
        PersonAndHome = 0x3,
        /// <summary>
        /// 下级
        /// </summary>
        Lower = 0x4,
        /// <summary>
        /// 本级及以下
        /// </summary>
        HomeAndLower = 0x6,
        /// <summary>
        /// 本人本级及下级
        /// </summary>
        Full = 0x7,
        /// <summary>
        /// 无限制
        /// </summary>
        Unlimited = 0x8,
    }

}