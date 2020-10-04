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
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    /// <typeparam name="TData">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TBusinessLogic">业务对象</typeparam>
    public abstract class ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic> :
        ApiController<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     重置数据状态
        /// </summary>
        [Route("state/reset")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Reset(string[] selects)
        {
            await OnReset();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     废弃数据
        /// </summary>
        [Route("state/discard")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Discard(string[] selects)
        {
            await OnDiscard();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     禁用数据
        /// </summary>
        [Route("state/disable")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Disable(string[] selects)
        {
            await OnDisable();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     启用数据
        /// </summary>
        [Route("state/enable")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Enable(string[] selects)
        {
            await OnEnable();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 操作

        /// <summary>
        ///     恢复对象
        /// </summary>
        private async Task OnReset()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!await Business.LoopIds(ids, Business.Reset))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private async Task OnDiscard()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!await Business.LoopIds(ids, Business.Discard))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private async Task OnEnable()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!await Business.LoopIds(ids, Business.Enable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private async Task OnDisable()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!await Business.LoopIds(ids, Business.Disable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        #endregion

        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override Task<ApiPageData<TData>> GetListData(LambdaItem<TData> lambda)
        {
            if (!RequestArgumentConvert.TryGet("_state_", out int state) || state < 0 || state >= 0x100)
                return base.GetListData(lambda);
            lambda.AddRoot(p => p.DataState == (DataStateType)state);
            return base.GetListData(lambda);
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