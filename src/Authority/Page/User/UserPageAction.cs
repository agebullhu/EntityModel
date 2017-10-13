/*design by:agebull designer date:2017/5/26 19:43:33*/

using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel;

namespace Agebull.SystemAuthority.Organizations.UserPage
{
    public partial class UserPageAction : ApiPageBaseForDataState<UserData, UserDataAccess, UserBusinessLogic>
    {
        protected UserPageAction()
        {
            AllAction=true;
            RegisteDenyAction("addnew","delete", "reset", "lock", "discard");
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var root = new LambdaItem<UserData>();
            var name = GetArg("name");
            if (!string.IsNullOrEmpty(name))
            {
                root.AddAnd(p => p.UserName.Contains(name) || p.RealName.Contains(name) || p.Memo.Contains(name));
            }
            base.GetListData(root);
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "reset_pwd":
                    ResetPassword();
                    break;
                default:
                    base.DoActinEx(action);
                    break;
            }
        }

        void ResetPassword()
        {
            Business.ResetPassword(GetIntArrayArg("selects"));
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(UserData data, FormConvert convert)
        {
            //数据
            //data.RealName = convert.ToString("RealName");
            //data.UserName = convert.ToString("UserName");
            //data.PassWord = convert.ToString("PassWord");
            data.RoleId = convert.ToInteger("RoleId");
            data.Memo = convert.ToString("Memo");
        }
    }
}