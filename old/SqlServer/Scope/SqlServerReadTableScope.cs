// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.Common.Base;

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     修改读取对象范围
    /// </summary>
    /// <typeparam name="TEntity">实体对象</typeparam>
    public sealed class SqlServerReadTableScope<TEntity> : ScopeBase
        where TEntity : EditDataObject, new()
    {
        private readonly SqlServerTable<TEntity> _table;
        private readonly string _oldName;
        private SqlServerReadTableScope(SqlServerTable<TEntity> table, string name)
        {
            _table = table;
            _oldName = table.SetDynamicReadTable(name);
        }

        /// <summary>
        /// 生成读取对象范围
        /// </summary>
        /// <param name="table">作用的表对象</param>
        /// <param name="name">表名</param>
        /// <returns>读取对象范围</returns>
        public static SqlServerReadTableScope<TEntity> CreateScope(SqlServerTable<TEntity> table, string name)
        {
            return new SqlServerReadTableScope<TEntity>(table, name);
        }

        /// <summary>
        /// 析构
        /// </summary>
        protected override void OnDispose()
        {
            _table.SetDynamicReadTable(_oldName);
        }
    }
}