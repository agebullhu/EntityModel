using Agebull.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agebull.Common.WebApi
{
    /// <summary>
    ///     Http进站出站的日志记录
    /// </summary>
    public sealed class HttpIoLogHandler : IHttpSystemHandler
	{
		/// <summary>
		///     开始时的处理
		/// </summary>
		/// <returns>如果返回内容不为空，直接返回,后续的处理不再继续</returns>
		Task<HttpResponseMessage> IHttpSystemHandler.OnBegin(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (!LogRecorder.LogMonitor)
			{
				return null;
			}
			LogRecorder.BeginMonitor(request.RequestUri.ToString());
			try
			{
				StringBuilder args = new StringBuilder();
				args.Append("Headers：");
				foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
				{
					args.Append($"【{header.Key}】{header.Value.LinkToString('|')}");
				}
				LogRecorder.MonitorTrace(args.ToString());
				LogRecorder.MonitorTrace($"Method：{request.Method}");
				LogRecorder.MonitorTrace($"QueryString：{request.RequestUri.Query}");
				//RecordRequestToCode(request);
			}
			catch (Exception ex)
			{
				LogRecorder.Exception(ex);
			}
			return null;
		}

		/// <summary>
		///     结束时的处理
		/// </summary>
		void IHttpSystemHandler.OnEnd(HttpRequestMessage request, CancellationToken cancellationToken, HttpResponseMessage response)
		{
			if (LogRecorder.LogMonitor)
			{
				try
				{
					Task<string> task = response.Content.ReadAsStringAsync();
					task.Wait(cancellationToken);
					LogRecorder.MonitorTrace($"Result：{task.Result}");
				}
				catch (Exception e)
				{
					LogRecorder.MonitorTrace($"Result：{e.Message}");
				}
				LogRecorder.EndMonitor();
			}
		}

		/// <summary>
		///     请求注册为代码
		/// </summary>
		private void RecordRequestToCode(HttpRequestMessage request)
		{
			StringBuilder code = new StringBuilder();
			if (request.Method == HttpMethod.Get)
			{
				code.Append($"\r\n                {{\r\n                    caller.Bear = \"{ExtractToken(request)}\";\r\n                    var result = caller.Get/*<>*/(\"{request.RequestUri}\");\r\n                    Console.WriteLine(JsonConvert.SerializeObject(result));\r\n                }}");
			}
			else
			{
				Task<string> task = request.Content.ReadAsStringAsync();
				task.Wait();
				LogRecorder.MonitorTrace($"Content：{task.Result}");
				code.Append($"\r\n                {{\r\n                    caller.Bear = \"{ExtractToken(request)}\";\r\n                    var result = caller.Post/*<>*/(\"{request.RequestUri}\", new Dictionary<string, string>\r\n                    {{");
				foreach (KeyValuePair<string, string> item in FormatParams(task.Result))
				{
					code.Append($"\r\n                        {{\"{item.Key}\",\"{item.Value}\"}},");
				}
				code.Append("\r\n                    });\r\n                    Console.WriteLine(JsonConvert.SerializeObject(result));\r\n                }");
			}
			LogRecorder.Record(code.ToString());
		}

		/// <summary>
		///     参数格式化
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private Dictionary<string, string> FormatParams(string args)
		{
			if (string.IsNullOrWhiteSpace(args))
			{
				return new Dictionary<string, string>();
			}
			Dictionary<string, string> result = new Dictionary<string, string>();
			string[] kw = args.Split(new char[1]
			{
				'&'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (kw.Length == 0)
			{
				return result;
			}
			string[] array = kw;
			for (int i = 0; i < array.Length; i++)
			{
				string[] words = array[i].Split(new char[1]
				{
					'='
				}, StringSplitOptions.RemoveEmptyEntries);
				switch (words.LongLength)
				{
				case 1L:
					result.Add(words[0], null);
					break;
				default:
					result.Add(words[0], words[1]);
					break;
				case 0L:
					break;
				}
			}
			return result;
		}

		/// <summary>
		///     取请求头的身份验证令牌
		/// </summary>
		/// <returns></returns>
		private string ExtractToken(HttpRequestMessage request)
		{
			AuthenticationHeaderValue authz = request.Headers.Authorization;
			if (authz != null)
			{
				if (!string.Equals(authz.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				return authz.Parameter;
			}
			if (!request.Headers.Contains("Authorization"))
			{
				return null;
			}
			string au = request.Headers.GetValues("Authorization").FirstOrDefault();
			if (au == null)
			{
				return null;
			}
			string[] aus = au.Split(new char[1]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries);
			if (aus.Length < 2 || aus[0] != "Bearer")
			{
				return null;
			}
			return aus[1];
		}
	}
}
