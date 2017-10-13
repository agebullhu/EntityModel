
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel.MySql;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 角色权限
    /// </summary>
    sealed partial class RolePowerDataAccess : MySqlTable<RolePowerData>
    {
    }
}
