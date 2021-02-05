// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TPrimaryKey, TBusinessLogic>
        : ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TPrimaryKey>, new()
    {
        #region ���

        /// <summary>
        ///     ��˲�ͨ��
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
        ///     �������ύ�����
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
        ///     �ύ���
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
        ///     У���������
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
        ///     ���ͨ��
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
        ///     �������
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
        ///     �˻�(���)
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

        #region �б�����

        /// <summary>
        ///     ��ȡ��ѯ����
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
                case 0x10: //����
                case 0xFF: //ɾ��
                    RequestArgumentConvert.SetArgument("_state_", audit);
                    break;
                case 0x13: //ͣ��
                    RequestArgumentConvert.SetArgument("_state_", (int)DataStateType.Disable);
                    break;
                case 0x11: //δ���
                    lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    break;
                case 0x12: //δ����
                    lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TBusinessLogic>
        : ApiControllerForAudit<TData, long, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<long>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, long>, new()
    {
    }
}