// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2018年6月12日, AM 12:25:44

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Agebull.Common.Base;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///     文本记录器
    /// </summary>
    internal sealed class TextLogger : ScopeBase, ILogger
    {
        #region 初始化与析构
        /// <summary>
        /// 配置
        /// </summary>
        internal TextLoggerOption Option { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        public IExternalScopeProvider ScopeProvider { get; set; }

        /// <summary>
        ///     构造
        /// </summary>
        internal TextLogger(TextLoggerOption option)
        {
            Used++;
            Option = option;
        }


        /// <inheritdoc/>
        protected override void OnDispose()
        {
            Used--;
            if (Used == 0)
                DisposeWriters();
        }
        #endregion

        #region 写文件

        /// <summary>
        ///     记录日志
        /// </summary>
        private void WriteFile(string type, string log)
        {
            FileInfo writer = null;
            try
            {
                writer = GetWriter(type);
                if (writer == null)
                    return;
                writer.Size += log.Length;
                writer.Stream.WriteLine(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), $"TextRecorder.WriteFile->{type}");
                writer?.Stream.Dispose();
                writer?.Stream.Close();
                ResetFile(type, writer);
            }
        }

        /// <summary>
        ///     当前记录的时间点
        /// </summary>
        private long _recordTimePoint;

        /// <summary>
        ///     需要立即重置
        /// </summary>
        internal void ResetFile()
        {
            _recordTimePoint = 0;
        }
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
            if (Option.DayFolder)
            {
                IOHelper.CheckPath(Option.LogPath, $"{pointTime.Year}{pointTime.Month:D2}{pointTime.Day:D2}");
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
        private static readonly Dictionary<string, FileInfo> _writers = new Dictionary<string, FileInfo>();

        private FileInfo GetWriter(string sub)
        {
            if (IsDisposed)
                return null;
            if (Option.DayFolder)
                CheckTimePoint();
            if (_writers.TryGetValue(sub, out var info) && info.Size < Option.SplitNumber)
            {
                return info;
            }
            if (info == null)
            {
                info = new FileInfo();
                ResetFile(sub, info);
                _writers.Add(sub, info);
            }
            else
            {
                info.Stream?.Flush();
                info.Stream?.Dispose();
                ResetFile(sub, info);
            }
            return info;
        }

        static int Used = 0;

        /// <summary>
        ///     任务结束,环境销毁
        /// </summary>
        private static void DisposeWriters()
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
            string fileName;
            string folder = LogRecorder.LogPath ?? Option.LogPath;
            info.Size = 0;
            if (!Option.DayFolder)//第一次
            {
                if (info.Index > 0)
                {
                    if (info.Index >= Option.maxFile)
                        info.Index = 1;
                    else
                        info.Index++;
                    fileName = Path.Combine(folder, $"{sub}.{info.Index:D3}.log");
                    if (File.Exists(fileName))
                    {
                        ReUseFile(sub, info);
                    }
                    else
                    {
                        info.Stream = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            AutoFlush = true
                        };
                        info.Size = 0;
                    }
                    return;
                }
                fileName = Path.Combine(folder, $"{sub}.{Option.maxFile:D3}.log");
                if (File.Exists(fileName))//达最大文件数量
                {
                    var stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    info.Size = stream.Length;
                    if (stream.Length < Option.SplitNumber)
                    {
                        info.Stream = new StreamWriter(stream)
                        {
                            AutoFlush = true
                        };
                        return;
                    }
                    else
                    {
                        DateTime time = File.GetLastWriteTime(fileName);
                        DateTime timeLast = time;
                        //找到最大写入时间
                        for (int idx = Option.maxFile - 1; idx > 0; idx--)
                        {
                            fileName = Path.Combine(folder, $"{sub}.{idx:D3}.log");
                            if (File.Exists(fileName))
                            {
                                DateTime time2 = File.GetLastWriteTime(fileName);
                                if (time2 > time)
                                {
                                    info.Index = idx + 1;
                                    ReUseFile(sub, info);
                                    return;
                                }
                                time = time2;
                            }
                        }
                        //最后的时间与1相比,最后的时间如果大则说明正好要循环到1;
                        info.Index = timeLast > time ? 1 : Option.maxFile;
                        ReUseFile(sub, info);
                        return;
                    }
                }
            }
            else
            {
                folder = Path.Combine(folder, $"{pointTime.Year}{pointTime.Month:D2}{pointTime.Day:D2}");
            }
            do
            {
                info.Index++;
                fileName = Path.Combine(folder, $"{sub}.{info.Index:D3}.log");

                if (!File.Exists(fileName))
                {
                    info.Stream = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        AutoFlush = true
                    };
                    info.Size = 0;
                    return;
                }

                var stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                info.Size = stream.Length;
                if (stream.Length >= Option.SplitNumber)
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
            while (true);
        }

        private void ReUseFile(string sub, FileInfo info)
        {
            var fileName = Path.Combine(Option.LogPath, $"{sub}.{info.Index:D3}.log");
            info.Stream = new StreamWriter(new FileStream(fileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            {
                AutoFlush = true
            };
            info.Size = 0;
        }

        #endregion

        #region ILogger

        /// <summary>
        /// 通过计数减少取磁盘大小的频率
        /// </summary>
        int cnt = 0;

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel < Option.LogLevel)
                return;
            if (Option.DayFolder && ++cnt >= 100)
            {
                cnt = 0;
                var size = IOHelper.FolderDiskInfo(Option.LogPath);
                if (size.AvailableSize < Option.MinFreeSize)
                    Option.disable = true;
            }
            string Text() => $"{DateTime.Now:MM-dd HH:mm:ss.ffff} [{(eventId.Name ?? logLevel.ToString())}]\t{LogRecorder.GetMachineName()}({LogRecorder.GetUserName()}) [{eventId.Id:D4}({LogRecorder.GetRequestId()})]\t{formatter(state, exception)}";
            if (!Option.disable)
                WriteFile("log", Text());

            switch (logLevel)
            {
                case LogLevel.Warning:
                    if (!Option.disable)
                        WriteFile("warning", Text());
                    break;
                case LogLevel.Error:
                    WriteFile("error", Text());
                    break;
                case LogLevel.Critical:
                    WriteFile("system", Text());
                    break;
            }
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return !Option.disable && logLevel != LogLevel.None;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return ScopeProvider?.Push(state) ?? new EmptyScope();
        }
        #endregion

    }
}