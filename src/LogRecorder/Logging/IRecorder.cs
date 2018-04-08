// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System ;
using Agebull.Common.Base;

#endregion

namespace Agebull.Common.Logging
{
    /// <summary>
    ///   日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        ///   无
        /// </summary>
        None ,

        /// <summary>
        ///   登录消息
        /// </summary>
        Login ,

        /// <summary>
        ///   网络请求
        /// </summary>
        Request ,

        /// <summary>
        ///   WCF消息
        /// </summary>
        WcfMessage ,

        /// <summary>
        ///   数据库
        /// </summary>
        DataBase ,

        /// <summary>
        ///   信息
        /// </summary>
        Message ,

        /// <summary>
        ///   调试信息
        /// </summary>
        Trace ,

        /// <summary>
        ///   警告
        /// </summary>
        Warning ,

        /// <summary>
        ///   计划
        /// </summary>
        Plan,

        /// <summary>
        ///   监视
        /// </summary>
        Monitor,

        /// <summary>
        ///   系統消息
        /// </summary>
        System,

        /// <summary>
        ///   错误
        /// </summary>
        Error,

        /// <summary>
        ///   异常
        /// </summary>
        Exception,
    }

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
    ///   记录信息
    /// </summary>
    public class RecordInfo
    {
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
        public Guid gID ;

        /// <summary>
        ///   名称
        /// </summary>
        public string Name ;

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

    /// <summary>
    ///   记录器
    /// </summary>
    public interface ILogRecorder
    {
        /// <summary>
        ///   初始化
        /// </summary>
        void Initialize() ;

        /// <summary>
        ///   停止
        /// </summary>
        void Shutdown() ;

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        void RecordLog(RecordInfo info) ;
    }

    /// <summary>
    ///   表示日志的监听器
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        ///   显示日志消息
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        void Trace(RecordInfo info) ;
    }
    /// <summary>
    /// 正在记录日志的范围
    /// </summary>
    public class LogRecordingScope : ScopeBase
    {
        /// <summary>
        /// 生成日志记录的范围
        /// </summary>
        /// <returns></returns>
        public static ScopeBase CreateScope()
        {
            LogRecorder.InRecording = true;
            return new LogRecordingScope();
        }

        private LogRecordingScope()
        {

        }
        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            LogRecorder.InRecording = false;
        }
    }
}
