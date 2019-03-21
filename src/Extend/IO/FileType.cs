// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System.ComponentModel ;


#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        ///   其它文件
        /// </summary>
        [Description("其它文件")]
        None ,

        /// <summary>
        ///   图像
        /// </summary>
        [Description("图像")]
        Image ,

        /// <summary>
        ///   音频
        /// </summary>
        [Description("音频")]
        Audio ,

        /// <summary>
        ///   视频
        /// </summary>
        [Description("视频")]
        Vedio ,

        /// <summary>
        ///   WORD文档
        /// </summary>
        [Description("WORD文档")]
        Doc ,

        /// <summary>
        ///   EXCEL文档
        /// </summary>
        [Description("EXCEL文档")]
        Xls ,

        /// <summary>
        ///   PPT文档
        /// </summary>
        [Description("PPT文档")]
        PPT ,

        /// <summary>
        ///   PDF文档
        /// </summary>
        [Description("PDF文档")]
        PDF,

        /// <summary>
        /// WPS文件
        /// </summary>
        [Description("WPS文件")]
        WPS,

        /// <summary>
        /// 文本文件
        /// </summary>
        [Description("文本文件")]
        TEXT,

        /// <summary>
        /// 网页
        /// </summary>
        [Description("网页")]
        HTML,

        /// <summary>
        /// 应用程序
        /// </summary>
        [Description("应用程序")]
        EXE,

        /// <summary>
        /// 压缩文件
        /// </summary>
        [Description("压缩文件")]
        ZIP,

        /// <summary>
        /// 帮助文件
        /// </summary>
        [Description("帮助文件")]
        CHM,

        /// <summary>
        /// 程序代码
        /// </summary>
        [Description("程序代码")]
        CODE,

        /// <summary>
        /// 数据文件
        /// </summary>
        [Description("数据文件")]
        Data
    }
}
