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
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TSqlServerDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public abstract class DataStateTable<TData, TSqlServerDataBase> : SqlServerTable<TData, TSqlServerDataBase>, IStateDataTable<TData>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TSqlServerDataBase : SqlServerDataBase
    {
        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE [{ContextWriteTable}] SET [{FieldDictionary["DataState"]}]=255";

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected virtual string ResetStateFileSqlCode => $@"[{FieldDictionary["DataState"]}]=0,[{FieldDictionary["IsFreeze"]}]=0";

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ContitionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.IsManageMode)
                return;
            conditions.Add($"[{FieldDictionary["DataState"]}] < 255");
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState(long id)
        {
            //await using (DataTableScope.CreateScope(this))
            {
                var sql = $@"UPDATE [{ContextWriteTable}]
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
            //await using (DataTableScope.CreateScope(this))
            {
                var convert = Compile(lambda);
                var sql = $@"UPDATE [{ContextWriteTable}]
SET {ResetStateFileSqlCode} 
WHERE {convert.ConditionSql}";
                return DataBase.Execute(sql, convert.Parameters) >= 1;
            }
        }
    }
}