/*design by:agebull designer date:2017/5/26 19:43:32*/

using Gboxt.Common.WebUI;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;
using MySql.Data.MySqlClient;

namespace Agebull.SystemAuthority.Organizations.OrganizationPage
{
    public partial class Action : ApiPageBaseForDataState<OrganizationData, OrganizationDataAccess, OrganizationBusinessLogic>
    {
        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(OrganizationData data, FormConvert convert)
        {
            DefaultReadFormData(data, convert);
        }

        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public override OrganizationData CreateData()
        {
            return new OrganizationData
            {
                ParentId = GetIntArg("pid", 0)
            };
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                default:
                    DefaultActin(action);
                    break;
            }
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var id = GetIntArg("id", 0);
            SetCustomJsonResult(id == 0 ? Business.LoadEditTree() : Business.LoadTree(id));
        }

        /// <summary>
        ///     数据准备返回的处理
        /// </summary>
        /// <param name="result">当前的查询结果</param>
        /// <param name="condition">当前的查询条件</param>
        /// <param name="args">当前的查询参数</param>
        protected override bool CheckListResult(EasyUiGridData<OrganizationData> result, string condition, params MySqlParameter[] args)
        {
            SetCustomJsonResult(result.Data);
            return base.CheckListResult(result, condition, args);
        }
    }
}