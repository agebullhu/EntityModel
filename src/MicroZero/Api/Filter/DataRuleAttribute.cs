using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据规则(仅用于文档生成)
    /// </summary>
    public class DataRuleAttribute : Attribute
    {
        /// <summary>
        /// 能否为空
        /// </summary>
        public bool CanNull { get; set; }
        /// <summary>
        /// 正则校验(文本)
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 包含的最小时间
        /// </summary>
        public DateTime MinDate { get; set; }
        /// <summary>
        /// 包含的最大时间
        /// </summary>
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        /// <summary>
        /// 最小(包含的数值或文本长度)
        /// </summary>
        public long Min { get; set; } = long.MinValue;
        /// <summary>
        /// 最大(包含的数值或文本长度)
        /// </summary>
        public long Max { get; set; } = long.MinValue;
    }
}
