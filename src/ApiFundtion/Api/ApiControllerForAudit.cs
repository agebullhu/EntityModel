// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using System.Web.Http;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.BusinessLogic;
using Agebull.Common.Rpc;
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
            var ids = GetLongArrayArg("selects");
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     �˻����
        /// </summary>
        private void OnBackAudit()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.Back(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnUnAudit()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.UnAudit(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        protected virtual void OnAuditPass()
        {
            var ids = GetLongArrayArg("selects");
            if (!DoValidate(ids))
            {
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
                return;
            }
            var result = Business.AuditPass(ids);
            if (!result)
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        private bool DoValidate(IEnumerable<long> ids)
        {
            var message = new ValidateResultDictionary();
            var succeed = Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                GlobalContext.Current.LastMessage = message.ToString();
            }
            return succeed;
        }

        /// <summary>
        ///     ����
        /// </summary>
        private void OnPullback()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.Pullback(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnAuditDeny()
        {
            var ids = GetLongArrayArg("selects");
            if (!Business.AuditDeny(ids))
                GlobalContext.Current.LastState = ErrorCode.LogicalError;
        }


        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        protected override ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            var audit = GetIntArg("audit", -1);
            if (audit == 0x100 || audit < 0)
                return base.GetListData(lambda);
            if (audit <= (int) AuditStateType.End)
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

        #region API

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/deny")]
        public ApiResponseMessage AuditDeny()
        {
            
            OnAuditDeny();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/pullback")]
        public ApiResponseMessage Pullback()
        {
            
            OnPullback();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/submit")]
        public ApiResponseMessage SubmitAudit()
        {
            
            OnSubmitAudit();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/validate")]
        public ApiResponseMessage Validate()
        {
            
            DoValidate(GetLongArrayArg("selects"));
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }


        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/pass")]
        public ApiResponseMessage AuditPass()
        {
            
            OnAuditPass();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
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
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
                : Request.ToResponse(ApiResult.Succees());
        }

        /// <summary>
        ///     ���ڽ����ϵĵ�ǰ�û����Է��ʵİ�ť����
        /// </summary>
        [HttpPost]
        [Route("audit/back")]
        public ApiResponseMessage BackAudit()
        {
            
            OnBackAudit();
            return IsFailed
                ? Request.ToResponse(new ApiResult
                {
                    Success = false,
                    Status = GlobalContext.Current.LastStatus
                })
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