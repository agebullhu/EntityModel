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

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    ///     数据库访问范围
    /// </summary>
    public class DataBaseScope : ScopeBase
    {
        /// <summary>
        ///     当前数据库对象
        /// </summary>
        private readonly OrmDataBase _dataBase;

        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private readonly bool _isHereOpen;

        /// <summary>
        ///     构造
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
        ///     当前数据库对象
        /// </summary>
        public OrmDataBase DataBase => _dataBase;

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        /// <returns>范围</returns>
        public static DataBaseScope CreateScope(OrmDataBase dataBase)
        {
            return new DataBaseScope(dataBase);
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        public static DataBaseScope CreateScope<TSqliteDataBase>()
            where TSqliteDataBase : OrmDataBase, new()
        {
            return new DataBaseScope(new TSqliteDataBase());
        }

        /// <summary>
        ///     清理资源
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