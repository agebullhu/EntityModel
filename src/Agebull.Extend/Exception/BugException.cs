// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   代码缺陷异常类
    /// </summary>
    [Serializable]
    public class BugException : AgebullException
    {
        /// <summary>
        ///   消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        public BugException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///   消息构造
        /// </summary>
        /// <param name="message"> 格式化基 </param>
        /// <param name="args"> 格式化参数 </param>
        public BugException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内联消息 </param>
        internal BugException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        internal BugException(Type type, string function, string message)
            : base("调用类" + type + "的" + function + "时发生编码不规范的调用异常：" + message)
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        internal BugException(string type, string function, string message)
            : base("调用类" + type + "的" + function + "时发生编码不规范的调用异常：" + message)
        {
        }

#if !SILVERLIGHT
        /// <summary>
        ///   序列化构造
        /// </summary>
        /// <param name="info"> 序列化对象 </param>
        /// <param name="context"> 数据流上下文 </param>
        protected BugException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
