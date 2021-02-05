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

            PropertyMap = new Dictionary<string, EntityProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Field))
                    continue;
                if (pro.TableName == null)
                    pro.TableName = DataStruct.ReadTableName;
                PropertyMap[pro.FieldName] = PropertyMap[pro.PropertyName] = pro;
                if (pro.JsonName.IsNotNull())
                    PropertyMap[pro.JsonName] = pro;
            }

            if (!PropertyMap.ContainsKey(ID))
                PropertyMap[ID] = PropertyMap[PrimaryProperty];

            ReadTableName ??= DataStruct.ReadTableName;
            WriteTableName ??= DataStruct.WriteTableName;
            ReadProperties ??= Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToList();
            LoadFields ??= SqlBuilder.BuilderLoadFields();
            if (IsQuery)
            {
                InsertValueCode = InsertFieldCode = DeleteSqlCode = UpdateFields = UpdateSqlCode = null;
            }
            else
            {
                var (fields, values) = SqlBuilder.BuilderInsertSqlCode();
                InsertFieldCode ??= fields;
                InsertValueCode ??= values;
                DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
                UpdateFields ??= SqlBuilder.BuilderUpdateFields();
                UpdateSqlCode ??= SqlBuilder.BuilderUpdateCode(UpdateFields, SqlBuilder.PrimaryKeyCondition);
            }
        }
    }
}