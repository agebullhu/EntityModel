// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2018年6月12日, AM 12:25:44

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Agebull.Common.Configuration;

#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///     文本记录器
    /// </summary>
    public sealed class TxtRecorder : TraceListener, ILogRecorder
    {
        /// <summary>
        ///     初始化
        /// </summary>
        public static TxtRecorder Recorder = new TxtRecorder();

        /// <summary>
        ///     阻止外部构造
        /// </summary>
        internal TxtRecorder()
        {
        }

        /// <summary>
        ///     文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public static string LogPath { get; set; }

        /// <summary>
        /// 拆分日志的数量
        /// </summary>
        public static int SplitNumber { get; set; }

        /// <summary>
        /// 每日一个文件夹吗
        /// </summary>
        public static bool dayFolder { get; set; }
        /// <summary>
        /// 是否初始化成功
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        private bool pathIsCheck;
        /// <summary>
        ///     初始化
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
                SplitNumber = sec.GetInt("split", 10) * 1024 * 1024;
                dayFolder = sec.GetBool("dayFolder", true);
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace(LogLevel.Error, "TextRecorder.Initialize", ex);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(LogPath))
                    LogPath = Path.Combine(Environment.CurrentDirectory, "logs");
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace(LogLevel.Error, "TextRecorder.Initialize LogPath", ex);
            }
            Trace.Listeners.Add(this);
            IsInitialized = true;
        }

        private bool _isDispose;
        /// <summary>
        ///     停止
        /// </summary>
        public void Shutdown()
        {
            if (_isDispose)
                return;
            _isDispose = true;
            DisposeWriters();
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="infos"> 日志消息 </param>
        public void RecordLog(List<RecordInfo> infos)
        {

            foreach (var info in infos)
                RecordLog(info);
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        public void RecordLog(RecordInfo info)
        {
            List<string> names = new List<string>();
            string log;
            switch (info.Type)
            {
                case LogType.NetWork:
                case LogType.Plan:
                case LogType.Request:
                case LogType.System:
                case LogType.Login:
                    names.Add("system");
                    log = $@"[{info.Time:u}] {info.Index:X8}-{info.Machine}-{info.RequestID}-{info.User} > {info.Message}";
                    break;
                case LogType.Warning:
                    names.Add("warning");
                    names.Add("system");
                    log = $@"[{info.Time:u}] {info.Index:X8}-{info.Machine}-{info.RequestID}-{info.User} > {info.Message}";
                    break;
                case LogType.Error:
                case LogType.Exception:
                    names.Add("error");
                    names.Add("system");
                    log = $@"[{info.Time:u}] {info.Index:X8}-{info.Machine}-{info.ThreadID}-{info.RequestID}-{info.User} > 
{info.Message}
";
                    break;
                case LogType.DataBase:
                    names.Add("sql");
                    names.Add("trace");
                    log = $@"/*[{info.Time:u}] {info.Index:X8}-{info.Machine}-{info.RequestID}-{info.User}*/
{info.Message}
";
                    break;
                case LogType.Monitor:
                    names.Add("monitor");
                    names.Add("trace");
                    log = info.Message;
                    break;
                case LogType.Debug:
                    names.Add("debug");
                    names.Add("trace");
                    log = info.Message;
                    break;
                default:
                    names.Add("trace");
                    log = $@"[{info.Time:u}] {info.Index:X8}-{info.Machine}-{info.RequestID}-{info.User} > {info.Message}";
                    break;
            }

            foreach (var name in names)
            {
                try
                {
                    var writer = GetWriter(name);
                    if (writer == null)
                        return;
                    writer.Size += log.Length;
                    writer.Stream.WriteLine(log);
                }
                catch (Exception ex)
                {
                    LogRecorder.SystemTrace(LogLevel.Error, "TextRecorder.RecordLog4", name, ex);
                }
            }
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        private void WriteFile(string log, string type)
        {
            var writer = GetWriter(type);
            if (writer == null)
                return;
            try
            {
                writer.Size += log.Length;
                writer.Stream.WriteLine(log);
            }
            catch (Exception ex)
            {
                LogRecorder.SystemTrace(LogLevel.Error, "TextRecorder.WriteFile", type, ex);
                writer.Stream.Dispose();
                writer.Stream.Close();
                ResetFile(type, writer);
            }
        }

        /// <summary>
        ///     写消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void Write(string message)
        {
            WriteFile($"[{DateTime.Now.ToShortTimeString()}] {message}", "trace");
        }

        /// <summary>
        ///     写一行消息--Trace
        /// </summary>
        /// <param name="message"> </param>
        public override void WriteLine(string message)
        {
            WriteFile($"[{DateTime.Now.ToShortTimeString()}] {message}", "trace");
        }

        #region 文件列表

        /// <summary>
        ///     当前记录的时间点
        /// </summary>
        private long _recordTimePoint;

        private DateTime pointTime;
        /// <summary>
        ///     当前记录的时间点
        /// </summary>
        private void CheckTimePoint()
        {
            var day = (DateTime.Today.Year << 16) + (DateTime.Today.Month << 8) + DateTime.Today.Day;
            if (day == _recordTimePoint)
                return;
            pointTime = DateTime.Today;
            if (dayFolder)
            {
                IOHelper.CheckPath(LogPath, $"{pointTime.Year}{pointTime.Month:D2}{pointTime.Day:D2}");
            }
            DisposeWriters();
            _recordTimePoint = day;
        }

        private class FileInfo
        {
            public long Size;
            public int Index;
            public StreamWriter Stream;
        }
        /// <summary>
        ///     所有写入的文件句柄
        /// </summary>
        private readonly Dictionary<string, FileInfo> _writers = new Dictionary<string, FileInfo>();

        private FileInfo GetWriter(string sub)
        {
            if (_isDispose)
                return null;
            if (!pathIsCheck)
            {
                pathIsCheck = true;
                if (LogPath == null)
                {
                    Initialize();
                }
                // ReSharper disable once AssignNullToNotNullAttribute
                if (!Directory.Exists(LogPath))
                    Directory.CreateDirectory(LogPath);
            }
            CheckTimePoint();
            if (_writers.TryGetValue(sub, out var info) && info.Size < SplitNumber)
            {
                return info;
            }
            if (info == null)
            {
                info = new FileInfo();
                ResetFile(sub, info);
                _writers.Add(sub, info);
            }
            ResetFile(sub, info);
            return info;
        }

        /// <summary>
        ///     任务结束,环境销毁
        /// </summary>
        private void DisposeWriters()
        {
            foreach (var info in _writers)
            {
                if (info.Value.Stream == null)
                    continue;
                info.Value.Stream.Flush();
                info.Value.Stream.Dispose();
            }
            _writers.Clear();
        }

        private void ResetFile(string sub, FileInfo info)
        {
            info.Size = 0;
            info.Index = 0;
            do
            {
                info.Index++;
                var fileName = dayFolder
                    ? Path.Combine(LogPath,
                        $"{pointTime.Year}{pointTime.Month:D2}{pointTime.Day:D2}",
                        $"{sub}.{info.Index:D3}.log")
                     : Path.Combine(LogPath,
                        $"{pointTime.Year}{pointTime.Month:D2}{pointTime.Day:D2}.{sub}.{info.Index:D3}.log");
                if (File.Exists(fileName))
                {
                    var stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    info.Size = stream.Length;
                    if (stream.Length >= SplitNumber)
                    {
                        stream.Close();
                        stream.Dispose();
                        continue;
                    }
                    info.Stream = new StreamWriter(stream)
                    {
                        AutoFlush = true
                    };
                    return;
                }
                IOHelper.CheckPath(Path.GetDirectoryName(fileName));
                info.Stream = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    AutoFlush = true
                };
                info.Size = 0;
                return;
            }
            while (true);
        }

        #endregion
    }
}