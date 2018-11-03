// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:CF_WeiYue
// // ����:2016-07-22
// // �޸�:2016-07-27
// // *****************************************************/

#region ����

using Agebull.Common.Base;

#endregion

namespace Agebull.Common.DataModel.Sqlite
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    public class SqliteDataBaseScope : ScopeBase
    {
        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        private readonly SqLiteDataBase _dataBase;

        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private readonly bool _isHereOpen;

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        public SqliteDataBaseScope(SqLiteDataBase dataBase)
        {
            //Trace.WriteLine("Create SqliteDataBaseScope", "SqliteDataBase");
            _dataBase = dataBase;
            SqLiteDataBase.DataBase = dataBase;
            if (dataBase.Open())
            {
                _isHereOpen = true;
            }
            dataBase.QuoteCount += 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public SqLiteDataBase DataBase => _dataBase;

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static SqliteDataBaseScope CreateScope(SqLiteDataBase dataBase)
        {
            return new SqliteDataBaseScope(dataBase);
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static SqliteDataBaseScope CreateScope<TSqliteDataBase>()
            where TSqliteDataBase : SqLiteDataBase, new()
        {
            return new SqliteDataBaseScope(new TSqliteDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            //Trace.WriteLine("Dispose SqliteDataBaseScope", "SqliteDataBase");
            _dataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                _dataBase.Close();
            }
        }
    }
}