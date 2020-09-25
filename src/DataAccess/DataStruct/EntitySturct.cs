// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示实体结构
    /// </summary>
    public class EntitySturct : SimpleConfig
    {
        /// <summary>
        ///     导出的名称
        /// </summary>
        public string ImportName { get; set; }

        /// <summary>
        ///     项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        ///     实体名称
        /// </summary>
        public string EntityName { get => Name; set => Name = value; }

        /// <summary>
        ///     读表名
        /// </summary>
        public string ReadTableName { get; set; }

        /// <summary>
        ///     写表名
        /// </summary>
        public string WriteTableName { get; set; }

        /// <summary>
        ///     实体类型
        /// </summary>
        public int EntityType { get; set; }

        /// <summary>
        /// 是否自增主键
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        ///     主键名称
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     属性
        /// </summary>
        public List<EntitiyProperty> Properties { get; set; }

        /// <summary>
        ///     属性总量
        /// </summary>
        public int Count => Properties.Count;

    }
}