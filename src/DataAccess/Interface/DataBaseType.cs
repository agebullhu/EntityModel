using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    [Flags]
    public enum DataBaseType
    {
        /// <summary>
        /// 不支持
        /// </summary>
        None = 0x0,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 0x1,
        /// <summary>
        /// SqlServer
        /// </summary>
        SqlServer = 0x2,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite = 0x4,
        /// <summary>
        /// 全支持
        /// </summary>
        Full = 0xFF
    }
}