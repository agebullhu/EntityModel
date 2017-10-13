using System;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// API调用上下文（流程中使用）
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiContext
    {
        /// <summary>
        /// 是否客户端模拟
        /// </summary>
        public static bool IsClientTest { get; set; }

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        private InternalCallContext _requestContext;

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        private IUserProfile _user;

        /// <summary>
        /// 设置当前上下文
        /// </summary>
        /// <param name="context"></param>
        public static void SetContext(ApiContext context)
        {
            CallContext.LogicalSetData("ApiContext", context);
        }

        /// <summary>
        /// 设置当前用户
        /// </summary>
        /// <param name="customer"></param>
        public static void SetCustomer(IUserProfile customer)
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
                var current = CallContext.LogicalGetData("ApiContext") as ApiContext;
                if (current != null)
                    return current;
                CallContext.LogicalSetData("ApiContext", current = new ApiContext());
                return current;
            }
        }

        /// <summary>
        /// 当前调用上下文
        /// </summary>
        public static InternalCallContext RequestContext
        {
            get { return Current._requestContext; }
        }

        /// <summary>
        /// 当前调用的客户信息
        /// </summary>
        public static IUserProfile Customer
        {
            get { return Current._user; }
        }
    }
}