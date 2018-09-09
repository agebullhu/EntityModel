using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;


namespace Agebull.Common.WebApi
{
    /// <summary>
    /// 扩展使用基类公开的API的路由代理
    /// </summary>
    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        /// <inheritdoc />
        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>
                (inherit: true);
        }
    }
}