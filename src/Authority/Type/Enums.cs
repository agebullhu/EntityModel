using System;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    ///     机构类型
    /// </summary>
    public enum OrganizationType
    {
        /// <summary>
        ///     未确定
        /// </summary>
        None,
        /// <summary>
        ///     行政区域
        /// </summary>
        Area,
        /// <summary>
        ///     机构
        /// </summary>
        Organization,
        
        /// <summary>
        ///     部门
        /// </summary>
        Department
    }

    /// <summary>
    /// 职位数据视角类型
    /// </summary>
    public enum PositionDataScopeType
    {
        /// <summary>
        /// 默认值，没有任何权限制
        /// </summary>
        None,
        /// <summary>
        /// 仅限本人的数据
        /// </summary>
        Self,
        /// <summary>
        /// 本部门的数据
        /// </summary>
        Department,
        /// <summary>
        /// 本部门及下级的数据
        /// </summary>
        DepartmentAndLower,
        /// <summary>
        /// 本公司的数据
        /// </summary>
        Company,
        /// <summary>
        /// 本公司及下级的数据
        /// </summary>
        CompanyAndLower,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
    public static class EnumHelperOrg
    {


        /// <summary>
        ///     权限范围枚举类型名称转换
        /// </summary>
        public static string ToCaption(this PositionDataScopeType value)
        {
            switch (value)
            {
                case PositionDataScopeType.None:
                    return "没有任何权限制";
                case PositionDataScopeType.Self:
                    return "仅限本人的数据";
                case PositionDataScopeType.Department:
                    return "本部门的数据";
                case PositionDataScopeType.DepartmentAndLower:
                    return "本部门及下级的数据";
                case PositionDataScopeType.Company:
                    return "本区域的数据";
                case PositionDataScopeType.CompanyAndLower:
                    return "本区域及下级区域与部门的数据";
                case PositionDataScopeType.Custom:
                    return "自定义";
                default:
                    return "权限范围枚举类型(未知)";
            }
        }

        /// <summary>
        ///     机构类型名称转换
        /// </summary>
        public static string ToCaption(this OrganizationType value)
        {
            switch (value)
            {
                case OrganizationType.None:
                    return "未确定";
                case OrganizationType.Area:
                    return "行政区域";
                case OrganizationType.Organization:
                    return "机构";
                case OrganizationType.Department:
                    return "部门";
                default:
                    return "机构类型(未知)";
            }
        }
    }
}