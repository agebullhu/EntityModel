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
    public class MysqlDataAccessProvider<TEntity,TDataBase> : DataAccessProvider<TEntity>
        where TEntity : class, new()
        where TDataBase : MySqlDataBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public MysqlDataAccessProvider()
        {
            CreateDataBase = () => ServiceProvider.GetService<TDataBase>();
            DataOperator = new DataOperator<TEntity>
            {
                Provider = this
            };
            SqlBuilder = new MySqlSqlBuilder<TEntity>
            {
                Provider = this
            };
        }

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataAccess<TEntity> CreateDataAccess(IServiceProvider serviceProvider,
            DataAccessOption<TEntity> option,
            IDataOperator<TEntity> dataOperator)
        {
            var provider = new MysqlDataAccessProvider<TEntity, TDataBase>
            {
                Option = option,
                DataOperator= dataOperator,
                ServiceProvider = serviceProvider
            };
            option.Provider = provider;
            option.Initiate();
            dataOperator.Provider = provider;
            return new DataAccess<TEntity>(provider);
        }
    }
}