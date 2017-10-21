using System;
using System.Configuration;
using System.Diagnostics;

namespace GoodLin.Common.Configuration
{
    /// <summary>
    /// 全局配置变量
    /// </summary>
    public class GlobalVariable
    {
        /// <summary>
        /// 当前服务的标识
        /// </summary>
        public static readonly Guid ServiceKey;

        /// <summary>
        /// OAuth服务根地址
        /// </summary>
        public static string OAuthServiceURL;

        static GlobalVariable()
        {
            if (!Guid.TryParse(ConfigurationManager.AppSettings["ServiceKey"], out ServiceKey))
                throw new ConfigurationErrorsException("必须在AppSettings中配置ServiceKey项");
            OAuthServiceURL = ConfigurationManager.AppSettings["OAuthServiceURL"];
            if (string.IsNullOrWhiteSpace(OAuthServiceURL))
                throw new ConfigurationErrorsException("必须在AppSettings中配置OAuthServiceURL项");
        }
    }
}
