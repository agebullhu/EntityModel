// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     表关联的SQL诗句
    /// </summary>
    public sealed class TableSql
    {
        /// <summary>
        ///     表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     主键
        /// </summary>
        public string PimaryKey { get; set; }

        /// <summary>
        ///     创建表的SQL
        /// </summary>
        public string CreateSql { get; set; }
    }
}