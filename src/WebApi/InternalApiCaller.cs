using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Agebull.Common.Logging;
using Newtonsoft.Json;
using System.Web;
using GoodLin.Common.Configuration;

namespace Yizuan.Service.Api.WebApi
{

    /// <summary>
    /// 内部服务调用代理
    /// </summary>
    public class InternalApiCaller
    {
        /// <summary>
        /// 内部访问器
        /// </summary>
        public static readonly WebApiCaller Caller = new WebApiCaller
        {
            Host = GlobalVariable.OAuthServiceURL
        };

        #region 有返回值


        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Get<TResult>(string url, Dictionary<string, string> arguments)
            where TResult : IApiResultData
        {
            return Caller.Get<TResult>(url, arguments);
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Get<TResult>(string url, IApiArgument arguments)
            where TResult : IApiResultData
        {
            return Caller.Get<TResult>(url, arguments);
        }
        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Get<TResult>(string url)
            where TResult : IApiResultData
        {
            return Caller.Get<TResult>(url);
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Get<TResult>(string url, string arguments)
            where TResult : IApiResultData
        {
            return Caller.Get<TResult>(url, arguments);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Post<TResult>(string url, IApiArgument argument)
            where TResult : IApiResultData
        {
            return Caller.Post<TResult>(url, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Post<TResult>(string url, Dictionary<string, string> argument)
            where TResult : IApiResultData
        {
            return Caller.Post<TResult>(url, argument);
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ApiResult<TResult> Post<TResult>(string url, string form)
            where TResult : IApiResultData
        {
            return Caller.Post<TResult>(url, form);
        }

        #endregion


        #region 有返回值


        /// <summary>
        /// 通过Get调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult Get(string url, Dictionary<string, string> arguments)
            
        {
            return Caller.Get(url, arguments);
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult Get(string url, IApiArgument arguments)
            
        {
            return Caller.Get(url, arguments);
        }
        /// <summary>
        /// 通过Get调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <returns></returns>
        public static ApiResult Get(string url)
            
        {
            return Caller.Get(url);
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ApiResult Get(string url, string arguments)
            
        {
            return Caller.Get(url, arguments);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ApiResult Post(string url, IApiArgument argument)
            
        {
            return Caller.Post(url, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ApiResult Post(string url, Dictionary<string, string> argument)
            
        {
            return Caller.Post(url, argument);
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static ApiResult Post(string url, string form)
            
        {
            return Caller.Post(url, form);
        }

        #endregion
    }

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
            get { return _host; }
            set
            {
                _host = value?.TrimEnd('/') + "/";
            }
        }

        private string _beare;

        /// <summary>
        /// 主机
        /// </summary>
        public string Bearer
        {
            get { return _beare ?? (ApiContext.IsClientTest? "*TEST_CLIENT":ApiContext.RequestContext == null ? null : JsonConvert.SerializeObject(ApiContext.RequestContext)); }
            set
            {
                 _beare = value;
            }
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

        #region 强类型取得


        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string url)
            where TResult : IApiResultData
        {
            return Get<TResult>(url, "");
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string url, Dictionary<string, string> arguments)
            where TResult : IApiResultData
        {
            return Get<TResult>(url, FormatParams(arguments));
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string url, IApiArgument arguments)
            where TResult : IApiResultData
        {
            return Get<TResult>(url, arguments?.ToFormString());
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiResult<TResult> Get<TResult>(string url, string arguments)
            where TResult : IApiResultData
        {
            LogRecorder.BeginStepMonitor("内部API调用"+ Host + url);
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + arguments);

            if (!string.IsNullOrWhiteSpace(arguments))
                url = $"{url}?{arguments}";

            var req = (HttpWebRequest)WebRequest.Create(Host + url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add(HttpRequestHeader.Authorization, ctx);

            return GetResult<TResult>(req);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string url, IApiArgument argument)
            where TResult : IApiResultData
        {
            return Post<TResult>(url, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string url, Dictionary<string, string> argument)
            where TResult : IApiResultData
        {
            return Post<TResult>(url, FormatParams(argument));
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ApiResult<TResult> Post<TResult>(string url, string form)
            where TResult : IApiResultData
        {
            LogRecorder.BeginStepMonitor("内部API调用"+ Host + url);
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + form);

            var req = (HttpWebRequest)WebRequest.Create(Host + url);
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
        /// <param name="url"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string url)
        {
            return Get(url, "");
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string url, Dictionary<string, string> arguments)
        {
            return Get(url, FormatParams(arguments));
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string url, IApiArgument arguments)
        {
            return Get(url, arguments?.ToFormString());
        }

        /// <summary>
        /// 通过Get调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ApiValueResult<string> Get(string url, string arguments)
        {
            LogRecorder.BeginStepMonitor("内部API调用"+ Host + url);
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + arguments);

            if (!string.IsNullOrWhiteSpace(arguments))
                url = $"{url}?{arguments}";

            var req = (HttpWebRequest)WebRequest.Create(Host + url);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add(HttpRequestHeader.Authorization, ctx);

            return GetResult(req);
        }
        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string url, IApiArgument argument)
        {
            return Post(url, argument?.ToFormString());
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string url, Dictionary<string, string> argument)
        {
            return Post(url, FormatParams(argument));
        }

        /// <summary>
        /// 通过Post调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ApiValueResult<string> Post(string url, string form)
        {
            LogRecorder.BeginStepMonitor("内部API调用"+ Host + url);
            
            var ctx =string.IsNullOrEmpty(Bearer)? null: $"Bearer {Bearer}";
            LogRecorder.MonitorTrace(ctx);
            LogRecorder.MonitorTrace("Arguments:" + form);

            var req = (HttpWebRequest)WebRequest.Create(Host + url);
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