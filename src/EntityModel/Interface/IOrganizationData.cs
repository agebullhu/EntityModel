namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// ��֯�߽����
    /// </summary>
    public interface IOrganizationData<TPrimaryKey>
    {
        /// <summary>
        /// ��֯��ʶ
        /// </summary>
        TPrimaryKey OrganizationId { get; set; }
    }

    /// <summary>
    /// ��֯�߽����
    /// </summary>
    public interface IOrganizationData: IOrganizationData<long>
    {
    }
}