namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 组织信息接口
    /// </summary>
    public interface IOrganizational
    {
        /// <summary>
        /// 组织的唯一标识(数字)
        /// </summary>
        long OrgId { get; set; }

        /// <summary>
        /// 组织的唯一标识(文字)
        /// </summary>
        string OrgKey { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 组织的路由名称
        /// </summary>
        string RouteName { get; set; }

    }
}