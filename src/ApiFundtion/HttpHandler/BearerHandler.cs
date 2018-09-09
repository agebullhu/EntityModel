using Agebull.Common.DataModel.Redis;
using Agebull.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Ioc;
using Gboxt.Common.DataModel;
using Agebull.Common.WebApi.Auth;
using Agebull.Common.OAuth;
using Agebull.Common.Rpc;
using Agebull.Common.WebApi;

namespace Agebull.Common.WebApi.Auth
{
    /// <summary>
    ///     身份检查器
    /// </summary>
    public class BearerHandler : IHttpSystemHandler
	{
        HttpRequestMessage Request;
        /// <summary>
        ///     开始时的处理
        /// </summary>
        /// <returns>如果返回内容不为空，直接返回,后续的处理不再继续</returns>
        Task<HttpResponseMessage> IHttpSystemHandler.OnBegin(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;

            int code = Check(request);
			if (code == 0)
			{
				return null;
			}
			LogRecorder.MonitorTrace("Authorization：校验出错");
			return Task.Factory.StartNew(delegate
			{
				ApiResult apiResult = ApiResult.Error(code);
				return new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StringContent(JsonConvert.SerializeObject(apiResult))
				};
			}, cancellationToken);
		}

		/// <summary>
		///     结束时的处理
		/// </summary>
		void IHttpSystemHandler.OnEnd(HttpRequestMessage request, CancellationToken cancellationToken, HttpResponseMessage response)
		{
		}
        
		/// <summary>
		///     执行检查
		/// </summary>
		/// <returns>
		///     0:表示通过验证，可以继续
		///     1：令牌为空或不合格
		///     2：令牌是伪造的
		/// </returns>
		private int Check(HttpRequestMessage request)
		{
		    if (Request.RequestUri.LocalPath == "/v1/oauth/getdid")
		        return ErrorCode.Success;
		    if (Request.RequestUri.LocalPath == "/v1/sys/flush")
		        return ErrorCode.Success;
            var token = ExtractToken(request);
		    if (string.IsNullOrWhiteSpace(GlobalContext.Current.Token))
		        return ErrorCode.DenyAccess;
		    int state = ErrorCode.DenyAccess;
		    switch (GlobalContext.Current.Token[0])
            {
                case '{':
                    state = CheckServiceKey(token);
                    break;
                case '$':
                    state = RevertCallContext(token);
                    break;
                case '#':
                    GlobalContext.SetRequestContext(new RequestInfo(GlobalContext.ServiceKey, RandomOperate.Generate(8))
                    {
                        Token = token
                    });
                    state = CheckAccessToken(GlobalContext.Current.Token);
                    break;
                case '%':
                    GlobalContext.SetRequestContext(new RequestInfo(GlobalContext.ServiceKey, RandomOperate.Generate(8))
                    {
                        Token = token
                    });
                    state = 0;
                    break;
                default:
		            GlobalContext.SetRequestContext(new RequestInfo(GlobalContext.ServiceKey, RandomOperate.Generate(8))
                    {
		                Token = token
                    });
		            state = CheckDeviceId(token);
                    break;
            }

		    var page = Request.RequestUri.Segments[2];
		    BusinessContext.Context.PowerChecker.LoadAuthority(page);
            return state;
		}

		/// <summary>
		///     检查设备标识
		/// </summary>
		/// <returns>
		///     0:表示通过验证，可以继续
		///     1：令牌为空
		///     2：令牌是伪造的
		/// </returns>
		private int CheckDeviceId(string token)
		{
            if (Request.RequestUri.LocalPath == "/v1/oauth/getdid")
                return ErrorCode.Success;

            IBearValidater checker = IocHelper.Create<IBearValidater>();
			ApiResult<LoginUserInfo> result;
			try
			{
				result = checker.ValidateDeviceId(token);
			}
			catch (Exception ex)
			{
				LogRecorder.Exception(ex);
				return ErrorCode.Success;
			}
			if (!result.Success)
			{
				return result.Status.ErrorCode;
			}
			CreateApiContext(result.ResultData, token);
			LogRecorder.MonitorTrace("Authorization：匿名用户");
			return ErrorCode.Success;
		}

		/// <summary>
		///     构造上下文
		/// </summary>
		private void CreateApiContext(LoginUserInfo customer, string token)
		{
			GlobalContext.SetRequestContext(new RequestInfo 
			{
				Token = token
			});
            GlobalContext.SetUser(customer);
            BusinessContext.Context.Cache();
		}

		/// <summary>
		///     还原调用上下文
		/// </summary>
		private int RevertCallContext(string token)
		{
			GlobalContext context;
			using (RedisProxy proxy = new RedisProxy())
			{
				context = proxy.Get<GlobalContext>(BusinessContext.GetCacheKey(token));
			}
			if (context?.Request == null || context.User == null)
			{
				GlobalContext.TryCheckByAnymouse();
				return ErrorCode.Success;
			}
			ApiResult result = IocHelper.Create<IBearValidater>().ValidateServiceKey(context.Request.ServiceKey);
			if (!result.Success)
			{
				return result.Status.ErrorCode;
			}
			GlobalContext.SetContext(context);
		    BusinessContext.Context.Cache();
            return ErrorCode.Success;
		}

		/// <summary>
		///     检查旧标识
		/// </summary>
		/// <returns>
		///     0:表示通过验证，可以继续
		///     1：令牌为空
		///     2：令牌是伪造的
		/// </returns>
		private int CheckServiceKey(string token)
		{
			RequestInfo requestInfo;
			try
			{
			    requestInfo = JsonConvert.DeserializeObject<RequestInfo>(token);
			}
			catch (Exception ex)
			{
				LogRecorder.Exception(ex);
				return ErrorCode.Auth_ServiceKey_Unknow;
			}
			if (requestInfo == null)
			{
			    return ErrorCode.Auth_ServiceKey_Unknow;
			}
            IBearValidater checker = IocHelper.Create<IBearValidater>();
			ApiResult result = checker.ValidateServiceKey(requestInfo.ServiceKey);
			if (!result.Success)
			{
				return result.Status.ErrorCode;
			}
			ApiResult<LoginUserInfo> user = checker.GetLoginUser(requestInfo.Token);
			if (!user.Success)
			{
				return user.Status.ErrorCode;
			}
			GlobalContext.SetUser(user.ResultData);
			GlobalContext.SetRequestContext(requestInfo);
		    BusinessContext.Context.Cache();
			LogRecorder.MonitorTrace($"Authorization：{user.ResultData.Account}");
			return ErrorCode.Success;
		}

		/// <summary>
		///     检查AccessToken
		/// </summary>
		/// <returns>
		///     0:表示通过验证，可以继续
		///     1：令牌为空
		///     2：令牌是伪造的
		/// </returns>
		private int CheckAccessToken(string token)
		{
			IBearValidater checker = IocHelper.Create<IBearValidater>();
			ApiResult<LoginUserInfo> result;
			try
			{
				result = checker.VerifyAccessToken(token);
			}
			catch (Exception ex)
			{
				LogRecorder.Exception(ex);
			    return ErrorCode.Auth_AccessToken_Unknow;
			}
            if (!result.Success)
			{
				return result.Status.ErrorCode;
			}
			CreateApiContext(result.ResultData, token);
			LogRecorder.MonitorTrace("Authorization：" + result.ResultData.Account);
			return ErrorCode.Success;
		}

		/// <summary>
		///     取请求头的身份验证令牌
		/// </summary>
		/// <returns></returns>
		private static string ExtractToken(HttpRequestMessage request)
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
			string[] aus = au.Split(new[]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries);
			if (aus.Length < 2 || aus[0] != "Bearer")
			{
				return null;
			}
			return aus[1].Trim();
		}
	}
}
