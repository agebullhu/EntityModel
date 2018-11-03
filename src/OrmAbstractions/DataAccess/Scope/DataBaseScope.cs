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

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    public class DataBaseScope : ScopeBase
    {
        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        private readonly OrmDataBase _dataBase;

        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private readonly bool _isHereOpen;

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        public DataBaseScope(OrmDataBase dataBase)
        {
            //Trace.WriteLine("Create SqliteDataBaseScope", "OrmDataBase");
            _dataBase = dataBase;
            OrmDataBase.DataBase = dataBase;
            if (dataBase.Open())
            {
                _isHereOpen = true;
            }
            dataBase.QuoteCount += 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public OrmDataBase DataBase => _dataBase;

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static DataBaseScope CreateScope(OrmDataBase dataBase)
        {
            return new DataBaseScope(dataBase);
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static DataBaseScope CreateScope<TSqliteDataBase>()
            where TSqliteDataBase : OrmDataBase, new()
        {
            return new DataBaseScope(new TSqliteDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            //Trace.WriteLine("Dispose SqliteDataBaseScope", "OrmDataBase");
            _dataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                _dataBase.Close();
            }
        }
    }
}