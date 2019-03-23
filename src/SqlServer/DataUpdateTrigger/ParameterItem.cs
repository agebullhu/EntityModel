using System.Data;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    /// 参数节点
    /// </summary>
    public class ParameterItem
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public SqlDbType DbType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}