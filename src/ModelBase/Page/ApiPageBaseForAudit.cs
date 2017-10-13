// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using System.Linq;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     审核支持API页面的基类
    /// </summary>
    public abstract class ApiPageBaseForAudit<TData, TAccess, TBusinessLogic> :
        ApiPageBaseForDataState<TData, TAccess, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TAccess>, new()
    {
        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "validate":
                    OnValidate();
                    break;
                case "submit":
                    OnSubmitAudit();
                    break;
                case "pullback":
                    OnPullback();
                    break;
                case "deny":
                    OnAuditDeny();
                    break;
                case "pass":
                    OnAuditPass();
                    break;
                case "reaudit":
                    OnUnAudit();
                    break;
                case "back":
                    OnBackAudit();
                    break;
                default:
                    base.DoActinEx(action);
                    break;
            }
        }
        
        /// <summary>
        ///     提交审核
        /// </summary>
        protected virtual void OnSubmitAudit()
        {
            var ids = GetArg("selects");
            if (!DoValidate(ids))
                return;
            if (!Business.Submit(ids))
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     提交审核
        /// </summary>
        private void OnBackAudit()
        {
            if (!Business.Back(GetArg("selects")))
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     数据校验
        /// </summary>
        private void OnValidate()
        {
            DoValidate(GetArg("selects"));
        }

        /// <summary>
        ///     审核
        /// </summary>
        private void OnUnAudit()
        {
            if (!Business.UnAudit(GetArg("selects")))
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     审核
        /// </summary>
        protected virtual void OnAuditPass()
        {
            var ids = GetArg("selects");
            if (!DoValidate(ids))
                return;
            var result = Business.AuditPass(ids);
            if (!result)
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        private bool DoValidate(string ids)
        {
            var message = new ValidateResultDictionary();
            bool succeed = Business.Validate(ids, message.TryAdd);

            if (message.Result.Count > 0)
            {
                SetResultData(message);
                IsFailed = true;

                Message = message.ToString();
                Message2 = message.ToString();
                return false;
            }
            if (succeed)
                return true;
            SetFailed(BusinessContext.Current.GetFullMessage());
            return false;
        }

        /// <summary>
        ///     拉回
        /// </summary>
        private void OnPullback()
        {
            var ids = GetArg("selects");
            var result = Business.Pullback(ids);
            if (!result)
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     审核
        /// </summary>
        private void OnAuditDeny()
        {
            var ids = GetArg("selects");
            var result = Business.AuditDeny(ids);
            if (!result)
                SetFailed(BusinessContext.Current.GetFullMessage());
        }


        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData(LambdaItem<TData> lambda)
        {
            var audit = GetIntArg("audit", -1);
            if (audit != 0x100 && audit >= 0)
            {
                if (audit <= (int)AuditStateType.End)
                {
                    lambda.AddRoot(p => p.AuditState == (AuditStateType)audit);
                }
                else switch (audit)
                    {
                        case 0x10://废弃
                        case 0xFF://删除
                            SetArg("dataState", audit);
                            break;
                        case 0x13://停用
                            SetArg("dataState", (int)DataStateType.Disable);
                            break;
                        case 0x11://未审核
                            lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                            break;
                        case 0x12://未结束
                            lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                            break;
                    }
            }
            base.GetListData(lambda);
        }
    }

    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiPageBaseForAudit<TData, TAccess> :
        ApiPageBaseForAudit<TData, TAccess, BusinessLogicByAudit<TData, TAccess>>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : HitoryTable<TData>, new()
    {
    }
}