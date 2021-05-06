/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����: ��������
�޸�: -
*****************************************************/

#region ����

using Agebull.EntityModel.BusinessLogic;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using Agebull.EntityModel.Vue;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TPrimaryKey, TBusinessLogic>
        : ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     ��˲�ͨ��
        /// </summary>
        [Route("audit/deny")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> AuditDeny(IdsArgument<TPrimaryKey> args)
        {
            await OnAuditDeny();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �������ύ�����
        /// </summary>
        [Route("audit/pullback")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Pullback(IdsArgument<TPrimaryKey> args)
        {

            await OnPullback();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        [Route("audit/submit")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> SubmitAudit(IdsArgument<TPrimaryKey> args)
        {

            await OnSubmitAudit();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     У���������
        /// </summary>
        [Route("audit/validate")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> Validate(IdsArgument<TPrimaryKey> args)
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                return ApiResultHelper.Helper.ArgumentError;
            }

            await DoValidate(ids);
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }


        /// <summary>
        ///     ���ͨ��
        /// </summary>
        [Route("audit/pass")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> AuditPass(IdsArgument<TPrimaryKey> args)
        {
            await OnAuditPass();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �������
        /// </summary>
        [Route("audit/redo")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> UnAudit(IdsArgument<TPrimaryKey> args)
        {
            await OnUnAudit();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �˻�(���)
        /// </summary>

        [Route("audit/back")]
        [ApiOption(ApiOption.Public | ApiOption.DictionaryArgument)]
        public async Task<IApiResult> BackAudit(IdsArgument<TPrimaryKey> args)
        {
            await OnBackAudit();
            return IsFailed
                    ? ApiResultHelper.State(Business.Context.LastState, Business.Context.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region ����

        /// <summary>
        ///     �ύ���
        /// </summary>
        protected virtual async Task OnSubmitAudit()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }
            if (!await DoValidate(ids))
                return;
            if (!await Business.Submit(ids))
                Business.Context.LastState = OperatorStatusCode.BusinessError;

        }

        /// <summary>
        ///     �˻����
        /// </summary>
        private async Task OnBackAudit()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }
            if (!await Business.Back(ids))
                Business.Context.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private async Task OnUnAudit()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }
            if (!await Business.UnAudit(ids))
                Business.Context.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected virtual async Task OnAuditPass()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }
            if (!await DoValidate(ids))
            {
                Business.Context.LastState = OperatorStatusCode.BusinessError;
                return;
            }
            var result = await Business.AuditPass(ids);
            if (!result)
                Business.Context.LastState = OperatorStatusCode.BusinessError;
        }

        private async Task<bool> DoValidate(IEnumerable<TPrimaryKey> ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = await Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                Business.Context.LastMessage = message.ToString();
            }
            return succeed;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private async Task OnPullback()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }

            if (!await Business.Pullback(ids))
                Business.Context.LastState = OperatorStatusCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private async Task OnAuditDeny()
        {
            if (!RequestArgumentConvert.TryGetIDs("selects", Convert, out List<TPrimaryKey> ids))
            {
                SetFailed("û������");
                return;
            }
            if (!await Business.AuditDeny(ids))
                Business.Context.LastState = OperatorStatusCode.BusinessError;
        }
        #endregion

        #region �б�����

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        protected override async Task<ApiPageData<TData>> GetListData(LambdaItem<TData> lambda)
        {
            if (!RequestArgumentConvert.TryGet("_audit_", out int audit) || audit == 0x100 || audit < 0)
                return await base.GetListData(lambda);

            if (audit <= (int)AuditStateType.End)
            {
                lambda.AddRoot(p => p.AuditState == (AuditStateType)audit);
                return await base.GetListData(lambda);
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
            return await base.GetListData(lambda);
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