using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption
    {
        #region 基本设置


        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder SqlBuilder { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => SqlBuilder.DataBaseType;

        /// <summary>
        /// 是否查询
        /// </summary>
        public bool IsQuery { get; set; }

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool CanRaiseEvent { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => DataStruct.IsIdentity;

        /// <summary>
        /// 表配置
        /// </summary>
        public EntityStruct DataStruct { get; set; }

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntityProperty> Properties => DataStruct.Properties;

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntityProperty> PropertyMap { get; protected set; }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> FieldMap { get; protected set; }

        /// <summary>
        ///     主键字段(可动态覆盖 PrimaryProperty)
        /// </summary>
        private string _primaryProperty;

        /// <summary>
        ///     主键属性名称
        /// </summary>
        public string PrimaryProperty
        {
            get => _primaryProperty ?? DataStruct.PrimaryProperty;
            set => _primaryProperty = value;
        }

        /// <summary>
        ///     主键数据库字段名
        /// </summary>
        public string PrimaryDbField
        {
            get;
            set;
        }
        #endregion

        #region 配置项


        /// <summary>
        /// 基本条件
        /// </summary>
        public DynamicOption BaseOption { get; set; }

        /// <summary>
        /// 基本条件
        /// </summary>
        public DynamicOption DynamicOption { get; internal set; }

        /// <summary>
        /// 代码注入配置
        /// </summary>
        public InjectionLevel InjectionLevel => DynamicOption.InjectionLevel;

        /// <summary>
        /// 可读写的属性
        /// </summary>
        public List<EntityProperty> ReadProperties => DynamicOption.ReadProperties;

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition => DynamicOption.BaseCondition;

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName => DynamicOption.ReadTableName;

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName => DynamicOption.WriteTableName;

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields => DynamicOption.LoadFields;

        /// <summary>
        ///     排序字段
        /// </summary>
        public string OrderbyFields => DynamicOption.OrderbyFields;
        
        /// <summary>
        ///     分组字段
        /// </summary>
        public string GroupFields => DynamicOption.GroupFields;

        /// <summary>
        ///     汇总条件
        /// </summary>
        public string Having => DynamicOption.Having;

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateFields => DynamicOption.UpdateFields;

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        public string InsertSqlCode => DynamicOption.InsertSqlCode;

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        public string UpdateSqlCode => DynamicOption.UpdateSqlCode;

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        public string DeleteSqlCode => DynamicOption.DeleteSqlCode;

        #endregion

        #region 初始化

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public DataAccessOption Copy()
        {
            Initiate();
            var option = BaseOption.Copy();
            return new DataAccessOption
            {
                _isInitiated = true,
                DynamicOption = option,
                BaseOption = option,
                IsQuery = IsQuery,
                UpdateByMidified = UpdateByMidified,
                CanRaiseEvent = CanRaiseEvent,
                DataStruct = DataStruct,
                PropertyMap = PropertyMap,
                FieldMap = FieldMap,
                PrimaryProperty = PrimaryProperty,
                PrimaryDbField = PrimaryDbField,
                SqlBuilder = SqlBuilder
            };
        }

        /// <summary>
        /// 初始化标识
        /// </summary>
        bool _isInitiated;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initiate()
        {
            if (_isInitiated)
                return;
            _isInitiated = true;
            SqlBuilder.Option = this;
            if (BaseOption.InjectionLevel == InjectionLevel.None)
                BaseOption.InjectionLevel = InjectionLevel.All;
            DynamicOption = BaseOption;
            FieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PropertyMap = new Dictionary<string, EntityProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Field))
                    continue;
                PropertyMap[pro.FieldName] = PropertyMap[pro.PropertyName] = pro;

                FieldMap[pro.PropertyName] = FieldMap[pro.FieldName] = pro.FieldName;
            }
            PrimaryDbField = FieldMap[PrimaryProperty];
            if (!FieldMap.ContainsKey("id"))
                FieldMap["id"] = PrimaryDbField;
            BaseOption.ReadTableName ??= DataStruct.ReadTableName;
            BaseOption.WriteTableName ??= DataStruct.WriteTableName;
            BaseOption.ReadProperties ??= Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToList();
            BaseOption.LoadFields ??= SqlBuilder.BuilderLoadFields();
            if (IsQuery)
            {
                BaseOption.InsertSqlCode = BaseOption.DeleteSqlCode = BaseOption.UpdateFields = BaseOption.UpdateSqlCode = null;
            }
            else
            {
                BaseOption.InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
                BaseOption.DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
                BaseOption.UpdateFields ??= SqlBuilder.BuilderUpdateFields();
                BaseOption.UpdateSqlCode ??= SqlBuilder.BuilderUpdateCode(UpdateFields, SqlBuilder.PrimaryKeyCondition);
            }
        }

        #endregion

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