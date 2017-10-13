
using System.Text;
using Gboxt.Common.SystemModel;

namespace Agebull.Common.DataModel.SystemModel
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
        };

        /// <summary>
        /// 非法用户
        /// </summary>
        public static LoginUser Anymouse { get; } = new LoginUser
        {
        };
        
        /// <summary>
        ///     用户标识
        /// </summary>
        /// <remarks>
        ///     用户标识
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        /// <remarks>
        ///     用户名
        /// </remarks>
        public string UserName { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        public string PositionName { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        /// <remarks>
        ///     姓名
        /// </remarks>
        public string RealName { get; set; } = "测试";

        /// <summary>
        ///     机构级别
        /// </summary>
        public int DepartmentLevel { get; set; }

        /// <summary>
        ///     部门ID
        /// </summary>
        /// <remarks>
        ///     部门ID
        /// </remarks>
        public int DepartmentId { get; set; }

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        public string Organization { get; set; }

        /// <summary>
        ///     部门
        /// </summary>
        /// <remarks>
        ///     部门
        /// </remarks>
        public string Department { get; set; }

        /// <summary>
        ///     角色ID
        /// </summary>
        /// <remarks>
        ///     角色ID
        /// </remarks>
        public int RoleId => 0;

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
            sb.Append(RealName);
            sb.Append("<br/>部门:*");
            sb.Append("<br/>职位:*");
            sb.Append("<br/>角色:*");
            return sb.ToString();
        }
    }
}