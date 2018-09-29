using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Ioc;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// Handler
    /// </summary>
    public sealed class HttpHandler : DelegatingHandler
	{
		/// <summary>
		/// 所有注册的系统处理对象
		/// </summary>
		public static readonly List<IHttpSystemHandler> Handlers = new List<IHttpSystemHandler>();

		/// <summary>
		/// 重载
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
		    IocHelper.Update();
            foreach (IHttpSystemHandler handler in Handlers)
			{
				try
				{
					Task<HttpResponseMessage> task = handler.OnBegin(request, cancellationToken);
					if (task != null)
					{
						return DoEnd(task, request, cancellationToken);
					}
				}
				catch (Exception ex)
				{
					LogRecorder.Exception(ex);
				}
			}
			Task<HttpResponseMessage> t = base.SendAsync(request, cancellationToken);
			return DoEnd(t, request, cancellationToken);
		}

		private Task<HttpResponseMessage> DoEnd(Task<HttpResponseMessage> t1, HttpRequestMessage request, CancellationToken cancellationToken)
		{
		    try
		    {
		        t1.Wait(cancellationToken);
		        HttpResponseMessage result;
		        if (t1.IsCanceled)
		        {
		            LogRecorder.MonitorTrace("操作被取消");
		            result = request.ToResponse(ApiResult.Error(-7, "服务器正忙", "操作被取消"));
		            LogRecorder.EndMonitor();
		            return Task<HttpResponseMessage>.Factory.StartNew(() => result, cancellationToken);
		        }
		        if (t1.IsFaulted)
		        {
		            LogRecorder.MonitorTrace(t1.Exception?.Message);
		            LogRecorder.Exception(t1.Exception);
		            result = request.ToResponse(ApiResult.Error(-1, "未知错误", t1.Exception?.Message));
		        }
		        else
		        {
		            result = t1.Result;
		        }
		        return Task<HttpResponseMessage>.Factory.StartNew(delegate
		        {
		            OnEnd(request, result, cancellationToken);
		            return result;
		        }, cancellationToken);
            }
		    finally
		    {
		        IocHelper.DisposeScope();
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
		    using (IocScope.CreateScope())
		    {
		        if (!response.IsSuccessStatusCode)
		        {
		            HttpStatusCode statusCode = response.StatusCode;
		            if (statusCode != HttpStatusCode.NotFound && statusCode != HttpStatusCode.MethodNotAllowed)
		            {
		                ;
		            }
		        }
		        for (int index = Handlers.Count - 1; index >= 0; index--)
		        {
		            IHttpSystemHandler handler = Handlers[index];
		            try
		            {
		                handler.OnEnd(request, cancellationToken, response);
		            }
		            catch (Exception ex)
		            {
		                LogRecorder.Exception(ex);
		            }
		        }
            }
		}
	}
}
