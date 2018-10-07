using System;
using System.Text;
using Agebull.Common.Rpc;
using Gboxt.Common.DataModel.Extends;

namespace Gboxt.Common.DataModel.MySql
{
    /// <inheritdoc />
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TMySqlDataBase">数据库</typeparam>
    public abstract class HitoryTable<TData, TMySqlDataBase> : DataStateTable<TData, TMySqlDataBase>
        where TData : EditDataObject, IStateData, IHistoryData, IIdentityData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        protected override void AfterUpdateSql(StringBuilder code, string condition)
        {
            code.Append($@"
UPDATE `{WriteTableName}` 
SET `{FieldDictionary["LastReviserID"]}` = {GlobalContext.Current.LoginUserId},
    `{FieldDictionary["LastModifyDate"]}` = {DateTime.Now}");
            if (!string.IsNullOrEmpty(condition))
            {
                code.Append($@"
WHERE {condition}");
            }
            code.AppendLine(";");
        }

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected override void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {
            switch (operatorType)
            {
                case DataOperatorType.Insert:
                    entity.AddDate = DateTime.Now;
                    entity.AuthorId = GlobalContext.Current.LoginUserId;
                    break;
                default:
                    entity.LastModifyDate = DateTime.Now;
                    entity.LastReviserId = GlobalContext.Current.LoginUserId;
                    break;
            }
        }
    }
}