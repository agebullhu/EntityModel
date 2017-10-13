/*design by:agebull designer date:2017/5/26 19:43:33*/
using System;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.PersonnelPage
{
    public partial class Action : ApiPageBaseForAudit<PersonnelData, PersonnelDataAccess, PersonnelBusinessLogic>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Action()
        {
            AllAccess = true;
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var root = new LambdaItem<PersonnelData>();
            //if (!CanDoAction("all"))
            //{
            //    root.Expression = p => p.JoinOrgId == BusinessContext.Current.LoginUser.CompanyId;
            //}
            var key = GetArg("keyWord");
            if (!string.IsNullOrWhiteSpace(key))
            {
                root.AddAnd(p => p.FullName.Contains(key) || p.Tel.Contains(key) || p.Mobile.Contains(key));
            }
            base.GetListData(root);
        }
        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            DefaultActin(action);
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(PersonnelData data, FormConvert convert)
        {
            DefaultReadFormData(data,convert);
        }
    }
}