// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.DataEvents;
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
        ///     删除对象前置处理
        /// </summary>
        protected override async Task<bool> DoDelete(TPrimaryKey id)
        {
            if (await Access.AnyAsync(p => p.DataState == DataStateType.Delete && p.Id.Equals(id)))
                return await Access.PhysicalDeleteAsync(id);
            return await Access.DeletePrimaryKeyAsync(id);
        }

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
            await OnCommandSuccess(default, id, DataOperatorType.SetState);
            return true;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        public virtual Task<bool> Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => p.Id.Equals(id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public virtual Task<bool> Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => p.Id.Equals(id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     弃用对象
        /// </summary>
        public virtual Task<bool> Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => p.Id.Equals(id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        protected async Task<bool> SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            if (filter != null && !await Access.AnyAsync(filter))
                return false;
            await SetState(state, isFreeze, id);
            var cmd = state switch
            {
                DataStateType.Enable => DataOperatorType.Enable,
                DataStateType.Disable => DataOperatorType.Disable,
                DataStateType.Discard => DataOperatorType.Discard,
                DataStateType.None => DataOperatorType.Reset,
                _ => DataOperatorType.SetState,
            };
            if (cmd != DataOperatorType.SetState && Access.Provider.Option.CanRaiseEvent)
                await OnCommandSuccess(await Access.LoadByPrimaryKeyAsync(id), id, cmd);
            return true;
        }
        #endregion

        #region 修改状态

        /// <summary>
        /// 修改状态
        /// </summary>
        protected async Task<bool> SetState(DataStateType state, bool isFreeze, TPrimaryKey id)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            return await Access.SetValueAsync(id,
                (nameof(IStateData.DataState), state),
                ("is_freeze", isFreeze)) == 1;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        protected async Task<bool> SetState(DataStateType state, bool isFreeze, Expression<Func<TData, bool>> lambda)
        {
            using var levelScope = Access.InjectionScope(InjectionLevel.NotCondition);
            return await Access.SetValueAsync(lambda,
                (nameof(IStateData.DataState), state),
                ("is_freeze", isFreeze)) == 1;
        }



        /// <summary>
        /// 重置状态
        /// </summary>
        public Task<bool> ResetState(TPrimaryKey id)
        {
            return SetState(DataStateType.None, false, id);
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public Task<bool> ResetState(Expression<Func<TData, bool>> lambda)
        {
            return SetState(DataStateType.None, false, lambda);
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