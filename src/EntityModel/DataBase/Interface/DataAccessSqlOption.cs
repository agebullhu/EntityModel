// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using System.Data.Common;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public abstract class DataAccessSqlOption<TEntity>
        where TEntity : EditDataObject
    {
        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 表配置
        /// </summary>
        public DataTableInfomation TableOption { get; set; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;
        private string loadFields;
        private string readTableName;
        private string writeTableName;
        private string updateSqlCode;
        private string insertSqlCode;
        private string baseCondition;
        private string[] fields;

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        public string[] Fields { get => fields ?? TableOption.Fields; set => fields = value; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get => _keyField ?? TableOption.PrimaryKey;
            set => _keyField = value;
        }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        public Dictionary<string, string> FieldMap
        {
            get => _fieldMap ?? TableOption.FieldMap;
            set => _fieldMap = value;
        }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields { get => loadFields ?? TableOption.FullLoadFields; set => loadFields = value; }

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName { get => readTableName ?? TableOption.ReadTableName; set => readTableName = value; }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName { get => writeTableName ?? TableOption.WriteTableName; set => writeTableName = value; }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode { get => insertSqlCode ?? TableOption.InsertSqlCode; set => insertSqlCode = value; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode { get => updateSqlCode ?? TableOption.UpdateSqlCode; set => updateSqlCode = value; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get => baseCondition ?? TableOption.BaseCondition; set => baseCondition = value; }



        #region 方法实现


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public abstract void LoadEntity(DbDataReader reader, TEntity entity);

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public abstract void SetEntityParameter(DbCommand cmd, TEntity entity);

        #endregion
    }

}