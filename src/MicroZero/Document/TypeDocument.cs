using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ApiDocuments
{
    /// <summary>
    ///     Api结构的信息
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class TypeDocument : DocumentItem
    {
        /// <summary>
        ///     类型
        /// </summary>
        [DataMember] [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string ClassName;

        /// <summary>
        ///     字段
        /// </summary>
        [DataMember] [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, TypeDocument> fields;

        /// <summary>
        ///     枚举
        /// </summary>
        [DataMember] [JsonProperty("enum", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsEnum;

        /// <summary>
        ///     类型
        /// </summary>
        [DataMember] [JsonProperty("jsonName", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonName;

        /// <summary>
        ///     类型
        /// </summary>
        [DataMember] [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        public ObjectType ObjectType;

        /// <summary>
        ///     类型
        /// </summary>
        [DataMember] [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName;

        /// <summary>
        ///     能否为空
        /// </summary>
        [DataMember]
        [JsonProperty("canNull", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanNull { get; set; }

        /// <summary>
        ///     正则校验(文本)
        /// </summary>
        [DataMember]
        [JsonProperty("regex", NullValueHandling = NullValueHandling.Ignore)]
        public string Regex { get; set; }

        /// <summary>
        ///     最小(包含的数值或文本长度)
        /// </summary>
        [DataMember]
        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public string Min { get; set; }

        /// <summary>
        ///     最大(包含的数值或文本长度)
        /// </summary>
        [DataMember]
        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public string Max { get; set; }

        /// <summary>
        ///     字段
        /// </summary>
        public Dictionary<string, TypeDocument> Fields => fields ?? (fields = new Dictionary<string, TypeDocument>());
    }
}