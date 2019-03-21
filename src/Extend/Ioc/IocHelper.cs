using System;
using System.Threading;
using Agebull.Common.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    /// IOC��Χ����,�ڲ����ʹ��
    /// </summary>
    public class IocScope : ScopeBase
    {
        /// <summary>
        /// ����һ����Χ
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            IocHelper.CreateScope();
            return new IocScope();
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            IocHelper.DisposeScope();
        }
    }
    /// <summary>
    ///     �򵥵�����ע����(����ڲ�ʹ��,�벻Ҫ����)
    /// </summary>
    public static class IocHelper
    {
        internal class ScopeData
        {
            public IServiceProvider Provider;
            public IServiceScope Scope;
        }
        /// <summary>
        /// �ʵ��
        /// </summary>
        internal static readonly AsyncLocal<ScopeData> Local = new AsyncLocal<ScopeData>();

        #region ServiceCollection

        private static IServiceCollection _serviceCollection;

        private static IServiceProvider _rootProvider;
        /// <summary>
        ///     ����ע�����
        /// </summary>
        public static IServiceProvider RootProvider => _rootProvider ?? (_rootProvider = ServiceCollection.BuildServiceProvider());

        /// <summary>
        ///     ����ע�����
        /// </summary>
        public static IServiceProvider ServiceProvider => Local.Value?.Provider ?? RootProvider;


        /// <summary>
        ///     ȫ������
        /// </summary>
        public static IServiceCollection ServiceCollection => _serviceCollection ?? (_serviceCollection = new ServiceCollection());

        /// <summary>
        ///     ��ʾʽ�������ö���(����)
        /// </summary>
        /// <param name="service"></param>
        public static void SetServiceCollection(IServiceCollection service)
        {
            if (_serviceCollection != null)
                foreach (var dod in _serviceCollection)
                    service.Add(dod);
            _serviceCollection = service;
            Update();
        }

        /// <summary>
        ///    ���ɷ�Χ����
        /// </summary>
        /// <returns></returns>
        internal static IServiceProvider CreateScope()
        {
            lock (Local)
            {
                var provider = ServiceCollection.BuildServiceProvider(true);
                var scope = provider.GetService<IServiceScopeFactory>().CreateScope();
                Local.Value = new ScopeData
                {
                    Provider = scope.ServiceProvider,
                    Scope = scope
                };
                return Local.Value.Provider;
            }
        }

        #endregion
        #region Scope

        /// <summary>
        ///     ����(�벻Ҫ�ڿ��ܵ��ù���ĵط�����)
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider Update()
        {
            _rootProvider = null;
            Local.Value = null;
            return RootProvider;
        }

        /// <summary>
        ///     ������������Χ(�����ͷ�,����Ҫ�ȵ�GC����,��Դʹ���ʲ��ɿ�)
        /// </summary>
        public static void DisposeScope()
        {
            lock (Local)
            {
                if (Local.Value == null)
                    return;
                var scope = Local.Value.Scope;
                Local.Value.Scope = null;
                Local.Value.Provider = null;

                Local.Value = null;
                scope?.Dispose();
            }
        }

        #endregion

        #region ���ɽӿ�ʵ��

        /// <summary>
        ///     ���ɽӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            return ServiceProvider.GetService<TInterface>();
        }

        /// <summary>
        ///     ���ɽӿ�ʵ��(���û��ע����ʹ��Ĭ��ʵ��)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TDefault"></typeparam>
        /// <returns></returns>
        public static TInterface CreateBut<TInterface, TDefault>()
            where TInterface : class
            where TDefault : class, TInterface, new()
        {
            return ServiceProvider.GetService<TInterface>() ?? new TDefault();
        }


        #endregion

        #region ��������

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType, Type implementationType)
        {
            //DisposeScope();
            return ServiceCollection.AddTransient(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            //DisposeScope();
            return ServiceCollection.AddTransient(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddTransient<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType)
        {
            //DisposeScope();

            return ServiceCollection.AddTransient(serviceType);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService>() where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddTransient<TService>();
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddTransient(implementationFactory);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddTransient(implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType, Type implementationType)
        {
            //DisposeScope();

            return ServiceCollection.AddScoped(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            //DisposeScope();

            return ServiceCollection.AddScoped(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddScoped<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType)
        {
            //DisposeScope();

            return ServiceCollection.AddScoped(serviceType);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService>() where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddScoped<TService>();
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return ServiceCollection.AddScoped(implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddScoped(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType, Type implementationType)
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType)
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(serviceType);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>() where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton<TService>();
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///     instance specified in <paramref name="implementationInstance" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType, object implementationInstance)
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(serviceType, implementationInstance);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     instance specified in <paramref name="implementationInstance" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            //DisposeScope();

            return ServiceCollection.AddSingleton(typeof(TService), implementationInstance);
        }

        #endregion
    }
}