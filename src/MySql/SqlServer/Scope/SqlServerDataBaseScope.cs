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

namespace Gboxt.Common.DataModel.SqlServer
{
    /*// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    /// <typeparam name="TDataBase">���ݿ����</typeparam>
    public sealed class SqlServerDataBaseScope<TDataBase> : ScopeBase
        where TDataBase : SqlServerDataBase, new()
    {
        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        private readonly TDataBase _dataBase;
        
        /// <summary>
        ///     ����
        /// </summary>
        private SqlServerDataBaseScope()
        {
            _dataBase = new TDataBase();
            _dataBase.Open();
            _dataBase.QuoteCount = 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public TDataBase DataBase
        {
            get { return _dataBase; }
        }

        /// <summary>
        ///     ���ɷ�Χ
        /// </summary>
        /// <returns></returns>
        public static SqlServerDataBaseScope<TDataBase> CreateScope()
        {
            return new SqlServerDataBaseScope<TDataBase>();
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            this._dataBase.QuoteCount = 0;
            this._dataBase.Close();
        }
    }*/

    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    public class SqlServerDataBaseScope : ScopeBase
    {
        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        private readonly SqlServerDataBase _dataBase;

        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private readonly bool _isHereOpen;
        
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        protected SqlServerDataBaseScope(SqlServerDataBase dataBase)
        {
            //Trace.WriteLine("Create SqlServerDataBaseScope", "SqlServerDataBase");
            _dataBase = dataBase;

            if (dataBase.Open())
            {
                _isHereOpen = true;
            }
            dataBase.QuoteCount += 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public SqlServerDataBase DataBase
        {
            get { return _dataBase; }
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static SqlServerDataBaseScope CreateScope(SqlServerDataBase dataBase)
        {
            return new SqlServerDataBaseScope(dataBase);
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static SqlServerDataBaseScope CreateScope<TSqlServerDataBase>()
            where TSqlServerDataBase : SqlServerDataBase, new()
        {
            return new SqlServerDataBaseScope(new TSqlServerDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            //Trace.WriteLine("Dispose SqlServerDataBaseScope", "SqlServerDataBase");
            _dataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                _dataBase.Close();
            }
        }
    }
}