#region
using System;
using System.Collections.Generic;
using System.Data;
using Agebull.Common;
using Agebull.Common.Configuration;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace Zeroteam.MessageMVC.EventBus.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    public sealed partial class EventBusDb : MySqlDataBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public EventBusDb()
        {
            Name = @"EventManagement";
            Caption = @"事件管理";
            Description = @"事件管理";

            ConnectionStringName = "EventBusDb";
        }
    }

    /// <summary>
    /// 本地数据库
    /// </summary>
    public  static partial class DataAccessProviderHelper
    {

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataAccess<TEntity> CreateDataAccess<TEntity>(this IServiceProvider serviceProvider)
            where TEntity : class, new()
        {
            var provider = new DataAccessProvider<TEntity>
            {
                ServiceProvider = serviceProvider,
                Option = GetOption<TEntity>(),
                SqlBuilder = new MySqlSqlBuilder<TEntity>(),
                Injection = serviceProvider.GetService<IOperatorInjection<TEntity>>(),
                CreateDataBase = () => serviceProvider.GetService<EventBusDb>(),
                EntityOperator = (IEntityOperator<TEntity>)GetEntityOperator<TEntity>(),
                DataOperator = (IDataOperator<TEntity>)GetDataOperator<TEntity>()
            };
            provider.DataOperator.Provider = provider;
            provider.SqlBuilder.Provider = provider;
            if (provider.Injection != null)
                provider.Injection.Provider = provider;
            provider.Option.SqlBuilder = provider.SqlBuilder;
            provider.Option.Initiate();
            return new DataAccess<TEntity>(provider);
        }

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataQuery<TEntity> CreateDataQuery<TEntity>(this IServiceProvider serviceProvider)
            where TEntity : class, new()
        {
            var provider = new DataAccessProvider<TEntity>
            {
                ServiceProvider = serviceProvider,
                Option = GetOption<TEntity>(),
                SqlBuilder = new MySqlSqlBuilder<TEntity>(),
                Injection = serviceProvider.GetService<IOperatorInjection<TEntity>>(),
                CreateDataBase = () => serviceProvider.GetService<EventBusDb>(),
                EntityOperator = (IEntityOperator<TEntity>)GetEntityOperator<TEntity>(),
                DataOperator = (IDataOperator<TEntity>)GetDataOperator<TEntity>()
            };
            provider.DataOperator.Provider = provider;
            provider.SqlBuilder.Provider = provider;
            if (provider.Injection != null)
                provider.Injection.Provider = provider;
            provider.Option.SqlBuilder = provider.SqlBuilder;
            provider.Option.Initiate();
            return new DataQuery<TEntity>(provider);
        }
    }
}