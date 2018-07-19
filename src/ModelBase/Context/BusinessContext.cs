// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-24
// // *****************************************************/

#region 引用

using System;
using System.Text;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     为业务处理上下文对象
    /// </summary>
    public class BusinessContext : IDisposable, IBusinessContext
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

        /// <summary>
        ///     最后一个的错误消息ID
        /// </summary>
        public int LastError
        {
            get;
            set;
        }

        #endregion

        #region 权限对象

        /// <summary>
        ///     是否工作在不安全模式下
        /// </summary>
        public bool IsUnSafeMode { get; set; }

        /// <summary>
        ///     是否工作在系统模式下
        /// </summary>
        public bool IsSystemMode { get; set; }

        /// <summary>
        ///     当前登录的用户
        /// </summary>
        public ILoginUser LoginUser
        {
            get;
            set;
        }

        /// <summary>
        ///     当前登录的用户ID
        /// </summary>
        public long LoginUserId => LoginUser?.Id ?? -1;

        /// <summary>
        /// 用户令牌
        /// </summary>
        public string UserToken { get; set; }


        #endregion

        #region 线程单例

        /// <summary>
        ///     线程单例对象
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [ThreadStatic] internal static IBusinessContext _current;

        /// <summary>
        ///     取得或设置线程单例对象，当前对象不存在时，会自动构架一个
        /// </summary>
        public static IBusinessContext Current
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
        internal static IBusinessContext GetCurrentContext()
        {
            return _current;
        }

        /// <summary>
        ///     构建一个上下文对象
        /// </summary>
        /// <returns>上下文对象</returns>
        internal static IBusinessContext CreateContext()
        {
            return IocHelper.Create<IBusinessContext>();
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

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            DoDispose();
            GC.ReRegisterForFinalize(this);
            if (_current == this)
            {
                _current = null;
            }
        }

        /// <summary>
        ///     执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <filterpriority>2</filterpriority>
        protected virtual void DoDispose()
        {
        }

        #endregion
    }
}