using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.AddIn;

namespace Agebull.EntityModel.MySql
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
            //services.AddTransient<IEntityEventProxy, EntityEventProxy>();
            services.AddTransient<IDataTrigger, DataStateTrigger>();
            services.AddTransient<IDataTrigger, CharacterDataUpdateTrigger>();
            MySqlConnectionsManager.Instance = new MySqlConnectionsManager();
            //MySqlConnectionsManager.IsManagement = true;
            services.AddSingleton<IHealthCheck, MySqlConnectionsManager>(provider => MySqlConnectionsManager.Instance);
            services.AddSingleton<IFlowMiddleware, MySqlConnectionsManager>(provider => MySqlConnectionsManager.Instance);
            return Task.FromResult(false);
        }
    }
}
