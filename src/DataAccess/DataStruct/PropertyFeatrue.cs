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
        Interface = 0x4,

        /// <summary>
        /// 外部连接字段
        /// </summary>
        LinkField = 0x8,

        /// <summary>
        /// 主键
        /// </summary>
        PrimaryKey = 0x10,

        /// <summary>
        /// 普通字段：字段、属性
        /// </summary>
        General = Field | Property,

        /// <summary>
        /// 主键：字段、属性、主键
        /// </summary>
        PrimaryProperty = Field | Property | PrimaryKey,

        /// <summary>
        /// 外链：字段、属性、主键
        /// </summary>
        OutProperty = Field | Property | LinkField,

        /// <summary>
        /// 外键：字段、属性、主键、连接字段
        /// </summary>
        ForeignKey = Field | Property | LinkField | PrimaryKey,
    }
}