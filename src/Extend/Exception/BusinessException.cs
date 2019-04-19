// 所在工程：Agebull.EntityModel
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
    ///   老牛软件使用的业务逻辑异常类
    /// </summary>
    [Serializable]
    public class BusinessException : ExceptionEx
    {
        /// <summary>
        ///   基本构造
        /// </summary>
        public BusinessException()
        {
        }

        /// <summary>
        ///   消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        public BusinessException(string message) : base(message)
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内联消息 </param>
        public BusinessException(string message , Exception innerException) : base(message , innerException)
        {
        }

        /// <summary>
        ///   序列化构造
        /// </summary>
        /// <param name="info"> 序列化对象 </param>
        /// <param name="context"> 数据流上下文 </param>
        protected BusinessException(SerializationInfo info , StreamingContext context) : base(info , context)
        {
        }
    }
}
