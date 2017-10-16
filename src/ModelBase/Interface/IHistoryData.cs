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

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     表示这条数据记录修改历史
    /// </summary>
    public interface IHistoryData
    {
        /// <summary>
        ///     作者
        /// </summary>
        /// <value>int</value>
        int AuthorID { get; set; }

        /// <summary>
        ///     新增日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime AddDate { get; set; }

        /// <summary>
        ///     最后修改者
        /// </summary>
        /// <value>int</value>
        int LastReviserID { get; set; }

        /// <summary>
        ///     最后修改日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime LastModifyDate { get; set; }
    }
    /// <summary>
    ///     表示这条数据记录修改历史
    /// </summary>
    public interface IHistory
    {
        /// <summary>
        ///     作者
        /// </summary>
        /// <value>string</value>
        string Author { get; set; }

        /// <summary>
        ///     新增日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime AddDate { get; set; }

        /// <summary>
        ///     最后修改者
        /// </summary>
        /// <value>string</value>
        string LastReviser { get; set; }

        /// <summary>
        ///     最后修改日期
        /// </summary>
        /// <value>DateTime</value>
        DateTime LastModifyDate { get; set; }
    }
}