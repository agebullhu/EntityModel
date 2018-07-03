// 所在工程：Agebull.EntityModel
// 整理用户：agebull
// 建立时间：2012-08-13 5:35
// 整理时间：2018年5月16日 00:34:00
#region

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Ioc;
using Agebull.Common.Configuration;

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
        /// 是否将日志输出到控制台
        /// </summary>
        public static bool TraceToConsole { get; set; }

        /// <summary>
        /// 是否开启跟踪日志
        /// </summary>
        public static bool LogMonitor { get; set; }

        /// <summary>
        /// 是否开启SQL日志
        /// </summary>
        public static bool LogDataSql { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public static LogLevel Level { get; set; }

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
            return GetMachineNameFunc?.Invoke() ?? "Local";
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
            Task.Factory.StartNew(WriteRecordLoop);
        }

        /// <summary>
        ///   初始化
        /// </summary>
        public static void Initialize()
        {
            var sec = ConfigurationManager.Get("LogRecorder");
            if (sec != null)
            {
                TraceToConsole = sec.GetBool("console");
                LogMonitor = sec.GetBool("monitor");
                LogDataSql = sec.GetBool("sql");
                Level = Enum.TryParse<LogLevel>(sec["level"], out var level) ? level : LogLevel.Warning;
            }

            State = LogRecorderStatus.Initialized;
            var recorder = IocHelper.Create<ILogRecorder>();
            if (recorder != null && recorder != BaseRecorder)
            {
                _isTextRecorder = false;
                Recorder = recorder;
                Recorder.Initialize();
            }
            BaseRecorder.Initialize();
        }

        /// <summary>
        ///   中止
        /// </summary>
        public static void Shutdown()
        {
            State = LogRecorderStatus.Shutdown;
            syncSlim.Wait();
            if (RecordInfos.Line1.Count > 0)
                BaseRecorder.RecordLog(RecordInfos.Line1);
            if (RecordInfos.Line2.Count > 0)
                BaseRecorder.RecordLog(RecordInfos.Line2);

            if (!_isTextRecorder)
                BaseRecorder.Shutdown();
            Recorder.Shutdown();

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
                case LogType.Debug:
                    return "Debug";
                case LogType.Trace:
                    return "Trace";
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
                case LogType.NetWork:
                    return "NetWork";
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
            Record(type, LogType.Message, null, message, formatArgs);
        }

        ///<summary>
        ///  记录数据日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordDataLog(string message, params object[] formatArgs)
        {
            if (LogDataSql)
                Record("DataLog", LogType.DataBase, null, message, formatArgs);
        }

        ///<summary>
        ///  记录登录日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordLoginLog(string message, params object[] formatArgs)
        {
            Record("Login", LogType.Login, null, message, formatArgs);
        }

        ///<summary>
        ///  记录网络请求日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordRequestLog(string message, params object[] formatArgs)
        {
            Record("Request", LogType.Request, null, message, formatArgs);
        }

        ///<summary>
        ///  记录WCF消息日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void RecordNetLog(string message, params object[] formatArgs)
        {
            Record("NetWork", LogType.NetWork, null, message, formatArgs);
        }

        ///<summary>
        ///  记录消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Message(string message, params object[] formatArgs)
        {
            Record("Message", LogType.Message, null, message, formatArgs);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void SystemLog(string msg)
        {
            Record("System", msg, LogType.System);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void SystemLog(string message, params object[] formatArgs)
        {
            Record("System", FormatMessage(message, formatArgs), LogType.System);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void PlanLog(string msg)
        {
            Record("Plan", msg, LogType.Plan);
        }

        ///<summary>
        ///  写入一般日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordMessage(string message, params object[] formatArgs)
        {
            Record("Message", LogType.Message, null, message, formatArgs);
        }

        ///<summary>
        ///  记录警告消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Warning(string message, params object[] formatArgs)
        {
            Record("Warning", LogType.Warning, null, message, formatArgs);
        }

        ///<summary>
        ///  记录错误消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Error(string message, params object[] formatArgs)
        {
            Record("Error", LogType.Error, null, message, formatArgs);
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
            string title = "Exception";
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

        #region 跟踪

        /// <summary>
        ///   记录堆栈跟踪
        /// </summary>
        /// <param name="title"> 标题 </param>

        public static void RecordStackTrace(string title)
        {
            if (LogLevel.Trace >= Level)
                Record("StackTrace:", StackTraceInfomation(title), LogType.Trace);
        }

        ///<summary>
        ///  写入调试日志
        ///</summary>
        ///<param name="recordStackTrace"> 记录堆栈信息吗 </param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Trace(bool recordStackTrace, string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= Level)
                RecordInner(LogLevel.Trace, "Trace", FormatMessage(message, formatArgs), LogType.Trace);

        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Trace(string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= Level)
                RecordInner(LogLevel.Trace, "Trace", FormatMessage(message, formatArgs), LogType.Trace);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="obj"> 记录对象 </param>

        public static void Trace(object obj)
        {
            if (LogLevel.Trace >= Level)
                RecordInner(LogLevel.Trace, "Trace", obj == null ? "NULL" : obj.ToString(), LogType.Trace);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Trace(LogType type, string name, string message, params object[] formatArgs)
        {
            if (LogLevel.Trace >= Level)
                Record(name, FormatMessage(message, formatArgs), type);
        }

        #endregion

        #region 调试

        ///<summary>
        ///  写入调试日志
        ///</summary>
        ///<param name="recordStackDebug"> 记录堆栈信息吗 </param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Debug(bool recordStackDebug, string message, params object[] formatArgs)
        {
            if (LogLevel.Debug >= Level)
                RecordInner(LogLevel.Debug, "Debug", StackTraceInfomation(FormatMessage(message, formatArgs)), LogType.Debug);
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>

        public static void Debug(string message, params object[] formatArgs)
        {
            if (LogLevel.Debug >= Level)
                RecordInner(LogLevel.Debug, "Debug", FormatMessage(message, formatArgs), LogType.Debug);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="obj"> 记录对象 </param>

        public static void Debug(object obj)
        {
            if (LogLevel.Debug >= Level)
                RecordInner(LogLevel.Debug, "Debug", obj == null ? "NULL" : obj.ToString(), LogType.Debug);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="name"> </param>
        ///<param name="message"> 消息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>

        public static void Debug(LogType type, string name, string message, params object[] formatArgs)
        {
            if (LogLevel.Debug >= Level)
                Record(name, FormatMessage(message, formatArgs), type);
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
                case LogType.NetWork:
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
            RecordInner(level, name, FormatMessage(msg, formatArgs), type, typeName);
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
                case LogType.Debug:
                    level = LogLevel.Debug;
                    break;
                case LogType.DataBase:
                case LogType.Monitor:
                case LogType.Login:
                case LogType.NetWork:
                case LogType.Message:
                case LogType.Request:
                case LogType.Trace:
                case LogType.Plan:
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
                    level = LogLevel.System;
                    break;
            }
            RecordInner(level, name, msg, type, typeName);
        }


        #endregion

        #region 内部真实记录

        private static readonly SemaphoreSlim syncSlim = new SemaphoreSlim(1);

        /// <summary>
        /// 待写入的日志信息集合
        /// </summary>
        private static readonly LogQueue RecordInfos = new LogQueue();

        /// <summary>
        /// 日志序号
        /// </summary>
        private static ulong _id = 1;

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="level"> 所在等级 </param>
        /// <param name="name"> 原始的消息 </param>
        /// <param name="msg"> 处理后的消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="typeName"> 类型名称 </param>
        internal static void RecordInner(LogLevel level, string name, string msg, LogType type, string typeName = null)
        {
            if (level < Level)
                return;
            if (type == LogType.None)
            {
                type = LogType.Message;
            }

            RecordInfos.Push(new RecordInfo
            {
                Local = InRecording,
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
            bool doTrace = Listener != null || TraceToConsole;
            SystemTrace("日志开始");
            int cnt = 0;
            while (State != LogRecorderStatus.Shutdown)
            {
                //Thread.Sleep(10);//让子弹飞一会
                var items = RecordInfos.Switch();
                if (items.Count == 0)
                {
                    Thread.Sleep(50);
                    continue;
                }

                var array = items.ToList();
                items.Clear();
                foreach (var info in array)
                {
                    info.Index = ++_id;
                    if (_id == ulong.MaxValue)
                        _id = 1;
                    if (!_isTextRecorder && (info.Type >= LogType.System || info.Local))
                        BaseRecorder.RecordLog(info);
                    if (doTrace)
                        DoTrace(info);
                }
                try
                {
                    Recorder.RecordLog(array);
                }
                catch (Exception ex)
                {
                    SystemTrace("日志写入发生错误", ex);
                }
                if (++cnt == 24)
                {
                    GC.Collect();
                    cnt = 0;
                }
            }

            SystemTrace("日志结束");
            syncSlim.Release();
        }

        private static void DoTrace(RecordInfo info)
        {
            try
            {
                if (Listener != null)
                {
                    Listener.Trace(info);
                }
                else if (TraceToConsole)
                {
                    SystemTrace(info.TypeName, info.Message);
                }
            }
            catch (Exception ex)
            {
                SystemTrace("日志侦听器发生错误", ex);
            }
        }

        /// <summary>
        /// 写入系统跟踪
        /// </summary>
        /// <param name="title"></param>
        /// <param name="arg"></param>
        internal static void SystemTrace(string title, params object[] arg)
        {
            lock (BaseRecorder)
                Console.WriteLine($"[LogRecorder] {title} {arg.LinkToString(" > ")}");
        }

        #endregion
    }
}