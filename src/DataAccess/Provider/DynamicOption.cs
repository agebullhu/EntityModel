using System.Collections.Generic;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 动态配置
    /// </summary>
    public class DynamicOption
    {
        #region 配置项

        /// <summary>
        /// 代码注入配置
        /// </summary>
        public InjectionLevel InjectionLevel { get; set; }

        /// <summary>
        /// 可读写的属性
        /// </summary>
        public List<EntityProperty> ReadProperties { get; set; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName { get; set; }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName { get; set; }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields { get; set; }

        /// <summary>
        ///     排序字段
        /// </summary>
        public string OrderbyFields { get; set; }

        /// <summary>
        ///     分组字段
        /// </summary>
        public string GroupFields { get; set; }

        /// <summary>
        ///     汇总条件
        /// </summary>
        public string Having { get; set; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateFields { get; set; }

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode { get; set; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode { get; set; }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string DeleteSqlCode { get; set; }

        #endregion

        #region 复制

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public DynamicOption Copy()
        {
            return new DynamicOption
            {
                InjectionLevel = InjectionLevel,
                ReadProperties = ReadProperties,
                BaseCondition = BaseCondition,
                ReadTableName = ReadTableName,
                WriteTableName = WriteTableName,
                LoadFields = LoadFields,
                OrderbyFields = OrderbyFields,
                GroupFields = GroupFields,
                Having = Having,
                UpdateFields = UpdateFields,
                InsertSqlCode = InsertSqlCode,
                UpdateSqlCode = UpdateSqlCode,
                DeleteSqlCode = DeleteSqlCode
            };
        }

        #endregion
    }

}