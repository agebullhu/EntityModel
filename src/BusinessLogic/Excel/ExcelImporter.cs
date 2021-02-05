// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-22
// // *****************************************************/

#region 引用

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
    ///     Excel导入类
    /// </summary>
    public class ExcelImporter<TData> : IDisposable
        where TData : class, new()
    {
        #region 准备

        /// <summary>
        /// 数据访问对象
        /// </summary>
        public DataAccess<TData> DataAccess { get; set; }

        /// <summary>
        ///     生成工作溥
        /// </summary>
        /// <param name="buffer">文件内容</param>
        /// <returns>导入数量</returns>
        public bool Prepare(byte[] buffer)
        {
            Stream = new MemoryStream(buffer);
            Book = new XSSFWorkbook(Stream);
            Sheet = Book.GetSheetAt(0);
            return Prepare(Sheet);
        }

        /// <summary>
        ///     准备导入Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <returns>导入数量</returns>
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
        ///     导出Excel
        /// </summary>
        /// <returns>数据</returns>
        public byte[] ToStream()
        {
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        #endregion

        #region 导入

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <returns>导入数量</returns>
        public Task<bool> ImportExcel()
        {
            return ImportExcel(Write);
        }

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <param name="action">读到数据时的处理回调</param>
        /// <returns>导入数量</returns>
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
                row.SafeGetCell(MaxColumn).SetCellValue("错误");
                row.SafeGetCell(MaxColumn + 1).SetCellValue(msg);
            }

            return success;
        }

        #endregion

        #region 列与字段对应分析

        /// <summary>
        ///     分析得出列号与字段的对应图
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
                //    WriteCellState(row, column, "字段名字为空白,导入直接中止", true);
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
                cell.SetCellValue("校验状态");
                cell = row.SafeGetCell(MaxColumn + 1);
                cell.SetCellValue("校验信息");
            }
            return true;
        }

        #endregion

        #region 字段

        /// <summary>
        ///     分析出的字段名称(列号对应的字段)
        /// </summary>
        protected Dictionary<int, string> ColumnFields;

        /// <summary>
        ///     分析出的字段名称(字段对应的列号)
        /// </summary>
        protected Dictionary<string, int> ColumnFields2;

        /// <summary>
        ///     内部字段与Excel列的对照表
        /// </summary>
        public Dictionary<string, string> FieldMap { get; set; }

        /// <summary>
        ///     最大列
        /// </summary>
        public int MaxColumn { get; private set; }


        private MemoryStream Stream;

        /// <summary>
        /// 工作簿对象
        /// </summary>
        protected XSSFWorkbook Book { get; private set; }

        /// <summary>
        ///     当前导入的工作表
        /// </summary>
        public ISheet Sheet { get; set; }

        /// <summary>
        /// 清理资源
        /// </summary>
        void IDisposable.Dispose()
        {
            //Book.Dispose();
            Stream.Dispose();
        }

        #endregion

        #region 内部方法导入

        /// <summary>
        ///     写入读取的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        protected virtual async Task<string> Write(TData data, int line)
        {
            if (data is IValidate validate)
            {
                if (validate.Validate(out var result))
                    return await DataAccess.SaveAsync(data) ? "无法写入" : null;

                foreach (var item in result.Items)
                    if (ColumnFields2.TryGetValue(item.Name, out var col))
                        WriteCellState(line, col, item.Message, true);
                return result.ToString();
            }
            else
            {
                return await DataAccess.SaveAsync(data) ? "无法写入" : null;
            }
        }


        #endregion

        #region 可重载

        /// <summary>
        ///     构造并初始化数据对象
        /// </summary>
        /// <returns></returns>
        protected virtual TData CreateEntity(IRow row)
        {
            return new TData();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        protected virtual bool Initiate()
        {
            return true;
        }

        #endregion

        #region 数据读取

        /// <summary>
        ///     设置字段值
        /// </summary>
        /// <param name="data">数据类对象</param>
        /// <param name="column">列号</param>
        /// <param name="value">读取出的文本值</param>
        /// <param name="row">当前行</param>
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
                WriteCellState(row, column, $"值[{value}]无法写入({ex.Message})。", true);
                return false;
            }
        }


        /// <summary>
        ///     读取一条数据
        /// </summary>
        /// <param name="line"></param>
        /// <param name="entity"></param>
        /// <param name="row"></param>
        /// <returns>是否成功读取</returns>
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
        ///     读取一行的各个字段
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
                    WriteCellState(row, column, "公式错误,使用默认值", false);
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
        ///     取一个单元格的值
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
                    WriteCellState(row, column, "数据错误,使用默认值", false);
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

            //公式列
            var e = new XSSFFormulaEvaluator(Sheet.Workbook);
            var vc = e.Evaluate(cell);
            switch (vc.CellType)
            {
                case CellType.Error:
                    WriteCellState(row, column, "数据错误,使用默认值", false);
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

        #region 辅助方法

        /// <summary>
        ///     批注生成对象
        /// </summary>
        private IDrawing _drawingPatriarch;

        /// <summary>
        ///     通过批注写入单元格的导入状态
        /// </summary>
        /// <param name="line">行号</param>
        /// <param name="col">列号</param>
        /// <param name="message">状态消息</param>
        /// <param name="isError">是否错误(否则显示为警告)</param>
        protected void WriteCellState(int line, int col, string message, bool isError)
        {
            WriteCellState(Sheet.GetRow(line), col, message, isError);
        }

        /// <summary>
        ///     通过批注写入单元格的导入状态
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列号</param>
        /// <param name="message">状态消息</param>
        /// <param name="isError">是否错误(否则显示为警告)</param>
        protected void WriteCellState(IRow row, int col, string message, bool isError)
        {
            try
            {
                var cell = row.SafeGetCell(col);
                if (_drawingPatriarch == null) _drawingPatriarch = Sheet.CreateDrawingPatriarch();
                var msg = $"{(isError ? "错误" : "警告")}\r\n{message}";
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
        ///     写入行的导入状态
        /// </summary>
        /// <param name="line">行号</param>
        /// <param name="succeed">是否错误(否则显示为警告)</param>
        /// <param name="message">状态消息</param>
        protected void WriteRowState(int line, bool succeed, string message)
        {
            WriteRowState(Sheet.GetRow(line), succeed, message);
        }

        /// <summary>
        ///     写入行的导入状态
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="succeed">是否错误(否则显示为警告)</param>
        /// <param name="message">状态消息</param>
        protected void WriteRowState(IRow row, bool succeed, string message)
        {
            if (!succeed) row.SetCellValue("错误", MaxColumn);
            if (!string.IsNullOrEmpty(message)) row.SetCellValue(message, MaxColumn + 1);
        }

        /// <summary>
        ///     得数字类型的值(日期或数字)
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="vl">要取的值</param>
        /// <returns>数字类型的值(日期或数字)</returns>
        protected static string GetNumericValue(ICell cell, double vl)
        {
            if (DateUtil.IsCellDateFormatted(cell)) return ExcelHelper.ExcelBaseDate.AddDays(vl).ToString("s");
            return (decimal)vl == (int)vl ? ((int)vl).ToString() : vl.ToString("s");
        }

        #endregion

    }
}