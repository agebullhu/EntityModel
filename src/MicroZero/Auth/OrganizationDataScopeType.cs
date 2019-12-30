using System;

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 组织行级权限范围枚举类型
    /// </summary>
    /// <remark>
    /// 基于组织树形关系的范围关系
    /// </remark>
    [Flags]
    public enum OrganizationDataScopeType
    {
        /// <summary>
        /// 没有任何权限制
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 本人的数据
        /// </summary>
        Person = 0x1,
        /// <summary>
        /// 本级数据
        /// </summary>
        Home = 0x2,
        /// <summary>
        /// 下级数据
        /// </summary>
        Sub = 0x4,
    }
}