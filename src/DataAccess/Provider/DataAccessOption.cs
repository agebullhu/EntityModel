using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption
    {
        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool CanRaiseEvent { get; set; }

        /// <summary>
        /// 不做代码注入
        /// </summary>
        public bool NoInjection { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => DataSturct.IsIdentity;

        /// <summary>
        /// 表配置
        /// </summary>
        public EntityStruct DataSturct { get; set; }

        /// <summary>
        ///     主键字段(可动态覆盖PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     字段字典
        /// </summary>
        private List<EntityProperty> _properties;
        private string readTableName;
        private string writeTableName;

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntityProperty> Properties
        {
            get => _properties ?? DataSturct.Properties;
            set => _properties = value;
        }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntityProperty> PropertyMap { get; protected set; }

        /// <summary>
        /// 可读写的属性
        /// </summary>
        public EntityProperty[] ReadPproperties { get; protected set; }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> FieldMap { get; protected set; }

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

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder SqlBuilder { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => SqlBuilder.DataBaseType;

        bool _isInitiated;

        /// <summary>
        /// 构造
        /// </summary>
        public void Initiate()
        {
            if (_isInitiated)
                return;
            _isInitiated = true;

            FieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PropertyMap = new Dictionary<string, EntityProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Field))
                    continue;
                PropertyMap[pro.FieldName] = pro;
                PropertyMap[pro.PropertyName] = pro;

                FieldMap[pro.FieldName] = pro.FieldName;
                FieldMap[pro.PropertyName] = pro.FieldName;
            }

            LoadFields ??= SqlBuilder.BuilderLoadFields();
            InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
            DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
            UpdateFields ??= SqlBuilder.BuilderUpdateFields();
            UpdateSqlCode ??= SqlBuilder.CreateUpdateSqlCode(UpdateFields, SqlBuilder.PrimaryKeyCondition);

            ReadPproperties ??= Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToArray();

        }

        #region 迭代

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachProperties(PropertyFeatrue propertyFeatrue, Action<EntityProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(propertyFeatrue))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachProperties(PropertyFeatrue propertyFeatrue, ReadWriteFeatrue readWrite, Action<EntityProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(propertyFeatrue) && pro.DbReadWrite.HasFlag(readWrite))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachDbProperties(Action<EntityProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public void FroeachDbProperties(ReadWriteFeatrue readWrite, Action<EntityProperty> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(readWrite))
                    action(pro);
            }
        }

        /// <summary>
        /// 迭代循环属性
        /// </summary>
        public async Task FroeachDbProperties(ReadWriteFeatrue readWrite, Func<EntityProperty, Task> action)
        {
            var properties = Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(readWrite))
                    await action(pro);
            }
        }
        #endregion
    }
}