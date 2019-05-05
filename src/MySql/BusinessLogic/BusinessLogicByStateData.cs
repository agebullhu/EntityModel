// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;

#endregion

namespace Agebull.EntityModel.BusinessLogic.MySql
{
    /// <summary>
    /// 基于数据状态的业务逻辑基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TAccess">数据访问对象</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class BusinessLogicByStateData<TData, TAccess, TDatabase>
        : UiBusinessLogicBase<TData, TAccess, TDatabase>, IBusinessLogicByStateData<TData>
        where TData : EditDataObject, IIdentityData, IStateData, new()
        where TAccess : DataStateTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region 批量操作


        /// <summary>
        ///     启用对象
        /// </summary>
        public bool Enable(IEnumerable<long> sels)
        {
            return DoByIds(sels, Enable);
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public bool Disable(IEnumerable<long> sels)
        {
            return DoByIds(sels, Disable);
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public bool Lock(IEnumerable<long> sels)
        {
            return DoByIds(sels, Lock);
        }
        #endregion

        #region 数据状态逻辑

        /// <summary>
        ///     是否可以执行保存操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool CanSave(TData data, bool isAdd)
        {
            return !data.IsFreeze && data.DataState < DataStateType.Discard;
        }

        /// <summary>
        ///     删除对象前置处理
        /// </summary>
        protected override bool PrepareDelete(long id)
        {
            if (Access.Any(p => p.Id == id && p.IsFreeze))
                return false;
            return base.PrepareDelete(id);
        }
        /// <summary>
        ///     删除对象后置处理
        /// </summary>
        protected override void OnDeleted(long id)
        {
            OnStateChanged(id, BusinessCommandType.Delete);
            base.OnDeleted(id);
        }

        /// <summary>
        ///     删除对象操作
        /// </summary>
        protected override bool DoDelete(long id)
        {
            if (Access.Any(p => p.DataState == DataStateType.Delete && p.Id == id))
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
        protected sealed override void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            LogRecorderX.MonitorTrace("OnStateChanged");
            OnInnerCommand(data, cmd);
            DoStateChanged(data);
        }

        /// <summary>
        ///     内部命令执行完成后的处理
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="cmd">命令</param>
        protected sealed override void OnStateChanged(long id, BusinessCommandType cmd)
        {
            if (!unityStateChanged)
                return;
            var data = Access.LoadByPrimaryKey(id);
            if (data != null)
                OnStateChanged(data, cmd);
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
        /// <param name="data"></param>
        public bool ResetState(TData data)
        {
            if (data == null)
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                //if (!DoResetState(data))
                //    return false;

                data.DataState = DataStateType.None;
                data.IsFreeze = false;
                Access.ResetState(data.Id);
                OnStateChanged(data, BusinessCommandType.Reset);
                return scope.SetState(true);
            }
        }

        ///// <summary>
        /////     重置数据状态
        ///// </summary>
        ///// <param name="data"></param>
        //protected virtual bool DoResetState(TData data)
        //{
        //    data.DataState = DataStateType.None;
        //    data.IsFreeze = false;
        //    return true;
        //}

        /// <summary>
        ///     重置数据状态
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(long id)
        {
            return SetDataState(id, DataStateType.None, null, false);
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        public virtual bool Enable(long id)
        {
            return SetDataState(id, DataStateType.Enable, p => p.Id == id && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None), true);
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        public virtual bool Disable(long id)
        {
            return SetDataState(id, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.Enable, true);
        }
        /// <summary>
        ///     弃用对象
        /// </summary>
        public virtual bool Discard(long id)
        {
            return SetDataState(id, DataStateType.Discard, p => p.Id == id && p.DataState == DataStateType.None, true);
        }
        /// <summary>
        ///     锁定对象
        /// </summary>
        public virtual bool Lock(long id)
        {
            if (!Access.Any(p => p.Id == id && p.DataState < DataStateType.Discard && !p.IsFreeze))
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                Access.SetValue(p => p.IsFreeze, true, id);
                Access.SetValue(p => p.DataState, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.None);

                OnStateChanged(id, BusinessCommandType.Lock);
                return scope.SetState(true);
            }
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        protected bool SetDataState(long id, DataStateType state, Expression<Func<TData, bool>> filter, bool? setFreeze)
        {
            if (filter != null && !Access.Any(filter))
                return false;
            if (filter == null && !Access.ExistPrimaryKey(id))
                return false;
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                Access.SetValue(p => p.DataState, state, id);
                if (setFreeze != null)
                    Access.SetValue(p => p.IsFreeze, setFreeze.Value, id);
                OnStateChanged(id, BusinessCommandType.SetState);
                return scope.SetState(true);
            }
        }
        #endregion
    }
}