using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agebull.EntityModel.Vue
{
    /// <summary>
    /// ID参数
    /// </summary>
    public static class IdArgumentEx
    {
        /// <summary>
        /// 到参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IdArgument ToArgument(this long id) => new IdArgument { Id = id };

        /// <summary>
        /// 到参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IdArgument<TType> ToArgument<TType>(this TType id) => new IdArgument<TType> { Id = id };
    }

    /// <summary>
    /// ID参数
    /// </summary>
    public class IdArgument
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <example>1</example>
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }
    }

    /// <summary>
    /// ID参数
    /// </summary>
    public class IdsArgument<TType>
    {
        /// <summary>
        /// 选择的主键，用逗号连接
        /// </summary>
        /// <example>1</example>
        [JsonProperty("selects"), JsonPropertyName("selects")]
        public List<TType> Selects { get; set; }
    }

    /// <summary>
    /// ID参数
    /// </summary>
    public class IdArgument<TType>
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <example>1</example>
        [JsonProperty("id"), JsonPropertyName("id")]
        public TType Id { get; set; }
    }
}