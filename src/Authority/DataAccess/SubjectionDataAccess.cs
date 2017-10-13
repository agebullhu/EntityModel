
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gboxt.Common.DataModel.MySql;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 隶属关系表,包括职位与其它职位的隶属关系、机构之间的隶属关系、机构与职位的隶属关系
    /// </summary>
    sealed partial class SubjectionDataAccess : MySqlTable<SubjectionData>
    {
        
    }
}
