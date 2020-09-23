
using System;
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
        public EntitySturct DataSturct { get; set; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     字段字典
        /// </summary>
        private List<EntitiyProperty> _properties;
        private string readTableName;
        private string writeTableName;

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntitiyProperty> Properties
        {
            get => _properties ?? DataSturct.Properties;
            set => _properties = value;
        }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntitiyProperty> PropertyMap { get; set; }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> FieldMap { get; set; }

        /// <summary>
        ///     主键字段
        /// </summary>
        public string PrimaryKey
        {
            get => _keyField ?? DataSturct.PrimaryKey;
            set => _keyField = value;
        }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName
        {
            get => readTableName ?? DataSturct.ReadTableName;
            set => readTableName = value;
        }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName
        {
            get => writeTableName ?? DataSturct.WriteTableName;
            set => writeTableName = value;
        }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields { get; set; }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode { get; set; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateFields { get; set; }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string DeleteSqlCode { get; set; }

    }
}