using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    ///  简单的依赖注入器(框架内部使用,请不要调用)
    /// </summary>
    public class IocHelper
    {
        /// <summary>
        /// 依赖注入代理
        /// </summary>
        public IServiceProvider ServiceProvider => _provider;

        private static IServiceProvider _provider;

        /// <summary>
        /// 显示式设置配置对象(依赖)
        /// </summary>
        /// <param name="service"></param>
        public static void SetServiceCollection(IServiceCollection service)
        {
            if (_serviceCollection != null)
            {
                foreach (var dod in _serviceCollection)
                    service.Add(dod);
            }
            _serviceCollection = service;
        }

        private static IServiceCollection _serviceCollection;

        /// <summary>
        /// 全局依赖
        /// </summary>
        public static IServiceCollection ServiceCollection => _serviceCollection ?? (_serviceCollection = new ServiceCollection());
        /// <summary>
        /// 内部范围对象,记录TaskID是为了正常析构
        /// </summary>
        class ScopeData
        {
            public IServiceScope Scope;
            public int? TaskId;
        }

        private static readonly AsyncLocal<ScopeData> local = new AsyncLocal<ScopeData>();

        /// <summary>
        /// 生成接口实例
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ScopeProvider
        {
            get
            {
                if (local.Value != null)
                    return local.Value.Scope.ServiceProvider;
                if (_provider == null)
                {
                    _provider = ServiceCollection.BuildServiceProvider();
                }
                local.Value = new ScopeData
                {
                    Scope = _provider.GetService<IServiceScopeFactory>().CreateScope(),
                    TaskId = Task.CurrentId
                };
                return local.Value.Scope.ServiceProvider;
            }
        }
        /// <summary>
        /// 析构此依赖范围(主动释放,否则要等到GC回收,资源使用率不可控)
        /// </summary>
        public static void DisposeScope()
        {
            if (local.Value != null && local.Value.TaskId == Task.CurrentId)
                local.Value.Scope.Dispose();
        }
        /// <summary>
        /// 生成接口实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            return ScopeProvider.GetService<TInterface>();
        }

        /// <summary>
        /// 生成接口实例(如果没有注册则使用默认实现)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TDefault"></typeparam>
        /// <returns></returns>
        public static TInterface CreateBut<TInterface, TDefault>()
            where TInterface : class
            where TDefault : class, TInterface, new()
        {
            return ScopeProvider.GetService<TInterface>() ?? new TDefault();
        }
    }
}