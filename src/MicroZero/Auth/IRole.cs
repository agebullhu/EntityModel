using System.Collections.Generic;

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 角色节点
    /// </summary>
    public interface IRole
    {
        /// <summary>
        ///     用户角色标识
        /// </summary>
        long RoleId { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        string Role { get; set; }

        /// <summary>
        ///     所在的应用
        /// </summary>
        string AppName { get; set; }

        /// <summary>
        ///     公司标识
        /// </summary>
        long OrganizationId { get; }
        
        /// <summary>
        /// 权限节点
        /// </summary>
        List<IRoleItem> Items { get; }
    }


    /// <summary>
    /// 角色节点类型
    /// </summary>
    public enum RoleItemType
    {
        /// <summary>
        /// 不明确
        /// </summary>
        None,
        /// <summary>
        /// API站点
        /// </summary>
        Station,
        /// <summary>
        /// API接口
        /// </summary>
        Api,
        /// <summary>
        /// 任务
        /// </summary>
        Task,
        /// <summary>
        /// 页面
        /// </summary>
        Page
    }

    /// <summary>
    /// 角色节点
    /// </summary>
    public interface IRoleItem
    {
        /// <summary>
        ///     公司标识
        /// </summary>
        long OrganizationId { get; }

        /// <summary>
        /// 节点类型
        /// </summary>
        RoleItemType ItemType { get; }

        /// <summary>
        ///     节点名称
        /// </summary>
        string ItemName { get; }

        /// <summary>
        /// 是否有权限
        /// </summary>
        bool CanDo { get; }

        /// <summary>
        /// 行级权限
        /// </summary>
        DataScopeType DataScope { get; }

        
    }
}