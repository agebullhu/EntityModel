﻿// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;
using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using ZeroTeam.MessageMVC.ZeroApis;
using ZeroTeam.MessageMVC;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    public abstract class ApiControllerForDataState<TData, TBusinessLogic> :
        ApiController<TData, TBusinessLogic>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TBusinessLogic : class, IBusinessLogicByStateData<TData>, new()
    {
        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <param name="field"></param>
        protected override void CheckUnique<TValue>(string name, Expression<Func<TData, TValue>> field)
        {
            if (!TryGetValue(name, out var no))
            {
                SetFailed(name + "为空");
                return;
            }

            var id = GetIntArg("id", 0);
            Expression<Func<TData, bool>> condition;
            if (id == 0)
                condition = p => p.DataState < DataStateType.Delete;
            else
                condition = p => p.Id != id && p.DataState < DataStateType.Delete;
            if (Business.Access.IsUnique(field, no, condition))
                SetFailed(name + "[" + no + "]不唯一");
            else
                GlobalContext.Current.Status.LastMessage = name + "[" + no + "]唯一";
        }

        #endregion

        #region API

        /// <summary>
        ///     重置数据状态
        /// </summary>
        [Route("state/reset")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Reset(IdsArguent arg)
        {
            OnReset();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     锁定数据
        /// </summary>
        [Route("state/lock")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Lock(IdsArguent arg)
        {
            OnLock();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     废弃数据
        /// </summary>
        [Route("state/discard")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Discard(IdsArguent arg)
        {
            OnDiscard();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     禁用数据
        /// </summary>
        [Route("state/disable")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Disable(IdsArguent arg)
        {
            OnDisable();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     启用数据
        /// </summary>
        [Route("state/enable")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Enable(IdsArguent arg)
        {
            OnEnable();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 操作

        /// <summary>
        ///     锁定对象
        /// </summary>
        protected virtual void OnLock()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Lock))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     恢复对象
        /// </summary>
        private void OnReset()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Reset))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private void OnDiscard()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Discard))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private void OnEnable()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Enable))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private void OnDisable()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }

            if (!Business.LoopIds(ids, Business.Disable))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        #endregion


        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            if (!TryGet("_state_", out int state) || state < 0 || state >= 0x100)
                return base.GetListData(lambda);
            //BUG:using (ManageModeScope.CreateScope())
            {
                lambda.AddRoot(p => p.DataState == (DataStateType)state);
                return base.GetListData(lambda);
            }
        }

        #endregion
    }
}