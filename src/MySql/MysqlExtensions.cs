// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据访问提供器
    /// </summary>
    public static class MysqlExtensions
    {
        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataAccess<TEntity> CreateDataAccess<TEntity, TDataBase, TDataOperator>(this IServiceProvider serviceProvider, DataAccessOption option)
        where TEntity : class, new()
        where TDataBase : MySqlDataBase
        where TDataOperator : IDataOperator<TEntity>, new()
        {
            var provider = new DataAccessProvider<TEntity>
            {
                Option = option,
                DataOperator = new TDataOperator(),
                ServiceProvider = serviceProvider,
                CreateDataBase = () => serviceProvider.GetService<TDataBase>(),
                Injection = serviceProvider.GetService<IOperatorInjection<TEntity>>(),
                SqlBuilder = new MySqlSqlBuilder<TEntity>()
            };
            provider.SqlBuilder.Provider = provider;
            if (provider.Injection != null)
                provider.Injection.Provider = provider;
            provider.DataOperator.Provider = provider;
            option.SqlBuilder = provider.SqlBuilder;
            option.Initiate();

            return new DataAccess<TEntity>(provider);
        }
    }
}