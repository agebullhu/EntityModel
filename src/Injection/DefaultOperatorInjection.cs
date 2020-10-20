// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 操作注入
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DefaultOperatorInjection<TEntity> : IOperatorInjection<TEntity>
        where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 依赖对象
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        #endregion

        #region 注入

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public void InjectionQueryCondition(List<string> conditions)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<ISqlInjection>().Where(p => p.DataBaseType == Provider.Option.DataBaseType).ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                trigger.InjectionQueryCondition(Provider, conditions);
        }

        /// <summary>
        ///     注入数据插入代码
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public void InjectionInsertCode(StringBuilder fields, StringBuilder values)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<ISqlInjection>().Where(p => p.DataBaseType == Provider.Option.DataBaseType).ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                trigger.InjectionInsertCode(Provider, fields, values);
        }


        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public void InjectionUpdateCode(ref string valueExpression, ref string condition)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<ISqlInjection>().Where(p => p.DataBaseType == Provider.Option.DataBaseType).ToArray();
            if (services.Length == 0)
                return;
            var val = new StringBuilder();
            var con = new List<string>();

            foreach (var trigger in services)
                trigger.InjectionUpdateCode(Provider, val, con);
            if (val.Length > 0)
                valueExpression += val.ToString();
            if (con.Count > 0)
            {
                condition = string.IsNullOrEmpty(condition)
                    ? string.Join(" AND ", con)
                    : $"({condition}) AND {string.Join(" AND ", con)}";
            }
        }

        /// <summary>
        ///     删除条件注入
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string InjectionDeleteCondition(string condition)
        {
            if (Provider.Option.NoInjection)
                return condition;
            var services = Provider.ServiceProvider.GetServices<ISqlInjection>().Where(p => p.DataBaseType == Provider.Option.DataBaseType).ToArray();
            if (services.Length == 0)
                return condition;
            var con = new List<string>();

            foreach (var trigger in services)
                trigger.InjectionDeleteCondition(Provider, con);

            return con.Count > 0
                ? $"({condition}) {string.Join(" AND ", con)}"
                : condition;
        }

        #endregion

        #region 扩展 

        /// <summary>
        ///     实体保存前处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public async Task BeforeSave(TEntity entity, DataOperatorType operatorType)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<IDataUpdateTrigger>().ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                await trigger.BeforeSave(Provider, entity, operatorType);
        }

        /// <summary>
        ///     实体保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        public async Task AfterSave(TEntity entity, DataOperatorType operatorType)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<IDataUpdateTrigger>().ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                await trigger.AfterSave(Provider, entity, operatorType);

            if (!Provider.Option.CanRaiseEvent)
                return;
            await OnStatusChanged(operatorType, EntityEventValueType.EntityJson, entity);
        }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public async Task BeforeExecute(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<IDataUpdateTrigger>().ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                await trigger.BeforeExecute(Provider, operatorType, condition, parameter);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public async Task AfterExecute(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
            if (Provider.Option.NoInjection)
                return;
            var services = Provider.ServiceProvider.GetServices<IDataUpdateTrigger>().ToArray();
            if (services.Length == 0)
                return;
            foreach (var trigger in services)
                await trigger.AfterExecute(Provider, operatorType, condition, parameter);

            if (!Provider.Option.CanRaiseEvent)
                return;
            await OnStatusChanged(operatorType, EntityEventValueType.QueryCondition, (condition, parameter));
        }

        #endregion

        #region 数据事件

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
            var services = Provider.ServiceProvider.GetServices<IEntityModelEventProxy>().ToArray();
            if (services.Length == 0)
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
            foreach (var service in services)
                await service.OnEntityCommandSuccess(Provider.Option.DataStruct.ProjectName,
                    Provider.Option.DataStruct.EntityName, oType, valueType, value);
        }
        #endregion
    }
}