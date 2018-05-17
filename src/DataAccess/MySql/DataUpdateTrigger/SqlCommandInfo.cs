using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
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

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="subsist"></param>
        /// <param name="condition"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static SqlCommandInfo ToCommandInfo(EntitySubsist subsist,string condition, MySqlCommand cmd)
        {
            var info = new SqlCommandInfo
            {
                Condition = condition
            };
            foreach (MySqlParameter para in cmd.Parameters)
            {
                info.Parameters.Add(new ParameterItem
                {
                    Name=para.ParameterName,
                    DbType = para.MySqlDbType,
                    Value = para.Value == null || para.Value == DBNull.Value ? null : para.Value.ToString()
                });
            }
            return info;
        }
        
    }

    /// <summary>
    /// 参数节点
    /// </summary>
    public class ParameterItem
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public MySqlDbType DbType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

}