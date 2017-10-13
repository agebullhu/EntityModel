/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/9 19:38:57*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.Workflow.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    public partial class WorkflowDataBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public WorkflowDataBase()
        {
            /*tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase)
            {WorkflowDataBase
            };*/
            Initialize();
            //RegistToEntityPool();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();
    }
}