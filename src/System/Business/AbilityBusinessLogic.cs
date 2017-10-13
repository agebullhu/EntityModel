
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.SqlServer;
using YhxBank.WeiYue.DataAccess;

namespace YhxBank.WeiYue.BusinessLogic
{
    /// <summary>
    /// 即用户、职位或是职责对应的系统权限能力（技能）范围
    /// </summary>
    public sealed partial class AbilityBusinessLogic : BusinessLogicBase<AbilityData,AbilityDataAccess>
    {
        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(AbilityData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(AbilityData data, bool isAdd)
        {
             return true;
        }
    }
}
