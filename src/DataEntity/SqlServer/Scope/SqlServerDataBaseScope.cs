// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:CF_WeiYue
// // 建立:2016-07-22
// // 修改:2016-07-27
// // *****************************************************/

#region 引用

using Agebull.Common.Base;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /*// <summary>
    ///     数据库访问范围
    /// </summary>
    /// <typeparam name="TDataBase">数据库对象</typeparam>
    public sealed class SqlServerDataBaseScope<TDataBase> : ScopeBase
        where TDataBase : SqlServerDataBase, new()
    {
        /// <summary>
        ///     当前数据库对象
        /// </summary>
        private readonly TDataBase _dataBase;
        
        /// <summary>
        ///     构造
        /// </summary>
        private SqlServerDataBaseScope()
        {
            _dataBase = new TDataBase();
            _dataBase.Open();
            _dataBase.QuoteCount = 1;
        }

        /// <summary>
        ///     当前数据库对象
        /// </summary>
        public TDataBase DataBase
        {
            get { return _dataBase; }
        }

        /// <summary>
        ///     生成范围
        /// </summary>
        /// <returns></returns>
        public static SqlServerDataBaseScope<TDataBase> CreateScope()
        {
            return new SqlServerDataBaseScope<TDataBase>();
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        protected override void OnDispose()
        {
            this._dataBase.QuoteCount = 0;
            this._dataBase.Close();
        }
    }*/

    /// <summary>
    ///     数据库访问范围
    /// </summary>
    public class SqlServerDataBaseScope : ScopeBase
    {
        /// <summary>
        ///     当前数据库对象
        /// </summary>
        private readonly SqlServerDataBase _dataBase;

        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private readonly bool _isHereOpen;
        
        /// <summary>
        ///     构造
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
        ///     当前数据库对象
        /// </summary>
        public SqlServerDataBase DataBase
        {
            get { return _dataBase; }
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        /// <returns>范围</returns>
        public static SqlServerDataBaseScope CreateScope(SqlServerDataBase dataBase)
        {
            return new SqlServerDataBaseScope(dataBase);
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        public static SqlServerDataBaseScope CreateScope<TSqlServerDataBase>()
            where TSqlServerDataBase : SqlServerDataBase, new()
        {
            return new SqlServerDataBaseScope(new TSqlServerDataBase());
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        protected override void OnDispose()
        {
            //Trace.WriteLine("Dispose SqlServerDataBaseScope", "SqlServerDataBase");
            this._dataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                this._dataBase.Close();
            }
        }
    }
}