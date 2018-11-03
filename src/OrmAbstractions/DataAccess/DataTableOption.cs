using System.Collections.Generic;
using System.Linq;

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    /// 数据表配置
    /// </summary>
    public abstract class DataTableOption
    {
        /// <summary>
        /// 格式化为符合SQL的名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ToSqlName(string name) => $"{FieldLeftChar}{name}{FieldRightChar}";

        /// <summary>
        /// 格式化为符合SQL的名称
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string PropertyToSqlName(string property) => $"{FieldLeftChar}{FieldDictionary[property]}{FieldRightChar}";


        /// <summary>
        /// 字段左侧字符
        /// </summary>
        public char FieldLeftChar => '[';

        /// <summary>
        /// 字段右侧字符
        /// </summary>
        public char FieldRightChar => ']';

        /// <summary>
        /// 参数前导字符
        /// </summary>
        public char ArgumentChar => '$';

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        protected abstract string FullLoadFields { get; }

        /// <summary>
        ///     读表名
        /// </summary>
        protected abstract string ReadTableName { get; }
        /// <summary>
        ///     写表名
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected virtual string DeleteSqlCode => $@"DELETE FROM [{WriteTableName}]";

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }

        #region 数据结构

        /// <summary>
        ///     字段字典(运行时)
        /// </summary>
        public Dictionary<string, string> FieldDictionary => OverrideFieldMap ?? FieldMap;

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get
            {
                if (_keyField != null)
                    return _keyField;
                return _keyField = PrimaryKey;
            }
            set => _keyField = value;
        }

        #endregion

        #region 字段字典

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return _fieldMap ?? (_fieldMap = Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     字段字典(动态覆盖)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }


        #endregion
    }
}