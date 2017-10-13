
using System.Text;
using Gboxt.Common.SystemModel;
using Agebull.SystemAuthority.Organizations.BusinessLogic;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    /// 与组织机构关联的登录用户
    /// </summary>
    public class LoginUser : ILoginUser
    {
        /// <summary>
        /// 系统用户
        /// </summary>
        public static LoginUser SystemUser { get; } = new LoginUser
        {
            User = new UserData
            {
                RoleId = 1
            },
            Personnel = new PositionPersonnelData
            {
                Personnel = "系统用户",
                DepartmentId = 1,
                OrganizationId = 1,
                IsReadOnly = true
            },
            Position = new OrganizePositionData
            {
                OrganizationId = 1,
                Position = "系统用户"
            }
        };

        /// <summary>
        /// 非法用户
        /// </summary>
        public static LoginUser Anymouse { get; } = new LoginUser
        {
            User = new UserData
            {
                RoleId = -1
            },
            Personnel = new PositionPersonnelData
            {
                Personnel = "非法用户",
                OrganizationId = -1,
                RoleId = -1,
                DepartmentId = -1,
                OrgLevel = int.MaxValue,
                IsReadOnly = true
            },
            Position = new OrganizePositionData
            {
                Id = -1,
                OrganizationId = -1,
                Position = "非法用户",
                OrgLevel = int.MaxValue
            }
        };

        /// <summary> 
        /// 登录用户
        /// </summary>
        public UserData User { get; set; }

        /// <summary>
        /// 登录用户的员工信息
        /// </summary>
        public PositionPersonnelData Personnel { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public OrganizePositionData Position { get; set; }

        /// <summary>
        ///     用户标识
        /// </summary>
        /// <remarks>
        ///     用户标识
        /// </remarks>
        int ILoginUser.Id => User.Id;

        /// <summary>
        ///     用户名
        /// </summary>
        /// <remarks>
        ///     用户名
        /// </remarks>
        string ILoginUser.UserName => User.UserName;

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        public string PositionName => Personnel.Appellation;

        /// <summary>
        ///     姓名
        /// </summary>
        /// <remarks>
        ///     姓名
        /// </remarks>
        string ILoginUser.RealName => Personnel.Personnel;

        /// <summary>
        ///     机构级别
        /// </summary>
        int ILoginUser.DepartmentLevel => Position.OrgLevel;

        /// <summary>
        ///     部门ID
        /// </summary>
        /// <remarks>
        ///     部门ID
        /// </remarks>
        int ILoginUser.DepartmentId => Personnel.OrganizationId;

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        public string Organization => Personnel.Organization;

        /// <summary>
        ///     部门
        /// </summary>
        /// <remarks>
        ///     部门
        /// </remarks>
        public string Department => Position.Position;

        /// <summary>
        ///     角色ID
        /// </summary>
        /// <remarks>
        ///     角色ID
        /// </remarks>
        public int RoleId => User.RoleId > 0 
            ? User.RoleId
            : Personnel.RoleId == 0 
                ? Position.RoleId
                : Personnel.RoleId;

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns>
        /// 表示当前对象的字符串。
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("姓名:");
            sb.Append(Personnel.Personnel);
            sb.Append("<br/>部门:");
            sb.Append(Personnel.Organization);
            sb.Append("<br/>职位:");
            sb.Append(Personnel.Position);
            sb.Append("<br/>角色:");
            sb.Append(RoleCache.GetRole(RoleId).Caption);
            return sb.ToString();
        }
    }
}