
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.SystemModel.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    public partial class SystemDb
    {
        #region 构造
        
        /// <summary>
        /// 构造
        /// </summary>
        public SystemDb()
        {
            /*_tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase)
            {SystemDb
            };*/
            Initialize();
            //RegistToEntityPool();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize();

        #endregion

        #region 表相关对象
        /// <summary>
        /// 系统关联的表名的枚举表示方式
        /// </summary>
        public enum EnumTables
        {

            /// <summary>
            /// 数据字典
            /// </summary>
            DataDictionary = 0,

            /// <summary>
            /// 数据字典
            /// </summary>
            数据字典 = DataDictionary,

            /// <summary>
            /// 数据字典
            /// </summary>
            ST_Dictionary = DataDictionary,

            /// <summary>
            /// 用户的页面数据
            /// </summary>
            PageData = 1,

            /// <summary>
            /// 用户的页面数据
            /// </summary>
            用户的页面数据 = PageData,

            /// <summary>
            /// 用户的页面数据
            /// </summary>
            ST_PageData = PageData,

            /// <summary>
            /// 页面节点
            /// </summary>
            PageItem = 2,

            /// <summary>
            /// 页面节点
            /// </summary>
            页面节点 = PageItem,

            /// <summary>
            /// 页面节点
            /// </summary>
            st_pageitem = PageItem
        }
        #endregion

    }
}
