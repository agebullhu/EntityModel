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
    ///     读写特性
    /// </summary>
    [Flags]
    public enum ReadWriteFeatrue
    {
        /// <summary>
        /// 不读写
        /// </summary>
        None = 0,

        /// <summary>
        /// 可读
        /// </summary>
        Read = 0x1,

        /// <summary>
        /// 插入
        /// </summary>
        Insert = 0x2,

        /// <summary>
        /// 更新
        /// </summary>
        Update = 0x4
    }
}