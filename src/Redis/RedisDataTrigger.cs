using System.Text;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Agebull.EntityModel.Interfaces;

namespace Agebull.EntityModel.Redis
{

    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    public class RedisDataTrigger : IDataTrigger
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.Full;

        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (!DataUpdateHandler.IsType<TEntity>(DataUpdateHandler.TypeofIVersionData))
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
