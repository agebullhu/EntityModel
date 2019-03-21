// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Data.Common;
using System.Runtime.Serialization;

#if !NETCOREAPP
using System.Data.SqlClient ;
#endif

#endregion


namespace Agebull.Common
{
    /// <summary>
    ///   老牛软件使用的系统(操作系统,.NET,IIS,数据库等)异常类
    /// </summary>
    [Serializable]
    public class AgebullSystemException : AgebullException
    {
        /// <summary>
        ///   内容的详细信息
        /// </summary>
        public string InnerMessage { get; set; }

        /// <summary>
        ///   扩展信息
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        ///   基本构造
        /// </summary>
        public AgebullSystemException()
        {
        }

        /// <summary>
        ///   消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        public AgebullSystemException(string message)
            : base(message)
        {
        }
        
        /// <summary>
        ///   基本构造
        /// </summary>
        public AgebullSystemException(Exception serr) : this(serr.Message , serr)
        {
        }

        /// <summary>
        ///   错误类型
        /// </summary>
        public SystemErrorType ErrorType { get ; set ; }

        /// <summary>
        ///   基本构造
        /// </summary>
        public AgebullSystemException(System.Data.Common.DbException serr) : this(serr.Message , serr)
        {
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内联消息 </param>
        public AgebullSystemException(string message , Exception innerException) : base(message , innerException)
        {
            ErrorType = SystemErrorType.UnknowError ;
        }

        /// <summary>
        ///   内联消息构造
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内联消息 </param>
        /// <param name="errtype"> 异常类型 </param>
        /// <param name="innermessage"> 内部扩展消息 </param>
        public AgebullSystemException(string message , SystemErrorType errtype , string innermessage , Exception innerException) : base(message , innerException)
        {
            ErrorType = errtype ;
            InnerMessage = innermessage ;
        }

        /// <summary>
        ///   序列化构造
        /// </summary>
        /// <param name="info"> 序列化对象 </param>
        /// <param name="context"> 数据流上下文 </param>
        protected AgebullSystemException(SerializationInfo info , StreamingContext context) : base(info , context)
        {
        }
#if !NETCOREAPP
        /// <summary>
        ///   基本构造
        /// </summary>
        public static AgebullException OnAgebullDatabaseException(SqlException serr)
        {
            if(SqlExceptionLevel(serr) > 16)
            {
                return new AgebullSystemException("系统内部错误" , SystemErrorType.DataBaseError , serr.Message , serr) ;
            }
            return new BugException(serr.Message , serr) ;
        }

        /// <summary>
        ///   基本构造
        /// </summary>
        public static int SqlExceptionLevel(SqlException serr)
        {
            var errclass = 0 ;
            foreach(SqlError se in serr.Errors)
            {
                if(se.Class > errclass)
                {
                    errclass = se.Class ;
                }
            }
            return errclass ;
        }
#endif
        /// <summary>
        /// </summary>
        /// <param name="serr"> </param>
        /// <returns> </returns>
        public static Exception OnAgebullDatabaseException(DbException serr)
        {
            return new AgebullSystemException("系统内部错误" , SystemErrorType.DataBaseError , serr.Message , serr) ;
        }
    }
}
