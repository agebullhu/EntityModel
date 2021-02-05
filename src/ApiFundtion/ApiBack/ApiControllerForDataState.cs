// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TBusinessLogic">业务对象</typeparam>
    public abstract class ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic> :
        WriteApiController<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     重置数据状态
        /// </summary>
        protected async Task<IApiResult> Reset(List<TPrimaryKey> selects)
        {
            if (!await Business.LoopIds(selects, Business.Reset))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     废弃数据
        /// </summary>
        protected async Task<IApiResult> Discard(List<TPrimaryKey> selects)
        {
            if (!await Business.LoopIds(selects, Business.Discard))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     禁用数据
        /// </summary>
        protected async Task<IApiResult> Disable(List<TPrimaryKey> selects)
        {
            if (!await Business.LoopIds(selects, Business.Disable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     启用数据
        /// </summary>
        protected async Task<IApiResult> Enable(List<TPrimaryKey> selects)
        {
            if (!await Business.LoopIds(selects, Business.Enable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 列表数据

        /// <summary>
        ///     读取查询条件
        /// </summary>
        protected override void GetQueryFilter(LambdaItem<TData> lambda)
        {
            if (!RequestArgumentConvert.TryGet("_state_", out int state) || state < 0 || state >= 0x100)
                lambda.AddRoot(p => p.DataState == (DataStateType)state);
        }

        #endregion
    }
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiControllerForDataState<TData, TBusinessLogic> : ApiControllerForDataState<TData, long, TBusinessLogic>
        where TData : class, IStateData, IIdentityData<long>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, long>, new()
    {
        ///<inheritdoc/>
        protected sealed override (bool, long) Convert(string value)
        {
            if (value != null && long.TryParse(value, out var id))
            {
                return (true, id);
            }
            return (false, 0);
        }
    }
}