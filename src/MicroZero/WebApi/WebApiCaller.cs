using Agebull.Common.Context;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Agebull.MicroZero.WebApi
{
    /// <summary>
    ///     WebApi调用类
    /// </summary>
    public class WebApiCaller
	{
		/// <summary>
		///     主机
		/// </summary>
		public string Host
		{
			get;
			set;
		}

		/// <summary>
		///     身份头
		/// </summary>
		public string Bearer { get; }

		/// <summary>
		///     构造
		/// </summary>
		public WebApiCaller()
		{
			GlobalContext.TryCheckByAnymouse();
		}

		/// <summary>
		///     构造
		/// </summary>
		public WebApiCaller(string host)
		{
			Host = host;
		    GlobalContext.TryCheckByAnymouse();
        }

		/// <summary>
		///     构造
		/// </summary>
		public WebApiCaller(string host, string beare)
		{
			Host = host;
		    Bearer = beare;
		    GlobalContext.TryCheckByAnymouse();
        }

		/// <summary>
		///     参数格式化
		/// </summary>
		/// <param name="httpParams"></param>
		/// <returns></returns>
		private static string FormatParams(Dictionary<string, string> httpParams)
		{
			if (httpParams == null)
			{
				return null;
			}
			var first = true;
			var builder = new StringBuilder();
			foreach (var httpParam in httpParams)
			{
				if (first)
				{
					first = false;
				}
				else
				{
					builder.Append('&');
				}
				builder.Append($"{httpParam.Key}=");
				if (!string.IsNullOrEmpty(httpParam.Value))
				{
					builder.Append($"{HttpUtility.UrlEncode(httpParam.Value, Encoding.UTF8)}");
				}
			}
			return builder.ToString();
		}

		/// <summary>
		///     转为合理的API地址
		/// </summary>
		/// <param name="api"></param>
		/// <returns></returns>
		private string ToUrl(string api)
		{
			return $"{Host?.TrimEnd('/') + "/"}{api?.TrimStart('/')}";
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <returns></returns>
		public ApiResult<TResult> Get<TResult>(string apiName) 
		{
			return Get<TResult>(apiName, "");
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiResult<TResult> Get<TResult>(string apiName, Dictionary<string, string> arguments) 
		{
			return Get<TResult>(apiName, FormatParams(arguments));
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiResult<TResult> Get<TResult>(string apiName, IWebApiArgument arguments) 
		{
			return Get<TResult>(apiName, arguments?.ToFormString());
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiResult<TResult> Get<TResult>(string apiName, string arguments) 
		{
			LogRecorderX.BeginStepMonitor("内部API调用" + ToUrl(apiName));
			var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
			LogRecorderX.MonitorTrace(ctx);
			LogRecorderX.MonitorTrace("Arguments:" + arguments);
			if (!string.IsNullOrWhiteSpace(arguments))
			{
				apiName = $"{apiName}?{arguments}";
			}
			var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
			req.Method = "GET";
			req.ContentType = "application/x-www-form-urlencoded";
			req.Headers.Add(HttpRequestHeader.Authorization, ctx);
			return GetResult<TResult>(req);
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="argument"></param>
		/// <returns></returns>
		public ApiResult<TResult> Post<TResult>(string apiName, IWebApiArgument argument) 
		{
			return Post<TResult>(apiName, argument?.ToFormString());
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="argument"></param>
		/// <returns></returns>
		public ApiResult<TResult> Post<TResult>(string apiName, Dictionary<string, string> argument) 
		{
			return Post<TResult>(apiName, FormatParams(argument));
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="apiName"></param>
		/// <param name="form"></param>
		/// <returns></returns>
		public ApiResult<TResult> Post<TResult>(string apiName, string form) 
		{
			LogRecorderX.BeginStepMonitor("内部API调用" + ToUrl(apiName));
			var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
			LogRecorderX.MonitorTrace(ctx);
			LogRecorderX.MonitorTrace("Arguments:" + form);
			var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.Headers.Add(HttpRequestHeader.Authorization, ctx);
			try
			{
				using (var rs = req.GetRequestStream())
				{
					var formData = Encoding.UTF8.GetBytes(form);
					rs.Write(formData, 0, formData.Length);
				}
			}
			catch (Exception ex)
			{
				LogRecorderX.Exception(ex);
				LogRecorderX.EndStepMonitor();
				return ApiResult.Error<TResult>(ErrorCode.RemoteError);
			}
			return GetResult<TResult>(req);
		}

		/// <summary>
		///     取返回值
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="req"></param>
		/// <returns></returns>
		public ApiResult<TResult> GetResult<TResult>(HttpWebRequest req) 
		{
			string jsonResult;
			try
			{
				using (var webResponse = req.GetResponse())
				{
					var receivedStream2 = webResponse.GetResponseStream();
					if (receivedStream2 == null)
					{
						LogRecorderX.EndStepMonitor();
						return ApiResult.Error<TResult>(-1, "服务器无返回值");
					}
					jsonResult = new StreamReader(receivedStream2).ReadToEnd();
					receivedStream2.Dispose();
					webResponse.Close();
				}
			}
			catch (WebException e)
			{
				if (e.Status != WebExceptionStatus.ProtocolError)
				{
					LogRecorderX.EndStepMonitor();
					switch (e.Status)
					{
					case WebExceptionStatus.CacheEntryNotFound:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "找不到指定的缓存项");
					case WebExceptionStatus.ConnectFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "在传输级别无法联系远程服务点");
					case WebExceptionStatus.ConnectionClosed:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "过早关闭连接");
					case WebExceptionStatus.KeepAliveFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "指定保持活动状态的标头的请求的连接意外关闭");
					case WebExceptionStatus.MessageLengthLimitExceeded:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "已收到一条消息的发送请求时超出指定的限制或从服务器接收响应");
					case WebExceptionStatus.NameResolutionFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "名称解析程序服务或无法解析主机名");
					case WebExceptionStatus.Pending:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "内部异步请求处于挂起状态");
					case WebExceptionStatus.PipelineFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "该请求是管线请求和连接被关闭之前收到响应");
					case WebExceptionStatus.ProxyNameResolutionFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "名称解析程序服务无法解析代理服务器主机名");
					case WebExceptionStatus.ReceiveFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "从远程服务器未收到完整的响应");
					case WebExceptionStatus.RequestCanceled:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "请求已取消");
					case WebExceptionStatus.RequestProhibitedByCachePolicy:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "缓存策略不允许该请求");
					case WebExceptionStatus.RequestProhibitedByProxy:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "由该代理不允许此请求");
					case WebExceptionStatus.SecureChannelFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "使用 SSL 建立连接时出错");
					case WebExceptionStatus.SendFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "无法与远程服务器发送一个完整的请求");
					case WebExceptionStatus.ServerProtocolViolation:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "服务器响应不是有效的 HTTP 响应");
					case WebExceptionStatus.Timeout:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "请求的超时期限内未不收到任何响应");
					case WebExceptionStatus.TrustFailure:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "无法验证服务器证书");
					default:
						return ApiResult.Error<TResult>(ErrorCode.RemoteError, "发生未知类型的异常");
					}
				}
				using (var response = e.Response)
				{
					var receivedStream = response.GetResponseStream();
					if (receivedStream == null)
					{
						LogRecorderX.EndStepMonitor();
						return ApiResult.Error<TResult>(-1, "服务器无返回值");
					}
					jsonResult = new StreamReader(receivedStream).ReadToEnd();
					receivedStream.Dispose();
					response.Close();
				}
			}
			catch (Exception ex)
			{
				LogRecorderX.Exception(ex);
				LogRecorderX.EndStepMonitor();
				return ApiResult.Error<TResult>(ErrorCode.RemoteError);
			}
			LogRecorderX.MonitorTrace(jsonResult);
			try
			{
			    return string.IsNullOrWhiteSpace(jsonResult) 
			        ? ApiResult.Error<TResult>(-1) 
			        : JsonConvert.DeserializeObject<ApiResult<TResult>>(jsonResult);
			}
			catch (Exception ex2)
			{
				LogRecorderX.Exception(ex2);
				return ApiResult.Error<TResult>(-1);
			}
			finally
			{
				LogRecorderX.EndStepMonitor();
			}
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <returns></returns>
		public ApiValueResult Get(string apiName)
		{
			return Get(apiName, "");
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiValueResult Get(string apiName, Dictionary<string, string> arguments)
		{
			return Get(apiName, FormatParams(arguments));
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiValueResult Get(string apiName, IWebApiArgument arguments)
		{
			return Get(apiName, arguments?.ToFormString());
		}

		/// <summary>
		///     通过Get调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public ApiValueResult Get(string apiName, string arguments)
		{
			LogRecorderX.BeginStepMonitor("内部API调用" + ToUrl(apiName));
			var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
			LogRecorderX.MonitorTrace(ctx);
			LogRecorderX.MonitorTrace("Arguments:" + arguments);
			if (!string.IsNullOrWhiteSpace(arguments))
			{
				apiName = $"{apiName}?{arguments}";
			}
			var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
			req.Method = "GET";
			req.ContentType = "application/x-www-form-urlencoded";
			req.Headers.Add(HttpRequestHeader.Authorization, ctx);
		    using (MonitorScope.CreateScope("Caller Remote"))
		    {
		        return GetResult(req);
            }
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="argument"></param>
		/// <returns></returns>
		public ApiValueResult Post(string apiName, IWebApiArgument argument)
		{
			return Post(apiName, argument?.ToFormString());
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="argument"></param>
		/// <returns></returns>
		public ApiValueResult Post(string apiName, Dictionary<string, string> argument)
		{
			return Post(apiName, FormatParams(argument));
		}

		/// <summary>
		///     通过Post调用
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="form"></param>
		/// <returns></returns>
		public ApiValueResult Post(string apiName, string form)
		{
			LogRecorderX.BeginStepMonitor("内部API调用" + ToUrl(apiName));
			var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
			LogRecorderX.MonitorTrace(ctx);
			LogRecorderX.MonitorTrace("Arguments:" + form);
			var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.Headers.Add(HttpRequestHeader.Authorization, ctx);
			try
			{
				using (var rs = req.GetRequestStream())
				{
					var formData = Encoding.UTF8.GetBytes(form);
					rs.Write(formData, 0, formData.Length);
				}
			}
			catch (Exception ex)
			{
				LogRecorderX.Exception(ex);
				LogRecorderX.EndStepMonitor();
				return ErrorResult(-3);
		    }
		    using (MonitorScope.CreateScope("Caller Remote"))
		    {
		        return GetResult(req);
		    }
		}

		/// <summary>
		///     取返回值
		/// </summary>
		/// <param name="req"></param>
		/// <returns></returns>
		public ApiValueResult GetResult(HttpWebRequest req)
		{
			string jsonResult;
			try
			{
				using (var response = req.GetResponse())
				{
					var receivedStream2 = response.GetResponseStream();
					if (receivedStream2 == null)
					{
						LogRecorderX.EndStepMonitor();
						return ErrorResult(-1, "服务器无返回值");
					}
					jsonResult = new StreamReader(receivedStream2).ReadToEnd();
					receivedStream2.Dispose();
					response.Close();
				}
			}
			catch (WebException e3)
			{
			    try
			    {
                    if (e3.Status != WebExceptionStatus.ProtocolError)
                    {
                        LogRecorderX.EndStepMonitor();
                        switch (e3.Status)
                        {
                            case WebExceptionStatus.CacheEntryNotFound:
                                return ErrorResult(-3, "找不到指定的缓存项");
                            case WebExceptionStatus.ConnectFailure:
                                return ErrorResult(-3, "在传输级别无法联系远程服务点");
                            case WebExceptionStatus.ConnectionClosed:
                                return ErrorResult(-3, "过早关闭连接");
                            case WebExceptionStatus.KeepAliveFailure:
                                return ErrorResult(-3, "指定保持活动状态的标头的请求的连接意外关闭");
                            case WebExceptionStatus.MessageLengthLimitExceeded:
                                return ErrorResult(-3, "已收到一条消息的发送请求时超出指定的限制或从服务器接收响应");
                            case WebExceptionStatus.NameResolutionFailure:
                                return ErrorResult(-3, "名称解析程序服务或无法解析主机名");
                            case WebExceptionStatus.Pending:
                                return ErrorResult(-3, "内部异步请求处于挂起状态");
                            case WebExceptionStatus.PipelineFailure:
                                return ErrorResult(-3, "该请求是管线请求和连接被关闭之前收到响应");
                            case WebExceptionStatus.ProxyNameResolutionFailure:
                                return ErrorResult(-3, "名称解析程序服务无法解析代理服务器主机名");
                            case WebExceptionStatus.ReceiveFailure:
                                return ErrorResult(-3, "从远程服务器未收到完整的响应");
                            case WebExceptionStatus.RequestCanceled:
                                return ErrorResult(-3, "请求已取消");
                            case WebExceptionStatus.RequestProhibitedByCachePolicy:
                                return ErrorResult(-3, "缓存策略不允许该请求");
                            case WebExceptionStatus.RequestProhibitedByProxy:
                                return ErrorResult(-3, "由该代理不允许此请求");
                            case WebExceptionStatus.SecureChannelFailure:
                                return ErrorResult(-3, "使用 SSL 建立连接时出错");
                            case WebExceptionStatus.SendFailure:
                                return ErrorResult(-3, "无法与远程服务器发送一个完整的请求");
                            case WebExceptionStatus.ServerProtocolViolation:
                                return ErrorResult(-3, "服务器响应不是有效的 HTTP 响应");
                            case WebExceptionStatus.Timeout:
                                return ErrorResult(-3, "请求的超时期限内未不收到任何响应");
                            case WebExceptionStatus.TrustFailure:
                                LogRecorderX.EndStepMonitor();
                                return ErrorResult(-3, "无法验证服务器证书");
                            default:
                                return ErrorResult(-3, "发生未知类型的异常");
                        }
                    }
                    var codes = e3.Message.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                    if (codes.Length == 3 && int.TryParse(codes[1], out var s) && s == 404)
                    {
                        return ErrorResult(-3, "服务器内部错误", "页面不存在");
                    }
                    using (var webResponse = e3.Response)
                    {
                        var receivedStream = webResponse.GetResponseStream();
                        if (receivedStream == null)
                        {
                            LogRecorderX.EndStepMonitor();
                            return ErrorResult(-1, "服务器无返回值");
                        }
                        jsonResult = new StreamReader(receivedStream).ReadToEnd();
                        receivedStream.Dispose();
                        webResponse.Close();
                    }
                }
			    catch (Exception e)
			    {
			        LogRecorderX.Exception(e);
			        LogRecorderX.EndStepMonitor();
			        return ErrorResult(-1, "未知错误", e.Message);
                }
			}
			catch (Exception e2)
			{
				LogRecorderX.Exception(e2);
				LogRecorderX.EndStepMonitor();
				return ErrorResult(-1, "未知错误", e2.Message);
			}
			LogRecorderX.MonitorTrace(jsonResult);
			try
			{
			    if (string.IsNullOrWhiteSpace(jsonResult))
			        return ErrorResult(-1);
			    var baseResult = JsonConvert.DeserializeObject<ApiResult>(jsonResult);
			    return (!baseResult.Success) ? ErrorResult(baseResult.Status.ErrorCode, baseResult.Status.ClientMessage) : ApiValueResult.Succees(ReadResultData(jsonResult, "ResultData"));
			}
			catch (Exception e)
			{
				LogRecorderX.Exception(e);
				return ErrorResult(-1, "未知错误", e.Message);
			}
		}

        private ApiValueResult ErrorResult(int code,string msg = null, string inner=null)
	    {
	        var result= ApiValueResult.ErrorResult(code, msg,inner);
	        result.Status.Point += "-apiCaller";
            return result;

        }

        private static string ReadResultData(string json, string property)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected I4, but got Unknown
			if (json == null || json.Trim()[0] != '{')
			{
				return json;
			}
			var code = new StringBuilder();
			using (var textReader = new StringReader(json))
			{
				var reader = new JsonTextReader(textReader);
				var isResultData = false;
				var levle = 0;
				while (reader.Read())
				{
					if (!isResultData && (int)reader.TokenType == 4)
					{
						if (reader.Value.ToString() == property)
						{
							isResultData = true;
						}
					}
					else if (isResultData)
					{
						var tokenType = reader.TokenType;
						switch ((int)tokenType)
						{
						case 2:
							code.Append('[');
							continue;
						case 1:
							code.Append('{');
							levle++;
							continue;
						case 4:
							code.Append($"\"{reader.Value}\"=");
							continue;
						case 17:
							code.Append($"\"{reader.Value}\"");
							goto default;
						case 9:
						case 16:
							code.Append($"\"{reader.Value}\"");
							goto default;
						case 7:
						case 8:
						case 10:
							code.Append("null");
							goto default;
						case 11:
							code.Append("null");
							goto default;
						case 13:
							if (code.Length > 0 && code[code.Length - 1] == ',')
							{
								code[code.Length - 1] = '}';
							}
							else
							{
								code.Append('}');
							}
							levle--;
							goto default;
						case 14:
							if (code.Length > 0 && code[code.Length - 1] == ',')
							{
								code[code.Length - 1] = ']';
							}
							else
							{
								code.Append(']');
							}
							goto default;
						case 6:
							code.Append(reader.Value);
							goto default;
						default:
							if (levle == 0)
							{
								break;
							}
							code.Append(',');
							continue;
						}
						break;
					}
				}
			}
			if (code.Length > 0 && code[code.Length - 1] == ',')
			{
				code[code.Length - 1] = ' ';
			}
			return code.ToString().Trim('\'', '"');
		}
	}
}
