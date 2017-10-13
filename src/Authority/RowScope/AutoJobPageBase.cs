using System.Collections.Generic;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    ///     表示一个工作流页面基类
    /// </summary>
    public abstract class AutoJobPageBase<TData, TAccess, TBusinessLogic> :
        ApiPageBaseForAudit<TData, TAccess, TBusinessLogic>
        where TData : AutoJobEntity, IWorkflowData, new()
        where TAccess : HitoryTable<TData>, new()
        where TBusinessLogic : AutoJobBusinessLogicBase<TData, TAccess>, new()
    {
        /// <summary>
        ///     详细数据载入
        /// </summary>
        protected override void OnDetailsLoaded(TData data, bool isNew)
        {
            if (!isNew)
                Business.CheckCanAudit(data);
            base.OnDetailsLoaded(data, isNew);
        }


        /// <summary>
        ///     数据载入的处理
        /// </summary>
        /// <param name="datas"></param>
        protected override void OnListLoaded(IList<TData> datas)
        {
            foreach (var data in datas)
            {
                Business.CheckCanAudit(data);
            }
            base.OnListLoaded(datas);
        }
    }
}