
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
    /// 预约规则,设置每个月的预约时间规则
    /// </summary>
    public sealed partial class ReservationRegularBusinessLogic : BusinessLogicBase<ReservationRegularData,ReservationRegularDataAccess>
    {
        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(ReservationRegularData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(ReservationRegularData data, bool isAdd)
        {
             return true;
        }
    }
}
