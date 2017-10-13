

using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class HttpIoLogHandler : DelegatingHandler
    {
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogRecorder.BeginMonitor(request.RequestUri.ToString());
            RecordRequestInfo(request, cancellationToken);
            var result = base.SendAsync(request, cancellationToken);
            result.ContinueWith((task, state) => RecordResponseInfo(task.Result), null,
                TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously);
            return result;
        }

        /// <summary>
        /// 记录API请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        private void RecordRequestInfo(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var args = new StringBuilder();
            args.Append("Headers：");
            foreach (var head in request.Headers)
            {
                args.Append($"【{head.Key}】{head.Value.LinkToString('|')}");
            }
            LogRecorder.MonitorTrace(args.ToString());
            LogRecorder.MonitorTrace($"Method：{request.Method}");

            LogRecorder.MonitorTrace($"QueryString：{request.RequestUri.Query}");

            StringBuilder code = new StringBuilder();
            if (request.Method == HttpMethod.Get)
            {
                code.Append($@"
                {{
                    caller.Bear = ""{ExtractToken(request)}"";
                    var result = caller.Get/*<>*/(""{request.RequestUri}"");
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }}");
            }
            else
            {
                var task = request.Content.ReadAsStringAsync();
                task.Wait(cancellationToken);
                LogRecorder.MonitorTrace($"Content：{task.Result}");
                code.Append($@"
                {{
                    caller.Bear = ""{ExtractToken(request)}"";
                    var result = caller.Post/*<>*/(""{request.RequestUri}"", new Dictionary<string, string>
                    {{");
                var di = FormatParams(task.Result);
                foreach(var item in di)
                {
                    code.Append($@"
                        {{""{item.Key}"",""{item.Value}""}},");
                }
                code.Append($@"
                    }});
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }}");
            }
            LogRecorder.Record(code.ToString(), LogType.Message);
        }


        /// <summary>
        /// 参数格式化
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Dictionary<string, string> FormatParams(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return new Dictionary<string, string>();
            var result = new Dictionary<string, string>();
            var kw = args.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            if (kw.Length == 0)
                return result;
            foreach (var item in kw)
            {
                var words = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                switch (words.Length)
                {
                    case 0:
                        continue;
                    case 1:
                        result.Add(words[0], null);
                        continue;
                    default:
                        result.Add(words[0], words[1]);
                        continue;
                }
            }
            return result;
        }
        /// <summary>
        /// 记录API返回
        /// </summary>
        /// <param name="response"></param>
        private static void RecordResponseInfo(HttpResponseMessage response)
        {
            try
            {
                var task = response.Content.ReadAsStringAsync();
                task.Wait();
                LogRecorder.MonitorTrace($"Result：{task.Result}");
            }
            catch (Exception e)
            {
                LogRecorder.MonitorTrace($"Result：{e.Message}");
            }
            LogRecorder.EndMonitor();
        }
        /// <summary>
        /// 取请求头的身份验证令牌
        /// </summary>
        /// <returns></returns>
        private string ExtractToken(HttpRequestMessage request)
        {
            const string bearer = "Bearer";
            var authz = request.Headers.Authorization;
            if (authz != null)
                return string.Equals(authz.Scheme, bearer, StringComparison.OrdinalIgnoreCase) ? authz.Parameter : null;
            if (!request.Headers.Contains("Authorization"))
                return null;
            string au = request.Headers.GetValues("Authorization").FirstOrDefault();
            if (au == null)
                return null;
            var aus = au.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (aus.Length < 2 || aus[0] != bearer)
                return null;
            return aus[1];
        }
    }
}