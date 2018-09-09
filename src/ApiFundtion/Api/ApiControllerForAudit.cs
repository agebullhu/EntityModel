// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Web.Http;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.BusinessLogic;
using Agebull.Common.Rpc;
using Agebull.Common.WebApi;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.Extends;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.Common.WebApi
{
    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TAccess, TDatabase, TBusinessLogic> :
        ApiControllerForDataState<TData, TAccess, TDatabase, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : HitoryTable<TData, TDatabase>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TAccess, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        /// <summary>
        ///     �ύ���
        /// </summary>
        protected virtual void OnSubmitAudit()
        {
            var ids = GetArg("selects");
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                SetFailed(ApiContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        private void OnBackAudit()
        {
            if (!Business.Back(GetArg("selects")))
                SetFailed(ApiContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnUnAudit()
        {
            if (!Business.UnAudit(GetArg("selects")))
                SetFailed(ApiContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected virtual void OnAuditPass()
        {
            var ids = GetArg("selects");
            if (!DoValidate(ids))
                return;
            var result = Business.AuditPass(ids);
            if (!result)
                SetFailed(ApiContext.Current.GetFullMessage());
        }

        private bool DoValidate(string ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                IsFailed = true;
                Message = message.ToString();
                Message2 = message.ToString();
                return false;
            }

            if (succeed)
                return true;
            SetFailed(ApiContext.Current.GetFullMessage());
            return false;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private void OnPullback()
        {
            var ids = GetArg("selects");
            var result = Business.Pullback(ids);
            if (!result)
                SetFailed(ApiContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnAuditDeny()
        {
            var ids = GetArg("selects");
            var result = Business.AuditDeny(ids);
            if (!result)
                SetFailed(ApiContext.Current.GetFullMessage());
        }


        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            var audit = GetIntArg("audit", -1);
            if (audit != 0x100 && audit >= 0)
            {
                if (audit <= (int) AuditStateType.End)
                    lambda.AddRoot(p => p.AuditState == (AuditStateType) audit);
                else
                    switch (audit)
                    {
                        case 0x10: //����
                        case 0xFF: //ɾ��
                            SetArg("dataState", audit);
                            break;
                        case 0x13: //ͣ��
                            SetArg("dataState", (int) DataStateType.Disable);
                            break;
                        case 0x11: //δ���
                            lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                            break;
                        case 0x12: //δ����
                            lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                            break;
                    }
            }

            return base.GetListData(lambda);
        }

        #region API

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/deny")]
        public ApiResponseMessage AuditDeny()
        {
            InitForm();
            OnAuditDeny();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/pullback")]
        public ApiResponseMessage Pullback()
        {
            InitForm();
            OnPullback();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/submit")]
        public ApiResponseMessage SubmitAudit()
        {
            InitForm();
            OnSubmitAudit();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/validate")]
        public ApiResponseMessage Validate()
        {
            InitForm();
            DoValidate(GetArg("selects"));
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }


        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/pass")]
        public ApiResponseMessage AuditPass()
        {
            InitForm();
            OnAuditPass();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/redo")]
        public ApiResponseMessage UnAudit()
        {
            OnUnAudit();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/back")]
        public ApiResponseMessage BackAudit()
        {
            InitForm();
            OnBackAudit();
            return IsFailed
                ? Request.ToResponse(ApiResult.Error(State, Message))
                : Request.ToResponse(ApiResult.Succees());
        }

        #endregion
    }

    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TAccess, TDatabase> :
        ApiControllerForDataState<TData, TAccess, TDatabase, BusinessLogicByAudit<TData, TAccess, TDatabase>>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : HitoryTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
    }
}