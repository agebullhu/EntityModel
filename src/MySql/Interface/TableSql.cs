// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     �������SQLʫ��
    /// </summary>
    public sealed class TableSql
    {
        /// <summary>
        ///     ����
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        public string PimaryKey { get; set; }

        /// <summary>
        ///     �������SQL
        /// </summary>
        public string CreateSql { get; set; }
    }
}