using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.ComponentModel;

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

    /// <summary>
    /// 当前发生业务的组织
    /// </summary>
    [DataContract, Category("组织信息")]
    [JsonObject(MemberSerialization.OptIn)]
    public class OrganizationalInfo : IOrganizational
    {
        /// <summary>
        /// 组织的唯一标识(数字)
        /// </summary>
        [JsonProperty("orgId")]
        public long OrgId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 组织的唯一标识(文字)
        /// </summary>
        [JsonProperty("key")]
        public string OrgKey { get; set; }

        /// <summary>
        /// 组织的路由名称
        /// </summary>
        [JsonProperty("routeName")]
        public string RouteName { get; set; }


        #region 预定义

        /// <summary>
        /// 系统用户
        /// </summary>
        public static OrganizationalInfo System { get; } = new OrganizationalInfo
        {
            OrgId  = 0,
            OrgKey = "zero_center",
            RouteName = "*"
        };

        #endregion
    }
}