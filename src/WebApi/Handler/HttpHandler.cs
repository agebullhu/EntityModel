using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common;
using Agebull.Common.Logging;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// Handler
    /// </summary>
    internal sealed class HttpHandler : DelegatingHandler
    {
        /// <summary>
        /// 所有注册的系统处理对象
        /// </summary>
        internal static List<IHttpSystemHandler> Handlers = new List<IHttpSystemHandler>();
        
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (MonitorScope.CreateScope(request.RequestUri.ToString()))
            {
                foreach (var handler in Handlers)
                {
                    try
                    {
                        var task = handler.OnBegin(request,cancellationToken);
                        if (task == null)
                            continue;
                        ContextHelper.Remove("ApiContext");
                        return task;
                    }
                    catch (Exception e)
                    {
                        LogRecorder.Exception(e);
                    }
                }
                var result = base.SendAsync(request, cancellationToken);

                result.ContinueWith((task, state) => OnEnd(request, result.Result, cancellationToken), null,
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously);

                return result;
            }
        }

        /// <summary>
        /// 结束处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        private void OnEnd(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                //https://www.cnblogs.com/fengsiyi/archive/2013/05/27/3101404.html
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound://指示请求的资源不在服务器上。
                        break;
                    case HttpStatusCode.MethodNotAllowed://指示请求的资源上不允许请求方法（POST 或   GET）。
                        break;
                    case HttpStatusCode.UnsupportedMediaType://指示请求是不支持的类型。
                        break;
                }
            }
            for (var index = Handlers.Count - 1; index > 0; index++)
            {
                var handler = Handlers[index];
                try
                {
                    handler.OnEnd(request, cancellationToken,response);
                }
                catch (Exception e)
                {
                    LogRecorder.Exception(e);
                }
            }
            ContextHelper.Remove("ApiContext");
        }
    }
}