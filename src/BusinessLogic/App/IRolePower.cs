// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 角色权限类型
    /// </summary>
    public enum RolePowerType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None,
        /// <summary>
        /// 允许
        /// </summary>
        Allow,
        /// <summary>
        /// 拒绝
        /// </summary>
        Deny
    }
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
}