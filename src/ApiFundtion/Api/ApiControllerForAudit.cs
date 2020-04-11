// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using ZeroTeam.MessageMVC.Context;
using Agebull.EntityModel.BusinessLogic;
using ZeroTeam.MessageMVC.ZeroApis;
using ZeroTeam.MessageMVC;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     审核支持API页面的基类
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TBusinessLogic>
        : ApiControllerForDataState<TData, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TBusinessLogic : class, IBusinessLogicByAudit<TData>, new()
    {
        #region API

        /// <summary>
        ///     审核不通过
        /// </summary>

        [Route("audit/deny")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult AuditDeny(IdsArguent arg)
        {

            OnAuditDeny();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     拉回已提交的审核
        /// </summary>

        [Route("audit/pullback")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Pullback(IdsArguent arg)
        {

            OnPullback();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     提交审核
        /// </summary>

        [Route("audit/submit")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult SubmitAudit(IdsArguent arg)
        {

            OnSubmitAudit();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     校验审核数据
        /// </summary>

        [Route("audit/validate")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Validate(IdsArguent arg)
        {
            if (!TryGet("selects", out long[] ids))
            {
                return ApiResultHelper.Helper.ArgumentError;
            }

            DoValidate(ids);
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }


        /// <summary>
        ///     审核通过
        /// </summary>

        [Route("audit/pass")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult AuditPass(IdsArguent arg)
        {

            OnAuditPass();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     重新审核
        /// </summary>

        [Route("audit/redo")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult UnAudit(IdsArguent arg)
        {
            OnUnAudit();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     退回(审核)
        /// </summary>

        [Route("audit/back")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.Employe | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult BackAudit(IdsArguent arg)
        {

            OnBackAudit();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 抽象

        /// <summary>
        ///     提交审核
        /// </summary>
        protected virtual void OnSubmitAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;

        }

        /// <summary>
        ///     退回审核
        /// </summary>
        private void OnBackAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!Business.Back(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     审核
        /// </summary>
        private void OnUnAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!Business.UnAudit(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     审核
        /// </summary>
        protected virtual void OnAuditPass()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!DoValidate(ids))
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
                return;
            }
            var result = Business.AuditPass(ids);
            if (!result)
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        private bool DoValidate(IEnumerable<long> ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                GlobalContext.Current.Status.LastMessage = message.ToString();
            }
            return succeed;
        }

        /// <summary>
        ///     拉回
        /// </summary>
        private void OnPullback()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!Business.Pullback(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     审核
        /// </summary>
        private void OnAuditDeny()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!Business.AuditDeny(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }
        #endregion

        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            if (!TryGet("_audit_", out int audit) || audit == 0x100 || audit < 0)
                return base.GetListData(lambda);

            if (audit <= (int)AuditStateType.End)
            {
                lambda.AddRoot(p => p.AuditState == (AuditStateType)audit);
                return base.GetListData(lambda);
            }

            switch (audit)
            {
                case 0x10: //废弃
                case 0xFF: //删除
                    SetArg("dataState", audit);
                    break;
                case 0x13: //停用
                    SetArg("dataState", (int)DataStateType.Disable);
                    break;
                case 0x11: //未审核
                    lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    break;
                case 0x12: //未结束
                    lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                    break;
            }
            return base.GetListData(lambda);
        }
        #endregion
    }

}