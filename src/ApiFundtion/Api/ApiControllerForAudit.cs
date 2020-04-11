// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TBusinessLogic>
        : ApiControllerForDataState<TData, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TBusinessLogic : class, IBusinessLogicByAudit<TData>, new()
    {
        #region API

        /// <summary>
        ///     ��˲�ͨ��
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
        ///     �������ύ�����
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
        ///     �ύ���
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
        ///     У���������
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
        ///     ���ͨ��
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
        ///     �������
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
        ///     �˻�(���)
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

        #region ����

        /// <summary>
        ///     �ύ���
        /// </summary>
        protected virtual void OnSubmitAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;

        }

        /// <summary>
        ///     �˻����
        /// </summary>
        private void OnBackAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.Back(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnUnAudit()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.UnAudit(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected virtual void OnAuditPass()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
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
        ///     ����
        /// </summary>
        private void OnPullback()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.Pullback(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnAuditDeny()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("û������");
                return;
            }
            if (!Business.AuditDeny(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }
        #endregion

        #region �б�����

        /// <summary>
        ///     ȡ���б�����
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
                case 0x10: //����
                case 0xFF: //ɾ��
                    SetArg("dataState", audit);
                    break;
                case 0x13: //ͣ��
                    SetArg("dataState", (int)DataStateType.Disable);
                    break;
                case 0x11: //δ���
                    lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    break;
                case 0x12: //δ����
                    lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                    break;
            }
            return base.GetListData(lambda);
        }
        #endregion
    }

}