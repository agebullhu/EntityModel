using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// 跨域支持
    /// </summary>
    public sealed class CorsHandler : IHttpSystemHandler
    {
        private static bool Cors = Configuration.ConfigurationManager.AppSettings["Cors"] == "True";
        /// <summary>
        ///     开始时的处理
        /// </summary>
        /// <returns>如果返回内容不为空，直接返回,后续的处理不再继续</returns>
        Task<HttpResponseMessage> IHttpSystemHandler.OnBegin(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (HttpMethod.Options != request.Method)
			{
				return null;
			}
			return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK)
			{
				Headers = 
				{
					{
						"Access-Control-Allow-Methods",
						new string[2]
						{
						    "GET",
						    "POST"
						}
					},
					{
						"Access-Control-Allow-Headers",
						new string[4]
						{
						    "x-requested-with",
						    "content-type",
						    "authorization",
						    "*"
						}
					}
				}
			}, cancellationToken);
		}

		/// <summary>
		///     结束时的处理
		/// </summary>
		void IHttpSystemHandler.OnEnd(HttpRequestMessage request, CancellationToken cancellationToken, HttpResponseMessage response)
		{
		    if (HttpMethod.Options == request.Method || Cors)
		    {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
		}
	}
}
