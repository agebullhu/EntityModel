// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using System.Linq;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     ���֧��APIҳ��Ļ���
    /// </summary>
    public abstract class ApiPageBaseForAudit<TData, TAccess, TBusinessLogic> :
        ApiPageBaseForDataState<TData, TAccess, TBusinessLogic>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
        where TBusinessLogic : BusinessLogicByAudit<TData, TAccess>, new()
    {
        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="action">����Ķ�������,��תΪСд</param>
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
        ///     �ύ���
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
        ///     �ύ���
        /// </summary>
        private void OnBackAudit()
        {
            if (!Business.Back(GetArg("selects")))
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     ����У��
        /// </summary>
        private void OnValidate()
        {
            DoValidate(GetArg("selects"));
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnUnAudit()
        {
            if (!Business.UnAudit(GetArg("selects")))
                SetFailed(BusinessContext.Current.GetFullMessage());
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
        ///     ����
        /// </summary>
        private void OnPullback()
        {
            var ids = GetArg("selects");
            var result = Business.Pullback(ids);
            if (!result)
                SetFailed(BusinessContext.Current.GetFullMessage());
        }

        /// <summary>
        ///     ���
        /// </summary>
        private void OnAuditDeny()
        {
            var ids = GetArg("selects");
            var result = Business.AuditDeny(ids);
            if (!result)
                SetFailed(BusinessContext.Current.GetFullMessage());
        }


        /// <summary>
        ///     ȡ���б�����
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
                        case 0x10://����
                        case 0xFF://ɾ��
                            SetArg("dataState", audit);
                            break;
                        case 0x13://ͣ��
                            SetArg("dataState", (int)DataStateType.Disable);
                            break;
                        case 0x11://δ���
                            lambda.AddRoot(p => p.AuditState <= AuditStateType.Again);
                            break;
                        case 0x12://δ����
                            lambda.AddRoot(p => p.AuditState < AuditStateType.End);
                            break;
                    }
            }
            base.GetListData(lambda);
        }
    }

    /// <summary>
    ///     �Զ�ʵ�ֻ�����ɾ�Ĳ�APIҳ��Ļ���
    /// </summary>
    public abstract class ApiPageBaseForAudit<TData, TAccess> :
        ApiPageBaseForAudit<TData, TAccess, BusinessLogicByAudit<TData, TAccess>>
        where TData : EditDataObject, IStateData, IHistoryData, IAuditData, IIdentityData, new()
        where TAccess : HitoryTable<TData>, new()
    {
    }
}