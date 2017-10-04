// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-24
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using Agebull.Common.DataModel;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     为业务处理上下文对象
    /// </summary>
    public class BusinessContext : IDisposable
    {
        #region 全局消息

        private readonly StringBuilder _messageBuilder = new StringBuilder();

        private string _lastMessage;

        /// <summary>
        ///     最后一个的操作消息
        /// </summary>
        public string LastMessage
        {
            get => _lastMessage;
            set
            {
                if (string.Equals(_lastMessage, value, StringComparison.OrdinalIgnoreCase))
                    return;
                _lastMessage = value;
                if (_messageBuilder.Length > 0)
                    _messageBuilder.Append("；");
                _messageBuilder.Append(value);
            }
        }
        /// <summary>
        /// 取所有消息
        /// </summary>
        /// <returns></returns>
        public string GetFullMessage()
        {
            return _messageBuilder.ToString();
        }
        #endregion 

        #region 依赖对象字典

        /// <summary>
        ///     依赖对象字典
        /// </summary>
        public DependencyObjects DependencyObjects { get; } = new DependencyObjects();

        #endregion

        #region 权限对象
        
        /// <summary>
        ///     是否工作在系统模式下
        /// </summary>
        public bool IsSystemMode { get;set; }

        /// <summary>
        ///     后期注入的生成IPowerChecker对象的方法
        /// </summary>
        public static Func<IPowerChecker> CreatePowerChecker { private get; set; }

        /// <summary>
        ///     未认证用户
        /// </summary>
        public static ILoginUser Anymouse
        {
            get;
            set;
        }

        /// <summary>
        ///     系统用户
        /// </summary>
        public static ILoginUser SystemUser
        {
            get;
            set;
        }
        /// <summary>
        ///     当前登录的用户
        /// </summary>
        public ILoginUser LoginUser
        {
            get { return IsSystemMode ? SystemUser  : _loginUser ?? Anymouse; }
            set { _loginUser = value; }
        }

        /// <summary>
        ///     当前登录的用户ID
        /// </summary>
        public int LoginUserId => LoginUser?.Id ?? -1;

        /// <summary>
        ///     当前页面节点配置
        /// </summary>
        public IPageItem PageItem { get; set; }

        /// <summary>
        ///     权限校验对象
        /// </summary>
        private IPowerChecker _powerChecker;


        /// <summary>
        ///     权限校验对象
        /// </summary>
        public IPowerChecker PowerChecker => _powerChecker ?? (_powerChecker = CreatePowerChecker());

        /// <summary>
        ///     用户的角色权限
        /// </summary>
        private List<IRolePower> _powers;

        /// <summary>
        ///     用户的角色权限
        /// </summary>
        public List<IRolePower> Powers => _powers ?? (_powers = PowerChecker.LoadUserPowers(LoginUser));

        /// <summary>
        /// 当前页面权限设置
        /// </summary>
        public IRolePower CurrentPagePower
        {
            get;
            set;
        }
        /// <summary>
        /// 在当前页面检查是否可以执行操作
        /// </summary>
        /// <param name="action">操作</param>
        /// <returns></returns>
        public bool CanDoCurrentPageAction(string action)
        {
            return PowerChecker == null || PowerChecker.CanDoAction(LoginUser, PageItem, action);
        }

        /// <summary>
        /// 用户令牌
        /// </summary>
        public Guid Tooken { get; set; }

        /// <summary>
        /// 用户令牌是否保存在COOKIE中;
        /// </summary>
        public bool WorkByCookie { get; set; }

        #endregion

        #region 线程单例

        /// <summary>
        ///     线程单例对象
        /// </summary>
        [ThreadStatic] internal static BusinessContext _current;

        /// <summary>
        ///     取得或设置线程单例对象，当前对象不存在时，会自动构架一个
        /// </summary>
        public static BusinessContext Current
        {
            get => _current ?? CreateContext();
            internal set => _current = value;
        }

        #endregion

        #region 构造与析构

        /// <summary>
        ///     构造
        /// </summary>
        public BusinessContext()
        {
            _current = this;
            LogRecorder.MonitorTrace("BusinessContext.ctor");
        }

        /// <summary>
        ///     取得当前的上下文对象
        /// </summary>
        /// <returns>如果当前的上下文对象为null,则为null</returns>
        internal static BusinessContext GetCurrentContext()
        {
            return _current;
        }

        /// <summary>
        ///     构建一个上下文对象方法的后期注入
        /// </summary>
        /// <returns>上下文对象</returns>
        public static Func<BusinessContext> CreateFunc;

        /// <summary>
        ///     构建一个上下文对象
        /// </summary>
        /// <returns>上下文对象</returns>
        public static BusinessContext CreateContext()
        {
            return CreateFunc != null ? CreateFunc() : new BusinessContext();
        }

        /// <summary>
        ///     析构
        /// </summary>
        ~BusinessContext()
        {
            DoDispose();
        }

        /// <summary>
        ///     是否正确析构的标记
        /// </summary>
        private bool _isDisposed;

        private ILoginUser _loginUser;

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            DoDispose();
        }

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        private void DoDispose()
        {
            if (_isDisposed)
            {
                return;
            }
            GC.ReRegisterForFinalize(this);
            TransactionScope.EndAll();
            LogRecorder.MonitorTrace("BusinessContext.DoDispose");
            _isDisposed = true;
            if (_current == this)
            {
                _current = null;
            }
        }

        #endregion
    }
}