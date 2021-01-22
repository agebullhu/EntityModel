using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Messages;
using ZeroTeam.MessageMVC.Services;
using ZeroTeam.MessageMVC.ZeroApis;

namespace ZeroTeam.MessageMVC.ModelApi
{
    /// <summary>
    /// 业务异常中间件
    /// </summary>
    public class BusinessExceptionMiddleware : IMessageMiddleware
    {

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
        /// <param name="exception"></param>
        /// <param name="tag">扩展信息</param>
        /// <returns></returns>
        Task IMessageMiddleware.OnGlobalException(IService service, IInlineMessage message, Exception exception, object tag)
        {
            DependencyRun.Logger.Exception(exception);
            FlowTracer.MonitorInfomation(() => $"发生未处理异常.{exception.Message}");

            CheckException(message, exception);

            return Task.CompletedTask;
        }

        private static void CheckException(IInlineMessage message, Exception exception)
        {
            switch (exception)
            {
                case ArgumentException _:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.ArgumentError, $"参数错误.{exception.Message}");
                    break;
                case DbException _:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.Ignore, $"数据库异常.{exception.Message}");
                    break;
                case NotSupportedException _:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.Ignore, $"不支持的方法.{exception.Message}");
                    break;
                case NullReferenceException _:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.Ignore, $"空引用错误.{exception.Message}");
                    break;
                case SystemException _:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.Ignore, $"系统级错误.{exception.Message}");
                    break;
                case MessageBusinessException mbe:
                    CheckException(message, mbe.InnerException);
                    break;
                default:
                    message.ResultData = ApiResultHelper.State(OperatorStatusCode.UnhandleException, $"未知错误.{exception.Message}");
                    break;
            }
        }
    }
}