using System;

namespace Agebull.Common
{
    /// <summary>
    /// Unix时间互转
    /// </summary>
    public static class UnixTimeConvert
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="unix">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertUnixTimeToDateTime(this double unix)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unix);
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static double ConvertDateTimeToUnixTime(this DateTime time)
        {
            return (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
