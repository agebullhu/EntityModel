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
        ApiController<TData, TPrimaryKey, TBusinessLogic>
        where TData : EditDataObject, IStateData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : class, IBusinessLogicByStateData<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     重置数据状态
        /// </summary>
        [Route("state/reset")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public IApiResult Reset(IdsArguent arg)
        {
            OnReset();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     废弃数据
        /// </summary>
        [Route("state/discard")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public IApiResult Discard(IdsArguent arg)
        {
            OnDiscard();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     禁用数据
        /// </summary>
        [Route("state/disable")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public IApiResult Disable(IdsArguent arg)
        {
            OnDisable();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     启用数据
        /// </summary>
        [Route("state/enable")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public IApiResult Enable(IdsArguent arg)
        {
            OnEnable();
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <param name="field"></param>
        protected override void CheckUnique<TValue>(string name, Expression<Func<TData, TValue>> field)
        {
            if (!RequestArgumentConvert.TryGet(name, out string no))
            {
                SetFailed(name + "为空");
                return;
            }

            var id = RequestArgumentConvert.GetInt("id", 0);
            Expression<Func<TData, bool>> condition;
            if (id == 0)
                condition = p => p.DataState < DataStateType.Delete;
            else
                condition = p => !Equals(p.Id , id) && p.DataState < DataStateType.Delete;
            if (Business.Access.IsUnique(field, no, condition))
                SetFailed(name + "[" + no + "]不唯一");
            else
                GlobalContext.Current.Status.LastMessage = name + "[" + no + "]唯一";
        }

        #endregion

        #region 操作

        /// <summary>
        ///     恢复对象
        /// </summary>
        private void OnReset()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Reset))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private void OnDiscard()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Discard))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private void OnEnable()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Enable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private void OnDisable()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Disable))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
        }

        #endregion

        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            if (!RequestArgumentConvert.TryGet("_state_", out int state) || state < 0 || state >= 0x100)
                return base.GetListData(lambda);
            //BUG:using (ManageModeScope.CreateScope())
            {
                lambda.AddRoot(p => p.DataState == (DataStateType)state);
                return base.GetListData(lambda);
            }
        }

        #endregion
    }
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiControllerForDataState<TData, TBusinessLogic> : ApiControllerForDataState<TData, long, TBusinessLogic>
        where TData : EditDataObject, IStateData, IIdentityData<long>, new()
        where TBusinessLogic : class, IBusinessLogicByStateData<TData, long>, new()
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