using System;
using System.Text;

namespace Agebull.Common.Redis
{
    /// <summary>
    /// Redis的Key生成器
    /// </summary>
    public class RedisKeyBuilder
    {
        /// <summary>
        /// 系统数据
        /// </summary>
        public const int DbSystem = 15;

        /// <summary>
        /// 上下文数据
        /// </summary>
        public const int DbContext = 11;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public const int DbWebCache = 14;

        /// <summary>
        /// 权限数据
        /// </summary>
        public const int DbAuthority = 13;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public const int DbComboCache = 12;

        /// <summary>
        /// 系统分区
        /// </summary>
        public const string SystemRegion = "sy";

        /// <summary>
        /// 用户身份分区
        /// </summary>
        public const string AuthRegion = "au";

        /// <summary>
        /// 业务分区
        /// </summary>
        public const string BusinessRegion = "bu";

        /// <summary>
        /// 生成系统分区的键
        /// </summary>
        /// <returns></returns>
        public static string ToSystemKey(params object[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("参数不能为空");
            StringBuilder code = new StringBuilder();
            code.Append(SystemRegion);
            foreach (var arg in args)
            {
                code.Append(':');
                code.Append(arg);
            }
            return code.ToString();
        }
        /// <summary>
        /// 生成用户身份分区的键
        /// </summary>
        /// <returns></returns>
        public static string ToAuthKey(params object[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("参数不能为空");
            StringBuilder code = new StringBuilder();
            code.Append(AuthRegion);
            foreach (var arg in args)
            {
                code.Append(':');
                code.Append(arg);
            }
            return code.ToString();
        }
        /// <summary>
        /// 生成业务分区的键
        /// </summary>
        /// <returns></returns>
        public static string ToBusinessKey(params object[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("参数不能为空");
            StringBuilder code = new StringBuilder();
            code.Append(BusinessRegion);
            foreach (var arg in args)
            {
                code.Append(':');
                code.Append(arg);
            }
            return code.ToString();
        }
    }
}
