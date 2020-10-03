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
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace ZeroTeam.AspNet.ModelApi
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
        public async Task<ApiResult> AuditDeny(List<TPrimaryKey> selects)
        {
            bool success = await Business.AuditDeny(selects);
            return !success || IsFailed
                ? ApiResultHelper.FromContext(Business.Context)
                : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �������ύ�����
        /// </summary>
        [Route("audit/pullback")]
        public async Task<ApiResult> Pullback(List<TPrimaryKey> selects)
        {
            bool success = await Business.Pullback(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �ύ���
        /// </summary>
        [Route("audit/submit")]
        public async Task<ApiResult> SubmitAudit(List<TPrimaryKey> selects)
        {
            bool success = await DoValidate(selects) && await Business.Submit(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     У���������
        /// </summary>
        [Route("audit/validate")]
        public async Task<ApiResult> Validate(List<TPrimaryKey> selects)
        {
            bool success = await DoValidate(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }


        /// <summary>
        ///     ���ͨ��
        /// </summary>
        [Route("audit/pass")]
        public async Task<ApiResult> AuditPass(List<TPrimaryKey> selects)
        {
            bool success = await Business.AuditPass(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �������
        /// </summary>
        [Route("audit/redo")]
        public async Task<ApiResult> UnAudit(List<TPrimaryKey> selects)
        {
            bool success = await Business.UnAudit(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        /// <summary>
        ///     �˻�(���)
        /// </summary>

        [Route("audit/back")]
        public async Task<ApiResult> BackAudit(List<TPrimaryKey> selects)
        {
            bool success = await Business.Back(selects);
            return !success || IsFailed
                    ? ApiResultHelper.FromContext(Business.Context)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region ����


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

        #endregion

        #region �б�����

        /// <summary>
        ///     ��ȡ��ѯ����
        /// </summary>
        /// <param name="filter">ɸѡ��</param>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        protected override void GetQueryFilter(LambdaItem<TData> filter, string field, string value)
        {
            if (field != "_audit_" || !int.TryParse(value, out int audit) || audit == 0x100 || audit < 0)
            {
                base.GetQueryFilter(filter, field, value);
                return;
            }

            if (audit <= (int)AuditStateType.End)
            {
                filter.AddRoot(p => p.AuditState == (AuditStateType)audit);
                return;
            }

            switch (audit)
            {
                case 0x10: //����
                case 0xFF: //ɾ��
                    base.GetQueryFilter(filter, "_state_", value);
                    return;
                case 0x13: //ͣ��
                    base.GetQueryFilter(filter, "_state_", ((int)DataStateType.Disable).ToString());
                    return;
                case 0x11: //δ���
                    filter.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    return;
                case 0x12: //δ����
                    filter.AddRoot(p => p.AuditState < AuditStateType.End);
                    return;
            }
            filter.AddRoot(p => p.AuditState == (AuditStateType)audit);
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