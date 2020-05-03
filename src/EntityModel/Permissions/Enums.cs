namespace Agebull.EntityModel.Permissions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class AuthEnumHelper
    {
        /// <summary>
        ///     节点类型枚举类型名称转换
        /// </summary>
        public static string ToCaption(this PageItemType value)
        {
            switch (value)
            {
                case PageItemType.Root:
                    return "顶级";
                case PageItemType.Folder:
                    return "文件夹";
                case PageItemType.Page:
                    return "页面";
                case PageItemType.Button:
                    return "按钮";
                case PageItemType.Action:
                    return "动作";
                default:
                    return "节点类型枚举类型(未知)";
            }
        }
        /// <summary>
        ///     权限范围枚举类型名称转换
        /// </summary>
        public static string ToCaption(this DataScopeType value)
        {
            switch (value)
            {
                case DataScopeType.None:
                    return "无";
                case DataScopeType.Person:
                    return "本人";
                case DataScopeType.Home:
                    return "本级";
                case DataScopeType.PersonAndHome:
                    return "本人及本级";
                case DataScopeType.Lower:
                    return "下级";
                case DataScopeType.HomeAndLower:
                    return "本级及以下";
                case DataScopeType.Full:
                    return "本人本级及下级";
                case DataScopeType.Unlimited:
                    return "无限制";
                default:
                    return "权限范围枚举类型(错误)";
            }
        }

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

    }
}