using System;
using Agebull.Common.Base;
using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TMySqlDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public sealed class MySqlReaderScope<TEntity, TMySqlDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TMySqlDataBase : MySqlDataBase
    {
        private readonly MySqlTable<TEntity, TMySqlDataBase> _table;
        private readonly string _fields;
        private readonly Action<MySqlDataReader, TEntity> _loadAction;

        private MySqlReaderScope(MySqlTable<TEntity, TMySqlDataBase> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
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
        public static MySqlReaderScope<TEntity, TMySqlDataBase> CreateScope(MySqlTable<TEntity, TMySqlDataBase> table, string fields, Action<MySqlDataReader, TEntity> loadAction)
        {
            return new MySqlReaderScope<TEntity, TMySqlDataBase>(table, fields, loadAction);
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