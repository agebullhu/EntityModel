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
using Agebull.EntityModel.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     审核支持API页面的基类
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TPrimaryKey, TBusinessLogic>
        : ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TPrimaryKey>, new()
    {
        #region 审核

        /// <summary>
        ///     审核不通过
        /// </summary>
        protected async Task<IApiResult> AuditDeny(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.AuditDeny(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     拉回已提交的审核
        /// </summary>
        protected async Task<IApiResult> Pullback(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.Pullback(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     提交审核
        /// </summary>
        protected async Task<IApiResult> SubmitAudit(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.Submit(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     校验审核数据
        /// </summary>
        protected async Task<IApiResult> Validate(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) )
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }


        /// <summary>
        ///     审核通过
        /// </summary>
        protected async Task<IApiResult> AuditPass(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.AuditPass(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     重新审核
        /// </summary>
        protected async Task<IApiResult> UnAudit(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.UnAudit(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     退回(审核)
        /// </summary>
        protected async Task<IApiResult> BackAudit(List<TPrimaryKey> selects)
        {
            if (!await DoValidate(selects) || !await Business.Back(selects))
                GlobalContext.Current.Status.LastState = OperatorStatusCode.BusinessError;
            return IsFailed
                    ? ApiResultHelper.State(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        private async Task<bool> DoValidate(IEnumerable<TPrimaryKey> ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = await Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                GlobalContext.Current.Status.LastMessage = message.ToString();
            }
            return succeed;
        }

        #endregion

        #region 列表数据

        /// <summary>
        ///     读取查询条件
        /// </summary>
        protected override void GetQueryFilter(LambdaItem<TData> lambda)
        {
            if (!RequestArgumentConvert.TryGet("_audit_", out int audit) || audit == 0x100 || audit < 0)
            {
                base.GetQueryFilter(lambda);
                return;
            }

            if (audit <= (int)AuditStateType.End)
            {
                lambda.AddRoot(p => p.AuditState == (AuditStateType)audit);
                base.GetQueryFilter(lambda);
                return;
            }

            switch (audit)
            {
                case 0x10: //废弃
                case 0xFF: //删除
                    RequestArgumentConvert.SetArgument("_state_", audit);
                    break;
                case 0x13: //停用
                    RequestArgumentConvert.SetArgument("_state_", (int)DataStateType.Disable);
                    break;
                case 0x11: //未审核
                    lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    break;
                case 0x12: //未结束
                    lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    ///     审核支持API页面的基类
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TBusinessLogic>
        : ApiControllerForAudit<TData, long, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<long>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, long>, new()
    {
    }
}