// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Diagnostics;
using System.Net;
//using System.ServiceModel;

#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   异常辅助类
    /// </summary>
    public class ExceptionHelper
    {
        #region 消息
        /// <summary>
        ///   得到一个异常的用户消息
        /// </summary>
        /// <param name="ex"> </param>
        /// <returns> </returns>
        public static string GetMessage(Exception ex)
        {
            if (ex == null)
            {
                return "发生未处理异常";
            }
            if (ex is BugException)
            {
                return "存在设计缺陷";
            }
            if (ex is BusinessException)
            {
                return $@"服务器因为数据错误无法正确执行，错误信息：{ex.Message}";
            }
            if (ex is SystemExException)
            {
                return "服务器发生内部错误";
            }
            //if (ex is EndpointNotFoundException)
            //{
            //    return "无法连接到服务器";
            //}
            //if (ex is FaultException)
            //{
            //    return string.Format(@"服务器内部错误，错误信息：{0}", ex.Message);
            //}
            //if (ex is CommunicationException)
            //{
            //    return "无法连接到服务器";
            //}
            if (ex is TimeoutException)
            {
                return "服务器连接超时";
            }
            if (ex is WebException)
            {
                return "下载失败:" + ex.Message;
            }
            if (ex.InnerException != null)
                return GetMessage(ex.InnerException);
            if (ex is SystemException)
            {
                return "系统错误:" + ex.Message;
            }
            return "发生未处理异常";
        } 
        #endregion
        #region 抛出BUG异常

        /// <summary>
        /// 抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">消息</param>
        /// <param name="args">格式化参数</param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(Func<bool> condition, string message, params object[] args)
        {
            if (condition())
                throw new BugException(message, args);
        }
        /// <summary>
        /// 抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">消息</param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(Func<bool> condition, string message)
        {
            if (condition())
                throw new BugException(message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(Func<bool> condition, Type type, string function, string message)
        {
            if (condition())
                throw new BugException(type, function, message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(Func<bool> condition, string type, string function, string message)
        {
            if (condition())
            throw new BugException(type, function, message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内部异常 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(Func<bool> condition, string message, Exception innerException)
        {
            if (condition())
                throw new BugException(message, innerException);
        }

        /// <summary>
        /// 抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">消息</param>
        /// <param name="args">格式化参数</param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(bool condition, string message, params object[] args)
        {
            if (condition)
                throw new BugException(message, args);
        }
        /// <summary>
        /// 抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">消息</param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(bool condition, string message)
        {
            if (condition)
                throw new BugException(message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(bool condition, Type type, string function, string message)
        {
            if (condition)
                throw new BugException(type, function, message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="message"> 消息 </param>
        /// <param name="type"> 类型 </param>
        /// <param name="function"> 方法或属性 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(string type, string function, string message)
        {
            throw new BugException(type, function, message);
        }
        /// <summary>
        ///   抛出BUG异常
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message"> 消息 </param>
        /// <param name="innerException"> 内部异常 </param>
        [Conditional("DEBUG")]
        public static void ThrowBugException(bool condition, string message, Exception innerException)
        {
            if (condition)
                throw new BugException(message, innerException);
        }
        #endregion
    }
}
