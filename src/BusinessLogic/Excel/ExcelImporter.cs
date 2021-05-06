/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����: ��������
�޸�:2016-06-22
*****************************************************/

#region ����

using Agebull.EntityModel.Common;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     Excel������
    /// </summary>
    public class ExcelImporter<TData> : IDisposable
        where TData : class, new()
    {
        #region ׼��

        /// <summary>
        /// ���ݷ��ʶ���
        /// </summary>
        public DataAccess<TData> DataAccess { get; set; }

        /// <summary>
        ///     ���ɹ�����
        /// </summary>
        /// <param name="buffer">�ļ�����</param>
        /// <returns>��������</returns>
        public bool Prepare(byte[] buffer)
        {
            Stream = new MemoryStream(buffer);
            Book = new XSSFWorkbook(Stream);
            Sheet = Book.GetSheetAt(0);
            return Prepare(Sheet);
        }

        /// <summary>
        ///     ׼������Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <returns>��������</returns>
        public bool Prepare(ISheet sheet)
        {
            Sheet = sheet;
            if (!Initiate() || !CheckFieldMaps())
                return false;
            ColumnFields2 = new Dictionary<string, int>();
            foreach (var mf in ColumnFields) ColumnFields2.Add(mf.Value, mf.Key);
            return true;
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <returns>����</returns>
        public byte[] ToStream()
        {
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <returns>��������</returns>
        public Task<bool> ImportExcel()
        {
            return ImportExcel(Write);
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="action">��������ʱ�Ĵ���ص�</param>
        /// <returns>��������</returns>
        public async Task<bool> ImportExcel(Func<TData, int, Task<string>> action)
        {
            var success = true;
            var emptyRow = 0;
            for (var line = 1; line < short.MaxValue; line++)
            {
                var nowSuccess = true;
                switch (ReadRow(line, out var data, out var row))
                {
                    case -1:
                        if (++emptyRow > 2)
                            return success;
                        continue;
                    case 1:
                        nowSuccess = false;
                        break;
                }

                var msg = await action(data, line);
                if (nowSuccess && string.IsNullOrWhiteSpace(msg))
                    continue;
                success = false;
                row.SafeGetCell(MaxColumn).SetCellValue("����");
                row.SafeGetCell(MaxColumn + 1).SetCellValue(msg);
            }

            return success;
        }

        #endregion

        #region �����ֶζ�Ӧ����

        /// <summary>
        ///     �����ó��к����ֶεĶ�Ӧͼ
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckFieldMaps()
        {
            var row = Sheet.GetRow(0);
            ColumnFields = new Dictionary<int, string>();
            var emptyCnt = 0;
            MaxColumn = 0;
            for (var column = 0; column < short.MaxValue; column++)
            {
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    if (++emptyCnt > 3) break;
                    continue;
                }

                if (cell.CellType != CellType.String) cell.SetCellType(CellType.String);
                var field = cell.StringCellValue;
                if (string.IsNullOrWhiteSpace(field))
                {
                    if (++emptyCnt > 3) break;
                    continue;
                }
                //if (emptyCnt > 0)
                //{
                //    WriteCellState(row, column, "�ֶ�����Ϊ�հ�,����ֱ����ֹ", true);
                //    return false;
                //}

                if (!FieldMap.TryGetValue(field.Trim(), out var innerFile)) continue;
                emptyCnt = 0;
                MaxColumn = column;
                ColumnFields.Add(cell.ColumnIndex, innerFile);
            }

            MaxColumn += 1;
            {
                var cell = row.SafeGetCell(MaxColumn);
                cell.SetCellValue("У��״̬");
                cell = row.SafeGetCell(MaxColumn + 1);
                cell.SetCellValue("У����Ϣ");
            }
            return true;
        }

        #endregion

        #region �ֶ�

        /// <summary>
        ///     ���������ֶ�����(�кŶ�Ӧ���ֶ�)
        /// </summary>
        protected Dictionary<int, string> ColumnFields;

        /// <summary>
        ///     ���������ֶ�����(�ֶζ�Ӧ���к�)
        /// </summary>
        protected Dictionary<string, int> ColumnFields2;

        /// <summary>
        ///     �ڲ��ֶ���Excel�еĶ��ձ�
        /// </summary>
        public Dictionary<string, string> FieldMap { get; set; }

        /// <summary>
        ///     �����
        /// </summary>
        public int MaxColumn { get; private set; }


        private MemoryStream Stream;

        /// <summary>
        /// ����������
        /// </summary>
        protected XSSFWorkbook Book { get; private set; }

        /// <summary>
        ///     ��ǰ����Ĺ�����
        /// </summary>
        public ISheet Sheet { get; set; }

        /// <summary>
        /// ������Դ
        /// </summary>
        void IDisposable.Dispose()
        {
            //Book.Dispose();
            Stream.Dispose();
        }

        #endregion

        #region �ڲ���������

        /// <summary>
        ///     д���ȡ������
        /// </summary>
        /// <param name="data"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        protected virtual async Task<string> Write(TData data, int line)
        {
            if (data is IValidate validate)
            {
                if (validate.Validate(out var result))
                    return await DataAccess.SaveAsync(data) ? "�޷�д��" : null;

                foreach (var item in result.Items)
                    if (ColumnFields2.TryGetValue(item.Name, out var col))
                        WriteCellState(line, col, item.Message, true);
                return result.ToString();
            }
            else
            {
                return await DataAccess.SaveAsync(data) ? "�޷�д��" : null;
            }
        }


        #endregion

        #region ������

        /// <summary>
        ///     ���첢��ʼ�����ݶ���
        /// </summary>
        /// <returns></returns>
        protected virtual TData CreateEntity(IRow row)
        {
            return new TData();
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        protected virtual bool Initiate()
        {
            return true;
        }

        #endregion

        #region ���ݶ�ȡ

        /// <summary>
        ///     �����ֶ�ֵ
        /// </summary>
        /// <param name="data">���������</param>
        /// <param name="column">�к�</param>
        /// <param name="value">��ȡ�����ı�ֵ</param>
        /// <param name="row">��ǰ��</param>
        private bool SetValue(TData data, int column, string value, IRow row)
        {
            var field = ColumnFields[column];
            try
            {
                DataAccess.Provider.EntityOperator.SetValue(data, field, value);
                return true;
            }
            catch (Exception ex)
            {
                WriteCellState(row, column, $"ֵ[{value}]�޷�д��({ex.Message})��", true);
                return false;
            }
        }


        /// <summary>
        ///     ��ȡһ������
        /// </summary>
        /// <param name="line"></param>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        /// <returns>�Ƿ�ɹ���ȡ</returns>
        private int ReadRow(int line, out TData entity, out IRow row)
        {
            row = Sheet.GetRow(line);
            if (row == null || row.Cells.Count == 0)
            {
                entity = null;
                return -1;
            }

            entity = CreateEntity(row);

            return ReadRowFields(row, entity) ? 0 : 1;
        }

        /// <summary>
        ///     ��ȡһ�еĸ����ֶ�
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        private bool ReadRowFields(IRow row, TData entity)
        {
            var success = true;
            foreach (var column in ColumnFields.Keys)
            {
                if (!GetCellValue(row, column, out var cell, out var value))
                {
                    success = false;
                    continue;
                }

                if (value == "#REF!" || value == "#N/A")
                {
                    WriteCellState(row, column, "��ʽ����,ʹ��Ĭ��ֵ", false);
                    success = false;
                }
                else if (!SetValue(entity, cell.ColumnIndex, value, row))
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        ///     ȡһ����Ԫ���ֵ
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool GetCellValue(IRow row, int column, out ICell cell, out string value)
        {
            cell = row.GetCell(column);
            if (cell == null)
            {
                //cell = row.CreateCell(column);
                value = null;
                return true;
            }

            //var style = Sheet.Workbook.CreateCellStyle();
            //style.CloneStyleFrom(cell.CellStyle);
            //style.FillForegroundColor = HSSFColor.White.Index;
            //cell.CellStyle = style;
            switch (cell.CellType)
            {
                default:
                    cell.SetCellType(CellType.String);
                    value = cell.StringCellValue?.Trim();
                    return true;
                case CellType.Error:
                    WriteCellState(row, column, "���ݴ���,ʹ��Ĭ��ֵ", false);
                    value = null;
                    return false;
                case CellType.Blank:
                    value = null;
                    return true;
                case CellType.Numeric:
                    value = GetNumericValue(cell, cell.NumericCellValue);
                    return true;
                case CellType.String:
                    value = cell.StringCellValue;
                    return true;
                case CellType.Boolean:
                    value = cell.BooleanCellValue.ToString();
                    return true;
                case CellType.Formula:
                    break;
            }

            //��ʽ��
            var e = new XSSFFormulaEvaluator(Sheet.Workbook);
            var vc = e.Evaluate(cell);
            switch (vc.CellType)
            {
                case CellType.Error:
                    WriteCellState(row, column, "���ݴ���,ʹ��Ĭ��ֵ", false);
                    value = null;
                    return false;
                case CellType.Blank:
                    value = null;
                    return false;
                case CellType.Numeric:
                    value = GetNumericValue(cell, vc.NumberValue);
                    break;
                case CellType.String:
                    value = vc.StringValue?.Trim();
                    break;
                case CellType.Boolean:
                    value = vc.BooleanValue.ToString();
                    break;
                default:
                    value = vc.StringValue?.Trim();
                    break;
            }

            return true;
        }

        #endregion

        #region ��������

        /// <summary>
        ///     ��ע���ɶ���
        /// </summary>
        private IDrawing _drawingPatriarch;

        /// <summary>
        ///     ͨ����עд�뵥Ԫ��ĵ���״̬
        /// </summary>
        /// <param name="line">�к�</param>
        /// <param name="col">�к�</param>
        /// <param name="message">״̬��Ϣ</param>
        /// <param name="isError">�Ƿ����(������ʾΪ����)</param>
        protected void WriteCellState(int line, int col, string message, bool isError)
        {
            WriteCellState(Sheet.GetRow(line), col, message, isError);
        }

        /// <summary>
        ///     ͨ����עд�뵥Ԫ��ĵ���״̬
        /// </summary>
        /// <param name="row">��</param>
        /// <param name="col">�к�</param>
        /// <param name="message">״̬��Ϣ</param>
        /// <param name="isError">�Ƿ����(������ʾΪ����)</param>
        protected void WriteCellState(IRow row, int col, string message, bool isError)
        {
            try
            {
                var cell = row.SafeGetCell(col);
                if (_drawingPatriarch == null) _drawingPatriarch = Sheet.CreateDrawingPatriarch();
                var msg = $"{(isError ? "����" : "����")}\r\n{message}";
                var comment =
                    _drawingPatriarch.CreateCellComment(new XSSFClientAnchor(0, 0, 1, 1, col, row.RowNum, col + 6,
                        row.RowNum + 3));
                comment.String = new XSSFRichTextString(msg);
                comment.Column = col;
                comment.Row = row.RowNum;
                cell.CellComment = comment;
                cell.CellStyle.FillBackgroundColor = isError ? HSSFColor.Red.Index : HSSFColor.Yellow.Index;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        ///     д���еĵ���״̬
        /// </summary>
        /// <param name="line">�к�</param>
        /// <param name="succeed">�Ƿ����(������ʾΪ����)</param>
        /// <param name="message">״̬��Ϣ</param>
        protected void WriteRowState(int line, bool succeed, string message)
        {
            WriteRowState(Sheet.GetRow(line), succeed, message);
        }

        /// <summary>
        ///     д���еĵ���״̬
        /// </summary>
        /// <param name="row">��</param>
        /// <param name="succeed">�Ƿ����(������ʾΪ����)</param>
        /// <param name="message">״̬��Ϣ</param>
        protected void WriteRowState(IRow row, bool succeed, string message)
        {
            if (!succeed) row.SetCellValue("����", MaxColumn);
            if (!string.IsNullOrEmpty(message)) row.SetCellValue(message, MaxColumn + 1);
        }

        /// <summary>
        ///     ���������͵�ֵ(���ڻ�����)
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="vl">Ҫȡ��ֵ</param>
        /// <returns>�������͵�ֵ(���ڻ�����)</returns>
        protected static string GetNumericValue(ICell cell, double vl)
        {
            if (DateUtil.IsCellDateFormatted(cell)) return ExcelHelper.ExcelBaseDate.AddDays(vl).ToString("s");
            return (decimal)vl == (int)vl ? ((int)vl).ToString() : vl.ToString("s");
        }

        #endregion

    }
}