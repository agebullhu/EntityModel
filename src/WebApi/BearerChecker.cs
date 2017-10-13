using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Logging;
using GoodLin.Common.Configuration;
using GoodLin.Common.Ioc;
using GoodLin.OAuth.Api;
using Newtonsoft.Json;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// 身份检查器
    /// </summary>
    public class BearerHandler : DelegatingHandler
    {
        static BearerHandler()
        {
            LogRecorder.GetRequestIdFunc = () => ApiContext.RequestContext?.RequestId ?? Guid.NewGuid();
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _request = request;
            int code = Check();
            //校验不通过，直接返回，不做任何处理
            if (code != 0)
            {
                LogRecorder.MonitorTrace("Authorization：校验出错");
                return Task.Factory.StartNew(() =>
                {
                    var result = code == ErrorCode.Auth_Device_Unknow ? ApiResult.Error(code, "*" + Guid.NewGuid().ToString("N")) : ApiResult.Error(code);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                }, cancellationToken);
            }
            var tid = ApiContext.RequestContext.ThreadId;
            //正常结束时清理调用上下文
            var resultTask = base.SendAsync(request, cancellationToken);
            resultTask.ContinueWith((task, state) =>
            {
                Debug.Assert(ApiContext.RequestContext.ThreadId == tid);
                CallContext.LogicalSetData("ApiContext", null);

            }, null, cancellationToken);

            return resultTask;
        }

        /// <summary>
        /// 请求对象
        /// </summary>
        private HttpRequestMessage _request;

        /// <summary>
        /// 令牌
        /// </summary>
        private string _token;
        /// <summary>
        /// 执行检查
        /// </summary>
        /// <returns>
        /// 0:表示通过验证，可以继续
        /// 1：令牌为空或不合格
        /// 2：令牌是伪造的
        /// </returns>
        private int Check()
        {
            _token = ExtractToken();
            if (string.IsNullOrWhiteSpace(_token))
                return ErrorCode.Auth_Device_Unknow;
            
            ApiContext.SetRequestContext(new InternalCallContext
            {
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = -2
            });
            _token = _token.Trim();
            switch (_token[0])
            {
                case '*':
                    return CheckDeviceId();
                case '{':
                    return CheckServiceKey();
                case '#':
                    return CheckAccessToken();
            }
            return ErrorCode.Auth_Device_Unknow;
        }
        /// <summary>
        /// 检查设备标识
        /// </summary>
        /// <returns>
        /// 0:表示通过验证，可以继续
        /// 1：令牌为空
        /// 2：令牌是伪造的
        /// </returns>
        int CheckDeviceId()
        {
            var checker = IocHelper.Create<IBearValidater>();
            IApiResult<IUserProfile> result;
            try
            {
                result = checker.ValidateDeviceId(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_Device_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;
            CreateApiContext(result.ResultData);
            LogRecorder.MonitorTrace("Authorization：匿名用户");
            return 0;
        }
        /// <summary>
        /// 构造上下文
        /// </summary>
        /// <param name="customer"></param>
        private void CreateApiContext(IUserProfile customer)
        {
            //var ts = _request.Content.ReadAsStringAsync();
            //ts.Wait();
            ApiContext.SetCustomer(customer);
            ApiContext.SetRequestContext(new InternalCallContext
            {
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = customer?.UserId ?? -1,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            });
        }

        /// <summary>
        /// 检查旧标识
        /// </summary>
        /// <returns>
        /// 0:表示通过验证，可以继续
        /// 1：令牌为空
        /// 2：令牌是伪造的
        /// </returns>
        int CheckServiceKey()
        {
            InternalCallContext context;
            try
            {
                context = JsonConvert.DeserializeObject<InternalCallContext>(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_ServiceKey_Unknow;
            }
            if (context == null)
            {
                return ErrorCode.Auth_ServiceKey_Unknow;
            }
            context.ThreadId = Thread.CurrentThread.ManagedThreadId;
            var checker = IocHelper.Create<IBearValidater>();
            var result = checker.ValidateServiceKey(context.ServiceKey.ToString());
            if (!result.Result)
                return result.Status.ErrorCode;
            if (context.UserId > 0)
            {
                var user = checker.GetUserProfile(context.UserId);
                if (!user.Result)
                    return user.Status.ErrorCode;
                ApiContext.SetCustomer(user.ResultData);
                LogRecorder.MonitorTrace("Authorization："+user.ResultData.PhoneNumber);
            }
            else
            {
                ApiContext.SetCustomer(new UserProfile
                {
                    UserId = context.UserId,
                });
                LogRecorder.MonitorTrace("Authorization：匿名用户");
            }
            ApiContext.SetRequestContext(context);
            return 0;
        }

        /// <summary>
        /// 检查AccessToken
        /// </summary>
        /// <returns>
        /// 0:表示通过验证，可以继续
        /// 1：令牌为空
        /// 2：令牌是伪造的
        /// </returns>
        int CheckAccessToken()
        {
            var checker = IocHelper.Create<IBearValidater>();
            IApiResult<IUserProfile> result;
            try
            {
                result = checker.VerifyAccessToken(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_AccessToken_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;

            CreateApiContext(result.ResultData);

            LogRecorder.MonitorTrace("Authorization：" + result.ResultData.PhoneNumber);
            return 0;
        }

        /// <summary>
        /// 取请求头的身份验证令牌
        /// </summary>
        /// <returns></returns>
        private string ExtractToken()
        {
            const string bearer = "Bearer";
            var authz = _request.Headers.Authorization;
            if (authz != null)
                return string.Equals(authz.Scheme, bearer, StringComparison.OrdinalIgnoreCase) ? authz.Parameter : null;
            if (!_request.Headers.Contains("Authorization"))
                return null;
            string au = _request.Headers.GetValues("Authorization").FirstOrDefault();
            if (au == null)
                return null;
            var aus = au.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (aus.Length < 2 || aus[0] != bearer)
                return null;
            return aus[1];
        }
    }
}