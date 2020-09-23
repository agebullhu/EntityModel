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

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库访问范围
    /// </summary>
    [Obsolete]
    public class DataBaseScope : IAsyncDisposable
    {
        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private bool _isHereOpen;

        /// <summary>
        ///     当前数据库对象
        /// </summary>
        public IDataBase DataBase { get; private set; }

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
        public static async Task<DataBaseScope> CreateScopeAsync(IDataBase dataBase)
        {
            return new DataBaseScope
            {
                DataBase = dataBase,
                _isHereOpen = await dataBase.OpenAsync()
            };
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
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_isHereOpen)
            {
                await DataBase.DisposeAsync();
            }
        }
    }
}