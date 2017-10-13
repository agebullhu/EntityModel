/*design by:agebull designer date:2017/5/26 19:43:33*/
using System.Linq;
using Gboxt.Common.WebUI;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.RolePage
{
    public partial class Action : ApiPageBaseForDataState<RoleData, RoleDataAccess, RoleBusinessLogic>
    {
        /// <summary>
        ///     取得列表数据
        /// </summary>
        public Action()
        {
            AllAction = true;
        }
        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            DefaultGetListData();
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(RoleData data, FormConvert convert)
        {
            DefaultReadFormData(data, convert);
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "powers":
                    LoadPowers();
                    break;
                case "roles":
                    LoadAll();
                    break;
                case "savepowers":
                    SavePowers();
                    break;
                default:
                    base.DoActinEx(action);
                    break;
            }
        }
        /// <summary>
        /// 载入全部
        /// </summary>
        private void LoadAll()
        {
            var roles = Business.All();
            roles.Insert(0, new RoleData
            {
                Role = "-",
                Caption = "-"
            });
            SetCustomJsonResult(roles.Select(p => new EasyComboValues
            {
                Key = p.Id,
                Value = p.Caption
            }));
        }

        /// <summary>
        /// 载入当前角色权限
        /// </summary>
        private void LoadPowers()
        {
            var rid = GetIntArg("rid", -1);
            var id = GetIntArg("id", -1);
            if (rid > 0 && id < 0)
                SetCustomJsonResult(RolePowerBusinessLogic.LoadPowers(rid));
            else
                SetCustomJsonResult("[]");
        }

        /// <summary>
        /// 保存权限分配
        /// </summary>
        private void SavePowers()
        {
            RolePowerBusinessLogic.SaveRolePagePower(ContextDataId, GetArg("selects"));
        }

    }
}