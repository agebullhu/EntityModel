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
    ///     属性特性
    /// </summary>
    [Flags]
    public enum PropertyFeatrue
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 数据库列
        /// </summary>
        DbCloumn,

        /// <summary>
        /// 属性
        /// </summary>
        Property,

        /// <summary>
        /// 接口
        /// </summary>
        Interface,

        /// <summary>
        /// 别名
        /// </summary>
        Alias
    }
}