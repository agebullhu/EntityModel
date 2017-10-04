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
    ///     表示登录用户对象
    /// </summary>
    public interface ILoginUser
    {
        /// <summary>
        ///     用户标识
        /// </summary>
        /// <remarks>
        ///     用户标识
        /// </remarks>
        int Id { get;  }

        /// <summary>
        ///     用户名
        /// </summary>
        /// <remarks>
        ///     用户名
        /// </remarks>
        string UserName { get;  }

        /// <summary>
        ///     角色ID
        /// </summary>
        /// <remarks>
        ///     角色ID
        /// </remarks>
        int RoleId { get; }

        /// <summary>
        ///     机构级别
        /// </summary>
        int DepartmentLevel { get; }

        /// <summary>
        ///     部门ID
        /// </summary>
        /// <remarks>
        ///     部门ID
        /// </remarks>
        int DepartmentId { get; }

        /// <summary>
        ///     姓名
        /// </summary>
        /// <remarks>
        ///     姓名
        /// </remarks>
        string RealName { get; }

    }
}