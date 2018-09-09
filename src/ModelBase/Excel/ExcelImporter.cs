// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-22
// // *****************************************************/
#if !NETSTANDARD
#region ����

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

#endregion

namespace Agebull.Common.DataModel.BusinessLogic.Report
{
    /// <summary>
    ///     Excel������
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    /// <typeparam name="TAccess">�������Ͷ�Ӧ�����ݷ�����</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class ExcelImporter<TData, TAccess, TDatabase> : BusinessLogicBase<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        /// <summary>
        ///     ���������ֶ�����(�кŶ�Ӧ���ֶ�)
        /// </summary>
        protected Dictionary<int, string> ColumnFields;

        /// <summary>
        ///     ��ǰ����Ĺ�����
        /// </summary>
        protected ISheet Sheet;

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <returns>��������</returns>
        public int ImportExcel(ISheet sheet)
        {

            Debug.WriteLine($"���빤������Ϊ:{sheet.SheetName}");

            Sheet = sheet;
            Initiate();

            if (!CheckFieldMaps())
            {
                return 0;
            }
            var emptyRow = 0;
            var cnt = 0;
            for (var line = 1; line < short.MaxValue; line++)
            {
                var result = ReadRow(line);
                if (result != null)
                {
                    emptyRow = 0;
                    if (result.Value)
                        cnt++;
                }
                else if (emptyRow > 2)
                {
                    break;
                }
                else
                {
                    emptyRow++;
                }
            }
            return cnt;
        }

        /// <summary>
        ///     ��ȡһ������
        /// </summary>
        /// <param name="line"></param>
        /// <returns>�Ƿ�ɹ���ȡ</returns>
        private bool? ReadRow(int line)
        {
            var row = Sheet.GetRow(line);
            if (row == null || row.Cells.Count == 0)
            {
                return null;
            }
            var entity = CreateEntity(row);
            ReadRowFields(row, entity);
            return Save(row, entity);
        }

        /// <summary>
        ///     ʵ�屣��
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        protected virtual bool Save(IRow row, TData entity)
        {
            if (!entity.__EntityStatus.IsModified)
            {
                WriteRowState(row, false, "����ȫΪĬ��ֵ,������");
                return false;
            }
            var vmsg = entity.Validate();
            if (!vmsg.succeed)
            {
                WriteRowState(row, false, "���ݲ��ϸ�,�����롣" + "\r\n" + vmsg);
                return false;
            }

            try
            {
                if (!Access.Insert(entity))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteRowState(row, false, "����ʧ�ܡ�" + "\r\n" + ex);
            }
            return true;
        }

        /// <summary>
        ///     ��ȡһ�еĸ����ֶ�
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        private void ReadRowFields(IRow row, TData entity)
        {
            foreach (var field in ColumnFields.Keys)
            {
                ICell cell;
                string value;
                if (!GetCellValue(row, field, out cell, out value))
                    continue;

                if (value == "#REF!" || value == "#N/A")
                {
                    WriteCellState(row, field, "��ʽ����,ʹ��Ĭ��ֵ", false);
                }
                else
                {
                    SetValue(entity, cell.ColumnIndex, value, row);
                }
            }
        }
        /// <summary>
        /// ȡһ����Ԫ���ֵ
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
                value = null;
                return false;
            }
            var style = Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);
            style.FillForegroundColor = HSSFColor.White.Index;
            cell.CellStyle = style;
            switch (cell.CellType)
            {
                case CellType.Error:
                    WriteCellState(row, column, "���ݴ���,ʹ��Ĭ��ֵ", false);
                    value = null;
                    return false;
                case CellType.Blank:
                    value = null;
                    break;
                case CellType.Numeric:
                    value = GetNumericValue(cell, cell.NumericCellValue);
                    break;
                case CellType.String:
                    value = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    value = cell.BooleanCellValue.ToString();
                    break;
                case CellType.Formula: //��ʽ��
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
                            value = vc.StringValue;
                            break;
                        case CellType.Boolean:
                            value = vc.BooleanValue.ToString();
                            break;
                        default:
                            value = vc.StringValue;
                            break;
                    }
                    break;
                default:
                    cell.SetCellType(CellType.String);
                    value = cell.StringCellValue;
                    break;
            }
            return true;
        }

        /// <summary>
        ///     �����ֶ�ֵ
        /// </summary>
        /// <param name="data">���������</param>
        /// <param name="column">�к�</param>
        /// <param name="value">��ȡ�����ı�ֵ</param>
        /// <param name="row">��ǰ��</param>
        protected virtual void SetValue(TData data, int column, string value, IRow row)
        {
            var field = ColumnFields[column];
            try
            {
                data.SetValue(field, value);
            }
            catch 
            {
                WriteCellState(row, column, $"ֵ[{value}]д�뵽�ֶ�[{field}]ʧ�ܡ�", true);
            }
        }

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
        protected virtual void Initiate()
        {
        }

        #region �����ֶζ�Ӧ����

        /// <summary>
        ///     �����
        /// </summary>
        protected int MaxColumn;

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
                MaxColumn = column;
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    if (++emptyCnt > 3)
                    {
                        break;
                    }
                    continue;
                }
                if (cell.CellType != CellType.String)
                {
                    cell.SetCellType(CellType.String);
                }
                var field = cell.StringCellValue;
                if (string.IsNullOrWhiteSpace(field))
                {
                    if (++emptyCnt > 3)
                    {
                        break;
                    }
                    continue;
                }
                if (emptyCnt > 0)
                {
                    WriteCellState(row, column, "�ֶ�����Ϊ�հ�,����ֱ����ֹ", true);
                    return false;
                }
                emptyCnt = 0;
                ColumnFields.Add(cell.ColumnIndex, field.Trim());
            }
            MaxColumn += 1;
            foreach (var f in ColumnFields)
            {
                Debug.Assert(Access.FieldDictionary.ContainsKey(f.Value));
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
            var cell = row.SafeGetCell(col);
            if (_drawingPatriarch == null)
            {
                _drawingPatriarch = Sheet.CreateDrawingPatriarch();
            }
            var msg = $"{(isError ? "����" : "����")}\r\n{message}";
            var comment = _drawingPatriarch.CreateCellComment(new XSSFClientAnchor(0, 0, 1, 1, col, row.RowNum, col + 3, row.RowNum + 3));
            comment.String = new XSSFRichTextString(msg);
            comment.Column = col;
            comment.Row = row.RowNum;
            cell.CellComment = comment;
            //cell.CellStyle.FillForegroundColor = isError ? HSSFColor.Red.Index : HSSFColor.Yellow.Index;
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
            if (!succeed)
            {
                row.SetCellValue( "����", MaxColumn);
            }
            if (!string.IsNullOrEmpty(message))
            {
                row.SetCellValue(message, MaxColumn + 1);
            }
        }

        /// <summary>
        ///     ���������͵�ֵ(���ڻ�����)
        /// </summary>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="vl">Ҫȡ��ֵ</param>
        /// <returns>�������͵�ֵ(���ڻ�����)</returns>
        protected static string GetNumericValue(ICell cell, double vl)
        {
            if (DateUtil.IsCellDateFormatted(cell))
            {
                return ExcelHelper.ExcelBaseDate.AddDays(vl).ToString("yyyy-MM-dd");
            }
            return (decimal)vl == (int)vl ? ((int)vl).ToString() : vl.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
#endif