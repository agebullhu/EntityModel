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
        None = 0x0,

        /// <summary>
        /// 数据库列
        /// </summary>
        Field = 0x1,

        /// <summary>
        /// 属性
        /// </summary>
        Property = 0x2,

        /// <summary>
        /// 接口
        /// </summary>
        Interface = 0x4
    }
}