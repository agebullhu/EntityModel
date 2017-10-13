/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 9:01:57*/
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Gboxt.Common.Workflow.DataAccess;

namespace Gboxt.Common.Workflow.UserJobPage
{
    /// <summary>
    /// 用户工作列表
    /// </summary>
    public class ExportAction : ExportPageBase<UserJobData, UserJobDataAccess>
    {
        /// <summary>
        /// 导出表名称
        /// </summary>
        protected override string Name => "用户工作列表";

        /// <summary>
        /// 当前数据筛选器
        /// </summary>
        /// <returns></returns>
        protected override LambdaItem<UserJobData> GetFilter()
        {
            var filter = new LambdaItem<UserJobData>
            {
                Root = p => p.DataState <= DataStateType.Discard
            };
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                filter.AddAnd(p => p.Title.Contains(keyWord) || p.Message.Contains(keyWord) || p.ToUserName.Contains(keyWord) || p.FromUserName.Contains(keyWord));
            }
            return filter;
        }
    }
}