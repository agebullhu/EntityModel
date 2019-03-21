using System;
using Agebull.Common.Base;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    public sealed class MySqlReaderScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly MySqlTable<TEntity> _table;
        private readonly string _fields;
        private readonly Action<MySqlDataReader, TEntity> _loadAction;

        private MySqlReaderScope(MySqlTable<TEntity> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            if (string.IsNullOrWhiteSpace(fields))
                fields = null;
            _table = table;
            _fields = table._contextReadFields;
            _loadAction = table.ContentLoadAction;
            table._contextReadFields = fields;
            table.ContentLoadAction = loadAction;
        }

        /// <summary>
        /// 生成修改读取字段范围
        /// </summary>
        /// <param name="table">作用的表对象</param>
        /// <param name="fields">字段</param>
        /// <param name="loadAction">读取方法</param>
        /// <returns>读取对象范围</returns>
        public static MySqlReaderScope<TEntity> CreateScope(MySqlTable<TEntity> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            return new MySqlReaderScope<TEntity>(table, fields, loadAction);
        }

        /// <summary>
        /// 析构
        /// </summary>
        protected override void OnDispose()
        {
            _table._contextReadFields = _fields;
            _table.ContentLoadAction = _loadAction;
        }
    }
}