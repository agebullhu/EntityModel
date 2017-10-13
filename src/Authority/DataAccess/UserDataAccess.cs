
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.SystemModel;
using Gboxt.Common.SystemModel.DataAccess;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 系统用户
    /// </summary>
    sealed partial class UserDataAccess : DataStateTable<UserData>
    {

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="entity"></param>
        protected sealed override void OnDataSaved(DataOperatorType operatorType,UserData entity)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                proxy.SetEntity(entity);
            }
        }
    }
}
