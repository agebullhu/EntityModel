/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����: ��������
�޸�: -
*****************************************************/
#region ����

using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;

#endregion

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     ����(Excel)������
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        ///     ��ȫ�õ���
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="line"></param>
        public static IRow SafeGetRow(this ISheet sheet, int line)
        {
            return sheet.GetRow(line) ?? sheet.CreateRow(line);
        }

        /// <summary>
        ///     ��ȫ�õ���
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="line"></param>
        /// <param name="height"></param>
        public static IRow SafeGetRow(this ISheet sheet, int line, short height)
        {
            var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            row.Height = height;
            return row;
        }

        /// <summary>
        ///     ��ȫ�õ���
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="line"></param>
        /// <param name="initAction"></param>
        public static IRow SafeGetRow(this ISheet sheet, int line, Action<IRow> initAction)
        {
            var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            initAction(row);
            return row;
        }

        /// <summary>
        ///     ��ȫ�õ���
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="line"></param>
        /// <param name="height"></param>
        /// <param name="initAction"></param>
        public static IRow SafeGetRow(this ISheet sheet, int line, short height, Action<IRow> initAction)
        {
            var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            row.Height = height;
            initAction(row);
            return row;
        }

        /// <summary>
        ///     ��ȫȡ��Ԫ��
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static ICell SafeGetCell(this IRow row, int col)
        {
            return row.GetCell(col) ?? row.CreateCell(col);
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void SetCellValue(this IRow row, string val, int col)
        {
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            cell.SetCellValue(val);
        }

        ///// <summary>
        /////     ���õ�Ԫ������
        ///// </summary>
        ///// <param name="val"></param>
        ///// <param name="row"></param>
        ///// <param name="col"></param>
        //public static void SetCellValue(this IRow row, int col, string val)
        //{
        //    var cell = row.GetCell(col) ?? row.CreateCell(col);
        //    cell.SetCellValue(val);
        //}

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        public static void SetCellValue(this IRow row, int col, string val, ICellStyle style)
        {
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            cell.SetCellValue(val);
            cell.CellStyle = style;
        }

        /// <summary>
        ///     �ϲ���Ԫ��
        /// </summary>
        /// <param name="sheet">������</param>
        /// <param name="scol">��ʼ�У�������</param>
        /// <param name="ecol">�����У�������</param>
        /// <param name="srow">��ʼ�У�������</param>
        /// <param name="erow">�����У�������</param>
        public static void Merge(this ISheet sheet, int srow, int erow, int scol, int ecol)
        {
            sheet.AddMergedRegion(new CellRangeAddress(srow, erow, scol, ecol));
        }

        /// <summary>
        ///     �ϲ���Ԫ��
        /// </summary>
        /// <param name="sheet">������</param>
        /// <param name="srow">��ʼ�У�������</param>
        /// <param name="erow">�����У�������</param>
        /// <param name="scol">��ʼ�У�������</param>
        /// <param name="ecol">�����У�������</param>
        /// <param name="style">��ʽ</param>
        public static void Merge(this ISheet sheet, int srow, int erow, int scol, int ecol, ICellStyle style)
        {
            for (var line = srow; line <= erow; line++)
            {
                var row = sheet.SafeGetRow(line);
                for (var col = scol; col <= ecol; col++)
                {
                    var cell = row.GetCell(col);
                    //if (line != srow && col != scol)
                    //{
                    //    if (cell != null)
                    //    {
                    //        row.RemoveCell(cell);
                    //    }
                    //}
                    //else
                    {
                        if (cell == null)
                        {
                            cell = row.CreateCell(col);
                        }
                        else if (line != srow && col != scol)
                        {
                            cell.SetCellType(CellType.Blank);
                        }
                        cell.CellStyle = style;
                    }
                }
            }
            sheet.AddMergedRegion(new CellRangeAddress(srow, erow, scol, ecol));
        }

        /// <summary>
        ///     ���õ�Ԫ����ʽ
        /// </summary>
        /// <param name="row">�ж���</param>
        /// <param name="scol">��ʼ�У�������</param>
        /// <param name="ecol">�����У�������</param>
        /// <param name="style">��ʽ</param>
        public static void SetCellStyle(this IRow row, int scol, int ecol, ICellStyle style)
        {
            for (var col = scol; col <= ecol; col++)
            {
                var cell = row.GetCell(col) ?? row.CreateCell(col);
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ����ʽ
        /// </summary>
        /// <param name="row">�ж���</param>
        /// <param name="scol">��ʼ�У�������</param>
        /// <param name="ecol">�����У�������</param>
        /// <param name="styles">��ʽ���飨���кŶ�Ӧʹ�ã�</param>
        public static void SetCellStyle(this IRow row, int scol, int ecol, List<ICellStyle> styles)
        {
            for (var col = scol; col <= ecol; col++)
            {
                var cell = row.GetCell(col) ?? row.CreateCell(col);
                cell.CellStyle = styles[col];
            }
        }

        /// <summary>
        ///     ���õ�Ԫ����ʽ
        /// </summary>
        /// <param name="row">�ж���</param>
        /// <param name="col">��</param>
        /// <param name="style">��ʽ���飨���кŶ�Ӧʹ�ã�</param>
        public static void SetCellStyle(this IRow row, int col, ICellStyle style)
        {
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            cell.CellStyle = style;
        }

        /// <summary>
        ///     ���õ�Ԫ������ֵ
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue(this IRow row, DateTime val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            SetDateCellValue(row, val, col, style, prepare);
        }
        /// <summary>
        /// Excel�Ļ�������(������)
        /// </summary>
        public static DateTime ExcelBaseDate = new DateTime(1899, 12, 30);

        /// <summary>
        ///     ���õ�Ԫ������ֵ
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetDateCellValue(this IRow row, DateTime val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val > DateTime.MinValue)
            {
                var days = val - ExcelBaseDate;
                cell.SetCellValue(days.Days);
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue(this IRow row, string val, int col, ICellStyle style, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val != null)
            {
                cell.SetCellValue(val);
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }
        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellMoney(this IRow row, decimal val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val != 0)
            {
                cell.SetCellValue((double)val);
            }
            if (style == null)
            {
                return;
            }
            cell.CellStyle = style;
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellMoney2(this IRow row, decimal val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            cell.SetCellValue((double)val);
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetNumberValue(this IRow row, int val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val != 0)
            {
                cell.SetCellValue(val);
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue(this IRow row, int val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val != 0)
            {
                cell.SetCellValue(val);
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue(this IRow row, decimal val, int col, ICellStyle style = null, Action<IRow> prepare = null)
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (val != 0)
            {
                cell.SetCellValue((double)val);
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue<T>(this IRow row, T val, int col, ICellStyle style = null, Action<IRow> prepare = null)
            where T : struct
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (!Equals(val, default(T)))
            {
                cell.SetCellValue(val.ToString());
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���õ�Ԫ������
        /// </summary>
        /// <param name="format"></param>
        /// <param name="val"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="style"></param>
        /// <param name="prepare"></param>
        public static void SetCellValue<T>(this IRow row, string format, T val, int col, ICellStyle style, Action<IRow> prepare = null)
            where T : struct
        {
            prepare?.Invoke(row);
            var cell = row.GetCell(col) ?? row.CreateCell(col);
            if (!Equals(val, default(T)))
            {
                cell.SetCellValue(string.Format(format, val));
            }
            if (style != null)
            {
                cell.CellStyle = style;
            }
        }

        /// <summary>
        ///     ���Ƹ�ʽ
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="styles"></param>
        /// <param name="line"></param>
        public static void CopyStyle(this ISheet sheet, int line, List<ICellStyle> styles)
        {
            var row = sheet.GetRow(line) ?? sheet.CreateRow(line);
            for (var col = 0; col < 16; col++)
            {
                var cell = row.GetCell(col) ?? row.CreateCell(col);
                cell.CellStyle = styles[col];
            }
        }
    }
}
/*
��Ԫ�����ݸ�ʽ

����      CellStyle styledate= workbook.CreateCellStyle();
            DataFormat format = workbook.CreateDataFormat();
            styledate.DataFormat = format.GetFormat("d-mmm-yy");

          0, "General"
           1, "0"
           2, "0.00"
           3, "#,##0"
           4, "#,##0.00"
           5, "($#,##0_);($#,##0)"
           6, "($#,##0_);[Red]($#,##0)"
           7, "($#,##0.00);($#,##0.00)"
           8, "($#,##0.00_);[Red]($#,##0.00)"
           9, "0%"
           0xa, "0.00%"
           0xb, "0.00E+00"
           0xc, "# ?/?"
           0xd, "# ??/??"
           0xe, "m/d/yy"
           0xf, "d-mmm-yy"
           0x10, "d-mmm"
           0x11, "mmm-yy"
           0x12, "h:mm AM/PM"
           0x13, "h:mm:ss AM/PM"
           0x14, "h:mm"
           0x15, "h:mm:ss"
           0x16, "m/d/yy h:mm"
   
            0x17 - 0x24 reserved for international and Undocumented
           0x25, "(#,##0_);(#,##0)"
           0x26, "(#,##0_);[Red](#,##0)"
           0x27, "(#,##0.00_);(#,##0.00)"
           0x28, "(#,##0.00_);[Red](#,##0.00)"
           0x29, "_(///#,##0_);_(///(#,##0);_(/// \"-\"_);_(@_)"
           0x2a, "_($///#,##0_);_($///(#,##0);_($/// \"-\"_);_(@_)"
           0x2b, "_(///#,##0.00_);_(///(#,##0.00);_(///\"-\"??_);_(@_)"
           0x2c, "_($///#,##0.00_);_($///(#,##0.00);_($///\"-\"??_);_(@_)"
           0x2d, "mm:ss"
           0x2e, "[h]:mm:ss"
           0x2f, "mm:ss.0"
           0x30, "##0.0E+0"
           0x31, "@" - This Is text format.
           0x31  "text" - Alias for "@"
*/

