/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/9/16 10:40:07*/
#region
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Agebull.Common;
using Agebull.Common.Configuration;
using Agebull.EntityModel.MySql;
#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    public sealed partial class EventBusDb : MySqlDataBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public EventBusDb()
        {
            Name = @"EventManagement";
            Caption = @"事件管理";
            Description = @"事件管理";

            ConnectionStringName = "EventBusDb";
        }
    }
}