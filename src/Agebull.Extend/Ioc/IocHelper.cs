using System;
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
        public IServiceProvider Provider => _provider;

        private static IServiceProvider _provider;

        /// <summary>
        /// 注册IOC代理
        /// </summary>
        public static void SetServiceProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 生成接口实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
           if(_provider == null)
               throw new NotSupportedException("IocHelper的Provider对象为空,是否忘记调用SetServiceProvider对象了");
            return _provider.GetService<TInterface>();
        }

        /// <summary>
        /// 生成接口实例(如果没有注册则使用默认实现)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TDefault"></typeparam>
        /// <returns></returns>
        public static TInterface CreateBut<TInterface,TDefault>()
            where TInterface : class
            where TDefault : class, TInterface,new()
        {
            if (_provider == null)
                throw new NotSupportedException("IocHelper的Provider对象为空,是否忘记调用SetServiceProvider对象了");
            return _provider.GetService<TInterface>() ?? new TDefault();
        }
    }
}