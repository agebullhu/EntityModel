using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Agebull.Common.Configuration;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;

namespace Agebull.Common.Http
{
    /// <summary>
    ///     HTTP
    /// </summary>
    public class HttpCaller
    {
        #region 基本构造

        /// <summary>
        ///     主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Http Method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///     Authorization
        /// </summary>
        public string Authorization { get; set; }
        /// <summary>
        ///     ApiName
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        ///     Query
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> Query { get; set; }

        /// <summary>
        ///     Form
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> Form { get; set; }

        /// <summary>
        ///     Json
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        ///     远程地址
        /// </summary>
        internal string RemoteUrl;

        /// <summary>
        /// 远程请求对象
        /// </summary>
        internal HttpWebRequest RemoteRequest;

        /// <summary>
        /// 执行状态
        /// </summary>
        public HttpOperatorStateType UserState { get; set; }

        /// <summary>
        ///     Message
        /// </summary>
        public string Message { get;private set; }

        /// <summary>
        ///     ErrorCode
        /// </summary>
        public int Error { get; private set; }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成请求对象
        /// </summary>
        public void CreateRequest()
        {
            var url = new StringBuilder();
            url.Append($"{Host?.TrimEnd('/') + "/"}{ApiName?.TrimStart('/')}");

            if (Query != null && Query.Count > 0)
            {
                bool first = true;
                foreach (var kvp in Query)
                {
                    if (string.IsNullOrEmpty(kvp.Key) || kvp.Value == null)
                        continue;
                    if (first)
                    {
                        first = false;
                        url.Append('?');
                    }
                    else
                        url.Append('&');
                    url.Append($"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value, Encoding.UTF8)}");
                }
            }
            RemoteUrl = url.ToString();
            RemoteRequest = (HttpWebRequest)WebRequest.Create(RemoteUrl);
            RemoteRequest.Headers.Add(HttpRequestHeader.Authorization, Authorization);
            RemoteRequest.Timeout = ConfigurationManager.AppSettings.GetInt("httpTimeout", 30);
            RemoteRequest.Method = Method;
            RemoteRequest.KeepAlive = true;

            if (Form != null && Form.Count > 0)
            {
                RemoteRequest.ContentType = "application/x-www-form-urlencoded";
                var builder = new StringBuilder();
                bool first = true;
                foreach (var kvp in Form)
                {
                    if (string.IsNullOrEmpty(kvp.Key) || kvp.Value == null)
                        continue;
                    if (first)
                        first = false;
                    else
                        url.Append('&');
                    builder.Append($"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value, Encoding.UTF8)}");
                }

                using (var rs = RemoteRequest.GetRequestStream())
                {
                    var formData = Encoding.UTF8.GetBytes(builder.ToString());
                    rs.Write(formData, 0, formData.Length);
                }
            }
            else if (!string.IsNullOrWhiteSpace(Json))
            {
                RemoteRequest.ContentType = "application/json;charset=utf-8";
                var buffer = Json.ToUtf8Bytes();
                using (var rs = RemoteRequest.GetRequestStream())
                {
                    rs.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                RemoteRequest.ContentType = "application/x-www-form-urlencoded";
            }
        }

        /// <summary>
        ///     取返回值
        /// </summary>
        /// <returns></returns>
        public async Task<string> Call()
        {
            string jsonResult;
            UserState = HttpOperatorStateType.Success;
            //ZeroState = ZeroOperatorStateType.Ok;
            try
            {
                var resp = await RemoteRequest.GetResponseAsync();
                jsonResult = await ReadResponse(resp);
            }
            catch (WebException e)
            {
                LogRecorderX.Exception(e);
                jsonResult = e.Status == WebExceptionStatus.ProtocolError
                    ? await ProtocolError(e)
                    : ResponseError(e);
            }
            catch (Exception e)
            {
                UserState = HttpOperatorStateType.LocalException;
                //ZeroState = ZeroOperatorStateType.LocalException;
                LogRecorderX.Exception(e);
                jsonResult = ToErrorString(ErrorCode.LocalException, "未知错误", e.Message);
            }
            return jsonResult;
        }

        /// <summary>
        ///     协议错误
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task<string> ProtocolError(WebException exception)
        {
            try
            {
                var codes = exception.Message.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (codes.Length == 3)
                    if (int.TryParse(codes[1], out var s))
                        switch (s)
                        {
                            case 404:
                                UserState = HttpOperatorStateType.NotFind;
                                //ZeroState = ZeroOperatorStateType.NotFind;
                                return ToErrorString(ErrorCode.NetworkError, "页面不存在", "页面不存在");
                            case 503:
                                UserState = HttpOperatorStateType.Unavailable;
                                //ZeroState = ZeroOperatorStateType.Unavailable;
                                return ToErrorString(ErrorCode.NetworkError, "拒绝访问", "页面不存在");
                        }

                var msg = await ReadResponse(exception.Response);
                LogRecorderX.Error($"Call {Host}/{ApiName} Error:{msg}");
                return msg; //ToErrorString(ErrorCode.NetworkError, "未知错误", );
            }
            catch (Exception e)
            {
                UserState = HttpOperatorStateType.LocalException;
                //ZeroState = ZeroOperatorStateType.LocalException;
                LogRecorderX.Exception(e);
                return ToErrorString(ErrorCode.NetworkError, "未知错误", e.Message);
            }
            finally
            {
                exception.Response?.Close();
            }
        }

        private string ResponseError(WebException e)
        {
            e.Response?.Close();
            switch (e.Status)
            {
                case WebExceptionStatus.CacheEntryNotFound:
                    UserState = HttpOperatorStateType.NotFind;
                    //ZeroState = ZeroOperatorStateType.NotFind;
                    return ToErrorString(ErrorCode.NetworkError, "找不到指定的缓存项");
                case WebExceptionStatus.ConnectFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "在传输级别无法联系远程服务点");
                case WebExceptionStatus.ConnectionClosed:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "过早关闭连接");
                case WebExceptionStatus.KeepAliveFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "指定保持活动状态的标头的请求的连接意外关闭");
                case WebExceptionStatus.MessageLengthLimitExceeded:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "已收到一条消息的发送请求时超出指定的限制或从服务器接收响应");
                case WebExceptionStatus.NameResolutionFailure:
                    UserState = HttpOperatorStateType.NotFind;
                    //ZeroState = ZeroOperatorStateType.NotFind;
                    return ToErrorString(ErrorCode.NetworkError, "名称解析程序服务或无法解析主机名");
                case WebExceptionStatus.Pending:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "内部异步请求处于挂起状态");
                case WebExceptionStatus.PipelineFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "该请求是管线请求和连接被关闭之前收到响应");
                case WebExceptionStatus.ProxyNameResolutionFailure:
                    UserState = HttpOperatorStateType.NotFind;
                    //ZeroState = ZeroOperatorStateType.NotFind;
                    return ToErrorString(ErrorCode.NetworkError, "名称解析程序服务无法解析代理服务器主机名");
                case WebExceptionStatus.ReceiveFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "从远程服务器未收到完整的响应");
                case WebExceptionStatus.RequestCanceled:
                    UserState = HttpOperatorStateType.Unavailable;
                    //ZeroState = ZeroOperatorStateType.Unavailable;
                    return ToErrorString(ErrorCode.NetworkError, "请求已取消");
                case WebExceptionStatus.RequestProhibitedByCachePolicy:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "缓存策略不允许该请求");
                case WebExceptionStatus.RequestProhibitedByProxy:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "由该代理不允许此请求");
                case WebExceptionStatus.SecureChannelFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "使用 SSL 建立连接时出错");
                case WebExceptionStatus.SendFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "无法与远程服务器发送一个完整的请求");
                case WebExceptionStatus.ServerProtocolViolation:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "服务器响应不是有效的 HTTP 响应");
                case WebExceptionStatus.Timeout:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "请求的超时期限内未不收到任何响应");
                case WebExceptionStatus.TrustFailure:
                    UserState = HttpOperatorStateType.NetWorkError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "无法验证服务器证书");
                default:
                    UserState = HttpOperatorStateType.RemoteError;
                    //ZeroState = ZeroOperatorStateType.NetError;
                    return ToErrorString(ErrorCode.NetworkError, "内部服务器异常(未知错误)");
            }
        }

        /// <summary>
        ///     读取返回消息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<string> ReadResponse(WebResponse response)
        {
            string json;
            using (response)
            {
                if (response.ContentLength == 0)
                {
                    return null;
                }
                var receivedStream = response.GetResponseStream();
                if (receivedStream == null)
                    return null;

                using (receivedStream)
                {
                    using (var streamReader = new StreamReader(receivedStream))
                    {
                        json = await streamReader.ReadToEndAsync();
                        streamReader.Close();
                    }
                    receivedStream.Close();
                }
                response.Close();
            }
            return json;
        }

        /// <summary>
        ///     序列化到错误内容
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="message2"></param>
        /// <returns></returns>
        private string ToErrorString(int code, string message, string message2 = null)
        {
            Error = code;
            Message = $"调用异常：{message}.{message2}";
            LogRecorderX.MonitorTrace(Message);
            return Message;
        }
        #endregion
    }

    /// <summary>
    /// 一次路由执行状态
    /// </summary>
    public enum HttpOperatorStateType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Success,
        /// <summary>
        /// 非法格式
        /// </summary>
        FormalError,
        /// <summary>
        /// 逻辑错误
        /// </summary>
        LogicalError,
        /// <summary>
        /// 本地异常
        /// </summary>
        LocalException,
        /// <summary>
        /// 本地错误
        /// </summary>
        LocalError,
        /// <summary>
        /// 远程错误
        /// </summary>
        RemoteError,
        /// <summary>
        /// 远程异常
        /// </summary>
        RemoteException,
        /// <summary>
        /// 网络错误
        /// </summary>
        NetWorkError,
        /// <summary>
        /// 拒绝服务
        /// </summary>
        Unavailable,
        /// <summary>
        /// 未准备好
        /// </summary>
        NotReady,
        /// <summary>
        /// 页面不存在
        /// </summary>
        NotFind,
        /// <summary>
        /// 非法请求
        /// </summary>
        DenyAccess
    }
}