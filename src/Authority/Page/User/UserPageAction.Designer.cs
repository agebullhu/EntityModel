/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/25 14:46:06*/
using System;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

using Agebull.SystemAuthority.Organizations;
using Agebull.SystemAuthority.Organizations.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.UserPage
{
    partial class UserPageAction
    {
        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void DefaultGetListData()
        {
            var filter = new LambdaItem<UserData>();
            SetKeywordFilter(filter);
            base.GetListData(filter);
        }

        /// <summary>
        ///     关键字查询缺省实现
        /// </summary>
        /// <param name="filter">筛选器</param>
        public void SetKeywordFilter(LambdaItem<UserData> filter)
        {
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                filter.AddAnd(p => p.RealName.Contains(keyWord) || p.UserName.Contains(keyWord) || p.PassWord.Contains(keyWord) || p.Role.Contains(keyWord) || p.Memo.Contains(keyWord));
            }
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected void DefaultReadFormData(UserData data, FormConvert convert)
        {
            //数据
            data.RealName = convert.ToString("RealName");
            data.UserName = convert.ToString("UserName");
            data.PassWord = convert.ToString("PassWord");
            data.RoleId = convert.ToInteger("RoleId");
            data.Memo = convert.ToString("Memo");
        }
        #region 设计器命令


        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        void DefaultActin(string action)
        { 
            switch (action)
            {
                default:
                    base.DoActinEx(action);
                    break;
            }
        }
        #endregion
    }
}