using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Agebull.Common.Base;
using Agebull.Common.DataModel;
using Agebull.Common.Ioc;
using Agebull.Common.OAuth;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;

namespace Agebull.Common.Rpc
{
    /// <summary>
    ///     全局上下文
    /// </summary>
    [DataContract]
    [Category("上下文")]
    [JsonObject(MemberSerialization.OptIn)]
    public class GlobalContext : ScopeBase, IGlobalContext
    {

        #region 依赖对象字典

        /// <summary>
        ///     依赖对象字典
        /// </summary>
        public DependencyObjects DependencyObjects { get; } = new DependencyObjects();

        #endregion


        #region 用户信息

        /// <summary>
        ///     当前登录的用户ID
        /// </summary>
        public long LoginUserId => _user == null ? -1 : User.UserId;

        /// <summary>
        ///     令牌
        /// </summary>
        public string Token => Request.Token;

        /// <summary>
        ///     当前调用的客户信息
        /// </summary>
        [JsonProperty("user")] private ILoginUserInfo _user;

        /// <summary>
        ///     当前调用的客户信息
        /// </summary>
        public static ILoginUserInfo Customer => Current.User;

        /// <summary>
        ///     当前调用的客户信息
        /// </summary>
        public ILoginUserInfo User => _user;

        /// <summary>
        ///     当前调用的组织信息
        /// </summary>
        [JsonProperty("org")] private IOrganizational _organizational;

        /// <summary>
        ///     当前调用的组织信息
        /// </summary>
        public IOrganizational Organizational => _organizational;

        #endregion

        #region 请求消息

        /// <summary>
        ///     当前调用上下文
        /// </summary>
        [JsonProperty("req")] private RequestInfo _requestInfo;

        /// <summary>
        ///     当前调用上下文
        /// </summary>
        public static RequestInfo RequestInfo => Current.Request;

        /// <summary>
        ///     当前调用上下文
        /// </summary>
        public RequestInfo Request => _requestInfo ?? (_requestInfo = new RequestInfo());

        #endregion

        #region 当前实例

        /// <summary>
        ///     当前线程的调用上下文
        /// </summary>
        [ThreadStatic] private static GlobalContext _current;

        /// <summary>
        ///     当前线程的调用上下文
        /// </summary>
        public static GlobalContext Current
        {
            get
            {
                if (_current != null)
                    return _current;
                var local = new AsyncLocal<GlobalContext>();
                if (local.Value != null)
                    return _current = local.Value;
                local.Value = _current = IocHelper.Create<GlobalContext>() ?? new GlobalContext();
                return _current;
            }
        }


        /// <summary>
        ///     内部构造
        /// </summary>
        protected GlobalContext()
        {
            _requestInfo = new RequestInfo(ServiceKey, $"{ServiceKey}-{Guid.NewGuid():N}");
            _user = LoginUserInfo.CreateAnymouse("<error>", "<error>", "<error>");
            _organizational = OrganizationalInfo.System;
        }

        /// <summary>
        ///     设置当前上下文（框架内调用，外部误用后果未知）
        /// </summary>
        /// <param name="context"></param>
        public static void SetContext(GlobalContext context)
        {
            if (Current == context)
                return;
            if (context._user != null)
                Current._user = context._user;
            if (context._requestInfo != null)
                Current._requestInfo = context._requestInfo;
        }

        /// <summary>
        ///     设置当前上下文（框架内调用，外部误用后果未知）
        /// </summary>
        public static void SetRequestContext(string globalId, string serviceKey, string requestId)
        {
            Current._requestInfo = new RequestInfo(globalId, serviceKey, requestId);
        }


        /// <summary>
        ///     设置当前上下文（框架内调用，外部误用后果未知）
        /// </summary>
        public static void SetRequestContext(string serviceKey, string requestId)
        {
            Current._requestInfo = new RequestInfo(serviceKey, requestId);
        }

        /// <summary>
        ///     设置当前上下文（框架内调用，外部误用后果未知）
        /// </summary>
        /// <param name="context"></param>
        public static void SetRequestContext(RequestInfo context)
        {
            Current._requestInfo = context;
        }

        /// <summary>
        ///     设置当前组织（框架内调用，外部误用后果未知）
        /// </summary>
        /// <param name="org"></param>
        public static void SetOrganizational(IOrganizational org)
        {
            Current._organizational = org;
        }

        /// <summary>
        ///     设置当前用户（框架内调用，外部误用后果未知）
        /// </summary>
        /// <param name="user"></param>
        public static void SetUser(ILoginUserInfo user)
        {
            Current._user = user;
        }

        /// <inheritdoc />
        protected override void OnDispose()
        {
            _current = null;
            var local = new AsyncLocal<GlobalContext>();
            if (local.Value != null)
                local.Value = null;
        }

        /// <summary>
        /// 检查上下文，如果信息为空，加入系统匿名用户上下文
        /// </summary>
        public static void TryCheckByAnymouse()
        {
            if (Current._requestInfo == null)
            {
                Current._requestInfo = new RequestInfo();
                Current._user = LoginUserInfo.CreateAnymouse("?", "*", "*");
            }
        }
        #endregion

        #region 全局状态

        /// <summary>
        ///     是否工作在不安全模式下
        /// </summary>
        public bool IsUnSafeMode { get; set; }

        /// <summary>
        ///     是否工作在系统模式下
        /// </summary>
        public bool IsSystemMode { get; set; }


        /// <summary>
        ///     最后操作的操作状态
        /// </summary>
        public int LastState
        {
            get => _status.ErrorCode;
            set => _status.ErrorCode = value;
        }

        private bool messageChanged;

        private StringBuilder _messageBuilder;

        /// <summary>
        ///     加入消息
        /// </summary>
        /// <param name="msg"></param>
        public void AppendMessage(string msg)
        {
            if (_messageBuilder == null) _messageBuilder = new StringBuilder(_status.ClientMessage);
            _messageBuilder.AppendLine(msg);
            messageChanged = true;
        }

        /// <summary>
        ///     重置状态
        /// </summary>
        public void ResetStatus()
        {
            _status.ErrorCode = ErrorCode.Success;
            _status.HttpCode = "200";
            ClearMessage();
        }

        /// <summary>
        ///     清除消息
        /// </summary>
        public void ClearMessage()
        {
            _messageBuilder = null;
            messageChanged = false;
            _status.ClientMessage = null;
            _status.InnerMessage = null;
        }

        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        public string LastMessage
        {
            get => _status.ClientMessage;
            set => _status.ClientMessage = value;
        }

        private readonly OperatorStatus _status = new OperatorStatus();

        /// <summary>
        ///     取所有消息
        /// </summary>
        /// <returns></returns>
        public string GetFullMessage()
        {
            return _messageBuilder?.ToString();
        }

        /// <summary>
        ///     最后状态(当前时间)
        /// </summary>
        public IOperatorStatus LastStatus
        {
            get
            {
                if (messageChanged)
                    _status.InnerMessage = _messageBuilder.ToString();
                return _status;
            }
        }

        #endregion

        #region 服务信息

        /// <summary>
        ///     当前服务器的标识
        /// </summary>
        /// <remarks>
        ///     服务注册时自动分配
        /// </remarks>
        public static string ServiceKey { get; set; }

        /// <summary>
        ///     当前服务器的名称
        /// </summary>
        /// <remarks>
        ///     但实际名称，会以服务器返回为准。
        /// </remarks>
        public static string ServiceName { get; set; }


        /// <summary>
        ///     当前服务器的运行时名称
        /// </summary>
        /// <remarks>
        ///     但实际名称，会以服务器返回为准。
        /// </remarks>
        public static string ServiceRealName { get; set; }

        #endregion
    }
}

namespace Agebull.Common.WebApi
{
    /// <summary>
    ///     全局上下文(版本兼容)
    /// </summary>
    [DataContract]
    [Category("上下文")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiContext : Rpc.GlobalContext
    {
    }
}
namespace Agebull.ZeroNet.Core
{
    /// <summary>
    ///     全局上下文(版本兼容)
    /// </summary>
    [DataContract]
    [Category("上下文")]
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiContext : Common.Rpc.GlobalContext
    {
    }
}