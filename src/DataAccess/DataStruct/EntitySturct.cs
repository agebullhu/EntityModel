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
        ///     实现的接口特性
        /// </summary>
        /// <remarks>
        /// 接口特性：
        /// 类似于接口的功能，但可能只从逻辑上保证接口特性，并不一定在数据模型上实现接口
        /// 如实现IHistoryData接口，即为一个接口特性
        /// 可通过接口特性完成SQL注入功能，从而独立化一些特殊功能
        /// 如：要自动记录操作记录信息，只需要数据库有对应字段（create_date\create_user\update_date\update_user）
        /// 接口器检查到此接口特性，即自动在Insert与Update语句中注入对应代码，实现自动化操作
        /// 又如：逻辑删除，可通过检查特性来自动设置强查询条件is_delete=0
        /// 再如：组织行级数据权限，可通过读取上下文中的角色设置与检查特性来自动设置强查询条件org_id=当前组织，
        /// 写入数据时自动写入组织信息
        /// </remarks>
        public string[] InterfaceFeatures { get; set; }

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