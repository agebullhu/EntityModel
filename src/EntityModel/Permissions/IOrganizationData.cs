using Agebull.EntityModel.Permissions;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// ��֯�߽����
    /// </summary>
    public interface IOrganizationData
    {
        /// <summary>
        /// ��֯��ʶ
        /// </summary>
        string OrganizationId { get; set; }

    }

    /// <summary>
    /// ���ű߽����
    /// </summary>
    public interface IDepartmentData
    {
        /// <summary>
        /// ���ű�ʶ
        /// </summary>
        string DepartmentId { get; set; }

        /// <summary>
        /// ���ű�ʶ
        /// </summary>
        string DepartmentCode { get; set; }
    }

    /// <summary>
    /// �������ݷ�Χ����
    /// </summary>
    public interface IDepartmentScopeData
    {
        /// <summary>
        /// ���ݷ�Χ
        /// </summary>
        DataScopeType DataScope { get; set; }
    }
}
