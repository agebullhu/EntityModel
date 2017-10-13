
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
using Gboxt.Common.SystemModel.DataAccess;

namespace Gboxt.Common.SystemModel.BusinessLogic
{
    /// <summary>
    /// 基金信息
    /// </summary>
    public sealed partial class FundBusinessLogic : BusinessLogicBase<FundData,FundDataAccess>
    {
        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(FundData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaved(FundData data, bool isAdd)
        {
             return true;
        }

        /// <summary>
        ///     启动募集
        /// </summary>
        /// <remark>
        ///     将产品状态改为募集中
        /// </remark>
        public bool DoStart(int[] ids)
        {
            return true;
        }

        /// <summary>
        ///     暂停募集
        /// </summary>
        /// <remark>
        ///     将当前产品状态设置为暂停（前提是产品当前状态为启动募集）
        /// </remark>
        public bool DoPause(int[] ids)
        {
            return true;
        }

        /// <summary>
        ///     额度分配
        /// </summary>
        /// <remark>
        ///     打开额度分配界面
        /// </remark>
        public bool DoAllocate(int[] ids)
        {
            return true;
        }

        /// <summary>
        ///     预约审核
        /// </summary>
        /// <remark>
        ///     打开预约审核页面
        /// </remark>
        public bool DoAuditReservation(int[] ids)
        {
            return true;
        }

        /// <summary>
        ///     新增预约
        /// </summary>
        /// <remark>
        ///     新增一条预约
        /// </remark>
        public bool DoNewReservation(int[] ids)
        {
            return true;
        }
    }
}
