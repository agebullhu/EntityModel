/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2017/6/4 14:55:10*/
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

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    public partial class Authorities
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Authorities()
        {
            /*tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase)
            {Authorities
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