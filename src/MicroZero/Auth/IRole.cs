using System;
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
    public interface IApp
    {
        /// <summary>
        ///     用户角色标识
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        ///     登录设备的应用
        /// </summary>
        string AppName { get; set; }

        /// <summary>
        ///     登录设备的应用
        /// </summary>
        string AppKey { get; set; }
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
        OrganizationDataScopeType DataScope { get; }

        
    }


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