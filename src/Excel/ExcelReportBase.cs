// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-14
// // 修改:2016-06-16
// // *****************************************************/
#region 引用

using Agebull.EntityModel.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     Excel报表生成的基类
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    public class ExcelReportBase<TData>
        where TData : EditDataObject, IIdentityData<long>, new()
    {
        /// <summary>
        ///     行高与列宽(第一个数组为列宽,第一个数组为行高)
        /// </summary>
        protected readonly List<List<int>> Matrix = new List<List<int>>();

        /// <summary>
        ///     可用的样式列表
        /// </summary>
        protected readonly List<List<ICellStyle>> Styles = new List<List<ICellStyle>>();

        /// <summary>
        ///     工作簿
        /// </summary>
        protected XSSFWorkbook Workbook { get; set; }

        /// <summary>
        ///     当前工作簿
        /// </summary>
        protected ISheet Sheet { get; set; }

        /// <summary>
        ///     把工作簿写到二进制流
        /// </summary>
        /// <returns></returns>
        protected byte[] SaveToBytes()
        {
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                Workbook.Write(ms);
                buffer = ms.ToArray();
            }
            return buffer;
        }

        /// <summary>
        ///     读样式模板
        /// </summary>
        /// <param name="sheet">Sheet对象</param>
        /// <param name="sl">行起始序号</param>
        /// <param name="el">总行数</param>
        /// <param name="sc">列起始序号</param>
        /// <param name="ec">总列数</param>
        protected void ReadStyles(ISheet sheet, int sl, int el, int sc, int ec)
        {
            Matrix.Clear();
            Styles.Clear();
            var l2 = new List<int>();
            Matrix.Add(l2);
            for (var col = sc; col <= ec; col++)
            {
                l2.Add(sheet.GetColumnWidth(col));
            }
            l2 = new List<int>();
            Matrix.Add(l2);

            for (var line = sl; line <= el; line++)
            {
                var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
                l2.Add(row.Height);
                var lines = new List<ICellStyle>();
                for (var col = sc; col <= ec; col++)
                {
                    var cell = row.GetCell(col) ?? row.CreateCell(col);
                    lines.Add(cell.CellStyle);
                }
                Styles.Add(lines);
            }
        }
    }
}
