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

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     数据库访问范围
    /// </summary>
    public class MySqlDataBaseScope : ScopeBase
    {
        /// <summary>
        ///     当前数据库对象
        /// </summary>
        private readonly MySqlDataBase _dataBase;

        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private readonly bool _isHereOpen;
        
        /// <summary>
        ///     构造
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
        ///     当前数据库对象
        /// </summary>
        public MySqlDataBase DataBase => _dataBase;

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        /// <returns>范围</returns>
        public static MySqlDataBaseScope CreateScope(MySqlDataBase dataBase)
        {
            return new MySqlDataBaseScope(dataBase);
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        public static MySqlDataBaseScope CreateScope<TMySqlDataBase>()
            where TMySqlDataBase : MySqlDataBase, new()
        {
            return new MySqlDataBaseScope(new TMySqlDataBase());
        }

        /// <summary>
        ///     清理资源
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