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

namespace Agebull.EntityModel.Interfaces
{

    /// <summary>
    ///     表示这条数据记录修改版本号
    /// </summary>
    public interface IVersionData
    {
        /// <summary>
        ///     数据版本
        /// </summary>
        /// <value>int</value>
        long DataVersion { get; set; }
    }

    /// <summary>
    ///     表示这条数据记录修改历史
    /// </summary>
    public interface IAuthorData
    {
        /// <summary>
        ///     作者
        /// </summary>
        /// <value>int</value>
        string AuthorId { get; set; }

        /*// <summary>
        ///     作者
        /// </summary>
        /// <value>int</value>
        string Author { get; set; }*/

        /// <summary>
        ///     新增日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime AddDate { get; set; }
    }

    /// <summary>
    ///     表示这条数据记录修改历史
    /// </summary>
    public interface IHistoryData : IAuthorData
    {

        /// <summary>
        ///     最后修改者
        /// </summary>
        /// <value>int</value>
        string LastReviserId { get; set; }

        /*// <summary>
        ///     最后修改者
        /// </summary>
        /// <value>int</value>
        string LastReviser { get; set; }*/

        /// <summary>
        ///     最后修改日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime LastModifyDate { get; set; }
    }
}