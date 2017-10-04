using MS.Lib.BusinessEntity;
using MS.Lib.Utility;

namespace MS.Lib
{
    /// <summary>
    /// MsLib相关对象的帮助类
    /// </summary>
    public static class MsLibHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string _connectionString;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _connectionString ??
                       (_connectionString =
                           ApplicationInfo.SystemEnvironmentManagerRef.GetValue("/SystemEnvironment/Connection/DataSource"));
            }
        }
        /// <summary>
        /// 数据库执行超时
        /// </summary>
        private static int _commandTimeout;
        /// <summary>
        /// 数据库执行超时
        /// </summary>
        public static int CommandTimeout
        {
            get
            {
                if (_commandTimeout != 0)
                    return _commandTimeout;

                string s = ApplicationInfo.SystemEnvironmentManagerRef.GetValue("/SystemEnvironment/Connection/SQLTimeOut");
                if (!int.TryParse(s, out _commandTimeout))
                {
                    _commandTimeout = -1;
                }
                return _commandTimeout;
            }
        }
        /// <summary>
        /// 构建一个数据库连接对象
        /// </summary>
        /// <returns></returns>
        public static MSConnectionManager CreateConnection()
        {
            return new MSConnectionManager(ConnectionString)
            {
                CommandTimeout = CommandTimeout
            };
        }
    }
}