using System;
using System.Data.SQLite;
using Agebull.Common.Base;


namespace Agebull.Common.DataModel.Sqlite
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TSqliteDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public sealed class SqliteReaderScope<TEntity, TSqliteDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqliteDataBase : SqLiteDataBase
    {
        private readonly SqliteTable<TEntity, TSqliteDataBase> _table;
        private readonly string _fields;
        private readonly Action<SQLiteDataReader, TEntity> _loadAction;

        private SqliteReaderScope(SqliteTable<TEntity, TSqliteDataBase> table, string fields, Action<SQLiteDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table.DynamicReadFields ;
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
        public static SqliteReaderScope<TEntity, TSqliteDataBase> CreateScope(SqliteTable<TEntity, TSqliteDataBase> table, string fields, Action<SQLiteDataReader, TEntity> loadAction)
        {
            return new SqliteReaderScope<TEntity, TSqliteDataBase>(table, fields, loadAction);
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