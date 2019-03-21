using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Agebull.Common.Context;
using Agebull.Common.OAuth;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Common.Extends;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    public class MySqlDataTrigger : IDataTrigger
    {
        void IDataUpdateTrigger.ContitionSqlCode<TEntity>(List<string> conditions)
        {
            if (GlobalContext.Current.IsSystemMode || GlobalContext.Current.User.UserId == LoginUserInfo.SystemUserId)
                return;
            if (DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIOrganizationData))
            {
                conditions.Add($"`organization_id` = {GlobalContext.Current.Organizational.OrgId}");
            }
        }

        void IDataUpdateTrigger.OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        {
            if (!GlobalContext.Current.IsSystemMode &&
                GlobalContext.Current.User.UserId != LoginUserInfo.SystemUserId &&
                entity is IOrganizationData organizationData)
            {
                organizationData.OrganizationId = GlobalContext.Current.Organizational.OrgId;
            }
        }

        void IDataTrigger.InitType<TEntity>()
        {
        }

        void IDataUpdateTrigger.OnDataSaved(EditDataObject entity, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecutd(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
        }

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIHistoryData))
            {
                code.Append($@"
UPDATE `{table.WriteTableName}` 
SET `{table.FieldDictionary[nameof(IHistoryData.LastReviserId)]}` = {GlobalContext.Current.LoginUserId},
    `{table.FieldDictionary[nameof(IHistoryData.LastModifyDate)]}` = Now()");
                if (!string.IsNullOrEmpty(condition))
                {
                    code.Append($@"
WHERE {condition}");
                }

                code.AppendLine(";");
            }
        }
    }
}
