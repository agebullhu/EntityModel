using System;
using System.Threading.Tasks;
using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.EntityModel.DataEvents
{
    /// <summary>
    /// 数据事件处理器接口定义
    /// </summary>
    public interface IEntityEventHandler
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        string Project { get; }

        /// <summary>
        /// 实体名称
        /// </summary>
        string[] EntityNames { get;}

        /// <summary>
        /// 数据事件处理
        /// </summary>
        /// <param name="argument">参数</param>
        Task OnEvent(EntityEventArgument argument)
        {
            switch (argument.EventType)
            {
                case DataEventType.Entity:
                    return OnEnitty(argument);
                case DataEventType.DataState:
                    return OnState(argument);
                case DataEventType.Audit:
                    return OnAudit(argument);
                default:
                    return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 实体事件
        /// </summary>
        /// <param name="argument">参数</param>
        Task OnEnitty(EntityEventArgument argument)
        {
            //var dataOperator = Enum.Parse<DataOperatorType>(argument.OperatorType);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 审核事件
        /// </summary>
        /// <param name="argument">参数</param>
        Task OnState(EntityEventArgument argument)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 审核事件
        /// </summary>
        /// <param name="argument">参数</param>
        Task OnAudit(EntityEventArgument argument)
        {
            //var cmd = Enum.Parse<DataCommandType>(argument.OperatorType);
            //var ent = JsonConvert.DeserializeObject<TEntity>(argument.Value);
            return Task.CompletedTask;
        }
    }
}
