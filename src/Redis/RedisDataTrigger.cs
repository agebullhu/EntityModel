using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;

namespace Agebull.EntityModel.Redis
{

    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    public class RedisDataTrigger : IDataTrigger
    {
        void IDataUpdateTrigger.OnDataSaved(EditDataObject entity, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecuted(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {

        }

        void IDataUpdateTrigger.ContitionSqlCode<TEntity>(List<string> conditions)
        {
        }

        void IDataUpdateTrigger.OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        {
        }

        void IDataTrigger.InitType<TEntity>()
        {
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.Full;

        void IDataUpdateTrigger.BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
        }

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (!DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIVersionData))
                return;

            long ver;
            using (RedisProxy proxy = new RedisProxy(RedisProxy.Option.DbSystem))
            {
                ver = proxy.Redis.Incr($"ent:ver:{table.Name}");
            }

            switch (table.DataBaseType)
            {
                case DataBaseType.MySql:
                    code.Append($@"
UPDATE `{table.ContextWriteTable}` 
SET `{table.FieldDictionary[nameof(IVersionData.DataVersion)]}` = {ver}");
                    break;
                default:
                    code.Append($@"
UPDATE [{table.ContextWriteTable}]
SET [{table.FieldDictionary[nameof(IVersionData.DataVersion)]}] = {ver}");
                    break;
            }
            if (!string.IsNullOrEmpty(condition))
            {
                code.Append($@"
WHERE {condition}");
            }
            code.AppendLine(";");
        }
    }
}
