// 所在工程：Agebull.EntityModel
// 整理用户：agebull
// 建立时间：2012-08-13 5:35
// 整理时间：2018年5月16日 00:34:00
#region

using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Agebull.Common.Ioc;

#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   文本记录器
    /// </summary>
    public static partial class LogRecorder
    {
        #region 对象

        /// <summary>
        /// 是否用Task而非线程方式后台记录日志
        /// </summary>
        public static bool LogByTask { get; set; } = (ConfigurationManager.AppSettings["LogByTask"] ?? "False").ToLower() == "true";


        /// <summary>
        /// 是否将日志输出到控制台
        /// </summary>
        public static bool TraceToConsole { get; set; } = (ConfigurationManager.AppSettings["TraceToConsole"] ?? "False").ToLower() == "true";

        /// <summary>
        /// 是否开启跟踪日志
        /// </summary>
        public static bool LogMonitor { get; set; } = (ConfigurationManager.AppSettings["LogMonitor"] ?? "False").ToLower() == "true";

        /// <summary>
        /// 是否开启SQL日志
        /// </summary>
        public static bool LogDataSql { get; set; } = (ConfigurationManager.AppSettings["LogSql"] ?? "False").ToLower() == "true";

        /// <summary>
        /// 是否开启SQL日志
        /// </summary>
        public static LogLevel LogLevel { get; set; } = (LogLevel)Enum.Parse(typeof(LogLevel), ConfigurationManager.AppSettings["LogLevel"] ?? "Trace");

        /// <summary>
        ///   消息跟踪器
        /// </summary>
        public static ILogListener Listener { get; set; }


        /// <summary>
        ///   记录器
        /// </summary>
        public static ILogRecorder Recorder { get; private set; }

        /// <summary>
        ///  基础记录器
        /// </summary>
        public static TxtRecorder BaseRecorder { get; }

        /// <summary>
        /// 是否仅使用文本记录器
        /// </summary>
        private static bool _isTextRecorder;

        /// <summary>
        /// 正在记录日志（用于防止重入）
        /// </summary>
        [ThreadStatic]
        internal static bool InRecording;
        /// <summary>
        /// 日志记录状态
        /// </summary>
        public enum LogRecorderStatus
        {
            /// <summary>
            /// 无
            /// </summary>
            None,
            /// <summary>
            /// 完成初始化
            /// </summary>
            Initialized,
            /// <summary>
            /// 已关闭
            /// </summary>
            Shutdown
        }
        /// <summary>
        /// 日志状态
        /// </summary>
        public static LogRecorderStatus State { get; private set; }

        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<string> GetRequestIdFunc;

        /// <summary>
        /// 取请求ID
        /// </summary>
        public static string GetRequestId()
        {
            return GetRequestIdFunc?.Invoke() ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 取得当前用户方法
        /// </summary>
        public static Func<string> GetUserNameFunc;

        /// <summary>
        /// 取得当前用户
        /// </summary>
        public static string GetUserName()
        {
            return GetRequestIdFunc?.Invoke() ?? "Unknow";
        }
        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<string> GetMachineNameFunc;

        /// <summary>
        /// 取得当前机器
        /// </summary>
        public static string GetMachineName()
        {
            return GetRequestIdFunc?.Invoke() ?? "Local";
        }
        #endregion

        #region 基本流程

        /// <summary>
        ///   静态构造
        /// </summary>
        static LogRecorder()
        {
            _isTextRecorder = true;
            Recorder = BaseRecorder = new TxtRecorder();
            BaseRecorder.Initialize();
            _logThread = new Thread(WriteRecordLoop)
            {
                IsBackground = true,
                Priority = LogByTask ? ThreadPriority.Normal : ThreadPriority.BelowNormal
            };
            _logThread.Start();
        }

        private static Thread _logThread;
        /// <summary>
        ///   初始化
        /// </summary>
        public static void Initialize()
        {
            State = LogRecorderStatus.Initialized;
            var recorder = IocHelper.Create<ILogRecorder>();
            if (recorder == null)
                return;
            _isTextRecorder = false;
            Recorder = recorder;
            Recorder.Initialize();
        }

        /// <summary>
        ///   中止
        /// </summary>
        public static void Shutdown()
        {
            Recorder.Shutdown();
            if (!_isTextRecorder)
                BaseRecorder.Shutdown();
            State = LogRecorderStatus.Shutdown;
        }
        #endregion

        #region 支持

        /// <summary>
        ///   日志类型到文本
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static string TypeToString(LogType type, string def = null)
        {
            switch (type)
            {
                default:
                    return def ?? "None";
                case LogType.Plan:
                    return "Plan";
                case LogType.Trace:
                    return "Debug";
                case LogType.Message:
                    return "Message";
                case LogType.Warning:
                    return "Warning";
                case LogType.Error:
                    return "Error";
                case LogType.Exception:
                    return "Exception";
                case LogType.System:
                    return "System";
                case LogType.Login:
                    return "Login";
                case LogType.Request:
                    return "Request";
                case LogType.DataBase:
                    return "DataBase";
                case LogType.WcfMessage:
                    return "WcfMessage";
            }
        }
        /// <summary>
        /// 格式化消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="formatArgs"></param>
        /// <returns></returns>
        private static string FormatMessage(string message, object[] formatArgs)
        {
            if (message == null) return null;
            string msg;
            if (formatArgs == null || formatArgs.Length == 0)
            {
                msg = message;
            }
            else
            {
                msg = String.Format(message, formatArgs);
            }
            return msg;
        }
        /// <summary>
        ///   堆栈信息
        /// </summary>
        /// <param name="title"> 标题 </param>
        public static string StackTraceInfomation(string title = null)
        {
            return $@"{title}:
{new StackTrace()}";
        }

        #endregion

        #region 记录
        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void Trace(LogType type, string name, string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= LogLevel)
                Record(name, FormatMessage(message, formatArgs), type);
        }
        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Record(LogType type, string name, string message, params object[] formatArgs)
        {
            Record(name, type, null, message, formatArgs);
        }

        /// <summary>
        ///   记录一般日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        /// <param name="type"> 日志类型(SG) </param>
        public static void Record(string msg, LogType type = LogType.Message)
        {
            Record(type.ToString(), msg, type);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Record(string type, string message, params object[] formatArgs)
        {
            Record(type, LogType.None, null, message, formatArgs);
        }

        ///<summary>
        ///  记录数据日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordDataLog(string message, params object[] formatArgs)
        {
            Record("数据日志", LogType.DataBase, null, message, formatArgs);
        }

        ///<summary>
        ///  记录登录日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordLoginLog(string message, params object[] formatArgs)
        {
            Record("登录日志", LogType.Login, null, message, formatArgs);
        }

        ///<summary>
        ///  记录网络请求日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordRequestLog(string message, params object[] formatArgs)
        {
            Record("网络请求", LogType.Request, null, message, formatArgs);
        }

        ///<summary>
        ///  记录WCF消息日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordWcfLog(string message, params object[] formatArgs)
        {
            Record("WCF消息", LogType.WcfMessage, null, message, formatArgs);
        }

        ///<summary>
        ///  记录消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Message(string message, params object[] formatArgs)
        {
            Record("消息", LogType.Message, null, message, formatArgs);
        }
        /// <summary>
        ///   记录堆栈跟踪
        /// </summary>
        /// <param name="title"> 标题 </param>
        [Conditional("TRACE")]
        public static void RecordStackTrace(string title)
        {
            if (LogLevel.Trace >= LogLevel)
                Record("堆栈跟踪:" + title, StackTraceInfomation(title), LogType.Trace);
        }

        ///<summary>
        ///  写入调试日志
        ///</summary>
        ///<param name="recordStackTrace"> 记录堆栈信息吗 </param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void Trace(bool recordStackTrace, string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= LogLevel)
                RecordInner("调试", FormatMessage(message, formatArgs), LogType.Trace);
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        [Conditional("TRACE")]
        public static void Trace(string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= LogLevel)
                RecordInner("调试", FormatMessage(message, formatArgs), LogType.Trace);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="obj"> 记录对象 </param>
        [Conditional("TRACE")]
        public static void Trace(object obj)
        {
            if (LogLevel.Trace >= LogLevel)
                RecordInner("调试", obj == null ? "NULL" : obj.ToString(), LogType.Trace);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void SystemLog(string msg)
        {
            Record("系统", msg, LogType.System);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void SystemLog(string message, params object[] formatArgs)
        {
            Record("系统", FormatMessage(message, formatArgs), LogType.System);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void PlanLog(string msg)
        {
            Record("计划", msg, LogType.Plan);
        }

        ///<summary>
        ///  写入一般日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordMessage(string message, params object[] formatArgs)
        {
            Record("消息", LogType.Message, null, message, formatArgs);
        }

        ///<summary>
        ///  记录警告消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Warning(string message, params object[] formatArgs)
        {
            Record("警告", LogType.Warning, null, message, formatArgs);
        }

        ///<summary>
        ///  记录错误消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Error(string message, params object[] formatArgs)
        {
            Record("错误", LogType.Error, null, message, formatArgs);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="exception"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static void RecordException(Exception exception, out string message)
        {
            message = ExceptionMessage(exception);
            string msg = ExceptionInfomation(exception, null);
            string title = "异常";
            if (exception != null)
            {
                title = exception.Message;
            }
            Record(title, msg, LogType.Exception);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="ex"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static string Exception(Exception ex, string message = null)
        {
            Record("异常", ExceptionInfomation(ex, message), LogType.Exception);
            return ExceptionMessage(ex);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="ex"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static string Exception(Exception ex, string message, params object[] formatArgs)
        {
            Record(message, ExceptionInfomation(ex, FormatMessage(message, formatArgs)), LogType.Exception);
            return ExceptionMessage(ex);
        }
        #endregion

        #region 异常
        /// <summary>
        ///   记录异常的外部信息
        /// </summary>
        /// <param name="ex"> 异常 </param>
        /// <returns> </returns>
        public static string ExceptionMessage(Exception ex)
        {
            switch (ex)
            {
                case null:
                    return "发生未处理异常";
                case SystemException _:
                    return $"发生系统错误,系统标识:{GetRequestId()}";
                case AgebullSystemException _:
                    return $"发生内部错误,系统标识:{GetRequestId()}";
                case BugException _:
                    return $"发生设计错误,系统标识:{GetRequestId()}";
                case AgebullBusinessException _:
                    return String.Format("发生业务逻辑错误,内容为:{1},系统标识:{0}", GetRequestId(), ex.Message);
            }

            return $"发生未知错误,系统标识:{GetRequestId()}";
        }

        /// <summary>
        ///   记录异常的详细信息
        /// </summary>
        /// <param name="ex"> </param>
        /// <param name="message"> </param>
        /// <returns> </returns>
        public static string ExceptionInfomation(Exception ex, string message)
        {
            StringBuilder sb = new StringBuilder();
            if (ex != null)
            {
                switch (ex)
                {
                    case AgebullSystemException _:
                        sb.AppendLine("系统致命错误");
                        break;
                    case BugException _:
                        sb.AppendLine("存在设计缺陷");
                        break;
                    case AgebullBusinessException _:
                        sb.AppendLine("业务逻辑错误");
                        break;
                    case SystemException _:
                        sb.AppendLine("系统错误");
                        break;
                    default:
                        sb.AppendLine("发生异常");
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(message))
                sb.AppendLine(message);
            sb.Append(ex);
            return sb.ToString();
        }
        #endregion

        #region 写入

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="name"> 原始的消息 </param>
        /// <param name="msg"> 处理后的消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="typeName"> 类型名称 </param>
        /// <param name="formatArgs">文本格式化参数</param>
        private static void Record(string name, LogType type, string typeName, string msg, params object[] formatArgs)
        {
            LogLevel level = LogLevel.None;
            switch (type)
            {
                case LogType.Monitor:
                    if (LogMonitor)
                        level = LogLevel.System;
                    break;
                case LogType.DataBase:
                    if (LogDataSql)
                        level = LogLevel.System;
                    break;
                case LogType.Login:
                case LogType.WcfMessage:
                case LogType.Message:
                case LogType.Request:
                case LogType.Trace:
                    level = LogLevel.Trace;
                    break;
                case LogType.Warning:
                    level = LogLevel.Warning;
                    break;
                case LogType.Exception:
                case LogType.Error:
                    level = LogLevel.Error;
                    break;
                case LogType.System:
                case LogType.Plan:
                    level = LogLevel.System;
                    break;
            }
            if (level >= LogLevel)
                RecordInner(name, FormatMessage(msg, formatArgs), type, typeName);
        }

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="name"> 原始的消息 </param>
        /// <param name="msg"> 处理后的消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="typeName"> 类型名称 </param>
        private static void Record(string name, string msg, LogType type, string typeName = null)
        {
            LogLevel level = LogLevel.None;
            switch (type)
            {
                case LogType.Monitor:
                    if (LogMonitor)
                        level = LogLevel.System;
                    break;
                case LogType.DataBase:
                    if (LogDataSql)
                        level = LogLevel.System;
                    break;
                case LogType.Login:
                case LogType.WcfMessage:
                case LogType.Message:
                case LogType.Request:
                case LogType.Trace:
                    level = LogLevel.Trace;
                    break;
                case LogType.Warning:
                    level = LogLevel.Warning;
                    break;
                case LogType.Exception:
                case LogType.Error:
                    level = LogLevel.Error;
                    break;
                case LogType.System:
                case LogType.Plan:
                    level = LogLevel.System;
                    break;
            }
            if (level >= LogLevel)
                RecordInner(name, msg, type, typeName);
        }


        #endregion

        #region 内部真实记录


        /// <summary>
        /// 待写入的日志信息集合
        /// </summary>
        private static MulitToOneQueue<RecordInfo> RecordInfos = new MulitToOneQueue<RecordInfo>();

        /// <summary>
        /// 日志序号
        /// </summary>
        private static ulong _id = 1;

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="name"> 原始的消息 </param>
        /// <param name="msg"> 处理后的消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="typeName"> 类型名称 </param>
        private static void RecordInner(string name, string msg, LogType type, string typeName = null)
        {
            if (type == LogType.None)
            {
                type = LogType.Trace;
            }
            RecordInfos.Push(new RecordInfo
            {
                RequestID = GetRequestId(),
                Machine = GetMachineName(),
                User = GetUserName(),
                Name = name,
                Type = type,
                Message = msg,
                Time = DateTime.Now,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                TypeName = typeName ?? TypeToString(type)
            });
        }

        /// <summary>
        ///  日志记录独立线程
        /// </summary>
        private static void WriteRecordLoop()
        {
            while (State != LogRecorderStatus.Shutdown)
            {
                if (!RecordInfos.StartProcess(out var info, 100))
                    continue;
                RecordInfos.EndProcess();
                info.Index = ++_id;
                if (_id == ulong.MaxValue)
                    _id = 1;
                try
                {
                    if (Listener != null)
                    {
                        Listener.Trace(info);
                    }
                    else if (TraceToConsole)
                    {
                        SystemTrace(info.Message);
                    }
                }
                catch (Exception ex)
                {
                    SystemTrace("日志侦听器发生错误", ex);
                }
                try
                {
                    if (InRecording)
                    {
                        BaseRecorder.RecordLog(info);
                        return;
                    }
                    if (!_isTextRecorder && info.Type > LogType.System)
                        BaseRecorder.RecordLog(info);
                    Recorder.RecordLog(info);
                }
                catch (Exception ex)
                {
                    SystemTrace("日志写入发生错误", ex);
                }
            }

            _logThread = null;
        }


        /// <summary>
        /// 写入系统跟踪
        /// </summary>
        /// <param name="arg"></param>
        [Conditional("TRACE")]
        public static void SystemTrace(object arg)
        {
            Console.WriteLine(arg);
        }

        /// <summary>
        /// 写入系统跟踪
        /// </summary>
        /// <param name="title"></param>
        /// <param name="arg"></param>
        [Conditional("TRACE")]
        public static void SystemTrace(string title, object arg)
        {
            Console.WriteLine($"{title}:{arg}");
        }

        #endregion
    }
}