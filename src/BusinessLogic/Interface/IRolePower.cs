// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    ///     角色权限
    /// </summary>
    public interface IRolePower
    {
        /// <summary>
        ///     对象标识
        /// </summary>
        int Id { get; set; }

        /// <summary>
        ///     角色标识
        /// </summary>
        /// <remarks>
        ///     角色标识
        /// </remarks>
        int RoleId { get; set; }

        /// <summary>
        ///     页面节点标识
        /// </summary>
        /// <remarks>
        ///     页面节点标识
        /// </remarks>
        int PageItemId { get; set; }

        /// <summary>
        ///     权限
        /// </summary>
        /// <remarks>
        ///     权限,0表示未允许,1表示允许,2表示拒绝
        /// </remarks>
        int Power { get; set; }
        
    }


    /// <summary>
    ///     角色权限
    /// </summary>
    public class SimpleRolePower : IRolePower
    {
        /// <summary>
        ///     对象标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     角色标识
        /// </summary>
        /// <remarks>
        ///     角色标识
        /// </remarks>
        public int RoleId { get; set; }

        /// <summary>
        ///     页面节点标识
        /// </summary>
        /// <remarks>
        ///     页面节点标识
        /// </remarks>
        public int PageItemId { get; set; }

        /// <summary>
        ///     权限
        /// </summary>
        /// <remarks>
        ///     权限,0表示未允许,1表示允许,2表示拒绝
        /// </remarks>
        public int Power { get; set; }

    }
}