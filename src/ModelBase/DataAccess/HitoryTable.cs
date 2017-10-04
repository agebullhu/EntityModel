using System;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class HitoryTable<TData> : DataStateTable<TData>
        where TData : EditDataObject, IStateData, IHistoryData, IIdentityData, new()
    {
        /// <summary>
        /// 与更新同时执行的SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override string AfterUpdateSql(string condition)
        {
            if (BusinessContext.Current.IsSystemMode)
                return null;
            return string.Format(@"UPDATE `{1}` SET LastReviserID={0},LastModifyDate=NOW(){2};"
                , BusinessContext.Current.LoginUserId
                , WriteTableName
                , string.IsNullOrEmpty(condition) ? null : " WHERE " + condition);
        }
        
        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected override void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {
            if (operatorType == DataOperatorType.Insert)
            {
                entity.AddDate = DateTime.Now;
                entity.AuthorID = BusinessContext.Current.LoginUserId;
            }
        }
    }
}