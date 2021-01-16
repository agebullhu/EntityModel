using Agebull.EntityModel.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
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
        /// <summary>
        /// 注册
        /// </summary>
        Task<bool> IAutoRegister.AutoRegist(IServiceCollection services)
        {
            services.AddScoped<IBusinessContext , BusinessContext> ();
            services.AddTransient<IMessageMiddleware, BusinessExceptionMiddleware>();
            return Task.FromResult(false);
        }
    }
}
