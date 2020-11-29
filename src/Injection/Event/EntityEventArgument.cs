﻿using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 数据事件参数
    /// </summary>
    public class EntityEventArgument
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        [JsonProperty("oType")]
        public DataOperatorType OperatorType { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        [JsonProperty("vType")]
        public EntityEventValueType ValueType { get; set; }

        /// <summary>
        /// 值文本
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}