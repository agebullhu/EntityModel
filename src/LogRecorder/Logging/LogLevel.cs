namespace Agebull.Common.Logging
{
    /// <summary>
    ///   日志类型
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///   无
        /// </summary>
        None,

        /// <summary>
        ///   调试信息
        /// </summary>
        Debug,

        /// <summary>
        ///   跟踪信息
        /// </summary>
        Trace,

        /// <summary>
        ///   警告
        /// </summary>
        Warning,

        /// <summary>
        ///   系统
        /// </summary>
        System,

        /// <summary>
        ///   错误
        /// </summary>
        Error
    }
    /// <summary>
    /// 日志枚举辅助
    /// </summary>
    public static class LogEnumHelper
    {
        /// <summary>
        /// 日志类型到级别
        /// </summary>
        /// <param name="type"></param>
        public static LogLevel Level(this LogType type)
        {
            switch (type)
            {
                case LogType.Debug:
                case LogType.Monitor:
                case LogType.DataBase:
                    return LogLevel.Debug;
                case LogType.Login:
                case LogType.NetWork:
                case LogType.Message:
                case LogType.Request:
                case LogType.Trace:
                    return LogLevel.Trace;
                case LogType.Warning:
                    return LogLevel.Warning;
                case LogType.Error:
                case LogType.Exception:
                    return LogLevel.Error;
                case LogType.System:
                case LogType.Plan:
                    return LogLevel.System;
            }
            return LogLevel.None;
        }

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
    }
}