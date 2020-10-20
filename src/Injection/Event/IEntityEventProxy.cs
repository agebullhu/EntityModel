using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    /// 实体事件代理
    /// </summary>
    public interface IEntityModelEventProxy
    {
        /// <summary>
        /// 实体操作命令事件
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
        Task OnEntityCommandSuccess(string database, string entity, DataOperatorType oType, EntityEventValueType valueType, string value) => Task.CompletedTask;

        /// <summary>
        /// 业务操作命令事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="object">实体</param>
        /// <param name="id">主键</param>
        /// <param name="cmd">命令</param>
        Task OnBusinessCommandSuccess(string database, string entity, object data, string id, BusinessCommandType cmd) => Task.CompletedTask;

    }
}