
using Agebull.Common.OAuth;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 等级权限关联类型
    /// </summary>
    /// <remark>
    /// 等级权限关联类型
    /// </remark>
    public enum SubjectionType
    {
        /// <summary>
        /// 没有任何权限制
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 仅限本人的数据
        /// </summary>
        Self = 0x1,
        /// <summary>
        /// 本部门的数据
        /// </summary>
        Department = 0x2,
        /// <summary>
        /// 本部门及下级的数据
        /// </summary>
        DepartmentAndLower = 0x3,
        /// <summary>
        /// 本区域的数据
        /// </summary>
        Company = 0x4,
        /// <summary>
        /// 本区域及下级区域与部门的数据
        /// </summary>
        CompanyAndLower = 0x5,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 0x6,
    }
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class AuthEnumHelper
    {
        /// <summary>
        ///     权限枚举类型名称转换
        /// </summary>
        public static string ToCaption(this RolePowerType value)
        {
            switch (value)
            {
                case RolePowerType.None:
                    return "未设置";
                case RolePowerType.Allow:
                    return "允许";
                case RolePowerType.Deny:
                    return "拒绝";
                default:
                    return "权限枚举类型(未知)";
            }
        }

        /// <summary>
        ///     等级权限关联类型名称转换
        /// </summary>
        public static string ToCaption(this SubjectionType value)
        {
            switch (value)
            {
                case SubjectionType.None:
                    return "没有任何权限制";
                case SubjectionType.Self:
                    return "仅限本人的数据";
                case SubjectionType.Department:
                    return "本部门的数据";
                case SubjectionType.DepartmentAndLower:
                    return "本部门及下级的数据";
                case SubjectionType.Company:
                    return "本区域的数据";
                case SubjectionType.CompanyAndLower:
                    return "本区域及下级区域与部门的数据";
                case SubjectionType.Custom:
                    return "自定义";
                default:
                    return "等级权限关联类型(未知)";
            }
        }

    }
}