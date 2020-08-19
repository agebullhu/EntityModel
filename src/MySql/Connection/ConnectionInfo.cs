using MySql.Data.MySqlClient;
using System;

namespace Agebull.EntityModel.MySql
{
    internal class ConnectionInfo
    {
        /// <summary>
        /// 是否已关闭
        /// </summary>
        internal bool IsClose { get; set; }

        /// <summary>
        /// 连接对象
        /// </summary>
        internal MySqlConnection Connection { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        internal DateTime DateTime { get; set; }
    }
}