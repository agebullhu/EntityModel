// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System.Collections.Generic;
using Agebull.Common.Base;

#endregion

namespace Agebull.Common.Logging
{
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
        void Shutdown();

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"> 日志消息 </param>
        void RecordLog(RecordInfo info);

        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="infos"> 日志消息 </param>
        void RecordLog(List<RecordInfo> infos);
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
