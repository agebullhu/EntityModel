using System.Collections.Generic;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     系统错误代码
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        ///     正确
        /// </summary>
        public const int Success = 0;

        /// <summary>
        ///     发生异常
        /// </summary>
        public const int LocalException = -1;

        /// <summary>
        ///     逻辑错误
        /// </summary>
        public const int LogicalError = -2;

        /// <summary>
        ///     远端错误
        /// </summary>
        public const int RemoteError = -3;

        /// <summary>
        ///     本地错误
        /// </summary>
        public const int LocalError = -4;

        /// <summary>
        ///     网络错误
        /// </summary>
        public const int NetworkError = -5;

        /// <summary>
        ///     参数错误
        /// </summary>
        public const int ArgumentError = -6;

        /// <summary>
        ///     系统未就绪
        /// </summary>
        public const int NoReady = -0xA;

        /// <summary>
        ///     客户端应中止请求
        /// </summary>
        public const int Ignore = -0xB;

        /// <summary>
        ///     客户端应重新请求
        /// </summary>
        public const int ReTry = -0xC;

        /// <summary>
        ///     拒绝访问
        /// </summary>
        public const int DenyAccess = -0xD;

        /// <summary>
        ///     方法不存在
        /// </summary>
        public const int NoFind = 404;

        /// <summary>
        ///     服务不可用
        /// </summary>
        public const int Unavailable = 503;

        /// <summary>
        ///     未知的Token
        /// </summary>
        public const int Auth_UnknowToken = 40001;

        /// <summary>
        ///     未知的用户
        /// </summary>
        public const int Auth_User_Unknow = 40421;

        /// <summary>
        ///     未知的设备识别码
        /// </summary>
        public const int Auth_Device_Unknow = 40022;

        /// <summary>
        ///     令牌过期
        /// </summary>
        public const int Auth_AccessToken_TimeOut = 40036;

        /// <summary>
        ///     未知的ServiceKey
        /// </summary>
        public const int Auth_ServiceKey_Unknow = 40082;

        /// <summary>
        ///     未知的AccessToken
        /// </summary>
        public const int Auth_AccessToken_Unknow = 40081;

        /// <summary>
        ///     未知的RefreshToken
        /// </summary>
        public const int Auth_RefreshToken_Unknow = 40083;

        #region 消息字典

        private static readonly Dictionary<int, string> Map = new Dictionary<int, string>
        {
            {Success, "操作成功"},
            {LocalException, "发生异常"},
            {LogicalError, "逻辑错误"},
            {RemoteError, "服务器外部错误"},
            {LocalError, "服务器一般错误"},
            {NetworkError, "网络错误"},
            {ArgumentError, "参数错误"},
            {DenyAccess, "拒绝访问"},
            {NoReady, "系统未就绪"},
            {ReTry, "客户端应重新请求"},
            {Ignore, "客户端应中止请求"},
            {NoFind, "页面不存在"},
            {Unavailable, "服务不可用"},
            {Auth_ServiceKey_Unknow, "未知的ServiceKey"},
            {Auth_UnknowToken, "未知的Token"},
            {Auth_RefreshToken_Unknow, "未知的RefreshToken"},
            {Auth_AccessToken_Unknow, "未知的AccessToken"},
            {Auth_User_Unknow, "未知的用户"},
            {Auth_Device_Unknow, "未知的设备识别码"},
            {Auth_AccessToken_TimeOut, "令牌过期"}
        };

        /// <summary>
        ///     取得错误码对应的消息文本
        /// </summary>
        /// <param name="eid">错误码</param>
        /// <returns>消息文本</returns>
        public static string GetMessage(int eid)
        {
            return Map.TryGetValue(eid, out var result) ? result : "发生未知错误";
        }

        #endregion
    }
}