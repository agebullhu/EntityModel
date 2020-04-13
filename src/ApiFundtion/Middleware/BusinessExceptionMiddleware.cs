using Agebull.Common.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Messages;
using ZeroTeam.MessageMVC.Services;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    /// 日志处理中间件
    /// </summary>
    public class BusinessExceptionMiddleware : IMessageMiddleware
    {
        /// <summary>
        /// 当前处理器
        /// </summary>
        public MessageProcessor Processor { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        int IMessageMiddleware.Level => 0;

        /// <summary>
        /// 消息中间件的处理范围
        /// </summary>
        MessageHandleScope IMessageMiddleware.Scope => MessageHandleScope.Exception;

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="service">当前服务</param>
        /// <param name="message">当前消息</param>
        /// <param name="tag">扩展信息</param>
        /// <returns></returns>
        Task IMessageMiddleware.OnGlobalException(IService service, IInlineMessage message, object tag)
        {
            LogRecorder.Exception(message.RuntimeStatus.Exception);
            LogRecorder.MonitorTrace(() => $"发生未处理异常.{message.RuntimeStatus.Exception.Message}");

            CheckException(message, message.RuntimeStatus.Exception);

            return Task.CompletedTask;
        }

        private static void CheckException(IInlineMessage message, Exception exception)
        {
            switch (exception)
            {
                case ArgumentException _:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.ArgumentError, $"参数错误.{exception.Message}");
                    break;
                case DbException _:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.Ignore, $"数据库异常.{exception.Message}");
                    break;
                case NotSupportedException _:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.Ignore, $"不支持的方法.{exception.Message}");
                    break;
                case NullReferenceException _:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.Ignore, $"空引用错误.{exception.Message}");
                    break;
                case SystemException _:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.Ignore, $"系统级错误.{exception.Message}");
                    break;
                case MessageBusinessException mbe:
                    CheckException(message, mbe.InnerException);
                    break;
                default:
                    message.ResultData = ApiResultHelper.Error(DefaultErrorCode.UnhandleException, $"未知错误.{exception.Message}");
                    break;
            }
        }
    }
}