using System;
using System.Data.Common;
using System.Data.SQLite;
using Agebull.Common.Base;
using Gboxt.Common.DataModel;


namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TSqliteDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public sealed class ReaderScope<TEntity, TSqliteDataBase> : ScopeBase
        where TEntity : EditDataObject, new()
        where TSqliteDataBase : OrmDataBase
    {
        private readonly DataTable<TEntity, TSqliteDataBase> _table;
        private readonly string _fields;
        private readonly Action<DbDataReader, TEntity> _loadAction;

        private ReaderScope(DataTable<TEntity, TSqliteDataBase> table, string fields, Action<DbDataReader, TEntity> loadAction)
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
        public static ReaderScope<TEntity, TSqliteDataBase> CreateScope(DataTable<TEntity, TSqliteDataBase> table, string fields, Action<DbDataReader, TEntity> loadAction)
        {
            return new ReaderScope<TEntity, TSqliteDataBase>(table, fields, loadAction);
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