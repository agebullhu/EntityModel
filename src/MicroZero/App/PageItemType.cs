namespace Agebull.Common.AppManage
{
    /// <summary>
    /// 页面节点类型
    /// </summary>
    public enum PageItemType
    {
        /// <summary>
        /// 顶级
        /// </summary>
        Root,
        /// <summary>
        /// 文件夹
        /// </summary>
        Folder,
        /// <summary>
        /// 页面
        /// </summary>
        Page,
        /// <summary>
        /// 按钮
        /// </summary>
        Button,
        /// <summary>
        /// 动作
        /// </summary>
        Action
    }
    /// <summary>
    /// 页面节点类型
    /// </summary>
    public static class PageItemTypeEx
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
    }
}