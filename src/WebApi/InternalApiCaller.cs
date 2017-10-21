using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Agebull.Common.Logging;
using Newtonsoft.Json;
using System.Web;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// 内部服务调用代理
    /// </summary>
    public class WebApiCaller
    {
        private string _host;

        /// <summary>
        /// 主机
        /// </summary>
        public string Host
        {
            get => _host;
            set => _host = value?.TrimEnd('/') + "/";
        }

        private string _beare;

        /// <summary>
        /// 主机
        /// </summary>
        public string Bearer
        {
            get => _beare ?? (ApiContext.IsClientTest ? "*TEST_CLIENT" : ApiContext.RequestContext == null ? null : JsonConvert.SerializeObject(ApiContext.RequestContext));
            set => _beare = value;
        }

        /// <summary>
        /// 参数格式化
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private string FormatParams(Dictionary<string, string> httpParams)
        {
            if (httpParams == null)
                return null;
            bool first = true;
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in httpParams)
            {
                if (first)
                    first = false;
                else
                    builder.Append('&');
                builder.Append($"{kvp.Key}=");
                if (!string.IsNullOrEmpty(kvp.Value))
                    builder.Append($"{HttpUtility.UrlEncode(kvp.Value, Encoding.UTF8)}");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 转为合理的API地址
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        string ToUrl(string api)
        {
            return $"{Host}{api?.TrimStart('/')}";
        }
        #region 强类型取得


        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string apiName)
            where TResult : IApiResultData
        {
            return Get<TResult>(apiName, "");
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string apiName, Dictionary<string, string> arguments)
            where TResult : IApiResultData
        {
            return Get<TResult>(apiName, FormatParams(arguments));
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string apiName, IApiArgument arguments)
            where TResult : IApiResultData
        {
            return Get<TResult>(apiName, arguments?.ToFormString());
        }
        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string apiName, string arguments)
            where TResult : IApiResultData
        {
            LogRecorder.BeginStepMonitor("内部API调用" + ToUrl(apiName));

            var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + arguments);

            if (!string.IsNullOrWhiteSpace(arguments))
                apiName = $"{apiName}?{arguments}";

            var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add(HttpRequestHeader.Authorization, ctx);

            return GetResult<TResult>(req);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string apiName, IApiArgument argument)
            where TResult : IApiResultData
        {
            return Post<TResult>(apiName, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string apiName, Dictionary<string, string> argument)
            where TResult : IApiResultData
        {
            return Post<TResult>(apiName, FormatParams(argument));
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string apiName, string form)
            where TResult : IApiResultData
        {
            LogRecorder.BeginStepMonitor("内部API调用" + ToUrl(apiName));

            var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + form);

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
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                LogRecorder.EndStepMonitor();
                return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError);
            }

            return GetResult<TResult>(req);
        }

        /// <summary>
        /// 取返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public ApiResult<TResult> GetResult<TResult>(HttpWebRequest req)
            where TResult : IApiResultData
        {
            string result;
            try
            {
                using (var response = req.GetResponse())
                {
                    using (var receivedStream = response.GetResponseStream())
                    {
                        var streamReader = new StreamReader(receivedStream);
                        result = streamReader.ReadToEnd();
                    }
                    response.Close();
                }
            }
            catch (WebException e)
            {
                switch (e.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        break;
                    case WebExceptionStatus.CacheEntryNotFound:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "找不到指定的缓存项");
                    case WebExceptionStatus.ConnectFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "在传输级别无法联系远程服务点");
                    case WebExceptionStatus.ConnectionClosed:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "过早关闭连接");
                    case WebExceptionStatus.KeepAliveFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "指定保持活动状态的标头的请求的连接意外关闭");
                    case WebExceptionStatus.MessageLengthLimitExceeded:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "已收到一条消息的发送请求时超出指定的限制或从服务器接收响应");
                    case WebExceptionStatus.NameResolutionFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "名称解析程序服务或无法解析主机名");
                    case WebExceptionStatus.Pending:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "内部异步请求处于挂起状态");
                    case WebExceptionStatus.PipelineFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "该请求是管线请求和连接被关闭之前收到响应");
                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "名称解析程序服务无法解析代理服务器主机名");
                    case WebExceptionStatus.ReceiveFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "从远程服务器未收到完整的响应");
                    case WebExceptionStatus.RequestCanceled:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "请求已取消");
                    case WebExceptionStatus.RequestProhibitedByCachePolicy:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "缓存策略不允许该请求");
                    case WebExceptionStatus.RequestProhibitedByProxy:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "由该代理不允许此请求");
                    case WebExceptionStatus.SecureChannelFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "使用 SSL 建立连接时出错");
                    case WebExceptionStatus.SendFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "无法与远程服务器发送一个完整的请求");
                    case WebExceptionStatus.ServerProtocolViolation:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "服务器响应不是有效的 HTTP 响应");
                    case WebExceptionStatus.Timeout:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "请求的超时期限内未不收到任何响应");
                    case WebExceptionStatus.TrustFailure:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "无法验证服务器证书");
                    default:
                        //case WebExceptionStatus.UnknownError:
                        return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError, "发生未知类型的异常");
                }
                using (var response = e.Response)
                {
                    using (var receivedStream = response.GetResponseStream())
                    {
                        var streamReader = new StreamReader(receivedStream);
                        result = streamReader.ReadToEnd();
                    }
                    response.Close();
                }
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                LogRecorder.EndStepMonitor();
                return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError);
            }
            LogRecorder.MonitorTrace(result);
            try
            {
                if (string.IsNullOrWhiteSpace(result))
                {
                    return ApiResult<TResult>.ErrorResult(ErrorCode.UnknowError);
                }
                return JsonConvert.DeserializeObject<ApiResult<TResult>>(result);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ApiResult<TResult>.ErrorResult(ErrorCode.UnknowError);
            }
            finally
            {
                LogRecorder.EndStepMonitor();
            }
        }

        #endregion
        #region 无类型取得

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string apiName)
        {
            return Get(apiName, "");
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string apiName, Dictionary<string, string> arguments)
        {
            return Get(apiName, FormatParams(arguments));
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string apiName, IApiArgument arguments)
        {
            return Get(apiName, arguments?.ToFormString());
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string apiName, string arguments)
        {
            LogRecorder.BeginStepMonitor("内部API调用" + ToUrl(apiName));

            var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + arguments);

            if (!string.IsNullOrWhiteSpace(arguments))
                apiName = $"{apiName}?{arguments}";

            var req = (HttpWebRequest)WebRequest.Create(ToUrl(apiName));
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add(HttpRequestHeader.Authorization, ctx);

            return GetResult(req);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string apiName, IApiArgument argument)
        {
            return Post(apiName, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string apiName, Dictionary<string, string> argument)
        {
            return Post(apiName, FormatParams(argument));
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string apiName, string form)
        {
            LogRecorder.BeginStepMonitor("内部API调用" + ToUrl(apiName));

            var ctx = string.IsNullOrEmpty(Bearer) ? null : $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + form);

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
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                LogRecorder.EndStepMonitor();
                return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError);
            }

            return GetResult(req);
        }


        /// <summary>
        /// 取返回值
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ApiValueResult<string> GetResult(HttpWebRequest req)
        {
            string result;
            try
            {
                using (var response = req.GetResponse())
                {
                    using (var receivedStream = response.GetResponseStream())
                    {
                        var streamReader = new StreamReader(receivedStream);
                        result = streamReader.ReadToEnd();
                    }
                    response.Close();
                }
            }
            catch (WebException e)
            {
                switch (e.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        break;
                    case WebExceptionStatus.CacheEntryNotFound:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"找不到指定的缓存项");
                    case WebExceptionStatus.ConnectFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"在传输级别无法联系远程服务点");
                    case WebExceptionStatus.ConnectionClosed:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"过早关闭连接");
                    case WebExceptionStatus.KeepAliveFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"指定保持活动状态的标头的请求的连接意外关闭");
                    case WebExceptionStatus.MessageLengthLimitExceeded:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"已收到一条消息的发送请求时超出指定的限制或从服务器接收响应");
                    case WebExceptionStatus.NameResolutionFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"名称解析程序服务或无法解析主机名");
                    case WebExceptionStatus.Pending:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"内部异步请求处于挂起状态");
                    case WebExceptionStatus.PipelineFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"该请求是管线请求和连接被关闭之前收到响应");
                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"名称解析程序服务无法解析代理服务器主机名");
                    case WebExceptionStatus.ReceiveFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"从远程服务器未收到完整的响应");
                    case WebExceptionStatus.RequestCanceled:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"请求已取消");
                    case WebExceptionStatus.RequestProhibitedByCachePolicy:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError, "缓存策略不允许该请求");
                    case WebExceptionStatus.RequestProhibitedByProxy:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"由该代理不允许此请求");
                    case WebExceptionStatus.SecureChannelFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"使用 SSL 建立连接时出错");
                    case WebExceptionStatus.SendFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"无法与远程服务器发送一个完整的请求");
                    case WebExceptionStatus.ServerProtocolViolation:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"服务器响应不是有效的 HTTP 响应");
                    case WebExceptionStatus.Timeout:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"请求的超时期限内未不收到任何响应");
                    case WebExceptionStatus.TrustFailure:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"无法验证服务器证书");
                    default:
                    //case WebExceptionStatus.UnknownError:
                        return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError,"发生未知类型的异常");
                }

                using (var response = e.Response)
                {
                    using (var receivedStream = response.GetResponseStream())
                    {
                        var streamReader = new StreamReader(receivedStream);
                        result = streamReader.ReadToEnd();
                    }
                    response.Close();
                }
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                LogRecorder.EndStepMonitor();
                return ApiValueResult<string>.ErrorResult(ErrorCode.NetworkError);
            }
            LogRecorder.MonitorTrace(result);
            try
            {
                if (string.IsNullOrWhiteSpace(result))
                {
                    return ApiValueResult<string>.ErrorResult(ErrorCode.UnknowError);
                }
                return ApiValueResult<string>.Succees(result);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ApiValueResult<string>.ErrorResult(ErrorCode.UnknowError);
            }
            finally
            {
                LogRecorder.EndStepMonitor();
            }
        }

        #endregion
    }
}