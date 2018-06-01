// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Agebull.Common.Frame;

#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   文本记录器
    /// </summary>
    public sealed class TxtRecorder : TraceListener, ILogRecorder
    {
        /// <summary>
        ///   初始化
        /// </summary>
        public static TxtRecorder Recorder = new TxtRecorder();

        /// <summary>
        ///   初始化
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LogPath))
                {
                    var cfgpath = ConfigurationManager.AppSettings["LogPath"];
                    if (string.IsNullOrWhiteSpace(cfgpath))
                    {
                        cfgpath = Path.Combine(Environment.CurrentDirectory, "logs");
                    }
                    LogPath = cfgpath;
                }
                if (LogPath != null && !Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                Console.WriteLine($"LogPath:{LogPath}");
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("日志记录:TextRecorder.Initialize", ex);
            }
        }

        /// <summary>
        /// 阻止外部构造
        /// </summary>
        internal TxtRecorder()
        {

        }

        /// <summary>
        ///   停止
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        ///   文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public static string LogPath { get; set; }

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        public void RecordLog(RecordInfo info)
        {
#if CLIENT
            RecordLog(info.gID, info.Message, info.TypeName);
#else
            switch (info.Type)
            {
                case LogType.System:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "system");
                    break;
                case LogType.Login:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "user");
                    break;
                case LogType.Request:
                case LogType.WcfMessage:
                case LogType.Trace:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "trace");
                    break;
                case LogType.DataBase:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "sql");
                    break;
                case LogType.Warning:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "warning");
                    break;
                case LogType.Error:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "error");
                    break;
                case LogType.Exception:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "exception");
                    break;
                case LogType.Plan:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "plan");
                    break;
                case LogType.Monitor:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "monitor");
                    break;
                default:
                    RecordLog(info.RequestID, info.Message, info.TypeName, info.User, "info");
                    break;
            }
#endif
        }

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="id"> 标识 </param>
        /// <param name="msg"> 消息 </param>
        /// <param name="type"> 日志类型 </param>
        /// <param name="user"> 当前操作者 </param>
        /// <param name="name"> 标识的文件后缀(如.error,则文件名可能为 20160602.error.log) </param>
        private void RecordLog(string id, string msg, string type, string user = null, string name = null)
        {
            string log = type == "DataBase"
                ? $@"
/*Date:{ DateTime.Now.ToString(CultureInfo.InvariantCulture)}
RQID:{id}*/
{msg}"
                : $@"
RQID:{id}
Date:{DateTime.Now.ToString(CultureInfo.InvariantCulture)}
Type:{type}
User:{user}
{msg}";
            try
            {
                //if (!Directory.Exists(LogPath))
                //{
                //    Directory.CreateDirectory(LogPath);
                //}
                string ph = Path.Combine(LogPath, $"{DateTime.Today:yyyyMMdd}.{name ?? "info"}.log");

                //using (ThreadLockScope.Scope(this))
                {
                    File.AppendAllText(ph, log, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("日志记录:TextRecorder.RecordLog4", ex);
            }
        }

        /// <summary>
        ///   记录跟踪信息
        /// </summary>
        /// <param name="message"> </param>
        /// <param name="type"></param>
        public static void RecordTrace(string message, string type = "trace")
        {
            try
            {
                Recorder.RecordTraceInner(message, type);
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("日志记录:TextRecorder.RecordLog1", ex);
            }
        }

        /// <summary>
        ///   记录日志
        /// </summary>
        private void RecordTraceInner(string msg, string type = "trace")
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string ph = Path.Combine(LogPath, $"{DateTime.Today:yyyy-MM-dd}.{DateTime.Now.Hour / 2}.{type.Trim('.')}.log");

            using (ThreadLockScope.Scope(this))
            {
                File.AppendAllText(ph, msg + "\n", Encoding.UTF8);
            }
        }
        /// <summary>
        ///   写消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void Write(string message)
        {
            RecordLog(LogRecorder.GetRequestId(), message, LogRecorder.TypeToString(LogType.Trace));
        }

        /// <summary>
        ///   写一行消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void WriteLine(string message)
        {
            RecordLog(LogRecorder.GetRequestId(), message, LogRecorder.TypeToString(LogType.Trace));
        }
    }
}
