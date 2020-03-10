// 所在工程：Agebull.EntityModel.Core
// 整理用户：agebull
// 建立时间：2020-03-03 12:01:20
// 整理时间：2020-03-03 12:01:20

#region

using System;
using System.IO;
using Agebull.Common.Configuration;
using Microsoft.Extensions.Logging;
#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    /// 文本日志配置
    /// </summary>
    public class TextLoggerOption
    {
        /// <summary>
        ///     文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 拆分日志的数量
        /// </summary>
        public int split { get; set; }

        /// <summary>
        /// 最大文件数量
        /// </summary>
        public int maxFile { get; set; }

        /// <summary>
        /// 最小可用空间(小于时只记录系统与错误日志)
        /// </summary>
        public int minFreeSize { get; set; }

        /// <summary>
        /// 每日一个文件夹吗
        /// </summary>
        public bool dayFolder { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool disable { get; set; }

        /// <summary>
        ///     文本日志的路径,如果不配置,就为:[应用程序的路径]\log\
        /// </summary>
        public string LogPath => path;

        /// <summary>
        /// 拆分日志的数量
        /// </summary>
        public int SplitNumber => split;

        /// <summary>
        /// 最小可用空间(小于时只记录系统与错误日志)
        /// </summary>
        public int MinFreeSize => minFreeSize;

        /// <summary>
        /// 每日一个文件夹吗
        /// </summary>
        public bool DayFolder => dayFolder;

        /// <summary>
        ///     初始化
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (disable)
                    return;
                if (split <= 0)
                    split = 10;
                split <<= 20;
                if (minFreeSize <= 0)
                    minFreeSize = 1;
                if (maxFile <= 0)
                    maxFile = 999;
                if (string.IsNullOrWhiteSpace(LogPath))
                {
                    path = LogRecorder.LogPath;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        path = Path.Combine(Environment.CurrentDirectory, "logs");
                    }
                }
                IOHelper.CheckPath(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "TextRecorder.Initialize LogPath");
            }
            try
            {
                var size = IOHelper.FolderDiskInfo(LogPath);
                if (size.AvailableSize < MinFreeSize)
                    disable = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "TextRecorder.Initialize LogPath");
            }
        }
    }
}