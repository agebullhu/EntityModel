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
        private bool? canImport;
        private bool? canExport;
        private ReadWriteFeatrue? dbReadWrite;
        //基础定义
        PropertyDefault propertyDefault;

        public EntityProperty(PropertyDefault @default)
        {
            propertyDefault = @default;
        }

        public EntityProperty(PropertyDefault @default, int idx, string newName = null, string newField = null)
        {
            Index = idx;
            propertyName = newName;
            fieldName = newField;
            propertyDefault = @default;
        }


        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }


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
        public string PropertyName
        {
            get => propertyName ?? propertyDefault.Name;
            set => propertyName = value;
        }

        /// <summary>
        ///     JSON属性名称
        /// </summary>
        public string JsonName { get => jsonName ?? propertyDefault.JsonName; set => jsonName = value; }

        /// <summary>
        ///     数据库字段名称
        /// </summary>
        public string FieldName { get => fieldName ?? propertyDefault.FieldName; set => fieldName = value; }

        /// <summary>
        /// 数据库读写特性
        /// </summary>
        public ReadWriteFeatrue DbReadWrite
        {
            get => dbReadWrite.HasValue ? dbReadWrite.Value : propertyDefault.DbReadWrite;
            set => dbReadWrite = value;
        }

        /// <summary>
        ///     能否导入
        /// </summary>
        public bool CanImport
        {
            get => canImport.HasValue ? canImport.Value : propertyDefault.CanImport;
            set => canImport = value;
        }

        /// <summary>
        ///     能否导出
        /// </summary>
        public bool CanExport
        {
            get => canExport.HasValue ? canExport.Value : propertyDefault.CanExport;
            set => canExport = value;
        }

        /// <summary>
        /// 字段特性
        /// </summary>
        public PropertyFeatrue PropertyFeatrue => propertyDefault.PropertyFeatrue;

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
    }
}