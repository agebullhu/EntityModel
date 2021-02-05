using Agebull.EntityModel.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Composition;
using ZeroTeam.MessageMVC.AddIn;
using ZeroTeam.MessageMVC.Messages;
using ZeroTeam.MessageMVC.ModelApi;

namespace ZeroTeam.MessageMVC.Tools
{
    /// <summary>
    ///   组件注册
    /// </summary>
    [Export(typeof(IAutoRegister))]
    [ExportMetadata("Symbol", '%')]
    public sealed class AutoRegister : IAutoRegister
    {
        void IAutoRegister.AutoRegist(IServiceCollection services,ILogger logger)
        {
            services.AddScoped<IBusinessContext, BusinessContext>();
            services.AddTransient<IMessageMiddleware, BusinessExceptionMiddleware>();
        }
    }
}
