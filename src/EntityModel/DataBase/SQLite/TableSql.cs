namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    /// 表关联的SQL诗句
    /// </summary>
    public sealed class TableSql
    {
        /// <summary>
        ///  表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PimaryKey { get; set; }

        /// <summary>
        ///  创建表的SQL
        /// </summary>
        public string CreateSql { get; set; }

    }
}