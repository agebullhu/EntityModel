using System;
using System.Threading.Tasks;
using Agebull.Common.Base;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库访问范围
    /// </summary>
    public class DataTableScope : IAsyncDisposable, IDisposable
    {
        /// <summary>
        ///     是否此处打开数据库
        /// </summary>
        private bool _isHereOpen;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="table"></param>
        protected DataTableScope(IDataTable table)
        {
            Table = table;
        }

        /// <summary>
        ///     当前数据库对象
        /// </summary>
        public IDataTable Table { get; }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <returns>范围</returns>
        private async Task<DataTableScope> CreateScope()
        {
            _isHereOpen = Table.OriDataBase == null;
            if (_isHereOpen)
            {
                await Table.DataBase.OpenAsync();
            }
            Table.DataBase.QuoteCount += 1;
            return this;
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="table">数据库对象</param>
        /// <returns>范围</returns>
        public static DataTableScope CreateScope(IDataTable table)
        {
            var scope = new DataTableScope(table)
            {
                _isHereOpen = table.OriDataBase == null
            };
            if (scope._isHereOpen)
            {
                //table.DataBase.Open();
            }
            table.DataBase.QuoteCount += 1;
            return scope;
        }

        /// <summary>
        ///     生成一个范围
        /// </summary>
        /// <param name="table">数据库对象</param>
        /// <returns>范围</returns>
        public static Task<DataTableScope> CreateScopeAsync(IDataTable table)
        {
            var scope = new DataTableScope(table);
            return scope.CreateScope();
        }

        /// <summary>
        ///     清理资源
        /// </summary>

        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask(Task.CompletedTask);
        }

        /// <summary>
        ///     清理资源
        /// </summary>
        public void Dispose()
        {
            Table.DataBase.QuoteCount -= 1;
            //if (_isHereOpen)
            //{
            //    Table.DataBase.Free();
            //}
        }
    }
}