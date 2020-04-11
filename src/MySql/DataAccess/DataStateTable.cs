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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;
using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TMySqlDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public abstract class DataStateTable<TData, TMySqlDataBase> : MySqlTable<TData, TMySqlDataBase>, IStateDataTable<TData>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE `{ContextWriteTable}` SET `{FieldDictionary[nameof(IStateData.DataState)]}`=255";

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected virtual string ResetStateFileSqlCode => $@"`{FieldDictionary[nameof(IStateData.DataState)]}`=0,`{FieldDictionary[nameof(IStateData.IsFreeze)]}`=0";

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ContitionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.Status.IsManageMode)
                return;
            conditions.Add($"`{FieldDictionary[nameof(IStateData.DataState)]}` < 255");
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState(long id)
        {
            using (DataTableScope.CreateScope(this))
            {
                var sql = $@"UPDATE `{ContextWriteTable}` 
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
            using (DataTableScope.CreateScope(this))
            {
                var convert = Compile(lambda);
                var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode} 
WHERE {convert.ConditionSql}";
                return DataBase.Execute(sql, convert.Parameters.Cast<MySqlParameter>()) >= 1;
            }
        }
    }
}