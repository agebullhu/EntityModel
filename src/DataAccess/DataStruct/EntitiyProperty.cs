// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示属性结构
    /// </summary>
    public sealed class EntityProperty
    {
        /// <summary>
        ///     标题
        /// </summary>
        private string caption;
        /// <summary>
        ///     说明
        /// </summary>
        private string description;
        private string propertyName;
        private string jsonName;
        private string fieldName;
        private string tableName;
        private bool? canImport;
        private bool? canExport;
        private ReadWriteFeatrue? dbReadWrite;
        private PropertyFeatrue? property;


        //基础定义
        readonly PropertyDefault propertyDefault;

        public EntityProperty(PropertyDefault @default)
        {
            propertyDefault = @default;
        }

        public EntityProperty(PropertyDefault @default, int idx)
        {
            propertyDefault = @default;
            Index = idx;
        }

        public EntityProperty(PropertyDefault @default, int idx, string newName, string newTable, string newField, ReadWriteFeatrue readWrite)
        {
            Index = idx;
            propertyName = newName;
            tableName = newTable;
            fieldName = newField;
            dbReadWrite = readWrite;
            propertyDefault = @default;
        }

        public EntityProperty(PropertyDefault @default, int idx, string newName, string newTable, string newField, ReadWriteFeatrue readWrite, PropertyFeatrue propertyFeatrue)
        {
            Index = idx;
            propertyName = newName;
            tableName = newTable;
            fieldName = newField;
            dbReadWrite = readWrite;
            property =propertyFeatrue;
            propertyDefault = @default;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }


        /// <summary>
        ///     属性实现对应的接口
        /// </summary>
        public string Entity => propertyDefault.Entity;

        /// <summary>
        ///     属性类型
        /// </summary>
        public Type PropertyType => propertyDefault.PropertyType;

        /// <summary>
        ///     数据库类型(直接对应特定数据库的类型,不是通用的DbType)
        /// </summary>
        public int DbType => propertyDefault.DbType;

        /// <summary>
        ///     能否为空
        /// </summary>
        public bool CanNull => propertyDefault.CanNull;

        /// <summary>
        ///     属性类型
        /// </summary>
        public PropertyValueType ValueType => propertyDefault.ValueType;


        /// <summary>
        ///     标题
        /// </summary>
        public string Caption
        {
            get => caption ?? propertyName ?? propertyDefault.Caption ?? propertyDefault.Name;
            set => caption = value;
        }

        /// <summary>
        ///     说明
        /// </summary>
        public string Description
        {
            get => description ?? propertyDefault.Description;
            set => description = value;
        }

        /// <summary>
        ///     名称
        /// </summary>
        public string PropertyName { get => propertyName ?? propertyDefault.Name; set => propertyName = value; }

        /// <summary>
        ///     JSON属性名称
        /// </summary>
        public string JsonName
        {
            get => jsonName ?? propertyDefault.JsonName; set => jsonName = value;
        }

        /// <summary>
        ///     数据库字段名称
        /// </summary>
        public string FieldName { get => fieldName ?? propertyDefault.FieldName; set => fieldName = value; }

        /// <summary>
        ///     数据库表名称
        /// </summary>
        public string TableName { get => tableName ?? propertyDefault.TableName; set => tableName = value; }

        /// <summary>
        /// 数据库读写特性
        /// </summary>
        public ReadWriteFeatrue DbReadWrite { get => dbReadWrite ?? propertyDefault.DbReadWrite; set => dbReadWrite = value; }

        /// <summary>
        ///     能否导入
        /// </summary>
        public bool CanImport { get => canImport ?? propertyDefault.CanImport; set => canImport = value; }

        /// <summary>
        ///     能否导出
        /// </summary>
        public bool CanExport { get => canExport ?? propertyDefault.CanExport; set => canExport = value; }

        /// <summary>
        /// 字段特性
        /// </summary>
        public PropertyFeatrue PropertyFeatrue { get => property ?? propertyDefault.PropertyFeatrue; set => property = value; }

        /// <summary>
        /// 用于查询表达式解析
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsEquals<T>(T value)
        {
            return true;
        }

        /// <summary>
        /// 用于SQL中的文本字段比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public bool Expression<T>(string expression, T value) => true;
    }
}