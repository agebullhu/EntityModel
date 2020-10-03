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
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>
    ///     审核支持API页面的基类
    /// </summary>
    public abstract class ApiControllerForAudit<TData, TPrimaryKey, TBusinessLogic>
        : ApiControllerForDataState<TData, TPrimaryKey, TBusinessLogic>
        where TData : class, IStateData, IHistoryData, IAuditData, IIdentityData<TPrimaryKey>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TPrimaryKey>, new()
    {
        #region API

        /// <summary>
        ///     审核不通过
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
        ///     拉回已提交的审核
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
        ///     提交审核
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
        ///     校验审核数据
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
        ///     审核通过
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
        ///     重新审核
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
        ///     退回(审核)
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

        #region 抽象


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

        #region 列表数据

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
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
                case 0x10: //废弃
                case 0xFF: //删除
                    base.GetQueryFilter(filter, "_state_", value);
                    return;
                case 0x13: //停用
                    base.GetQueryFilter(filter, "_state_", ((int)DataStateType.Disable).ToString());
                    return;
                case 0x11: //未审核
                    filter.AddRoot(p => p.AuditState <= AuditStateType.Again);
                    return;
                case 0x12: //未结束
                    filter.AddRoot(p => p.AuditState < AuditStateType.End);
                    return;
            }
            filter.AddRoot(p => p.AuditState == (AuditStateType)audit);
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