using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     表明是一个数据操作对象
    /// </summary>
    public class EventDataOperator<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }



        #region 扩展 

        /// <summary>
        ///     实体保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public Task AfterSave(TEntity entity, DataOperatorType operatorType)
        {
            if (!Provider.Option.CanRaiseEvent)
                return Task.CompletedTask;
            return OnStatusChanged(operatorType, EntityEventValueType.EntityJson, entity);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public Task AfterExecute(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
            if (!Provider.Option.CanRaiseEvent)
                return Task.CompletedTask;
            return OnStatusChanged(operatorType, EntityEventValueType.QueryCondition, (condition, parameter));
        }

        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="val">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        async Task OnStatusChanged(DataOperatorType oType, EntityEventValueType valueType, object val)
        {
            var service = Provider.ServiceProvider.GetService<IEntityEventProxy>();
            if (service == null)
                return;
            string value;
            switch (valueType)
            {
                case EntityEventValueType.EntityJson:
                    value = JsonConvert.SerializeObject(val);
                    break;
                case EntityEventValueType.Key:
                case EntityEventValueType.Keys:
                    value = val.ToString();
                    break;
                case EntityEventValueType.QueryCondition:
                    {
                        var arg = val as Tuple<string, DbParameter[]>;
                        var parameter = arg.Item2;
                        var queryCondition = new MulitCondition
                        {
                            Condition = arg.Item1,
                            Parameters = new ConditionParameter[parameter.Length]
                        };
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            queryCondition.Parameters[i] = new ConditionParameter
                            {
                                Name = parameter[i].ParameterName,
                                Value = parameter[i].Value == DBNull.Value ? null : parameter[i].Value.ToString(),
                                Type = parameter[i].DbType
                            };
                        }
                        value = JsonConvert.SerializeObject(queryCondition);
                    }
                    break;
                default:
                    value = null;
                    break;
            }

            await service.OnStatusChanged(Provider.Option.DataStruct.ProjectName, Provider.Option.DataStruct.EntityName, oType, valueType, value);
        }
        #endregion
    }
}