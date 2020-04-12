// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:CF_WeiYue
// // 建立:2016-07-22
// // 修改:2016-07-27
// // *****************************************************/

#region 引用

using System;
using System.Threading.Tasks;
using Agebull.Common.Base;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库访问范围
    /// </summary>
    [Obsolete]
    public class DataBaseScope : ScopeBase
    {
        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private bool _isHereOpen;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dataBase"></param>
        protected DataBaseScope(IDataBase dataBase)
        {
            DataBase = dataBase;
            _isHereOpen = DataBase.Open();
        }

        /// <summary>
        ///     当前数据库对象
        /// </summary>
        public IDataBase DataBase { get; }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        private async Task<DataBaseScope> CreateScope()
        {
            _isHereOpen = await DataBase.OpenAsync();
            return this;
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        /// <returns>范围</returns>
        public static DataBaseScope CreateScope(IDataBase dataBase)
        {
            return new DataBaseScope(dataBase);
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="dataBase">数据库对象</param>
        /// <returns>范围</returns>
        public static Task<DataBaseScope> CreateScopeAsync(IDataBase dataBase)
        {
            var scope = new DataBaseScope(dataBase);
            return scope.CreateScope();
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        public static DataBaseScope CreateScope<TDataBase>() where TDataBase : IDataBase, new()
        {
            return new DataBaseScope(new TDataBase());
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        public static async Task<DataBaseScope> CreateScopeAsync<TDataBase>() where TDataBase : IDataBase, new()
        {
            return await CreateScopeAsync(new TDataBase());
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        protected override void OnDispose()
        {
            if (_isHereOpen)
            {
                DataBase.Dispose();
            }
        }
    }
}