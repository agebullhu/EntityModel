/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/27 9:01:56*/
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.OrganizationPage
{
    /// <summary>
    /// 机构
    /// </summary>
    public class ExportAction : ExportPageBase<OrganizationData, OrganizationDataAccess>
    {
        /// <summary>
        /// 导出表名称
        /// </summary>
        protected override string Name => "机构";

        /// <summary>
        /// 当前数据筛选器
        /// </summary>
        /// <returns></returns>
        protected override LambdaItem<OrganizationData> GetFilter()
        {
            var filter = new LambdaItem<OrganizationData>
            {
                Root = p => p.DataState <= DataStateType.Discard
            };
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                filter.AddAnd(p => p.Code.Contains(keyWord) || p.FullName.Contains(keyWord) || p.ShortName.Contains(keyWord) || p.TreeName.Contains(keyWord) || p.Memo.Contains(keyWord));
            }
            return filter;
        }
    }
}