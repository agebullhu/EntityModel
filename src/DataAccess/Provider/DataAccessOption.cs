using System;
using System.Collections.Generic;
using System.Linq;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据表配置
    /// </summary>
    public class DataTableOption : DynamicOption
    {
        /// <summary>
        /// ID名称
        /// </summary>
        public const string ID = "id";
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
        /// 是否允许数据变更事件
        /// </summary>
        public bool CanRaiseEvent => EventLevel > EventEventLevel.None;

        /// <summary>
        /// 事件参数等级
        /// </summary>
        public EventEventLevel EventLevel { get; set; }

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        /// 表配置
        /// </summary>
        public EntityStruct DataStruct { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => DataStruct.IsIdentity;

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntityProperty> Properties => DataStruct.Properties;

        /// <summary>
        ///     主键属性名称
        /// </summary>
        public string PrimaryProperty => DataStruct.PrimaryProperty;

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntityProperty> PropertyMap { get; protected set; }


        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initiate()
        {
            SqlBuilder.Option = new DataAccessOption
            {
                DynamicOption = this,
                TableOption = this
            };

            if (InjectionLevel == InjectionLevel.None)
                InjectionLevel = InjectionLevel.NoInsert;

            PropertyMap = new Dictionary<string, EntityProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Field))
                    continue;
                if(pro.TableName == null)
                    pro.TableName = DataStruct.ReadTableName;
                PropertyMap[pro.FieldName] = PropertyMap[pro.PropertyName] = pro;
            }

            if (!PropertyMap.ContainsKey(ID))
                PropertyMap[ID] = PropertyMap[PrimaryProperty];

            ReadTableName ??= DataStruct.ReadTableName;
            WriteTableName ??= DataStruct.WriteTableName;
            ReadProperties ??= Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToList();
            LoadFields ??= SqlBuilder.BuilderLoadFields();
            if (IsQuery)
            {
                InsertSqlCode = DeleteSqlCode = UpdateFields = UpdateSqlCode = null;
            }
            else
            {
                InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
                DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
                UpdateFields ??= SqlBuilder.BuilderUpdateFields();
                UpdateSqlCode ??= SqlBuilder.BuilderUpdateCode(UpdateFields, SqlBuilder.PrimaryKeyCondition);
            }
        }
    }

    /// <summary>
    /// 实体事件层级
    /// </summary>
    public enum EventEventLevel
    {
        /// <summary>
        /// 无事件
        /// </summary>
        None,
        /// <summary>
        /// 仅事件名称,无参数
        /// </summary>
        Simple,
        /// <summary>
        /// 事件名称+参数
        /// </summary>
        Details
    }

    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption
    {
        /// <summary>
        /// 构造
        /// </summary>
        public DataAccessOption()
        {

        }

        /// <summary>
        /// 构造
        /// </summary>
        public DataAccessOption(DataTableOption option)
        {
            DynamicOption = TableOption = option;
        }
        #region 基本设置

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder SqlBuilder => TableOption.SqlBuilder;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => TableOption.DataBaseType;

        /// <summary>
        /// 是否查询
        /// </summary>
        public bool IsQuery => TableOption.IsQuery;

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified => TableOption.UpdateByMidified;

        /// <summary>
        /// 是否允许全局事件(如全局事件器,则永为否)
        /// </summary>
        public bool CanRaiseEvent => TableOption.CanRaiseEvent;

        /// <summary>
        /// 事件参数等级
        /// </summary>
        public EventEventLevel EventLevel => TableOption.EventLevel;

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => TableOption.IsIdentity;

        /// <summary>
        /// 表配置
        /// </summary>
        public EntityStruct DataStruct => TableOption.DataStruct;

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntityProperty> Properties => TableOption.Properties;

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntityProperty> PropertyMap => TableOption.PropertyMap;


        /// <summary>
        ///     主键属性名称
        /// </summary>
        public string PrimaryProperty => TableOption.PrimaryProperty;

        #endregion

        #region 配置项

        /// <summary>
        /// 表配置
        /// </summary>
        public DataTableOption TableOption { get; set; }

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
            return new DataAccessOption
            {
                TableOption = TableOption,
                DynamicOption = TableOption
            };
        }
        #endregion

    }
}