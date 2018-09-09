using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;


namespace Agebull.Common.WebApi
{
    /// <summary>
    /// ��չʹ�û��๫����API��·�ɴ���
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