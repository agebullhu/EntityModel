/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/2 17:24:52*/
using System;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.RolePowerPage
{
    public partial class Action : ApiPageBaseEx<RolePowerData, RolePowerDataAccess, RolePowerBusinessLogic>
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
            DefaultGetListData();
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
        protected override void ReadFormData(RolePowerData data, FormConvert convert)
        {
            DefaultReadFormData(data,convert);
        }
    }
}