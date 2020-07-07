namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 组织边界对象
    /// </summary>
    public interface IOrganizationData<TPrimaryKey>
    {
        /// <summary>
        /// 组织标识
        /// </summary>
        TPrimaryKey OrganizationId { get; set; }
    }

    /// <summary>
    /// 组织边界对象
    /// </summary>
    public interface IOrganizationData: IOrganizationData<long>
    {
    }
}