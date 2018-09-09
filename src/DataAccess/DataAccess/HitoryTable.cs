using System;
using Agebull.Common;
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
        where TData : EditDataObject, IStateData,IHistoryData, IIdentityData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        /// 与更新同时执行的SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override string AfterUpdateSql(string condition)
        {
            var filter= string.IsNullOrEmpty(condition) ? null : "\r\nWHERE " + condition;
            return $@"UPDATE `{WriteTableName}` 
SET `{FieldDictionary["LastReviserID"]}`={GlobalContext.Current.LoginUserId},
{FieldDictionary["LastModifyDate"]}=NOW(){filter};";
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