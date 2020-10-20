﻿using System;
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
        #region 配置项

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
        /// 不做代码注入
        /// </summary>
        public bool NoInjection { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity => DataStruct.IsIdentity;

        /// <summary>
        /// 表配置
        /// </summary>
        public EntityStruct DataStruct { get; set; }

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
            get => _properties ?? DataStruct.Properties;
            set => _properties = value;
        }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, EntityProperty> PropertyMap { get; protected set; }

        /// <summary>
        /// 可读写的属性
        /// </summary>
        public List<EntityProperty> ReadProperties { get; protected set; }

        /// <summary>
        ///     属性字典
        /// </summary>
        public Dictionary<string, string> FieldMap { get; protected set; }

        /// <summary>
        ///     主键字段
        /// </summary>
        public string PrimaryKey
        {
            get => _keyField ?? DataStruct.PrimaryKey;
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
            get => readTableName ?? DataStruct.ReadTableName;
            set => readTableName = value;
        }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName
        {
            get => writeTableName ?? DataStruct.WriteTableName;
            set => writeTableName = value;
        }

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        public string LoadFields { get; set; }

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

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder SqlBuilder { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => SqlBuilder.DataBaseType;
        #endregion

        #region 初始化

        bool _isInitiated;

        /// <summary>
        /// 初始化
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
                PropertyMap[pro.FieldName] = PropertyMap[pro.PropertyName] = pro;

                FieldMap[pro.PropertyName] = FieldMap[pro.FieldName] = pro.FieldName;
            }
            ReadProperties ??= Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToList();
            LoadFields ??= SqlBuilder.BuilderLoadFields();
            if (!IsQuery)
            {
                InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
                DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
                UpdateFields ??= SqlBuilder.BuilderUpdateFields();
                UpdateSqlCode ??= SqlBuilder.CreateUpdateSqlCode(UpdateFields, SqlBuilder.PrimaryKeyCondition);
            }
            else
            {
                InsertSqlCode = DeleteSqlCode = UpdateFields = UpdateSqlCode = null;
            }

        }
        /// <summary>
        /// 设置字段
        /// </summary>
        /// <param name="fields"></param>
        public void Select(params string[] fields)
        {
            ReadProperties = new List<EntityProperty>();
            foreach(var field in fields)
            {
                if (PropertyMap.TryGetValue(field, out var property))
                    ReadProperties.Add(property);
            }
            LoadFields = SqlBuilder.BuilderLoadFields();
        }

        /// <summary>
        /// 设置字段
        /// </summary>
        /// <param name="fields"></param>
        public void SelectAll()
        {
            ReadProperties = Properties.Where(pro => pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToList();
            LoadFields = SqlBuilder.BuilderLoadFields();
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