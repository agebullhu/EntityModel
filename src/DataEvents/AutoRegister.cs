using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.AddIn;
using ZeroTeam.MessageMVC.Documents;
using ZeroTeam.MessageMVC.Messages;
using ZeroTeam.MessageMVC.Services;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.DataEvents
{
    /// <summary>
    /// 数据事件配置
    /// </summary>
    public class DataEventOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Service { get; set; }
    }
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
            var opt = ConfigurationHelper.Get<DataEventOption>("DataEvent");
            if (string.IsNullOrWhiteSpace(opt.Service))
                return Task.FromResult(false);
            var receiver = DependencyHelper.GetService<IMessageConsumer>();
            if (receiver == null)
                return Task.FromResult(false);
            var service = new ZeroService
            {
                IsAutoService = true,
                ServiceName = opt.Service,
                Receiver = receiver,
                Serialize = new NewtonJsonSerializeProxy()
            } as IService;
            service.RegistWildcardAction(new ApiActionInfo
            {
                Name = "*",
                Route = "*",
                ControllerName = "DataEventProxy",
                ControllerCaption = "DataEventProxy",
                AccessOption = ApiOption.Public | ApiOption.Anymouse,
                ResultType = typeof(Task),
                IsAsync = true,
                Action = (msg, seri, arg) => DataEventProxy.OnEvent(msg, seri, arg)
            });
            ZeroFlowControl.RegistService(service);

            return Task.FromResult(false);
        }
    }
}