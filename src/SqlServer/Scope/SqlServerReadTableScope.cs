// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

using System;
using System.Data.SqlClient;
using Agebull.Common.Base;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TSqlServerDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public sealed class SqlServerReadTableScope<TEntity, TSqlServerDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqlServerDataBase : SqlServerDataBase
    {
        private readonly SqlServerTable<TEntity, TSqlServerDataBase> _table;
        private readonly string _fields;
        private readonly Action<SqlDataReader, TEntity> _loadAction;

        private SqlServerReadTableScope(SqlServerTable<TEntity, TSqlServerDataBase> table, string fields, Action<SqlDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table.DynamicReadFields;
            _loadAction = table.DynamicLoadAction;
            table.DynamicReadFields = fields;
            table.DynamicLoadAction = loadAction;
        }

        /// <summary>
        /// 生成修改读取字段范围
        /// </summary>
        /// <param name="table">作用的表对象</param>
        /// <param name="fields">字段</param>
        /// <param name="loadAction">读取方法</param>
        /// <returns>读取对象范围</returns>
        public static SqlServerReadTableScope<TEntity, TSqlServerDataBase> CreateScope(SqlServerTable<TEntity, TSqlServerDataBase> table, string fields, Action<SqlDataReader, TEntity> loadAction)
        {
            return new SqlServerReadTableScope<TEntity, TSqlServerDataBase>(table, fields, loadAction);
        }

        /// <summary>
        /// 析构
        /// </summary>
        protected override void OnDispose()
        {
            _table.DynamicReadFields = _fields;
            _table.DynamicLoadAction = _loadAction;
        }
    }
}