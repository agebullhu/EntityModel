using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ApiDocuments
{
    /// <summary>
    ///     Api方法的信息
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiDocument : DocumentItem
    {
        /// <summary>
        ///     访问设置
        /// </summary>
        [DataMember] [JsonProperty("access", NullValueHandling = NullValueHandling.Ignore)]
        public ApiAccessOption AccessOption;

        /// <summary>
        ///     参数说明
        /// </summary>
        [DataMember] [JsonProperty("argument", NullValueHandling = NullValueHandling.Ignore)]
        public TypeDocument ArgumentInfo;

        /// <summary>
        ///     分类
        /// </summary>
        [DataMember] [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)] [Category]
        public string Category;

        /// <summary>
        ///     返回值说明
        /// </summary>
        [DataMember] [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public TypeDocument ResultInfo;

        /// <summary>
        ///     Api路由名称
        /// </summary>
        [DataMember] [JsonProperty("route", NullValueHandling = NullValueHandling.Ignore)] [Category]
        public string RouteName;

        /// <summary>
        ///     Api名称
        /// </summary>
        [DataMember]
        [JsonProperty("api", NullValueHandling = NullValueHandling.Ignore)]
        [Category]
        public string ApiName;
        
    }
}