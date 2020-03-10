using Agebull.Common.Configuration;
using System;
using System.IO;
using System.Text;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数字与BYTE的转换辅助对象
    /// </summary>
    public class RedisOption
    {
        #region 配置

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <example>
        /// $"{Address}:{Port},password={PassWord},defaultDatabase={db},poolsize=50,ssl=false,writeBuffer=10240";
        /// </example>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 系统数据
        /// </summary>
        public int DbSystem { get; set; }= 15;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public int DbWebCache { get; set; } = 14;

        /// <summary>
        /// 权限数据
        /// </summary>
        public int DbAuthority { get; set; } = 13;

        /// <summary>
        /// WEB端的缓存
        /// </summary>
        public int DbComboCache { get; set; } = 12;

        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string ConnectionName { get; set; }= "Redis";

        #endregion

    }
}
