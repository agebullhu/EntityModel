// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Agebull.EntityModel.MySql.GlobalDataInterfaces;
using IStateData = Agebull.EntityModel.MySql.GlobalDataInterfaces.IStateData;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 操作注入
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataInterfaceFeatureInjection<TEntity> : IOperatorInjection<TEntity>
        where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 依赖对象
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        void AddValue<T>(StringBuilder fields, StringBuilder values, PropertyDefault field, T value)
        {
            fields.Append($",`{field.FieldName}`");
            values.Append($",'{value}'");
        }
        void AddValue<T>(StringBuilder fields, StringBuilder values, PropertyDefault field, string value)
        {
            fields.Append($",`{field.FieldName}`");
            values.Append($",'{value.Replace("'", "\\'")}'");
        }

        void AddValue(StringBuilder fields, StringBuilder values, PropertyDefault field, DateTime value)
        {
            fields.Append($",`{field.FieldName}`");
            values.Append($",'{value:yyyy-MM-dd HH:mm:ss}'");
        }

        void SetValue(StringBuilder valueExpression, PropertyDefault field, DateTime value)
        {
            valueExpression.Append($",`{field.FieldName}` = '{value:yyyy-MM-dd HH:mm:ss}'");
        }

        void SetValue<T>(StringBuilder valueExpression, PropertyDefault field, string value)
        {
            valueExpression.Append($",`{field.FieldName}` = '{value.Replace("'", "\\'")}'");
        }

        void SetValue<T>(StringBuilder valueExpression, PropertyDefault field, T value)
        {
            valueExpression.Append($",`{field.FieldName}` = '{value}'");
        }
        #endregion

        #region 注入

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public void InjectionQueryCondition(List<string> conditions)
        {
            if (Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.QueryCondition))
                return;
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                conditions.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                conditions.Add($"{GlobalDataInterfaces.ILogicDeleteData.IsDeleted.FieldName} = 0");
            }
        }

        /// <summary>
        ///     注入数据插入代码
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public void InjectionInsertCode(StringBuilder fields, StringBuilder values)
        {
            if (Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.InsertField))
                return;
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IAuthorData)))
            {
                AddValue(fields, values, IAuthorData.AddDate, DateTime.Now);
                //AddValue(fields, values, IAuthorData.Author, GlobalContext.Current?.User.NickName);
                //AddValue(fields, values, IAuthorData.AuthorId, GlobalContext.Current?.User.UserId);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IHistoryData)))
            {
                AddValue(fields, values, IHistoryData.LastModifyDate, DateTime.Now);
                //AddValue(fields, values, IHistoryData.LastReviser, GlobalContext.Current?.User.NickName);
                //AddValue(fields, values, IHistoryData.LastReviserId, GlobalContext.Current?.User.UserId);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                AddValue(fields, values, IStateData.IsFreeze, 0);
                AddValue(fields, values, IStateData.DataState, 0);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                AddValue(fields, values, GlobalDataInterfaces.ILogicDeleteData.IsDeleted, 0);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IDepartmentData)))
            {
                //AddValue(fields, values, IDepartmentData.DepartmentId, GlobalContext.Current.User.OrganizationId);
                //AddValue(fields, values, IDepartmentData.DepartmentId, GlobalContext.Current?.User.OrganizationId);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IVersionData)))
            {
                AddValue(fields, values, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
        }


        /// <summary>
        ///     注入数据更新代码
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public void InjectionUpdateCode(ref string valueExpression, ref string condition)
        {
            if (Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                (!Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateField) &&
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateCondition)))
            {
                return;
            }
            var val = new StringBuilder();
            var conditions = new List<string>();

            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IHistoryData)))
            {
                SetValue(val, IHistoryData.LastModifyDate, DateTime.Now);
                //SetValue(valueExpression, IHistoryData.LastReviser, GlobalContext.Current?.User.NickName);
                //SetValue(valueExpression, IHistoryData.LastReviserId, GlobalContext.Current?.User.UserId);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IVersionData)))
            {
                SetValue(val, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                conditions.Add($"{IStateData.IsFreeze.FieldName} = 0");
                conditions.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                conditions.Add($"{GlobalDataInterfaces.ILogicDeleteData.IsDeleted.FieldName} = 0");
            }
            if (Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateField) && val.Length > 0)
                valueExpression += val.ToString();
            if (Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateCondition) && conditions.Count > 0)
            {
                condition = string.IsNullOrEmpty(condition)
                    ? string.Join(" AND ", conditions)
                    : $"({condition}) AND {string.Join(" AND ", conditions)}";
            }
        }

        /// <summary>
        ///     删除条件注入
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string InjectionDeleteCondition(string condition)
        {
            if (Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.DeleteCondition))
                return condition;
            var conditions = new List<string>();

            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(IStateData)))
            {
                conditions.Add($"{IStateData.IsFreeze.FieldName} = 0");
                conditions.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (Provider.Option.DataStruct.InterfaceFeature.Contains(nameof(GlobalDataInterfaces.ILogicDeleteData)))
            {
                conditions.Add($"{GlobalDataInterfaces.ILogicDeleteData.IsDeleted.FieldName} = 0");
            }

            if (conditions.Count > 0)
                return string.IsNullOrEmpty(condition)
                     ? string.Join(" AND ", conditions)
                     : $"({condition}) AND {string.Join(" AND ", conditions)}";
            return condition;
        }

        #endregion

        #region 扩展 

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
            if (!Provider.Option.CanRaiseEvent)
                return;
            await OnStatusChanged(operatorType, EntityEventValueType.EntityJson, entity);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public async Task AfterExecute(DataOperatorType operatorType, string condition, DbParameter[] parameter)
        {
            if (!Provider.Option.CanRaiseEvent)
                return;
            await OnStatusChanged(operatorType, EntityEventValueType.QueryCondition, (condition, parameter));
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
            var service = Provider.ServiceProvider.GetService<IEntityModelEventProxy>();
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
                    break;
                default:
                    value = null;
                    break;
            }
            await service.OnEntityCommandSuccess(Provider.Option.DataStruct.ProjectName,
                    Provider.Option.DataStruct.EntityName, oType, valueType, value);
        }
        #endregion
    }
}