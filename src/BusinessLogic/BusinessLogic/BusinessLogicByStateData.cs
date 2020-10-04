// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class BusinessLogicByStateData<TData, TPrimaryKey> : BusinessLogicBase<TData, TPrimaryKey>
        where TData : class, IIdentityData<TPrimaryKey>, IStateData, new()
    {
        #region 数据状态逻辑

        /// <summary>
        ///     是否可以执行保存操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override async Task<bool> CanSave(TData data, bool isAdd)
        {
            if (!await base.CanSave(data, isAdd))
                return false;
            if (!data.IsFreeze)
                return true;
            Context.LastMessage = "数据已锁定";
            Context.LastState = Context.ArgumentError;
            return false;
        }

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override async Task<bool> PrepareDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => id.Equals(p.Id) && !p.IsFreeze))
                return await base.PrepareDelete(id);
            Context.LastMessage = "数据已锁定";
            Context.LastState = Context.ArgumentError;
            return false;
        }

        /// <summary>
        ///     删除对象后置处理
        /// </summary>
        protected override async Task OnDeleted(TPrimaryKey id)
        {
            await OnStateChanged(id, BusinessCommandType.Delete);
            await base.OnDeleted(id);
        }

        /// <summary>
        ///     删除对象操作
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.DataState == DataStateType.Delete && Equals(p.Id, id)))
                return await Access.PhysicalDeleteAsync(id);
            return await Access.DeletePrimaryKeyAsync(id);
        }

        #endregion

        #region 数据状态修改

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override async Task<bool> LastSaved(TData data, bool isAdd)
        {
            await OnStateChanged(data, isAdd ? BusinessCommandType.AddNew : BusinessCommandType.Update);
            return await base.LastSaved(data, isAdd);
        }

        /// <summary>
        /// 是否统一处理状态变化
        /// </summary>
        protected bool unityStateChanged = false;

        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="cmd">命令</param>
        protected override async Task OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var old = Access.Option.NoInjection;
            Access.Option.NoInjection = true;
            try
            {
                await DoStateChanged(data);
                await OnInnerCommand(data, cmd);
            }
            finally
            {
                Access.Option.NoInjection = old;
            }
        }

        /// <summary>
        ///     内部命令执行完成后的处理
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="cmd">命令</param>
        protected sealed override async Task OnStateChanged(TPrimaryKey id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return;
            await OnStateChanged(data, cmd);
        }

        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task DoStateChanged(TData data) => Task.CompletedTask;

        #endregion

        #region 状态处理

        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<bool> Reset(TPrimaryKey id)
        {
            if (!await ResetState(id))
                return false;
            await OnStateChanged(id, BusinessCommandType.SetState);
            return true;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        public virtual Task<bool> Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => Equals(p.Id, id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public virtual Task<bool> Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => Equals(p.Id, id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     弃用对象
        /// </summary>
        public virtual Task<bool> Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => Equals(p.Id, id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        protected async Task<bool> SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            await using var connectionScope = await Access.DataBase.CreateConnectionScope();
            if (filter != null && !await Access.AnyAsync(filter))
                return false;
            if (filter == null && !await Access.ExistPrimaryKeyAsync(id))
                return false;
            await SetState(state, isFreeze, id);
            await OnStateChanged(id, BusinessCommandType.SetState);
            await connectionScope.Commit();
            return true;
        }

        #endregion

        #region MyRegion

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected string DeleteSqlCode => $@"UPDATE {Access.Option.ReadTableName} SET 
`{Access.Option.FieldMap[nameof(IStateData.DataState)]}`=255";

        /// <summary>
        ///     重置状态的SQL语句
        /// </summary>
        protected string ResetStateFileSqlCode(int state = 0, int isFreeze = 0) => $@"
{Access.Option.FieldMap[nameof(IStateData.DataState)]}={state},
{Access.Option.FieldMap[nameof(IStateData.IsFreeze)]}={isFreeze}";


        /// <summary>
        /// 修改状态
        /// </summary>
        protected virtual async Task<bool> SetState(DataStateType state, bool isFreeze, TPrimaryKey id)
        {
            var para = Access.ParameterCreater.CreateParameter(Access.Option.PrimaryKey,
                id,
                Access.SqlBuilder.GetDbType(Access.Option.PrimaryKey));


            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode((int)state, isFreeze ? 1 : 0)} 
WHERE {Access.SqlBuilder.PrimaryKeyCondition}";
            return await Access.DataBase.ExecuteAsync(sql, para) == 1;
        }


        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual async Task<bool> ResetState(TPrimaryKey id)
        {
            var para = Access.ParameterCreater.CreateParameter(Access.Option.PrimaryKey,
                id,
                Access.SqlBuilder.GetDbType(Access.Option.PrimaryKey));

            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode()} 
WHERE {Access.SqlBuilder.PrimaryKeyCondition}";

            return await Access.DataBase.ExecuteAsync(sql, para) == 1;
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual async Task<bool> ResetState(Expression<Func<TData, bool>> lambda)
        {
            var convert = Access.SqlBuilder.Compile(lambda);
            var sql = $@"UPDATE `{Access.Option.ReadTableName}` 
SET {ResetStateFileSqlCode()} 
WHERE {convert.ConditionSql}";

            return await Access.DataBase.ExecuteAsync(sql, convert.Parameters) > 0;
        }

        #endregion
    }

    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    public abstract class BusinessLogicByStateData<TData> : BusinessLogicByStateData<TData, long>
        where TData : class, IIdentityData<long>, IStateData, new()
    {
    }
}