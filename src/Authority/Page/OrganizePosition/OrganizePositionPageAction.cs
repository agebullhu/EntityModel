/*design by:agebull designer date:2017/5/26 19:43:33*/

using Gboxt.Common.WebUI;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

namespace Agebull.SystemAuthority.Organizations.OrganizePositionPage
{
    public partial class Action : ApiPageBaseForDataState<OrganizePositionData, OrganizePositionDataAccess, OrganizePositionBusinessLogic>
    {
        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public override OrganizePositionData CreateData()
        {
            return new OrganizePositionData
            {
                OrganizationId = GetIntArg("oid")
            };
        }
        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(OrganizePositionData data, FormConvert convert)
        {
            DefaultReadFormData(data,convert);
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "tree":
                    LoadOrgTree();
                    break;
                case "createall":
                    CreateAll();
                    break;
                default:
                    
                    base.DoActinEx(action);
                    break;
            }
        }

        /// <summary>
        /// 添加所有主管与办事员
        /// </summary>
        void CreateAll()
        {
            Business.CreateAll();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void LoadOrgTree()
        {
            SetCustomJsonResult(OrganizationBusinessLogic.LoadTreeForUi(0));
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var filter = new LambdaItem<OrganizePositionData>();
            var oid = GetIntArg("oid");
            if (oid > 0)
            {
                filter.Root = p => p.OrganizationId == oid;
            }
            SetKeywordFilter(filter);
            base.GetListData(filter);
        }
    }
}