/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/25 14:10:19*/
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
    partial class Action
    {
        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void DefaultGetListData()
        {
            var filter = new LambdaItem<PersonnelData>();
            SetKeywordFilger(filter);
            base.GetListData(filter);
        }

        /// <summary>
        ///     关键字查询缺省实现
        /// </summary>
        /// <param name="filter">筛选器</param>
        protected void DefaultGetListData(LambdaItem<PersonnelData> filter)
        {
            SetKeywordFilger(filter);
        }

        /// <summary>
        ///     关键字查询缺省实现
        /// </summary>
        /// <param name="filter">筛选器</param>
        public static void SetKeywordFilger(LambdaItem<PersonnelData> filter)
        {
            var keyWord = GetArg("keyWord");
            if (!string.IsNullOrEmpty(keyWord))
            {
                filter.AddAnd(p => p.FullName.Contains(keyWord) || p.Tel.Contains(keyWord) || p.Mobile.Contains(keyWord) || p.Memo.Contains(keyWord) || p.Role.Contains(keyWord));
            }
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected void DefaultReadFormData(PersonnelData data, FormConvert convert)
        {
            //数据
            data.FullName = convert.ToString("FullName");
            data.Six = convert.ToBoolean("Six");
            data.Birthday = convert.ToDateTime("Birthday");
            data.Tel = convert.ToString("Tel");
            data.Mobile = convert.ToString("Mobile");
            //备注
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