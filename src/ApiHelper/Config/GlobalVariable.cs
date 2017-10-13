using System;
using System.Configuration;

namespace GoodLin.Common.Configuration
{
    /// <summary>
    /// 全局配置变量
    /// </summary>
    public class GlobalVariable
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string ConnectionString;
        /// <summary>
        /// 当前服务的标识
        /// </summary>
        public static readonly Guid ServiceKey;
        /// <summary>
        /// 是否启用AT校验
        /// </summary>
        public static readonly bool EnableOAuthATValidation;
        /// <summary>
        /// 是否启用ClientKey校验
        /// </summary>
        public static readonly bool EnableClientKeyValidation;
        /// <summary>
        /// AT加密密钥
        /// </summary>
        public static readonly string Secret;
        /// <summary>
        /// REDIS连接字符串
        /// </summary>
        public static readonly string RedisConnectionString;
        /// <summary>
        /// REDIS缓存小时数（不应该这样用）
        /// </summary>
        public static readonly int RedisCacheHours;
        /// <summary>
        /// 是否使用Redis
        /// </summary>
        public static readonly bool UseRedis;
        /// <summary>
        /// RT有效时长
        /// </summary>
        public static readonly int RefreshTokenExpiresDayCount;
        /// <summary>
        /// AT有效时长
        /// </summary>
        public static readonly int AccessTokenExpiresHourCount;
        /// <summary>
        /// OAuth令牌缓存有效时长
        /// </summary>
        public static readonly int OAuthRedisCacheMinutes;
        /// <summary>
        /// OAuth服务根地址
        /// </summary>
        public static string OAuthServiceURL;

        static GlobalVariable()
        {
            Secret = ConfigurationManager.AppSettings["Secret"];
            RedisConnectionString = ConfigurationManager.AppSettings["RedisConnectionString"];
            ConnectionString = ConfigurationManager.AppSettings["GLDbContext"];
            OAuthServiceURL = ConfigurationManager.AppSettings["OAuthServiceURL"];
            if (!Guid.TryParse(ConfigurationManager.AppSettings["ServiceKey"], out ServiceKey))
                ServiceKey = Guid.NewGuid();
            bool.TryParse(ConfigurationManager.AppSettings["EnableOAuthATValidation"], out EnableOAuthATValidation);
            bool.TryParse(ConfigurationManager.AppSettings["EnableClientKeyValidation"], out EnableClientKeyValidation);
            int.TryParse(ConfigurationManager.AppSettings["RedisCacheHours"], out RedisCacheHours);
            bool.TryParse(ConfigurationManager.AppSettings["UseRedis"], out UseRedis);
            int.TryParse(ConfigurationManager.AppSettings["RefreshTokenExpiresDayCount"], out RefreshTokenExpiresDayCount);
            int.TryParse(ConfigurationManager.AppSettings["AccessTokenExpiresHourCount"], out AccessTokenExpiresHourCount);
            int.TryParse(ConfigurationManager.AppSettings["OAuthRedisCacheMinutes"], out OAuthRedisCacheMinutes);
        }
    }
}
