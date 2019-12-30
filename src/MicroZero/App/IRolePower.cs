// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using System;
using System.Collections.Generic;

namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     角色权限
    /// </summary>
    public interface IRolePower
    {
        /// <summary>
        ///     对象标识
        /// </summary>
        long Id { get; set; }

        /// <summary>
        ///     角色标识
        /// </summary>
        /// <remarks>
        ///     角色标识
        /// </remarks>
        long RoleId { get; set; }

        /// <summary>
        ///     页面节点标识
        /// </summary>
        /// <remarks>
        ///     页面节点标识
        /// </remarks>
        long PageItemId { get; set; }

        /// <summary>
        ///     权限
        /// </summary>
        /// <remarks>
        ///     权限,0表示未允许,1表示允许,2表示拒绝
        /// </remarks>
        RolePowerType Power { get; set; }
        
    }


    /// <summary>
    ///     角色权限
    /// </summary>
    public class SimpleRolePower : IRolePower
    {
        /// <summary>
        ///     对象标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     角色标识
        /// </summary>
        /// <remarks>
        ///     角色标识
        /// </remarks>
        public long RoleId { get; set; }

        /// <summary>
        ///     页面节点标识
        /// </summary>
        /// <remarks>
        ///     页面节点标识
        /// </remarks>
        public long PageItemId { get; set; }

        /// <summary>
        ///     权限
        /// </summary>
        /// <remarks>
        ///     权限,0表示未允许,1表示允许,2表示拒绝
        /// </remarks>
        public RolePowerType Power { get; set; }

    }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public interface IDuty
    {
        /// <summary>
        /// 岗位职责标识
        /// </summary>
        long DutyId { get; set; }

        /// <summary>
        /// 对应的权限角色设置
        /// </summary>
        long RoleId { get; set; }

        /// <summary>
        ///     组织标识
        /// </summary>
        long GroupId { get; set; }


        /// <summary>
        ///     组织标识
        /// </summary>
        long OrganizationId { get; set; }

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        string Organization { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        string Position { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        string Role { get; set; }


        /// <summary>
        /// 职责所在组织
        /// </summary>
        List<long> OrgIds { get; set; }

        /// <summary>
        /// 开始时间（对应到人）
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// 结束时间（对应到人）
        /// </summary>
        DateTime End { get; set; }
    }
}