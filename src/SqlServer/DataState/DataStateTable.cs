// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TSqlServerDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public abstract class DataStateTable<TData, TSqlServerDataBase> : SqlServerTable<TData, TSqlServerDataBase>, IStateDataTable<TData>
        where TData : EditDataObject, IStateData, IIdentityData<long>, new()
        where TSqlServerDataBase : SqlServerDataBase
    {
        static DataStateTable()
        {
            DependencyHelper.ServiceCollection.TryAddSingleton<IDataTrigger, DataStateTrigger>();
        }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE [{ContextWriteTable}] 
SET [{FieldDictionary[nameof(IStateData.DataState)]}]=255";

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected virtual string ResetStateFileSqlCode(int state = 0, int isFreeze = 0) =>
            $@"
[{FieldDictionary[nameof(IStateData.DataState)]}]={state},
[{FieldDictionary[nameof(IStateData.IsFreeze)]}]={isFreeze}";

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ConditionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.Status.IsManageMode)
                return;
            conditions.Add($"[{FieldDictionary[nameof(IStateData.DataState)]}] < 255");
        }

        /// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override void CheckUpdateContition(ref string condition)
        {
            if (GlobalContext.Current.Status.IsManageMode)
                return;
            if (condition == null)
                condition = $"[{FieldDictionary[nameof(IStateData.IsFreeze)]}] = 0";
            else
                condition = $"[{FieldDictionary[nameof(IStateData.IsFreeze)]}] = 0 AND ({condition})";
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        public virtual bool SetState<TPrimaryKey>(DataStateType state, bool isFreeze, TPrimaryKey id)
        {
            var sql = $@"UPDATE [{ContextWriteTable}]
SET {ResetStateFileSqlCode((int)state, isFreeze ? 1 : 0)} 
WHERE {PrimaryKeyConditionSQL}";
            return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
        }


        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState<TPrimaryKey>(TPrimaryKey id)
        {
            var sql = $@"UPDATE [{ContextWriteTable}]
SET {ResetStateFileSqlCode()} 
WHERE {PrimaryKeyConditionSQL}";

            return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual bool ResetState(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            var sql = $@"UPDATE [{ContextWriteTable}]
SET {ResetStateFileSqlCode()} 
WHERE {convert.ConditionSql}";
            return DataBase.Execute(sql, convert.Parameters) > 0;
        }
    }
}