using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Interfaces;
using System.Text;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    internal class DataStateTrigger : IDataTrigger
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (!DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIHistoryData))
                return;
            var name = GlobalContext.Current.User.NickName?.Replace('\'', '’');
            code.Append($@"
UPDATE `{table.ContextWriteTable}` 
SET `{table.FieldDictionary[nameof(IHistoryData.LastReviserId)]}` = {GlobalContext.Current.User.UserId},
    `{table.FieldDictionary[nameof(IHistoryData.LastReviser)]}` = '{name}',
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
