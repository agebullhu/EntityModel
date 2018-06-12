using System;

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   记录信息
    /// </summary>
    public class RecordInfo
    {
        /// <summary>
        /// 本地日志
        /// </summary>
        public bool Local;

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time;
        /// <summary>
        ///   日志记录序号
        /// </summary>
        public ulong Index;

        /// <summary>
        ///   线程ID
        /// </summary>
        public int ThreadID ;

        /// <summary>
        ///   日志ID
        /// </summary>
        public string RequestID;

        /// <summary>
        ///   机器
        /// </summary>
        public string Machine;

        /// <summary>
        ///   名称
        /// </summary>
        public string Name;

        /// <summary>
        ///   格式化消息
        /// </summary>
        public string Message ;

        /// <summary>
        ///   日志类型
        /// </summary>
        public LogType Type ;

        /// <summary>
        ///   日志扩展名称,类型为None
        /// </summary>
        public string TypeName ;
        
        /// <summary>
        ///   当前用户
        /// </summary>
        public string User ;
    }
}