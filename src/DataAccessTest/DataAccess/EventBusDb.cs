/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/3 3:43:45*/
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
    public static partial class DataAccessProviderHelper
    {
        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataAccess<TEntity> CreateDataAccess<TEntity>(this IServiceProvider serviceProvider)
            where TEntity : class, new()
        {
            var option = GetOption<TEntity>();
            if (option == null)
                throw new NotSupportedException($"{typeof(TEntity).FullName}没有对应配置项，请通过设计器生成");
            if (option.IsQuery)
                throw new NotSupportedException($"{typeof(TEntity).FullName}是一个查询，请使用CreateDataQuery方法");
            var provider = new DataAccessProvider<TEntity>
            {
                ServiceProvider = serviceProvider,
                Option = option,
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