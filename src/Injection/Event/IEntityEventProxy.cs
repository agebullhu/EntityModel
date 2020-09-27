using Agebull.EntityModel.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Events
{
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
        Task OnStatusChanged(string database, string entity, DataOperatorType oType, EntityEventValueType valueType, string value);
    }
}