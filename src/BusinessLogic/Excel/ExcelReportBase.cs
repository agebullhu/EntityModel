/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����:2016-06-14
�޸�: -
*****************************************************/
#region ����

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     Excel�������ɵĻ���
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    public class ExcelReportBase<TData>
            where TData : class, new()
    {
        /// <summary>
        ///     �и����п�(��һ������Ϊ�п�,��һ������Ϊ�и�)
        /// </summary>
        protected readonly List<List<int>> Matrix = new List<List<int>>();

        /// <summary>
        ///     ���õ���ʽ�б�
        /// </summary>
        protected readonly List<List<ICellStyle>> Styles = new List<List<ICellStyle>>();

        /// <summary>
        ///     ������
        /// </summary>
        protected XSSFWorkbook Workbook { get; set; }

        /// <summary>
        ///     ��ǰ������
        /// </summary>
        protected ISheet Sheet { get; set; }

        /// <summary>
        ///     �ѹ�����д����������
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
        ///     ����ʽģ��
        /// </summary>
        /// <param name="sheet">Sheet����</param>
        /// <param name="sl">����ʼ���</param>
        /// <param name="el">������</param>
        /// <param name="sc">����ʼ���</param>
        /// <param name="ec">������</param>
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
