/*design by:agebull designer date:2021/1/5 16:33:03*/
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ZeroTeam.MessageMVC.Messages;

namespace Agebull.EntityModel.DataEvents
{

    /// <summary>
    ///  标准事件控制器
    /// </summary>
    public class DataEventProxy
    {
        static readonly Dictionary<string, Func<IEntityEventHandler>> maps = new Dictionary<string, Func<IEntityEventHandler>>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 注册事件处理器
        /// </summary>
        /// <typeparam name="TEntityEventHandler"></typeparam>、
        public static void RegistEventHandler<TEntityEventHandler>()
            where TEntityEventHandler : class, IEntityEventHandler, new()
        {
            static IEntityEventHandler func() => new TEntityEventHandler();
            var handler = func();
            foreach (var name in handler.EntityNames)
            {
                if (!maps.TryAdd($"{handler.Project}/{name}", func))
                    maps[name] = func;
            }
        }

        internal static Task OnEvent(IInlineMessage message, ISerializeProxy serialize, object _)
        {
            if (!maps.TryGetValue(message.Method, out var func))
                return Task.CompletedTask;
            var argument = JsonHelper.DeserializeObject<EntityEventArgument>(message.Argument);
            var handler = func();
            return handler.OnEvent(argument);
        }
    }
}