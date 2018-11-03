using System;
using System.Collections.Generic;

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    /// 序列化的SQL命令对象
    /// </summary>
    public class SqlCommandInfo
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public List<ParameterItem> Parameters { get; } = new List<ParameterItem>();
    }
}