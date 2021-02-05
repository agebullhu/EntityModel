#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.DataEvents;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.Messages;
using static Agebull.EntityModel.MySql.GlobalDataInterfaces;
#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 操作注入
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class OperatorInjection<TEntity> : IOperatorInjection<TEntity>
        where TEntity : class, new()
    {
        #region 构造

        /// <summary>
        /// 依赖对象
        /// </summary>
        public IDataAccessProvider<TEntity> Provider { get; set; }

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
        bool CanDo(string name)
        {
            return !DataEventOption.Instance.ExcludeInterfaces.Contains(name) &&
                   Provider.Option.DataStruct.InterfaceFeature.Contains(name);
        }
        bool CanDo(params string[] names)
        {
            foreach (var name in names)
                if (DataEventOption.Instance.ExcludeInterfaces.Contains(name) ||
                   !Provider.Option.DataStruct.InterfaceFeature.Contains(name))
                    return false;
            return true;
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
            if (!DataEventOption.Instance.Injection ||
                Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.QueryCondition))
                return;
            if (CanDo(nameof(IStateData)))
            {
                conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{IStateData.DataState.FieldName} < 255");
            }
            if (CanDo(nameof(ILogicDeleteData)))
            {
                conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{ILogicDeleteData.IsDeleted.FieldName} = 0");
            }
            CheckDataScope(conditions);
        }

        private void CheckDataScope(List<string> conditions)
        {
            var user = GlobalContext.User;
            if (user == null || user.UserId == ZeroTeamJwtClaim.SystemUserId)
                return;
            var cliam = user[ZeroTeamJwtClaim.DataScope];
            if (!int.TryParse(cliam, out var vl))
            {
                return;
            }
            var scope = (DataScopeType)vl;
            if (scope.HasFlag(DataScopeType.Unlimited))
                return;
            if (0 == vl && !CanDo(nameof(IAuthorData), nameof(IOrganizationBoundaryData), nameof(IOrganizationBoundaryQuery)))
            {
                throw new NotSupportedException("当前用户无数据处理权限");
            }
            if (CanDo(nameof(IAuthorData)) && scope.HasFlags(DataScopeType.Person))
                conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{IAuthorData.AuthorId.FieldName} = {user.UserId}");

            if (CanDo(nameof(IOrganizationBoundaryData), IOrganizationBoundaryQuery))
            {
                var code = GlobalContext.User[ZeroTeamJwtClaim.OrganizationCode];
                if (scope.HasFlags(DataScopeType.HomeAndLower))
                    conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{IOrganizationBoundaryData.BoundaryCode.FieldName} LIKE '{code}%'");
                else if (scope.HasFlags(DataScopeType.Lower))
                    conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{IOrganizationBoundaryData.BoundaryCode.FieldName} LIKE '{code}.%'");
                else if (scope.HasFlags(DataScopeType.Home))
                    conditions.Add($"{Provider.Option.DataStruct.ReadTableName}.{IOrganizationBoundaryData.BoundaryCode.FieldName} = '{code}'");
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
            if (!DataEventOption.Instance.Injection ||
                Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.InsertField))
                return;
            var user = GlobalContext.User;
            if (CanDo(nameof(IAuthorData)))
            {
                AddValue(fields, values, IAuthorData.AddDate, DateTime.Now);
                //AddValue(fields, values, IAuthorData.Author, user?.NickName);
                AddValue(fields, values, IAuthorData.AuthorId, user?.UserId);
            }
            if (CanDo(nameof(IHistoryData)))
            {
                AddValue(fields, values, IHistoryData.LastModifyDate, DateTime.Now);
                //AddValue(fields, values, IHistoryData.LastReviser, user?.NickName);
                AddValue(fields, values, IHistoryData.LastReviserId, user?.UserId);
            }
            if (CanDo(nameof(IStateData)))
            {
                AddValue(fields, values, IStateData.IsFreeze, 0);
                AddValue(fields, values, IStateData.DataState, 0);
            }
            if (CanDo(nameof(ILogicDeleteData)))
            {
                AddValue(fields, values, ILogicDeleteData.IsDeleted, 0);
            }
            if (CanDo(nameof(IDepartmentData)))
            {
                AddValue(fields, values, IDepartmentData.DepartmentId, GlobalContext.User.OrganizationId);
                //AddValue(fields, values, IDepartmentData.DepartmentId, user?.OrganizationId);
            }
            if (CanDo(nameof(IVersionData)))
            {
                AddValue(fields, values, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
            if (CanDo(nameof(IOrganizationBoundaryData)) && user != null)
            {
                AddValue(fields, values, IOrganizationBoundaryData.BoundaryCode, user[ZeroTeamJwtClaim.OrganizationCode]);
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
            if (!DataEventOption.Instance.Injection ||
                Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                (!Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateField) &&
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.UpdateCondition)))
            {
                return;
            }
            var val = new StringBuilder();
            var conditions = new List<string>();

            if (CanDo(nameof(IHistoryData)))
            {
                SetValue(val, IHistoryData.LastModifyDate, DateTime.Now);
                //SetValue(val, IHistoryData.LastReviser, user?.NickName);
                var user = GlobalContext.User;
                SetValue(val, IHistoryData.LastReviserId, user?.UserId);
            }
            if (CanDo(nameof(IVersionData)))
            {
                SetValue(val, IVersionData.DataVersion, DateTime.Now.Ticks);
            }
            if (CanDo(nameof(IStateData)))
            {
                conditions.Add($"{IStateData.IsFreeze.FieldName} = 0");
                conditions.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (CanDo(nameof(ILogicDeleteData)))
            {
                conditions.Add($"{ILogicDeleteData.IsDeleted.FieldName} = 0");
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
            if (!DataEventOption.Instance.Injection ||
                Provider.Option.DataStruct.InterfaceFeature == null ||
                Provider.Option.DataStruct.InterfaceFeature.Count == 0 ||
                !Provider.Option.InjectionLevel.HasFlag(InjectionLevel.DeleteCondition))
                return condition;
            var conditions = new List<string>();

            if (CanDo(nameof(IStateData)))
            {
                conditions.Add($"{IStateData.IsFreeze.FieldName} = 0");
                conditions.Add($"{IStateData.DataState.FieldName} < 255");
            }
            if (CanDo(nameof(ILogicDeleteData)))
            {
                conditions.Add($"{ILogicDeleteData.IsDeleted.FieldName} = 0");
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
            if (!Provider.Option.CanRaiseEvent || !DataEventOption.Instance.Event || DataEventOption.Instance.ExcludeEvents.Contains(operatorType))
                return;
            if (Provider.Option.EventLevel == EventEventLevel.Details)
                await DetailsEvent(DataEventType.Entity, operatorType, EntityEventValueType.Entity, entity.ToJson());
            else
                await SimpleEvent(DataEventType.Entity, operatorType, Provider.EntityOperator.GetValue(entity, Provider.Option.PrimaryProperty).ToString());
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="parameter">参数值</param>
        /// <param name="operatorType">操作类型</param>
        /// <param name="sql"></param>
        public async Task AfterExecute(DataOperatorType operatorType, string sql, string condition, DbParameter[] parameter)
        {
            if (!Provider.Option.CanRaiseEvent || !DataEventOption.Instance.Event || DataEventOption.Instance.ExcludeEvents.Contains(operatorType))
                return;
            var queryCondition = new MulitCondition
            {
                SQL = sql,
                Condition = condition,
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
            if (Provider.Option.EventLevel == EventEventLevel.Details)
                await DetailsEvent(DataEventType.Entity, operatorType, EntityEventValueType.CustomSQL, queryCondition.ToJson());
            else
                await SimpleEvent(DataEventType.Entity, operatorType);
        }


        /// <summary>
        /// 业务操作命令事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="id">主键</param>
        /// <param name="cmd">命令</param>
        public async Task AfterCommand(TEntity entity, string id, DataOperatorType cmd)
        {
            if (!Provider.Option.CanRaiseEvent || !DataEventOption.Instance.Event || DataEventOption.Instance.ExcludeEvents.Contains(cmd))
                return;
            DataEventType eventType;
            if (cmd < DataOperatorType.SetState)
                eventType = DataEventType.None;
            else if (cmd < DataOperatorType.Submit)
                eventType = DataEventType.DataState;
            else if (cmd < DataOperatorType.End)
                eventType = DataEventType.Audit;
            else
                eventType = DataEventType.None;

            if (Provider.Option.EventLevel == EventEventLevel.Details)
                await DetailsEvent(eventType, cmd, entity == null ? EntityEventValueType.Id : EntityEventValueType.Entity, entity == null ? id : SmartSerializer.ToString(entity));
            else
                await SimpleEvent(eventType, cmd, entity == null ? id : Provider.EntityOperator.GetValue(entity, Provider.Option.PrimaryProperty).ToString());
        }

        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="eventType">操作</param>
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="val">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        Task DetailsEvent(DataEventType eventType, DataOperatorType oType, EntityEventValueType valueType, string val)
        {
            return MessagePoster.CallAsync(DataEventOption.Instance.Service,
                $"{Provider.Option.DataStruct.ProjectName }/{Provider.Option.DataStruct.EntityName}", new EntityEventArgument
                {
                    EventType = eventType,
                    OperatorType = oType,
                    ValueType = valueType,
                    Value = val
                });
        }

        /// <summary>
        /// 简单修改事件
        /// </summary>
        /// <param name="eventType">操作</param>
        /// <param name="oType">操作</param>
        /// <param name="id">主键</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        Task SimpleEvent(DataEventType eventType, DataOperatorType oType, string id = null)
        {
            return MessagePoster.CallAsync(DataEventOption.Instance.Service,
                $"{Provider.Option.DataStruct.ProjectName }/{Provider.Option.DataStruct.EntityName}", new EntityEventArgument
                {
                    EventType = eventType,
                    OperatorType = oType,
                    ValueType = id != null ? EntityEventValueType.Id : EntityEventValueType.None,
                    Value = id
                });
        }
        #endregion
    }

}