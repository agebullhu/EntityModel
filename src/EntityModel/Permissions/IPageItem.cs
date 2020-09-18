// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Agebull.EntityModel.Permissions
{
    /// <summary>
    ///     表示页面节点
    /// </summary>
    public interface IPageItem
    {
        /// <summary>
        ///     对象标识
        /// </summary>
        long Id { get; set; }

        /// <summary>
        ///     序号
        /// </summary>
        /// <remarks>
        ///     序号
        /// </remarks>
        int Index { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        PageItemType ItemType { get; set; }

        /// <summary>
        ///     目录
        /// </summary>
        /// <remarks>
        ///     目录
        /// </remarks>
        string Path { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        /// <remarks>
        ///     图标
        /// </remarks>
        string Icon { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        /// <remarks>
        ///     名称
        /// </remarks>
        string Name { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        /// <remarks>
        ///     标题
        /// </remarks>
        string Caption { get; set; }

        /// <summary>
        ///     页面连接
        /// </summary>
        /// <remarks>
        ///     页面连接
        /// </remarks>
        string Url { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        /// <remarks>
        ///     备注
        /// </remarks>
        string Memo { get; set; }

        /// <summary>
        ///     上级节点
        /// </summary>
        /// <remarks>
        ///     上级节点
        /// </remarks>
        long ParentId { get; set; }

        /// <summary>
        ///     不显示
        /// </summary>
        bool IsHide { get; set; }

        /// <summary>
        ///     无权限控制的公开页面
        /// </summary>
        bool IsPublic { get; set; }
    }
}