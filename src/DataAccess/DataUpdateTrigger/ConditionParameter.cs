using System.Data;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 参数节点
    /// </summary>
    public class ConditionParameter
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Value { get; set; }

    }

}