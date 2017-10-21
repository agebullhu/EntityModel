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
            get => _beare ?? (ApiContext.IsClientTest? "*TEST_CLIENT":ApiContext.RequestContext == null ? null : JsonConvert.SerializeObject(ApiContext.RequestContext));
            set => _beare = value;
        }

        /// <summary>
        /// 参数格式化
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private  string FormatParams(Dictionary<string, string> httpParams)
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
            LogRecorder.BeginStepMonitor("内部API调用"+ ToUrl(apiName));
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
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
            LogRecorder.BeginStepMonitor("内部API调用"+ ToUrl(apiName));
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
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
                try
                {
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
                catch (Exception exception)
                {
                    LogRecorder.Exception(exception);
                    LogRecorder.EndStepMonitor();
                    return ApiResult<TResult>.ErrorResult(ErrorCode.NetworkError);
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
            LogRecorder.BeginStepMonitor("内部API调用"+ ToUrl(apiName));
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
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
            LogRecorder.BeginStepMonitor("内部API调用"+ ToUrl(apiName));
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
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