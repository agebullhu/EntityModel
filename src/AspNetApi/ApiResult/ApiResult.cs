using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>
    ///     API状态返回接口实现
    /// </summary>
    [DataContract, JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiResult
    {
        /// <summary>
        ///     成功或失败标记
        /// </summary>
        /// <example>true</example>
        [DataMember(Name = "success"), JsonPropertyName("success"), JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public bool Success { get; set; }

        /// <summary>
        ///     错误码
        /// </summary>
        /// <remarks>
        ///     参见 ErrorCode 说明
        /// </remarks>
        /// <example>-1</example>
        [DataMember(Name = "code"), JsonPropertyName("code"), JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        /// <summary>
        ///     提示信息
        /// </summary>
        /// <remarks>
        ///  说明错误的原因
        /// </remarks>
        /// <example>你的数据不正确</example>
        [DataMember(Name = "message"), JsonPropertyName("message"), JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        ///     内部提示信息
        /// </summary>
        [IgnoreDataMember, System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public string InnerMessage { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        [IgnoreDataMember, System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public Exception Exception { get; set; }

    }

    /// <summary>
    ///     API返回数据泛型类
    /// </summary>
    [DataContract, JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiResult<TData> : ApiResult
    {
        /// <summary>
        ///     返回值
        /// </summary>
        [DataMember(Name = "data"), JsonPropertyName("data"), JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public TData ResultData { get; set; }

    }
}