// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System ;
using System.Runtime.Serialization ;


#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   老牛软件使用的异常基类
    /// </summary>
    [Serializable]
    public class AgebullException
#if !SILVERLIGHT
            : ApplicationException
#else
            : Exception
#endif
    {
        /// <summary>
        ///   格式化异常
        /// </summary>
        /// <param name="err"> 异常 </param>
        /// <returns> 文本 </returns>
        public static string FormatException(Exception err)
        {
            if(err == null)
            {
                return string.Empty ;
            }
#if !SILVERLIGHT
            try
            {
                return string.Format("发生错误\"{0}\":\r\n源{1} {2}\r\n堆栈:{3}\r\n{4}" ,
                                     err.Message ,
                                     err.Source ,
                                     err.TargetSite != null
                                             ? err.TargetSite.Name
                                             : "" ,
                                     err.StackTrace ,
                                     FormatException(err.InnerException)) ;
            }
            catch
            {
                return string.Format("发生错误\"{0}\":\r\n源{1} {2}\r\n堆栈:{3}\r\n{4}" , err.Message , err.Source , "" , err.StackTrace , FormatException(err.InnerException)) ;
            }
#else
            return string.Format("发生错误\"{0}\":\r\n堆栈:{1}\r\n{2}", err.Message, err.StackTrace, FormatException(err.InnerException));
#endif
        }

        /// <summary>
        ///   基本构造
        /// </summary>
        public AgebullException()
        {
        }

        /// <summary>
        ///   消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        public AgebullException(string message) : base(message)
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内联消息 </param>
        public AgebullException(string message , Exception innerException) : base(message , innerException)
        {
        }

#if !SILVERLIGHT
        /// <summary>
        ///   序列化构造
        /// </summary>
        /// <param name="info"> 序列化对象 </param>
        /// <param name="context"> 数据流上下文 </param>
        protected AgebullException(SerializationInfo info , StreamingContext context) : base(info , context)
        {
        }
#endif
    }
}
