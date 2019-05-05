using System;
using System.Data.Common;
using Agebull.Common.Base;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     修改读取字段范围
    /// </summary>
    public sealed class DbReaderScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly IDataTable<TEntity> _table;
        private readonly string _fields;
        private readonly Action<DbDataReader, TEntity> _loadAction;

        private DbReaderScope(IDataTable<TEntity> table, string fields, Action<DbDataReader, TEntity> loadAction)
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
        /// 构造
        /// </summary>
        /// <param name="table">数据访问对象</param>
        /// <param name="fields">字段</param>
        /// <param name="loadAction">载入方法</param>
        /// <returns></returns>
        public static IDisposable CreateScope(IDataTable<TEntity> table, string fields, Action<DbDataReader, TEntity> loadAction)
        {
            return new DbReaderScope<TEntity>(table, fields, loadAction);
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