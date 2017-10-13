/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/25 14:10:20*/
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.UserPage
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class ExportAction : ExportPageBase<UserData, UserDataAccess>
    {
        /// <summary>
        /// 导出表名称
        /// </summary>
        protected override string Name => "系统用户";

        /// <summary>
        /// 当前数据筛选器
        /// </summary>
        /// <returns></returns>
        protected override LambdaItem<UserData> GetFilter()
        {
            var filter = new LambdaItem<UserData>
            {
                Root = p => p.DataState <= DataStateType.Discard
            };
            SetKeywordFilger(filter);
            return filter;
        }
    }
}