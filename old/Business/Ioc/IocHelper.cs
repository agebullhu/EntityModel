using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using Autofac.Extensions.DependencyInjection;
using IContainer = Autofac.IContainer;

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    ///  简单的依赖注入器
    /// </summary>
    public class IocHelper
    {
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public static IServiceProvider Provider { get; set; }
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public static IContainer Container { get; set; }
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        public static ContainerBuilder Builder { get; set; }
        /// <summary>
        /// 绑定容器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider Binding(IServiceCollection services)
        {
            Builder = new ContainerBuilder();
            if (services != null)
                Builder.Populate(services);

            Container = Builder.Build();

            return Provider = new AutofacServiceProvider(Container);

        }
    }
}