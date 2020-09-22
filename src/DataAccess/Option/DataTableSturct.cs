// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示数据库实体结构
    /// </summary>
    public sealed class DataTableSturct : EntitySturct
    {

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> FieldMap { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        public override void Init()
        {
            FieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pro in Properties.Values)
            {
                if (!pro.Featrue.HasFlag(PropertyFeatrue.DbCloumn))
                    continue;
                FieldMap[pro.ColumnName] = pro.ColumnName;
                FieldMap[pro.PropertyName] = pro.ColumnName;
            }
            base.Init();
        }

        #region 数据库

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName { get; set; }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName { get; set; }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode { get; set; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode { get; set; }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string FullLoadFields { get; set; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }
        #endregion
    }
}