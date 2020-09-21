// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     Sql实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TMySqlDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public partial class MySqlDataAccess<TEntity, TMySqlDataBase> : DataAccess<TEntity>
        where TEntity : EditDataObject, new()
        where TMySqlDataBase : MySqlDataBase
    {

        /// <summary>
        /// 数据库类型
        /// </summary>
        public sealed override DataBaseType DataBaseType => DataBaseType.MySql;

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        protected sealed override IDataBase CreateDataBase() => DependencyHelper.GetService<TMySqlDataBase>();

    }
}