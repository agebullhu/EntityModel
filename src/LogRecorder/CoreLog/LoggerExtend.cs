// 所在工程：Agebull.EntityModel
// 整理用户：agebull
// 建立时间：2012-08-13 5:35
// 整理时间：2018年5月16日 00:34:00
#region

using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Diagnostics;

using System.Text;
using System.Threading;

#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   日志记录器
    /// </summary>
    public static class LoggerExtend
    {
        static EventId NewEventId(string name) => new EventId((int)Interlocked.Increment(ref LogRecorder.lastId), name);
        #region 记录

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="type"> 消息类型 </param>
        /// <param name="name"> 消息名称 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void Record(this ILogger logger, LogType type, string name, string message, params object[] formatArgs)
        {
            try
            {
                switch (type)
                {
                    case LogType.NetWork:
                    case LogType.Plan:
                    case LogType.Request:
                    case LogType.System:
                    case LogType.Login:
                        logger.LogCritical(NewEventId(name), message, formatArgs);
                        break;
                    case LogType.Warning:
                        logger.LogWarning(NewEventId(name), message, formatArgs);
                        break;
                    case LogType.Error:
                    case LogType.Exception:
                        logger.LogError(NewEventId(name), message, formatArgs);
                        break;
                    case LogType.DataBase:
                        logger.LogTrace(NewEventId(name), message, formatArgs);
                        break;
                    case LogType.Trace:
                    case LogType.Monitor:
                        logger.LogTrace(NewEventId(name), message, formatArgs);
                        break;
                    case LogType.Debug:
                        logger.LogDebug(NewEventId(name), message, formatArgs);
                        break;
                    default:
                        logger.LogInformation(NewEventId(name), message, formatArgs);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        ///<summary>
        ///  记录消息
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Message(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogInformation(NewEventId("Message"), message, formatArgs);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void SystemLog(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogCritical(NewEventId("System"), message, formatArgs);
        }

        ///<summary>
        ///  写入一般日志
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordMessage(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogInformation(NewEventId("Message"), message, formatArgs);
        }

        ///<summary>
        ///  记录警告消息
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Warning(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogWarning(NewEventId("Warning"), message, formatArgs);
        }

        ///<summary>
        ///  记录错误消息
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Error(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogError(NewEventId("Error"), message, formatArgs);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="exception"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static string Exception(this ILogger logger, Exception exception, string message = null)
        {
            logger.LogError(NewEventId("Exception"), exception, message);
            return exception.Message;
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="ex"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static string Exception(this ILogger logger, Exception ex, string message, params object[] formatArgs)
        {
            logger.LogError(NewEventId("Exception"), ex, message, formatArgs);
            return ex.Message;
        }
        #endregion

        #region 跟踪

        /// <summary>
        ///   记录堆栈跟踪
        /// </summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordStackTrace(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogTrace(NewEventId("Trace"), message, formatArgs);
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Trace(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogTrace(NewEventId("Trace"), message, formatArgs);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Trace(this ILogger logger, string name, string message, params object[] formatArgs)
        {
            logger.LogTrace(NewEventId(name), message, formatArgs);
        }

        #endregion

        #region 调试

        ///<summary>
        ///  写入调试日志同时记录堆栈信息
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void DebugByStackTrace(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogDebug(NewEventId("Debug"), LogRecorder.StackTraceInfomation(message, formatArgs));
        }

        /// <summary>
        ///   写入调试日志.
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Debug(this ILogger logger, string message, params object[] formatArgs)
        {
            logger.LogDebug(NewEventId("Debug"), message, formatArgs);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="obj"> 记录对象 </param>

        public static void Debug(this ILogger logger, object obj)
        {
            logger.LogDebug(NewEventId("Debug"), obj?.ToString());
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        /// <param name="logger">日志记录器</param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Debug(this ILogger logger, string name, string message, params object[] formatArgs)
        {
            logger.LogDebug(NewEventId(name), message, formatArgs);
        }

        #endregion
    }
}