// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System.Xml ;


#endregion

#if !SILVERLIGHT
#endif

namespace Agebull.Common
{
    /// <summary>
    ///   远程返回的异常的信息
    /// </summary>
    public class ExceptionInfo
    {
        /// <summary>
        ///   主要信息
        /// </summary>
        public string RootMessage { get ; set ; }

        /// <summary>
        ///   节点
        /// </summary>
        public RemoteExceptionInfoItem Item { get ; set ; }

        /// <summary>
        ///   简要信息,已做简单的HTML格式化
        /// </summary>
        public string BriefMessage
        {
            get
            {
                RemoteExceptionInfoItem it = this.Item ;
                while(it != null)
                {
                    if(!string.IsNullOrWhiteSpace(it.Reason))
                    {
                        return string.Format("{0}<BR/>代码:{2}<BR/>信息:{1}" , this.RootMessage , it.Reason , this.ErrorCode) ;
                    }
                    it = it.Item ;
                }
                return this.RootMessage ;
            }
        }

        /// <summary>
        ///   平台提供的错误代码--可能为空
        /// </summary>
        public string ErrorCode
        {
            get
            {
                RemoteExceptionInfoItem it = this.Item ;
                while(it != null)
                {
                    if(!string.IsNullOrWhiteSpace(it.ErrorCode))
                    {
                        return it.ErrorCode ;
                    }
                    it = it.Item ;
                }
                return "-1" ;
            }
        }

#if !SILVERLIGHT
        /// <summary>
        ///   反序列化异常
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <returns> 序列化后的XML </returns>
        public static ExceptionInfo DeserializeException(string message)
        {
            return new ExceptionInfo(message) ;
        }

        /// <summary>
        ///   反序列化异常
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <returns> 序列化后的XML </returns>
        public ExceptionInfo(string message)
        {
            XmlDocument doc = new XmlDocument() ;
            doc.LoadXml(message) ;
            XmlNode root = doc.SelectSingleNode("//Exceptions") ;
            if (root != null)
            {
                XmlNode rm = root.SelectSingleNode("//RootMessage") ;
                if(rm != null && this.RootMessage != null)
                {
                    this.RootMessage = rm.InnerText ; // HttpUtility.UrlDecode(rm.InnerText);
                }
            }
            this.Item = DeserializeException(root) ;
        }

        /// <summary>
        ///   反序列化异常(内部递归)
        /// </summary>
        /// <param name="par"> 上级节点 </param>
        private static RemoteExceptionInfoItem DeserializeException(XmlNode par)
        {
            RemoteExceptionInfoItem item = null ;
            XmlNode me = par.SelectSingleNode(@"./Exception") ;
            if(me != null)
            {
                item = new RemoteExceptionInfoItem() ;
                XmlNode Type = me.SelectSingleNode("//Type") ;
                if(Type != null)
                {
                    item.Type = Type.InnerText ;
                }
                //LogRecorder.Trace(item.Type) ;
                //XmlNode Message = me.SelectSingleNode("//Message") ;
                //if (Message != null)
                //    item.Message = HttpUtility.UrlDecode(Message.InnerText);
                //Agebull.Common.Logging.LogRecorder.Trace(item.Message);
                //XmlNode Source = me.SelectSingleNode("//Source");
                //if (Source != null)
                //    item.Source = Source.InnerText;
                //XmlNode StackTrace = me.SelectSingleNode("//StackTrace");
                //if (StackTrace != null)
                //    item.StackTrace = HttpUtility.UrlDecode(StackTrace.InnerText);
                //Agebull.Common.Logging.LogRecorder.Trace(item.StackTrace);
                //XmlNode Reason = me.SelectSingleNode("//Reason");
                //if (Reason != null)
                //    item.Reason = HttpUtility.UrlDecode(Reason.InnerText);
                //Agebull.Common.Logging.LogRecorder.Trace(item.Reason);
                //XmlNode ErrorCode = me.SelectSingleNode("//ErrorCode");
                //if (ErrorCode != null)
                //    item.ErrorCode = HttpUtility.UrlDecode(ErrorCode.InnerText);
                //Agebull.Common.Logging.LogRecorder.Trace(item.ErrorCode);
                //XmlNode ErrorMessage = me.SelectSingleNode("//ErrorMessage");
                //if (ErrorMessage != null)
                //    item.ErrorMessage = HttpUtility.UrlDecode(ErrorMessage.InnerText);
                //Agebull.Common.Logging.LogRecorder.Trace(item.ErrorMessage);
                item.Item = DeserializeException(me) ;
            }
            return item ;
        }
#endif
    }

    /// <summary>
    ///   远程返回的异常的信息节点
    /// </summary>
    public class RemoteExceptionInfoItem
    {
        /// <summary>
        ///   子节点
        /// </summary>
        public RemoteExceptionInfoItem Item { get ; set ; }

        /// <summary>
        ///   异常类型
        /// </summary>
        public string Type { get ; set ; }

        /// <summary>
        ///   异常的消息
        /// </summary>
        public string Message { get ; set ; }

        /// <summary>
        ///   异常的消息源
        /// </summary>
        public string Source { get ; set ; }

        /// <summary>
        ///   异常的栈跟踪
        /// </summary>
        public string StackTrace { get ; set ; }

        /// <summary>
        ///   平台异常的信息
        /// </summary>
        public string Reason { get ; set ; }

        /// <summary>
        ///   平台异常的错误代码
        /// </summary>
        public string ErrorCode { get ; set ; }

        /// <summary>
        ///   平台异常的其它错误信息
        /// </summary>
        public string ErrorMessage { get ; set ; }
    }
}
