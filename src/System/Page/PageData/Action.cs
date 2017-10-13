
using System;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.SqlServer;
using Gboxt.Common.WebUI;

using Gboxt.Common.SystemModel;
using Gboxt.Common.SystemModel.BusinessLogic;
using Gboxt.Common.SystemModel.DataAccess;

namespace Gboxt.Common.SystemModel.PageDataPage
{
    public class Action : ApiPageBaseEx<PageDataData, PageDataDataAccess, PageDataBusinessLogic>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Action()
        {
            AllAccess = true;
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(PageDataData data, FormConvert convert)
        {
            //未知
            data.UserId = convert.ToInteger("UserId");
            data.PageId = convert.ToInteger("PageId");
            data.PageData = convert.ToString("PageData",false);
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
                    base.DoActinEx(action);
                    break;
            }
        }
    }
}