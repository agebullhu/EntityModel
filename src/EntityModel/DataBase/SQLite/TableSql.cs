namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    /// �������SQLʫ��
    /// </summary>
    public sealed class TableSql
    {
        /// <summary>
        ///  ����
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string PimaryKey { get; set; }

        /// <summary>
        ///  �������SQL
        /// </summary>
        public string CreateSql { get; set; }

    }
}