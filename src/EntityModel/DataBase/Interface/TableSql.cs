// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据表信息
    /// </summary>
    public class DataTableInfomation
    {
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
        public string InsertSqlCode { get; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode { get; }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string FullLoadFields { get; set; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        public Dictionary<string, string> FieldMap => _fieldMap ??= Fields.ToDictionary(p => p, p => p);

    }

}