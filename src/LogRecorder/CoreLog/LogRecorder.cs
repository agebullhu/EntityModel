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
    public static partial class LogRecorder
    {
        #region 配置
        /// <summary>
        /// 日志序号
        /// </summary>
        internal static long lastId = 1;

        /// <summary>
        ///     文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public static string LogPath { get; set; }

        /// <summary>
        /// 是否使用基础文本日志记录器
        /// </summary>
        public static bool UseBaseLogger { get; set; }

        /// <summary>
        /// 是否使用基础文本日志记录器
        /// </summary>
        public static bool UseConsoleLogger { get; set; }

        /// <summary>
        /// 不注册日志器
        /// </summary>
        public static bool NoRegist { get; set; }

        /// <summary>
        /// 是否开启跟踪日志
        /// </summary>
        public static bool LogMonitor { get; set; }

        /// <summary>
        /// 是否开启SQL日志
        /// </summary>
        public static bool LogDataSql { get; set; }

        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<string> GetRequestIdFunc;

        /// <summary>
        /// 取得当前用户方法
        /// </summary>
        public static Func<string> GetUserNameFunc;

        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<string> GetMachineNameFunc;

        #endregion

        #region 流程

        /// <summary>
        ///   静态构造
        /// </summary>
        static LogRecorder()
        {
            Initialize();
        }
        /// <summary>
        ///     初始化
        /// </summary>
        public static void Initialize()
        {
            ReadConfig();
            if (!NoRegist)
                IocHelper.ServiceCollection.AddLogging(builder =>
                {
                    builder.AddConfiguration(ConfigurationManager.Root.GetSection("Logging"));
                    if (UseConsoleLogger)
                        builder.AddConsole();
                    if (!UseBaseLogger)
                        return;
                    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TextLoggerProvider>());
                    LoggerProviderOptions.RegisterProviderOptions<TextLoggerOption, TextLoggerProvider>(builder.Services);

                });
            ConfigurationManager.RegistOnChange(ReadConfig, false);

            SystemLog("日志开始");
        }
        /// <summary>
        /// 读取配置
        /// </summary>
        private static void ReadConfig()
        {
            var sec = ConfigurationManager.Get("Logging");
            if (sec != null)
            {
                LogMonitor = sec.GetBool("monitor");
                LogDataSql = sec.GetBool("sql");
                UseBaseLogger = sec.GetBool("innerLogger");
                UseConsoleLogger = sec.GetBool("console");
                NoRegist = sec.GetBool("noRegist");
            }
#if !NETCOREAPP
            if (LogMonitor)
            {
                AppDomain.MonitoringIsEnabled = true;
            }
#endif
        }

        #endregion

        #region 支持

        /// <summary>
        /// 日志记录器
        /// </summary>
        public static ILogger Logger => IocScope.Logger;

        /// <summary>
        /// 取请求ID
        /// </summary>
        public static string GetRequestId()
        {
            return GetRequestIdFunc?.Invoke() ?? RandomOperate.Generate(8);
        }

        /// <summary>
        /// 取得当前用户
        /// </summary>
        public static string GetUserName()
        {
            return GetUserNameFunc?.Invoke() ?? "*";
        }
        /// <summary>
        /// 取得当前机器
        /// </summary>
        public static string GetMachineName()
        {
            return GetMachineNameFunc?.Invoke() ?? "Local";
        }

        /// <summary>
        ///   堆栈信息
        /// </summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static string StackTraceInfomation(string message, object[] formatArgs)
        {
            if (message == null)
            {
                return new StackTrace().ToString();
            }
            if (formatArgs != null && formatArgs.Length != 0)
            {
                message = string.Format(message, formatArgs);
            }
            return $"{message}:\r\n{new StackTrace()}";
        }

        #endregion

        #region 记录

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="type"> 消息类型 </param>
        /// <param name="name"> 消息名称 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void Record(LogType type, string name, string message, params object[] formatArgs)
        {
            try
            {
                var eventId = new EventId((int)Interlocked.Increment(ref lastId), name);
                switch (type)
                {
                    case LogType.DataBase:
                    case LogType.Monitor:
                        Logger.LogInformation(eventId, message, formatArgs);
                        break;
                    case LogType.Debug:
                        Logger.LogDebug(eventId, message, formatArgs);
                        break;
                    case LogType.Warning:
                        Logger.LogWarning(eventId, message, formatArgs);
                        break;
                    case LogType.System:
                    case LogType.Error:
                    case LogType.Exception:
                        Logger.LogError(eventId, message, formatArgs);
                        break;
                    default:
                        Logger.LogTrace(eventId, message, formatArgs);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        ///<summary>
        ///  记录数据日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordDataLog(string message, params object[] formatArgs)
        {
            if (LogDataSql)
            {
                var eventId = new EventId((int)Interlocked.Increment(ref lastId), "DataLog");
                Logger.LogTrace(eventId, message);
            }
        }

        ///<summary>
        ///  记录登录日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordLoginLog(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Login");
            Logger.LogTrace(eventId, message);
        }

        ///<summary>
        ///  记录网络请求日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordRequestLog(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Request");
            Logger.LogTrace(eventId, message);
        }

        ///<summary>
        ///  记录WCF消息日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordNetLog(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "NetWork");
            Logger.LogTrace(eventId, message);
        }

        ///<summary>
        ///  记录消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Message(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Message");
            Logger.LogInformation(eventId, message, formatArgs);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 消息 </param>
        public static void SystemLog(string message)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "System");
            Logger.LogInformation(eventId, message);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void SystemLog(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "System");
            Logger.LogInformation(eventId, message, formatArgs);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 消息 </param>
        public static void PlanLog(string message)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Plan");
            Logger.LogTrace(eventId, message);
        }

        ///<summary>
        ///  写入一般日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordMessage(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Message");
            Logger.LogInformation(eventId, message, formatArgs);
        }

        ///<summary>
        ///  记录警告消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Warning(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Warning");
            Logger.LogWarning(eventId, message, formatArgs);
        }

        ///<summary>
        ///  记录错误消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Error(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Error");
            Logger.LogError(eventId, message, formatArgs);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="exception"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static string Exception(Exception exception, string message = null)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Exception");
            Logger.LogError(eventId, exception, message);
            return exception.Message;
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="ex"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static string Exception(Exception ex, string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Exception");
            Logger.LogError(eventId, ex, message, formatArgs);
            return ex.Message;
        }
        #endregion

        #region 跟踪

        /// <summary>
        ///   记录堆栈跟踪
        /// </summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordStackTrace(string message, params object[] formatArgs)
        {
            if (MonitorTrace(() => StackTraceInfomation(message, formatArgs)))
                return;
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Trace");
            Logger.LogTrace(eventId, StackTraceInfomation(message, formatArgs));
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Trace(string message, params object[] formatArgs)
        {
            if (MonitorTrace(message, formatArgs))
                return;
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Trace");
            Logger.LogTrace(eventId, message, formatArgs);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Trace(string name, string message, params object[] formatArgs)
        {
            if (MonitorTrace(message, formatArgs))
                return;
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), name);
            Logger.LogTrace(eventId, message, formatArgs);
        }

        #endregion

        #region 调试

        ///<summary>
        ///  写入调试日志同时记录堆栈信息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void DebugByStackTrace(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Debug");
            Logger.LogDebug(eventId, StackTraceInfomation(message, formatArgs));
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Debug(string message, params object[] formatArgs)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Debug");
            Logger.LogDebug(eventId, message, formatArgs);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="obj"> 记录对象 </param>

        public static void Debug(object obj)
        {
            var eventId = new EventId((int)Interlocked.Increment(ref lastId), "Debug");
            Logger.LogDebug(eventId, obj?.ToString());
        }

        #endregion
    }
}