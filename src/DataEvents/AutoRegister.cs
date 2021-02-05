using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Composition;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.AddIn;

namespace Agebull.EntityModel.DataEvents
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
        void IAutoRegister.AutoRegist(IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger)
        {
            services.AddSingleton<IZeroOption>(DataEventOption.Instance);
        }

        /// <summary>
        /// 注册
        /// </summary>
        void IAutoRegister.LateConfigRegist(IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger)
        {
            if (DataEventOption.Instance.Event || DataEventOption.Instance.Injection)
                services.AddSingleton(typeof(IOperatorInjection<>), typeof(OperatorInjection<>));

            if (DataEventOption.Instance.Service.IsNull())
            {
                logger.Information("数据事件配置不正确（服务名称必须配置），数据事件服务注册失败");
                return;
            }

            if (DataEventOption.Instance.Receiver.IsNotNull())
            {
                services.AddTransient<IZeroDiscover, DataEventProxy>();
            }
        }
    }
}