// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;

#endregion

namespace Agebull.EntityModel.Common
{

    /// <summary>
    ///     表示属性结构
    /// </summary>
    public sealed class EntitiyProperty : SimpleConfig
    {
        /// <summary>
        /// 数据库读写特性
        /// </summary>
        public ReadWriteFeatrue DbReadWrite { get; set; }

        /// <summary>
        /// 字段特性
        /// </summary>
        public PropertyFeatrue PropertyFeatrue { get; set; }

        /// <summary>
        ///     连接字段
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     属性实现对应的接口
        /// </summary>
        public string[] Interfaces { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title { get => Caption; set => Caption = value; }

        /// <summary>
        ///     属性名称
        /// </summary>
        public string PropertyName { get => Name; set => Name = value; }

        /// <summary>
        ///     JSON属性名称
        /// </summary>
        public string JsonName { get; set; }

        /// <summary>
        ///     数据库字段名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     属性类型
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        ///     数据库类型(直接对应特定数据库的类型,不是通用的DbType)
        /// </summary>
        public int DbType { get; set; }

        /// <summary>
        ///     索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     索引
        /// </summary>
        public int PropertyIndex { get; set; }

        /// <summary>
        ///     能否为空
        /// </summary>
        public bool CanNull { get; set; }

        /// <summary>
        ///     属性类型
        /// </summary>
        public PropertyValueType ValueType { get; set; }

        /// <summary>
        ///     能否导入
        /// </summary>
        public bool CanImport { get; set; }

        /// <summary>
        ///     能否导出
        /// </summary>
        public bool CanExport { get; set; }
    }
}