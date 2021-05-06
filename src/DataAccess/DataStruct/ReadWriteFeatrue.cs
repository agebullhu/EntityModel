/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立:2016-06-07
修改: -
*****************************************************/

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
        Update = 0x4,

        /// <summary>
        /// 所有
        /// </summary>
        All = Read | Insert | Update,

        /// <summary>
        /// 读与插入
        /// </summary>
        ReadInsert = Read | Insert,

        /// <summary>
        /// 读与更新
        /// </summary>
        ReadUpdate = Read | Update,

        /// <summary>
        /// 插入与更新
        /// </summary>
        Write = Insert | Update
    }
}