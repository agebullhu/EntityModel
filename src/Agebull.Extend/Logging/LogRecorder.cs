// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
#if !CLIENT
using System.Data.SqlClient;
#endif
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using Agebull.Common.Frame;
using Agebull.Common.Reflection;
using Agebull.Common.Xml;

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
        ///   记录数据库访问日志
        /// </summary>
        public static bool RecorderDataBaseRequest { get; set; }

        /// <summary>
        ///   记录WCF消息日志
        /// </summary>
        public static bool RecorderWcfRequest { get; set; }

        /// <summary>
        ///   消息跟踪器
        /// </summary>
        public static ILogListener Listener { get; set; }

        /// <summary>
        ///   静态构造
        /// </summary>
        static LogRecorder()
        {
            Recorder = new TxtRecorder();
            Recorder.Initialize();
            var logThread = new Thread(WriteRecordLoop) { IsBackground = true, Priority = ThreadPriority.Lowest };
            logThread.Start();
        }

        /// <summary>
        ///   记录器
        /// </summary>
        public static ILogRecorder Recorder { get; private set; }

        private static bool IsTextRecorder;
        /// <summary>
        ///   初始化
        /// </summary>
        /// <param name="record"> </param>
        public static void Initialize(ILogRecorder record)
        {
            Recorder = record ?? new TxtRecorder();
            IsTextRecorder = record == null || record is TxtRecorder;
            Recorder.Initialize();
        }

        /// <summary>
        ///   中止
        /// </summary>
        public static void Shutdown()
        {
            Recorder.Shutdown();
        }

        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        public static Func<Guid> GetRequestIdFunc;

        /// <summary>
        /// 取请求ID的方法
        /// </summary>
        static Guid GetRequestId()
        {
            return GetRequestIdFunc?.Invoke() ?? Guid.NewGuid();
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

        static string FormatMessage(string message, object[] formatArgs)
        {
            string msg = null;
            if (message != null)
            {
                if (formatArgs == null || formatArgs.Length == 0)
                {
                    msg = message;
                }
                else
                {
                    msg = String.Format(message, formatArgs);
                }
            }
            return msg;
        }
        /// <summary>
        ///   堆栈信息
        /// </summary>
        /// <param name="title"> 标题 </param>
        public static string StackTraceInfomation(string title = null)
        {
            return String.Format(@"{0}:
{1}", title, new StackTrace());
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
            Record(GetRequestId(), name, FormatMessage(message, formatArgs), type);
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
            Record(GetRequestId(), name, FormatMessage(message, formatArgs), type);
        }

        /// <summary>
        ///   记录一般日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        /// <param name="type"> 日志类型(SG) </param>
        public static void Record(string msg, LogType type = LogType.Message)
        {
            Record(GetRequestId(), type.ToString(), msg, type);
        }

        ///<summary>
        ///  记录一般日志
        ///</summary>
        ///<param name="type"> 日志类型(SG) </param>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Record(string type, string message, params object[] formatArgs)
        {
            Record(GetRequestId(), type, FormatMessage(message, formatArgs), LogType.None, type);
        }

        ///<summary>
        ///  记录数据日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordDataLog(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "数据日志", FormatMessage(message, formatArgs), LogType.DataBase);
        }

        ///<summary>
        ///  记录登录日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordLoginLog(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "登录日志", FormatMessage(message, formatArgs), LogType.Login);
        }

        ///<summary>
        ///  记录网络请求日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordRequestLog(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "网络请求", FormatMessage(message, formatArgs), LogType.Request);
        }

        ///<summary>
        ///  记录WCF消息日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        [Conditional("TRACE")]
        public static void RecordWcfLog(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "WCF消息", FormatMessage(message, formatArgs), LogType.WcfMessage);
        }

        ///<summary>
        ///  记录消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Message(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "消息", FormatMessage(message, formatArgs), LogType.Message);
        }
        /// <summary>
        ///   记录堆栈跟踪
        /// </summary>
        /// <param name="title"> 标题 </param>
        [Conditional("TRACE")]
        public static void RecordStackTrace(string title)
        {
            Record(GetRequestId(), "堆栈跟踪:" + title, StackTraceInfomation(title), LogType.Trace);
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
            Record(GetRequestId(), "调试", FormatMessage(message, formatArgs), LogType.Trace);
        }

        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        [Conditional("TRACE")]
        public static void Trace(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "调试", FormatMessage(message, formatArgs), LogType.Trace);
        }
        /// <summary>
        ///   写入调试日志
        /// </summary>
        /// <param name="obj"> 记录对象 </param>
        [Conditional("TRACE")]
        public static void Trace(object obj)
        {
            Record(GetRequestId(), "调试", obj == null ? "NULL" : obj.ToString(), LogType.Trace);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void SystemLog(string msg)
        {
            Record(GetRequestId(), "系统", msg, LogType.System);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static void SystemLog(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "系统", FormatMessage(message, formatArgs), LogType.Trace);
        }

        /// <summary>
        ///   记录系统日志
        /// </summary>
        /// <param name="msg"> 消息 </param>
        public static void PlanLog(string msg)
        {
            Record(GetRequestId(), "计划", msg, LogType.System);
        }

        ///<summary>
        ///  写入一般日志
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void RecordMessage(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "消息", FormatMessage(message, formatArgs), LogType.Message);
        }

        ///<summary>
        ///  记录警告消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Warning(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "警告", FormatMessage(message, formatArgs), LogType.Warning);
        }

        ///<summary>
        ///  记录错误消息
        ///</summary>
        ///<param name="message"> 日志详细信息 </param>
        ///<param name="formatArgs"> 格式化的参数 </param>
        public static void Error(string message, params object[] formatArgs)
        {
            Record(GetRequestId(), "错误", StackTraceInfomation(FormatMessage(message, formatArgs)), LogType.Error);
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="exception"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static Guid RecordException(Exception exception, out string message)
        {

            Guid id = GetRequestId();
            message = ExceptionMessage(id, exception);
            string xml;
            ExceptionInfomation(id, exception, null, out xml);
            string title = "异常";
            if (exception != null)
            {
                title = exception.Message;
            }
            Record(id, title, xml, LogType.Exception);
            return id;
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="ex"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        public static string Exception(Exception ex, string message = null)
        {
            Guid id = GetRequestId();
            string xml;
            string re = ExceptionInfomation(id, ex, message, out xml);
            Record(id, "异常", xml, LogType.Exception);
            return re;
        }

        /// <summary>
        ///   记录异常日志
        /// </summary>
        /// <param name="e"> 异常 </param>
        /// <param name="message"> 日志详细信息 </param>
        /// <param name="formatArgs">格式化参数</param>
        public static string Exception(Exception e, string message, params object[] formatArgs)
        {
            Guid id = GetRequestId();
            string xml;
            string re = ExceptionInfomation(id, e, FormatMessage(message, formatArgs), out xml);
            if (e != null)
            {
                message = e.Message;
            }
            Record(id, message, xml, LogType.Exception);
            return re;
        }
        #endregion

        #region 异常
        /// <summary>
        ///   记录异常的外部信息
        /// </summary>
        /// <param name="id"> 日志查询标识 </param>
        /// <param name="ex"> 异常 </param>
        /// <returns> </returns>
        public static string ExceptionMessage(Guid id, Exception ex)
        {
            if (ex != null)
            {
#if SERVICE
                if (ex is SqlException)
                {
                    return AgebullSystemException.SqlExceptionLevel(ex as SqlException) > 16
                                   ? String.Format("发生服务器错误,系统标识:{0}", id)
                                   : String.Format("发生服务器错误,{1},系统标识:{0}", id, ex.Message);
                }
#endif
                if (ex is SystemException)
                {
                    return String.Format("发生系统错误,系统标识:{0}", id);
                }
                if (ex is AgebullSystemException)
                {
                    return String.Format("发生内部错误,系统标识:{0}", id);
                }
                if (ex is BugException)
                {
                    return String.Format("发生设计错误,系统标识:{0}", id);
                }
                if (ex is AgebullBusinessException)
                {
                    return String.Format("发生业务逻辑错误,内容为:{1},系统标识:{0}", id, ex.Message);
                }
#if SERVICE
                return String.Format("发生未知错误,系统标识:{0}", id);
#endif
            }
            return "发生未处理异常";
        }

        /// <summary>
        ///   记录异常的详细信息
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="ex"> </param>
        /// <param name="message"> </param>
        /// <param name="xml"> </param>
        /// <returns> </returns>
        public static string ExceptionInfomation(Guid id, Exception ex, string message, out string xml)
        {
            string outmsg = "发生未处理异常";
            string tag = "";
            if (ex != null)
            {
                if (ex is AgebullSystemException)
                {
                    tag = "系统致命错误";
                    outmsg = String.Format("发生内部错误,系统标识:{0}", id);
                }
                else if (ex is BugException)
                {
                    tag = "存在设计缺陷";
                    outmsg = String.Format("发生设计错误,系统标识:{0}", id);
                }
                else if (ex is AgebullBusinessException)
                {
                    tag = "业务逻辑错误";
                    outmsg = String.Format("发生错误,内容为:{1},系统标识:{0}", id, ex.Message);
                }
#if SERVICE
                else if (ex is SqlException)
                {
                    if (AgebullSystemException.SqlExceptionLevel(ex as SqlException) > 16)
                    {
                        tag = "数据库致命错误(级别大于16)";
                        outmsg = String.Format("发生服务器错误,系统标识:{0}", id);
                    }
                    else
                    {
                        tag = "数据库一般错误(级别小等于16)";
                        outmsg = String.Format("发生服务器错误,{1},系统标识:{0}", id, ex.Message);
                    }
                }
#endif
                else if (ex is SystemException)
                {
                    tag = "系统错误";
                    outmsg = String.Format("发生系统错误,系统标识:{0}", id);
                }
                else
                {
                    tag = "未知错误";
                    outmsg = String.Format("发生未知错误,系统标识:{0}", id);
                }
            }
            XElement element = new XElement("ExceptionInfomation",
                                       new XElement("ID", id),
                                       new XElement("Tag", tag),
                                       new XElement("RecordType", "Exception"),
                                       new XElement("OutMessage", outmsg));
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (message[0] == '<')
                {
                    try
                    {
                        element.Add(new XElement("Tag", new XElement(message)));
                    }
                    catch
                    {
                        element.Add(new XElement("Message", message));
                    }
                }
                else
                {
                    element.Add(new XElement("Message", message));
                }
            }
            if (ex != null)
            {
                ReflectionHelper.SerializeException(ex, element);
            }
            xml = element.ToString(SaveOptions.None);
            return outmsg;
        }
        #endregion

        #region 写入
        /// <summary>
        /// 日志序号
        /// </summary>
        static int _id = 1;
        /// <summary>
        /// 用于对象锁定
        /// </summary>
        static readonly object lockTooken = new RecordInfo();

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="id"> 标识 </param>
        /// <param name="name"> 原始的消息 </param>
        /// <param name="msg"> 处理后的消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="typeName"> 类型名称 </param>
        private static void Record(Guid id, string name, string msg, LogType type, string typeName = null)
        {
            if (type == LogType.None)
            {
                type = LogType.Message;
            }
            int idx;
            using (ThreadLockScope.Scope(lockTooken))
            {
                idx = _id++;
            }
            Push(new RecordInfo
            {
                gID = id,
                Index = idx,
                Name = name,
                Type = type,
                Message = msg,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                TypeName = typeName ?? TypeToString(type)
            });
        }
        /// <summary>
        /// 待写入的日志信息集合
        /// </summary>
        static readonly List<RecordInfo> recordInfos = new List<RecordInfo>();

        /// <summary>
        ///   入队列
        /// </summary>
        /// <param name="info"> </param>
        private static void Push(RecordInfo info)
        {
            if (Thread.CurrentPrincipal != null)
            {
                info.User = Thread.CurrentPrincipal.Identity.Name;
            }
            using (ThreadLockScope.Scope(recordInfos))
            {
                recordInfos.Add(info);
            }
        }

        /// <summary>
        ///  日志记录独立线程
        /// </summary>
        /// <param name="arg"> </param>
        private static void WriteRecordLoop(object arg)
        {
            while (true)
            {
                Thread.Sleep(3);
                RecordInfo[] infos;
                using (ThreadLockScope.Scope(recordInfos))
                {
                    infos = recordInfos.ToArray();
                    recordInfos.Clear();
                }
                foreach (var info in infos)
                {
                    Thread.Sleep(3);//释放一次时间片,以保证主要线程的流畅性
                    WriteToLog(info);
                }
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        private static void WriteToLog(RecordInfo info)
        {
            try
            {
                if (Listener != null)
                {
                    Listener.Trace(info);
                    Thread.Sleep(0);
                }
                else
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
                if (info.Type == LogType.Trace)
                {
                    TxtRecorder.RecordTrace(info.Message);
                    return;
                }
                if (info.Type == LogType.Monitor)
                {
                    TxtRecorder.RecordTrace(info.Message,".monitor");
                    return;
                }
#if SERVICE
                if ((info.Type == LogType.Error || info.Type == LogType.Exception) && !IsTextRecorder)
                {
                    TxtRecorder.Recorder.RecordLog(info);
                }
                Recorder?.RecordLog(info);
#else
                Recorder.RecordLog(info);
#endif
            }
            catch (Exception ex)
            {
                SystemTrace("日志写入发生错误", ex);
            }
        }

        /// <summary>
        /// 写入系统跟踪
        /// </summary>
        /// <param name="arg"></param>
        [Conditional("TRACE")]
        public static void SystemTrace(object arg)
        {
            System.Diagnostics.Trace.WriteLine(arg);
        }

        /// <summary>
        /// 写入系统跟踪
        /// </summary>
        /// <param name="title"></param>
        /// <param name="arg"></param>
        [Conditional("TRACE")]
        public static void SystemTrace(string title, object arg)
        {
            System.Diagnostics.Trace.WriteLine(arg, title);
        }

        #endregion
    }
}