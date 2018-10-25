using Newtonsoft.Json;

namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     当前登录的用户信息
    /// </summary>
    public interface ILoginUserInfo
    {
        /// <summary>
        ///     用户数字标识
        /// </summary>
        long UserId { get; set; }
        /// <summary>
        ///     用户开放标识
        /// </summary>
        string OpenId { get; set; }

        /// <summary>
        ///     用户角色标识
        /// </summary>
        long RoleId { get; set; }

        /// <summary>
        ///     集团(医联体)标识
        /// </summary>
        long GroupId { get; set; }

        /// <summary>
        ///     公司(医院)标识
        /// </summary>
        long OrganizationId { get; set; }

        /// <summary>
        ///     部门(科室)标识
        /// </summary>
        long DepartmentId { get; set; }

        /// <summary>
        ///     当前用户登录方式
        /// </summary>
        int LoginType { get; set; }

        /// <summary>
        ///     登录者的手机号
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        ///     登录者的账号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        ///     登录设备的标识
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        ///     身份令牌
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        ///     登录设备的操作系统
        /// </summary>
        string Os { get; set; }

        /// <summary>
        ///     登录设备的应用
        /// </summary>
        string App { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        string NickName { get; set; }

        /// <summary>
        ///     头像链接
        /// </summary>
        /// <remarks>
        ///     头像链接
        /// </remarks>
        string AvatarUrl { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        UserStateType State { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        string Position { get; set; }

        /// <summary>
        ///     机构级别
        /// </summary>
        int DepartmentLevel { get; set; }

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        string Organization { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        string Role { get; set; }

        /// <summary>
        ///     部门
        /// </summary>
        /// <remarks>
        ///     部门
        /// </remarks>
        string Department { get; set; }

        /// <summary>
        ///     集团
        /// </summary>
        string Group { get; set; }
    }
}