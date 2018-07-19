using System.Data;

namespace Gboxt.Common.DataModel.ExtendEvents
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class MulitCondition
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public ConditionParameter[] Parameters { get; set; }

    }

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
    /// <summary>
    /// 实体事件代理
    /// </summary>
    public interface IEntityEventProxy
    {
        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="type">状态类型</param>
        /// <param name="value">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        void OnStatusChanged(string database, string entity, DataOperatorType type, string value);

    }
}