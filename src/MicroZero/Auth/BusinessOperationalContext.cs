using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agebull.Common.OAuth
{
    /// <summary>
    /// 业务的操作上下文
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BusinessOperationalContext
    {
        /// <summary>
        ///     应用标识
        /// </summary>
        [JsonProperty("appid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long AppId { get; set; }

        /// <summary>
        ///     应用名称
        /// </summary>
        [JsonProperty("app",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string AppName { get; set; }

        /// <summary>
        ///     页面标识
        /// </summary>
        [JsonProperty("pid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long PageId { get; set; }

        /// <summary>
        ///     页面名称
        /// </summary>
        [JsonProperty("pn",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string PageName { get; set; }

        /// <summary>
        ///     接口标识
        /// </summary>
        [JsonProperty("aid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long ApiId { get; set; }

        /// <summary>
        ///     接口名称
        /// </summary>
        [JsonProperty("an",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string ApiName { get; set; }


        /// <summary>
        ///     集团标识
        /// </summary>
        [JsonProperty("gid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long GroupId { get; set; }

        /// <summary>
        ///     集团名称
        /// </summary>
        [JsonProperty("gn",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string GroupName { get; set; }

        /// <summary>
        ///     组织标识
        /// </summary>
        [JsonProperty("oid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long OrganizationId { get; set; }

        /// <summary>
        ///     机构名称
        /// </summary>
        [JsonProperty("on",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string Organization { get; set; }

        /// <summary>
        ///     用户角色标识
        /// </summary>
        [JsonProperty("rid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long RoleId { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        [JsonProperty("rn",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        /// <summary>
        ///     岗位标识
        /// </summary>
        [JsonProperty("did",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public long DutyId { get; set; }

        /// <summary>
        ///     岗位名称
        /// </summary>
        [JsonProperty("dn",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string DutyName { get; set; }

        /// <summary>
        /// 开始时间（对应到人）
        /// </summary>
        [JsonProperty("ds",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DutyStart { get; set; }

        /// <summary>
        /// 结束时间（对应到人）
        /// </summary>
        [JsonProperty("de",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DutyEnd { get; set; }

        /// <summary>
        /// 不可读数据列
        /// </summary>
        [JsonProperty("ur",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Unreadables { get; set; }
    }
}