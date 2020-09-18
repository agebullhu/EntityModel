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
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class BusinessLogicByStateData<TData, TPrimaryKey, TAccess>
        : UiBusinessLogicBase<TData, TPrimaryKey, TAccess>, IBusinessLogicByStateData<TData, TPrimaryKey>
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, IStateData, new()
        where TAccess : class, IStateDataTable<TData>, new()
    {
        #region 数据状态逻辑

        /// <summary>
        ///     是否可以执行保存操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool CanSave(TData data, bool isAdd)
        {
            if (!base.CanSave(data, isAdd))
                return false;
            if (!data.IsFreeze)
                return true;
            GlobalContext.Current.Status.LastMessage = "数据已锁定";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return false;
        }

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override bool PrepareDelete(TPrimaryKey id)
        {
            if (Access.Any(p => id.Equals(p.Id ) && !p.IsFreeze))
                return base.PrepareDelete(id);
            GlobalContext.Current.Status.LastMessage = "数据已锁定";
            GlobalContext.Current.Status.LastState = OperatorStatusCode.ArgumentError;
            return false;
        }

        /// <summary>
        ///     删除对象后置处理
        /// </summary>
        protected override void OnDeleted(TPrimaryKey id)
        {
            OnStateChanged(id, BusinessCommandType.Delete);
            base.OnDeleted(id);
        }

        /// <summary>
        ///     删除对象操作
        /// </summary>
        protected override bool DoDelete(TPrimaryKey id)
        {
            if (Access.Any(p => p.DataState == DataStateType.Delete && Equals(p.Id , id)))
                return Access.PhysicalDelete(id);
            return Access.DeletePrimaryKey(id);
        }

        #endregion

        #region 数据状态修改

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool LastSaved(TData data, bool isAdd)
        {
            OnStateChanged(data, isAdd ? BusinessCommandType.AddNew : BusinessCommandType.Update);
            return base.LastSaved(data, isAdd);
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
        protected override void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var old = Access.NoInjection;
            Access.NoInjection = true;
            try
            {
                DoStateChanged(data);
                OnInnerCommand(data, cmd);
            }
            finally
            {
                Access.NoInjection = old;
            }
        }

        /// <summary>
        ///     内部命令执行完成后的处理
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="cmd">命令</param>
        protected sealed override void OnStateChanged(TPrimaryKey id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = Access.LoadByPrimaryKey(id);
            if (data == null)
                return;
            OnStateChanged(data,cmd);
        }

        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void DoStateChanged(TData data)
        {
        }

        #endregion

        #region 状态处理

        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(TPrimaryKey id)
        {
            if (!Access.ResetState(id))
                return false;
            OnStateChanged(id, BusinessCommandType.SetState);
            return true;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        public virtual bool Enable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Enable, true,
                p => Equals(p.Id , id) && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public virtual bool Disable(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Disable, true,
                p => Equals(p.Id , id) && (p.DataState == DataStateType.Enable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     弃用对象
        /// </summary>
        public virtual bool Discard(TPrimaryKey id)
        {
            return SetDataState(id, DataStateType.Discard, true, p => Equals(p.Id , id) && p.DataState == DataStateType.None);
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        protected bool SetDataState(TPrimaryKey id, DataStateType state, bool isFreeze, Expression<Func<TData, bool>> filter)
        {
            if (filter != null && !Access.Any(filter))
                return false;
            if (filter == null && !Access.ExistPrimaryKey(id))
                return false;
            Access.SetState(state, isFreeze, id);
            using var scope = TransactionScope.CreateScope(Access.DataBase);
            Access.SetValue(p => p.IsFreeze, isFreeze, id);
            Access.SetValue(p => p.DataState, state, id);
            OnStateChanged(id, BusinessCommandType.SetState);
            return scope.Succeed();
        }

        #endregion
    }

    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    public class BusinessLogicByStateData<TData, TAccess>
        : BusinessLogicByStateData<TData,long, TAccess>
        where TData : EditDataObject, IIdentityData<long>, IStateData, new()
        where TAccess : class, IStateDataTable<TData>, new()
    {
    }
}