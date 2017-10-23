

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Logging;
using Newtonsoft.Json;


namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// Http进站出站的日志记录
    /// </summary>
    public sealed class CorsHandler : DelegatingHandler
    {
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = base.SendAsync(request, cancellationToken);
            result.ContinueWith((task, state) =>
            {
                result.Result.Headers.Add("Access-Control-Allow-Origin", "*");
            }, cancellationToken, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously);
            return result;
        }
    }
}