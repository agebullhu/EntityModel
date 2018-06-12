// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2018年6月12日, AM 12:25:44

#region

using System;
using System.Collections.Generic;
using Agebull.Common.Configuration;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

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
                var sec = ConfigurationManager.Get("LogRecorder");
                var cfgpath = sec["txtPath"];
                if (string.IsNullOrWhiteSpace(cfgpath))
                {
                    cfgpath = Path.Combine(Environment.CurrentDirectory, "logs");
                    sec["txtPath"] = cfgpath;
                }
                LogPath = cfgpath;
                if (LogPath != null && !Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("TextRecorder.Initialize", ex);
            }
            Trace.Listeners.Add(this);
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
            DisposeWriters();
        }

        /// <summary>
        ///   文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public static string LogPath { get; set; }

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="infos"> 日志消息 </param>
        public void RecordLog(List<RecordInfo> infos)
        {
            foreach (var info in infos)
                RecordLog(info);
        }

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        public void RecordLog(RecordInfo info)
        {
            string name;
            string log;
            switch (info.Type)
            {
                case LogType.NetWork:
                case LogType.System:
                    name = "system";
                    break;
                case LogType.Request:
                case LogType.Login:
                    name = "user";
                    break;
                case LogType.DataBase:
                    name = "sql";
                    break;
                case LogType.Warning:
                    name = "warning";
                    break;
                case LogType.Exception:
                case LogType.Error:
                    name = "error";
                    break;
                case LogType.Plan:
                    name = "plan";
                    break;
                case LogType.Monitor:
                    name = "monitor";
                    break;
                case LogType.Trace:
                case LogType.Debug:
                    name = "trace";
                    return;
                default:
                    name = "info";
                    break;
            }

            switch (info.Type)
            {
                case LogType.Trace:
                case LogType.Debug:
                    log = $@"[{info.Time:u}] ({info.Machine}-{info.RequestID}):{info.Message}";
                    break;
                case LogType.DataBase:
                    log = $@"/*{info.Machine}:{info.RequestID}:{info.Time:u}*/
{info.Message}";
                    break;
                case LogType.Monitor:
                    log = info.Message;
                    break;
                default:
                    log = JsonConvert.SerializeObject(info, Formatting.Indented);
                    break;
            }


            try
            {
                StreamWriter writer = GetWriter(name);
                writer.WriteLine(log);
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("TextRecorder.RecordLog4", ex);
            }
        }
        /// <summary>
        ///   记录日志
        /// </summary>
        private void WriteFile(string log, string type)
        {
            try
            {
                StreamWriter writer = GetWriter(type);
                writer.WriteLine(log);
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace("TextRecorder.WriteFile", ex);
            }
        }

        /// <summary>
        ///   写消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void Write(string message)
        {
            WriteFile($"[{DateTime.Now.ToShortTimeString()}] {message}", "trace");
        }

        /// <summary>
        ///   写一行消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void WriteLine(string message)
        {
            WriteFile($"[{DateTime.Now.ToShortTimeString()}] {message}", "trace");
        }

        #region 文件列表

        /// <summary>
        /// 当前记录的时间点
        /// </summary>
        private long _recordTimePoint;

        /// <summary>
        /// 当前记录的时间点
        /// </summary>
        private void CheckTimePoint()
        {
            var now = DateTime.Today.Year * 1000000 + DateTime.Today.Month * 10000 + DateTime.Today.Day * 100 +
                      (DateTime.Now.Hour / 2);
            if (now == _recordTimePoint) return;
            foreach (var w in _writers.Values)
            {
                w.Flush();
                w.Dispose();
            }
            _writers.Clear();
            _recordTimePoint = now;
        }

        /// <summary>
        /// 所有写入的文件句柄
        /// </summary>
        private readonly Dictionary<string, StreamWriter> _writers =
            new Dictionary<string, StreamWriter>(StringComparer.OrdinalIgnoreCase);

        private StreamWriter GetWriter(string sub)
        {
            CheckTimePoint();
            if (_writers.TryGetValue(sub, out var writer))
                return writer;
            string ph = Path.Combine(LogPath, $"{_recordTimePoint}.{sub}.log");
            var file = new FileStream(ph, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            writer = new StreamWriter(file) { AutoFlush = true };

            _writers.Add(sub, writer);

            return writer;
        }

        /// <summary>
        /// 任务结束,环境销毁
        /// </summary>
        private void DisposeWriters()
        {
            foreach (var writer in _writers.Values)
            {
                writer.Flush();
                writer.Dispose();
            }
            _writers.Clear();
        }

        #endregion
    }
}
