using Agebull.EntityModel.Common;
using System.Data;
using ZeroTeam.MessageMVC;

namespace Agebull.EntityModel.Events
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
    ///     数据操作状态
    /// </summary>
    public enum EntityEventValueType
    {
        /// <summary>
        ///     未知
        /// </summary>
        None,

        /// <summary>
        ///     实例的Json数据
        /// </summary>
        EntityJson,

        /// <summary>
        ///     主键数据
        /// </summary>
        Key,

        /// <summary>
        ///     主键列表
        /// </summary>
        Keys,

        /// <summary>
        ///     查询条件
        /// </summary>
        QueryCondition
    }
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
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="value">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        void OnStatusChanged(string database, string entity, DataOperatorType oType, EntityEventValueType valueType, string value);

    }

    /// <summary>
    /// 默认的数据事件代理 
    /// </summary>
    public class EntityEventProxy : IEntityEventProxy
    {
        /// <summary>状态修改事件</summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="value">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        void IEntityEventProxy.OnStatusChanged(string database, string entity, DataOperatorType oType, EntityEventValueType valueType, string value)
        {
            MessagePoster.Publish("EntityEvent", database, new EntityEventArgument
            {
                OperatorType = oType,
                ValueType = valueType,
                DataBase = database,
                EntityName = entity,
                Value = value
            });
        }
    }

}