﻿using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    /// 单个ID
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class IdArguent
    {
        /// <summary>
        /// 选择的ID
        /// </summary>
        /// <example>1</example>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
    /// <summary>
    /// 多个ID
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class IdsArguent
    {
        /// <summary>
        /// 选择的ID
        /// </summary>
        /// <value>不是数组,而是逗号分开的文本</value>
        /// <example>2,1</example>
        [JsonProperty("selects")]
        public string Ids { get; set; }
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class QueryArgument
    {
        /// <summary>
        /// 查询值
        /// </summary>
        /// <example>nike</example>
        [JsonProperty("_value_")]
        public string Value { get; set; }

        /// <summary>
        /// 查询字段(为_any_时则为模糊查询),也可以直接将字段作为名称传递,其中日期类型字段可加(_beign _end )后缀查询时间范围,外键字段可以传多个值(逗号分开)使用包含查询
        /// </summary>
        /// <example>name</example>
        [JsonProperty("_field_")]
        public string Field { get; set; }

        /// <summary>
        ///页号(1起始)
        /// </summary>
        /// <example>1</example>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        /// <value>1-999的数字</value>
        /// <example>20</example>
        [JsonProperty("rows")]
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <example>id</example>
        [JsonProperty("sort")]
        public string SortField { get; set; }

        /// <summary>
        /// 正反序
        /// </summary>
        /// <value>asc: 正序 desc:反序</value>
        /// <example>asc</example>
        [JsonProperty("order")]
        public string SortAsc { get; set; }
    }

}
