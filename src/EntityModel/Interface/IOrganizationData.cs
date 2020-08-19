using Agebull.EntityModel.Permissions;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 组织边界对象
    /// </summary>
    public interface IOrganizationData
    {
        /// <summary>
        /// 组织标识
        /// </summary>
        string OrganizationId { get; set; }

    }

    /// <summary>
    /// 部门边界对象
    /// </summary>
    public interface IDepartmentData
    {
        /// <summary>
        /// 部门标识
        /// </summary>
        string DepartmentId { get; set; }

        /// <summary>
        /// 部门标识
        /// </summary>
        string DepartmentCode { get; set; }
    }

    /// <summary>
    /// 部门数据范围对象
    /// </summary>
    public interface IDepartmentScopeData
    {
        /// <summary>
        /// 数据范围
        /// </summary>
        DataScopeType DataScope { get; set; }
    }
}
