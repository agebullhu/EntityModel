/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 1:01:40*/
#region
using System;
using System.Collections.Generic;
using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            //Name = @"EventManagement";
            //Caption = @"事件管理";
            //Description = @"事件管理";
            ConnectionString = ConfigurationHelper.GetConnectionString("EventBusDb");
            //Logger = logger;
        }

        /// <summary>
        /// 生成数据库对象
        /// </summary>
        /// <returns></returns>
        internal static EventBusDb GetDb() => DependencyHelper.GetService<EventBusDb>();

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <param name="isDynamic">是否支持动态功能</param>
        /// <returns></returns>
        public static DataAccess<EventSubscribeEntity> CreateEventSubscribeEntityAccess(bool isDynamic = true)
        {
            if (!isDynamic)
            {
                if (DataAccessProviderHelper.providers.TryGetValue(typeof(EventSubscribeEntity), out var pro))
                    return new DataAccess<EventSubscribeEntity>((DataAccessProvider<EventSubscribeEntity>)pro);
            }
            var opt = EventSubscribeEntityDataOperator.GetOption();
            var ope = new EventSubscribeEntityDataOperator();
            var provider = new DataAccessProvider<EventSubscribeEntity>
            {
                Option = opt,
                ServiceProvider = DependencyHelper.ServiceProvider,
                CreateDataBase = DataAccessProviderHelper.CreateDataBase,
                SqlBuilder = new MySqlSqlBuilder<EventSubscribeEntity> { Option = opt },
                EntityOperator = ope,
                DataOperator = ope
            };
            provider.SqlBuilder.Provider = provider;
            if (!isDynamic)
            {
                DataAccessProviderHelper.providers[typeof(EventSubscribeEntity)] = provider;
            }
            return new DataAccess<EventSubscribeEntity>(provider);
        }

    }

    /// <summary>
    /// 本地数据库
    /// </summary>
    public static partial class DataAccessProviderHelper
    {
        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        internal static readonly Func<IDataBase> CreateDataBase = EventBusDb.GetDb;

        /// <summary>
        /// 提供器字典
        /// </summary>
        internal static readonly Dictionary<Type, object> providers = new Dictionary<Type, object>();

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        static DataAccessProvider<TEntity> GetProvider<TEntity>(IServiceProvider serviceProvider, bool isDynamic)
              where TEntity : class, new()
        {
            var type = typeof(TEntity);
            if (!isDynamic && providers.TryGetValue(type, out var pro))
                return (DataAccessProvider<TEntity>)pro;
            var name = type.Name;
            var option = GetOption(name);
#if DEBUG
            if (option == null)
                throw new NotSupportedException($"{typeof(TEntity).FullName}没有对应配置项，请通过设计器生成");
#endif
            var opt = GetOperator(name);
            var provider = new DataAccessProvider<TEntity>
            {
                Option = option,
                ServiceProvider = serviceProvider,
                CreateDataBase = CreateDataBase,
                SqlBuilder = new MySqlSqlBuilder<TEntity>(),
                EntityOperator = (IEntityOperator<TEntity>)opt,
                DataOperator = (IDataOperator<TEntity>)opt
            };
            provider.DataOperator.Provider = provider;
            provider.SqlBuilder.Provider = provider;
            if (!isDynamic)
                providers.TryAdd(type, provider);
            return provider;
        }

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataAccess<TEntity> CreateDataAccess<TEntity>(this IServiceProvider serviceProvider, bool isDynamic = true)
            where TEntity : class, new()
        {
            //var type = typeof(TEntity);
            //if (DataAccess.TryGetValue(type, out var access))
            //    return (DataAccess<TEntity>)access;
            var provider = GetProvider<TEntity>(serviceProvider, isDynamic);
#if DEBUG
            if (provider.Option.IsQuery)
                throw new NotSupportedException($"{typeof(TEntity).FullName}是一个查询，请使用CreateDataQuery方法");
#endif
            return new DataAccess<TEntity>(provider);
        }

        /// <summary>
        /// 构造数据访问对象
        /// </summary>
        /// <returns></returns>
        public static DataQuery<TEntity> CreateDataQuery<TEntity>(this IServiceProvider serviceProvider, bool isDynamic = true)
            where TEntity : class, new()
        {
            return new DataQuery<TEntity>(GetProvider<TEntity>(serviceProvider, isDynamic));
        }
    }
}