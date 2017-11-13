using System;
using System.Runtime.Remoting.Messaging;
using Agebull.Common;
using Agebull.Common.DataModel.Redis;
using GoodLin.Common.Redis;
using Newtonsoft.Json;
using Yizuan.Service.Api.OAuth;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// API调用上下文（流程中使用）
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiContext
    {
        /// <summary>
        /// 当前调用上下文
        /// </summary>
        [JsonProperty]
        private InternalCallContext _requestContext;

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        [JsonProperty]
        private ILoginUserInfo _user;

        /// <summary>
        /// 缓存当前上下文
        /// </summary>
        public void Cache()
        {
            using (var proxy = new RedisProxy())
            {
                proxy.Client.Set(RedisKeyBuilder.ToSystemKey("api", "ctx",this.Request.RequestId), this);
            }
        }


        /// <summary>
        /// 得到缓存的键
        /// </summary>
        public static string GetCacheKey(string requestId)
        {
            return RedisKeyBuilder.ToSystemKey("api", "ctx", requestId.Trim('$'));
        }
        /// <summary>
        /// 设置当前上下文
        /// </summary>
        /// <param name="context"></param>
        public static void SetContext(ApiContext context)
        {
            ContextHelper.LogicalSetData("ApiContext", context);
        }

        /// <summary>
        /// 设置当前用户
        /// </summary>
        /// <param name="customer"></param>
        public static void SetCustomer(ILoginUserInfo customer)
        {
            Current._user = customer;
        }

        /// <summary>
        /// 设置当前请求上下文
        /// </summary>
        /// <param name="context"></param>
        public static void SetRequestContext(InternalCallContext context)
        {
            Current._requestContext = context;
        }
        
        /// <summary>
        /// 当前实例对象
        /// </summary>
        public static ApiContext Current
        {
            get
            {
                var current = ContextHelper.LogicalGetData< ApiContext>("ApiContext");
                if (current != null)
                    return current;
                ContextHelper.LogicalSetData("ApiContext", current = new ApiContext());
                return current;
            }
        }

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        public static InternalCallContext RequestContext
        {
            get
            {
                return Current._requestContext;
            }
        }
        /// <summary>
        /// 检查上下文，如果信息为空，加入系统匿名用户上下文
        /// </summary>
        public static void TryCheckByAnymouse()
        {
            if (Current.Request == null)
            {
                Current._requestContext = new InternalCallContext
                {
                    RequestId = Guid.NewGuid()
                };
                Current._user = new LoginUserInfo
                {
                    UserId = -2,
                    Account = "Anymouse",
                    NickName = "匿名用户",
                    DeviceId = "*SYSTEM",
                    Os = "SYSTEM",
                    Browser = "SYSTEM",
                    LoginSystem = "None",
                    LoginType = 0
                };
            }
        }
        /// <summary>
        /// 当前调用的客户信息
        /// </summary>
        public static ILoginUserInfo Customer
        {
            get { return Current._user; }
        }

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        public InternalCallContext Request
        {
            get { return _requestContext; }
        }

        /// <summary>
        /// 当前调用的客户信息
        /// </summary>
        public ILoginUserInfo LoginUser
        {
            get { return _user; }
        }
    }
}