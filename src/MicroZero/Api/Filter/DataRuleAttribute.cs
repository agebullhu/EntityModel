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
        /// 最小(包含的数值或文本长度)
        /// </summary>
        public string Min { get; set; }
        /// <summary>
        /// 最大(包含的数值或文本长度)
        /// </summary>
        public string Max { get; set; }
    }
}
