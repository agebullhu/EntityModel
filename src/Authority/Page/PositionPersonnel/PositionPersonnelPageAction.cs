/*design by:agebull designer date:2017/5/26 19:43:33*/

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.Workflow;
using MySql.Data.MySqlClient;

namespace Agebull.SystemAuthority.Organizations.PositionPersonnelPage
{
    public partial class Action : AutoJobPageBase<PositionPersonnelData, PositionPersonnelDataAccess, PositionPersonnelBusinessLogic>
    {
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
                default:
                    base.DoActinEx(action);
                    break;
            }
        }
        /// <summary>
        /// 职位机构树
        /// </summary>
        private void LoadOrgTree()
        {
            SetCustomJsonResult(OrganizationBusinessLogic.LoadPostTreeForUi(BusinessContext.Current.LoginUser.DepartmentId));
        }

        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public override PositionPersonnelData CreateData()
        {
            return new PositionPersonnelData
            {
                Six = true,
                DepartmentId = GetIntArg("oid", 0),
                OrganizePositionId = GetIntArg("pid", 0)
            };
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        /// <remarks>安全检查有漏洞</remarks>
        protected override void GetListData()
        {
            var pid = GetIntArg("pid", 0);
            var oid = GetIntArg("oid", 0);
            var kw = GetArg("keyWord");
            var condition = new LambdaItem<PositionPersonnelData>();
            if (!string.IsNullOrWhiteSpace(kw))
            {
                condition.Root = p => p.Personnel.Contains(kw) || p.Mobile.Contains(kw) || p.Tel.Contains(kw);
            }
            if (pid > 0)
            {
                condition.Root = p => p.OrganizePositionId == pid;
                base.GetListData(condition);
            }
            else if(oid > 1)
            {
                using (MySqlReadTableScope<PositionPersonnelData>.CreateScope(Business.Access, "view_sys_position_personnel_master"))
                {
                    condition.Root = p => p.master_id == oid;
                    base.GetListData(condition);
                }
            }
            else if(BusinessContext.Current.LoginUser.DepartmentId == 1)
            {
                base.GetListData(condition);
            }
            else
            {
                using (MySqlReadTableScope<PositionPersonnelData>.CreateScope(Business.Access, "view_sys_position_personnel_master"))
                {
                    condition.Root = p => p.master_id == BusinessContext.Current.LoginUser.DepartmentId;
                    base.GetListData(condition);
                }
            }
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(PositionPersonnelData data, FormConvert convert)
        {
            //数据
            data.Appellation = convert.ToString("Appellation");
            data.Personnel = convert.ToString("Personnel", false);
            data.Six = convert.ToBoolean("Six");
            data.Birthday = convert.ToDateTime("Birthday");
            data.Tel = convert.ToString("Tel", false);
            data.Mobile = convert.ToString("Mobile", false);

            data.RoleId = convert.ToInteger("RoleId", 0);
            data.OrganizePositionId = convert.ToInteger("OrganizePositionId", 0);
            //备注
            data.Memo = convert.ToString("Memo", true);
        }
    }
}