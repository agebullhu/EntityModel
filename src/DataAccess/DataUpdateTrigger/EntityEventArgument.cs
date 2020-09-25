using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 实体事件参数
    /// </summary>
    public class EntityEventArgument
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public DataOperatorType OperatorType { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public EntityEventValueType ValueType { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        public string Value { get; set; }
    }

}