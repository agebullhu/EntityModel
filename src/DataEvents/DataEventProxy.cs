/*design by:agebull designer date:2021/1/5 16:33:03*/
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC;
using ZeroTeam.MessageMVC.Documents;
using ZeroTeam.MessageMVC.Messages;
using ZeroTeam.MessageMVC.Services;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.DataEvents
{

    /// <summary>
    ///  标准事件控制器
    /// </summary>
    public class DataEventProxy : IZeroDiscover
    {
        internal static readonly Dictionary<string, List<IEntityEventHandler>> maps = new Dictionary<string, List<IEntityEventHandler>>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 服务发现
        /// </summary>
        /// <returns></returns>
        Task IZeroDiscover.Discovery()
        {
            var opt = DataEventOption.Instance;
            var handlers = DependencyHelper.GetServices<IEntityEventHandler>();
            foreach (var handler in handlers)
            {
                foreach (var name in handler.EntityNames)
                {
                    var method = $"{handler.Project}/{name}";
                    if (!maps.TryGetValue(method, out var list))
                        maps.Add(method, list = new List<IEntityEventHandler>());
                    list.Add(handler);
                }
            }
            if (maps.Count == 0)
                return Task.CompletedTask;

            var logger = DependencyHelper.LoggerFactory.CreateLogger<DataEventProxy>();
            var receiver = DependencyHelper.GetService<IMessageReceiver>(opt.Receiver);
            if (receiver == null)
            {
                logger.Error($"数据事件接收器({opt.Receiver})无法构造，数据事件服务注册失败");
                return Task.CompletedTask;
            }
            var service = new ZeroService
            {
                IsAutoService = true,
                ServiceName = opt.Service,
                Receiver = receiver,
                CanRun = () => maps.Count > 0,
                Serialize = new NewtonJsonSerializeProxy()
            } as IService;

            service.RegistWildcardAction(new ApiActionInfo
            {
                Name = "*",
                Routes = new[] { "*" },
                ControllerName = "DataEventProxy",
                ControllerCaption = "DataEventProxy",
                AccessOption = ApiOption.Public | ApiOption.Anymouse,
                ResultType = typeof(Task),
                IsAsync = true,
                Action = (msg, seri, arg) => DataEventProxy.OnEvent(msg, seri, arg)
            });
            ZeroFlowControl.RegistService(service);
            logger.Information($"已注册数据事件服务,服务：{opt.Service}，接收器：{opt.Receiver}");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serialize"></param>
        /// <param name="_"></param>
        /// <returns></returns>
        internal static async Task OnEvent(IInlineMessage message, ISerializeProxy serialize, object _)
        {
            if (!maps.TryGetValue(message.Method, out var handlers) || handlers.Count == 0)
                return;
            if (!serialize.TryDeserialize<EntityEventArgument>(message.Argument, out var argument))
                return;
            var tasks = new Task[handlers.Count];
            for (int i = 0; i < handlers.Count; i++)
            {
                IEntityEventHandler handler = handlers[i];
                tasks[i] = handler.OnEvent(argument);
            }
            for (int i = 0; i < handlers.Count; i++)
            {
                await tasks[i];
            }
        }
    }
}