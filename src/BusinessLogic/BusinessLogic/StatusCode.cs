// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     操作状态码
    /// </summary>
    public class StatusCode
    {
        /// <summary>
        ///     正在排队
        /// </summary>
        public const int Queue =  1;

        /// <summary>
        ///     成功
        /// </summary>
        public const int Success =  0;

        /// <summary>
        ///     参数错误
        /// </summary>
        public const int ArgumentError = /*ArgumentError*/-1;

        /// <summary>
        ///     发生处理业务错误
        /// </summary>
        public const int BusinessError = /*BusinessError*/-2;

        /// <summary>
        ///     发生未处理业务异常
        /// </summary>
        public const int BusinessException = /*BusinessException*/-3;

        /// <summary>
        ///     发生未处理系统异常
        /// </summary>
        public const int UnhandleException = /*UnhandleException*/-4;

        /// <summary>
        ///     网络错误
        /// </summary>
        public const int NetworkError = /*NetworkError*/-5;

        /// <summary>
        ///     执行超时
        /// </summary>
        public const int TimeOut = /*TimeOut*/-6;

        /// <summary>
        ///     拒绝访问
        /// </summary>
        public const int DenyAccess = /*DenyAccess*/-7;

        /// <summary>
        ///     未知的令牌
        /// </summary>
        public const int TokenUnknow = /*TokenUnknow*/-8;

        /// <summary>
        ///     令牌过期
        /// </summary>
        public const int TokenTimeOut = /*TokenTimeOut*/-9;

        /// <summary>
        ///     系统未就绪
        /// </summary>
        public const int NoReady = /*NoReady*/-0xA;

        /// <summary>
        ///     异常中止
        /// </summary>
        public const int Ignore = /*Ignore*/-0xB;

        /// <summary>
        ///     重试
        /// </summary>
        public const int ReTry = /*ReTry*/-0xC;

        /// <summary>
        ///     方法不存在
        /// </summary>
        public const int NoFind = /*NoFind*/-0xD;

        /// <summary>
        ///     服务不可用
        /// </summary>
        public const int Unavailable = /*Unavailable*/-0xE;

        /// <summary>
        ///     未知结果
        /// </summary>
        public const int Unknow = /*Unknow*/0xF;

    }
}