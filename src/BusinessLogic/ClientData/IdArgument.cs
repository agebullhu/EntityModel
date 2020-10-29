using Newtonsoft.Json;
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
        /// 名称
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }
    }

    /// <summary>
    /// ID参数
    /// </summary>
    public class IdArgument<TType>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public TType Id { get; set; }
    }
}