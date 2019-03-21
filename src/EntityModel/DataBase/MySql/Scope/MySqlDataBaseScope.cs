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

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    public class MySqlDataBaseScope : ScopeBase
    {
        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        private readonly MySqlDataBase _dataBase;

        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private readonly bool _isHereOpen;
        
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        protected MySqlDataBaseScope(MySqlDataBase dataBase)
        {
            //Trace.WriteLine("Create MySqlDataBaseScope", "MySqlDataBase");
            _dataBase = dataBase;
            MySqlDataBase.DefaultDataBase = dataBase;
            if (dataBase.Open())
            {
                _isHereOpen = true;
            }
            dataBase.QuoteCount += 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public MySqlDataBase DataBase => _dataBase;

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static MySqlDataBaseScope CreateScope(MySqlDataBase dataBase)
        {
            return new MySqlDataBaseScope(dataBase);
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static MySqlDataBaseScope CreateScope<TMySqlDataBase>()
            where TMySqlDataBase : MySqlDataBase, new()
        {
            return new MySqlDataBaseScope(new TMySqlDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            //Trace.WriteLine("Dispose MySqlDataBaseScope", "MySqlDataBase");
            _dataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                _dataBase.Close();
            }
        }
    }
}