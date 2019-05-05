using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Agebull.EntityModel.Common
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
        public static SqlCommandInfo ToCommandInfo(EntitySubsist subsist,string condition, DbCommand cmd)
        {
            var info = new SqlCommandInfo
            {
                Condition = condition
            };
            foreach (DbParameter para in cmd.Parameters)
            {
                info.Parameters.Add(new ParameterItem
                {
                    Name=para.ParameterName,
                    DbType = para.DbType,
                    Value = para.Value == null || para.Value == DBNull.Value ? null : para.Value.ToString()
                });
            }
            return info;
        }
        
    }
}