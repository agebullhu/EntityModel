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
    public sealed class EntitySturct : SimpleConfig
    {
        /// <summary>
        ///     实体名称
        /// </summary>
        public string EntityName { get => Name; set => Name = value; }

        /// <summary>
        ///     实体类型
        /// </summary>
        public int EntityType { get; set; }

        /// <summary>
        ///     主键名称
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     属性
        /// </summary>
        public Dictionary<int, PropertySturct> Properties { get; set; }
    }
}