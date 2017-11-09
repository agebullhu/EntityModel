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
        public static readonly string ServiceKey;

        /// <summary>
        /// OAuth服务根地址
        /// </summary>
        public static string OAuthServiceURL;

        static GlobalVariable()
        {
            ServiceKey = ConfigurationManager.AppSettings["ServiceKey"];
            if (string.IsNullOrWhiteSpace(ServiceKey))
                throw new ConfigurationErrorsException("必须在AppSettings中配置ServiceKey项");
            OAuthServiceURL = ConfigurationManager.AppSettings["OAuthServiceURL"];
            if (string.IsNullOrWhiteSpace(OAuthServiceURL))
                throw new ConfigurationErrorsException("必须在AppSettings中配置OAuthServiceURL项");
        }
    }
}
