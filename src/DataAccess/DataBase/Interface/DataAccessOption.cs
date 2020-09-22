
using System.Collections.Generic;
namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption
    {
        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => DataSturct.IsIdentity;

        /// <summary>
        /// 不做代码注入
        /// </summary>
        public bool NoInjection { get; set; }

        /// <summary>
        /// 表配置
        /// </summary>
        public DataTableSturct DataSturct { get; set; }

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

        /// <summary>
        ///     主键字段
        /// </summary>
        public string PrimaryKey
        {
            get => _keyField ?? DataSturct.PrimaryKey;
            set => _keyField = value;
        }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        public Dictionary<string, string> FieldMap
        {
            get => _fieldMap ?? DataSturct.FieldMap;
            set => _fieldMap = value;
        }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields { get => loadFields ?? DataSturct.FullLoadFields; set => loadFields = value; }

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName { get => readTableName ?? DataSturct.ReadTableName; set => readTableName = value; }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName { get => writeTableName ?? DataSturct.WriteTableName; set => writeTableName = value; }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode { get => insertSqlCode ?? DataSturct.InsertSqlCode; set => insertSqlCode = value; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode { get => updateSqlCode ?? DataSturct.UpdateSqlCode; set => updateSqlCode = value; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get => baseCondition ?? DataSturct.BaseCondition; set => baseCondition = value; }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string DeleteSqlCode => $@"DELETE FROM {ReadTableName}";

    }
}