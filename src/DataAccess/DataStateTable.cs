// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

#endregion

using System;
using System.Linq.Expressions;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class DataStateTable<TData> : MySqlTable<TData>
        where TData : EditDataObject, IStateData, IIdentityData, new()
    {
        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE `{WriteTableName}` SET `{FieldDictionary["DataState"]}`=255";

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected virtual string ResetStateFileSqlCode => $@"`{FieldDictionary["DataState"]}`=0,`{FieldDictionary["IsFreeze"]}`=0";

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState(int id)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                var sql = $@"UPDATE `{WriteTableName}` 
SET {ResetStateFileSqlCode} 
WHERE {PrimaryKeyConditionSQL}";
                return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
            }
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState(Expression<Func<TData, bool>> lambda)
        {
            using (MySqlDataBaseScope.CreateScope(DataBase))
            {
                var convert = Compile(lambda);
                var sql = $@"UPDATE `{WriteTableName}` 
SET {ResetStateFileSqlCode} 
WHERE {convert.ConditionSql}";
                return DataBase.Execute(sql, convert.Parameters) >= 1;
            }
        }
    }
}