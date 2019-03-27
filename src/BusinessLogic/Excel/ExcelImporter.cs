// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-22
// // *****************************************************/
#region 引用

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Agebull.Common.Logging;
using Agebull.EntityModel.BusinessLogic.MySql;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

#endregion

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     Excel导入类
    /// </summary>
    public class ExcelImporter<TData>
        where TData : EditDataObject, IIdentityData, new()
    {
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

        /// <summary>
        ///     当前导入的工作表
        /// </summary>
        public ISheet Sheet { get; set; }

        /// <summary>
        ///     准备导入Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <returns>导入数量</returns>
        public bool Prepare(ISheet sheet)
        {
            Sheet = sheet;
            if (!Initiate() || !CheckFieldMaps())
            {
                return false;
            }

            ColumnFields2 = new Dictionary<string, int>();
            foreach (var mf in ColumnFields)
            {
                ColumnFields2.Add(mf.Value, mf.Key);
            }
            return true;
        }
        /// <summary>
        ///     准备导入Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <param name="map">内部字段与Excel列的对照表</param>
        /// <returns>导入数量</returns>
        public bool Prepare(ISheet sheet, Dictionary<string, string> map)
        {
            FieldMap = map;
            Sheet = sheet;
            Sheet = sheet;
            if (!Initiate() || !CheckFieldMaps())
            {
                return false;
            }

            ColumnFields2 = new Dictionary<string, int>();
            foreach (var mf in ColumnFields)
            {
                ColumnFields2.Add(mf.Value, mf.Key);
            }
            return true;
        }

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


        /// <summary>
        ///     数据取到后的处理
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnRead(TData data, IRow row, bool isFailed, out string msg)
        {
            msg = null;
            return true;
        }

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <param name="action">读到数据时的处理回调</param>
        /// <returns>导入数量</returns>
        public bool ImportExcel(Func<TData, int, string> action)
        {
            var success = true;
            var emptyRow = 0;
            for (var line = 1; line < short.MaxValue; line++)
            {
                bool nowSuccess = true;
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
                var msg = action(data, line);
                if (nowSuccess && string.IsNullOrWhiteSpace(msg))
                    continue;
                success = false;
                row.SafeGetCell(MaxColumn).SetCellValue("错误");
                row.SafeGetCell(MaxColumn + 1).SetCellValue(msg);
            }
            return success;
        }

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <param name="action">读到数据时的处理回调</param>
        /// <returns>导入数量</returns>
        public bool ImportExcelSync(Func<TData, int, string> action)
        {
            var success = true;
            var emptyRow = 0;
            for (var line = 1; line < short.MaxValue; line++)
            {
                bool nowSuccess = true;
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
                var msg = action(data, line);
                if (nowSuccess && string.IsNullOrWhiteSpace(msg))
                    continue;
                success = false;
                row.SafeGetCell(MaxColumn).SetCellValue("错误");
                row.SafeGetCell(MaxColumn + 1).SetCellValue(msg);
            }
            return success;
        }

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <returns>导入数量</returns>
        public bool ImportExcel()
        {
            var success = true;
            var emptyRow = 0;
            for (var line = 1; line < short.MaxValue; line++)
            {
                bool nowSuccess = true;
                var msg = new StringBuilder();
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

                var info = data.Validate();
                if (!info.Succeed)
                {
                    nowSuccess = false;
                    foreach (var item in info.Items)
                    {
                        if (ColumnFields2.TryGetValue(item.Name, out var col))
                        {
                            WriteCellState(line, col, item.Message, true);
                            msg.AppendFormat("{0}:{1}", item.Caption, item.Message);
                        }
                        else
                        {
                            msg.AppendFormat("字段{0}未提供导入内容且校验不通过:{1}", item.Caption, item.Message);
                        }
                        msg.AppendLine();
                    }
                }
                if (!OnRead(data, row, !nowSuccess, out var msg2))
                {
                    nowSuccess = false;
                    msg.AppendLine();
                    msg.Append(msg2);
                }

                if (nowSuccess)
                {
                    continue;
                }
                success = false;
                row.SafeGetCell(MaxColumn).SetCellValue("错误");
                row.SafeGetCell(MaxColumn + 1).SetCellValue(msg.ToString());
            }
            return success;
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
            bool success = true;
            foreach (var column in ColumnFields.Keys)
            {
                if (!GetCellValue(row, column, out var cell, out var value))
                {
                    success = false;
                    continue;
                }

                if (value == "#REF!" || value == "#N/A")
                {
                    LogRecorder.Debug($"{row}行{column}列公式错误,使用默认值");
                    WriteCellState(row, column, "公式错误,使用默认值", false);
                    success = false;
                }
                else
                {
                    if (!SetValue(entity, cell.ColumnIndex, value, row))
                    {
                        success = false;
                    }
                }
            }

            return success;
        }
        /// <summary>
        /// 取一个单元格的值
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
                cell = row.CreateCell(column);
                value = null;
                return true;
            }
            var style = Sheet.Workbook.CreateCellStyle();
            style.CloneStyleFrom(cell.CellStyle);
            style.FillForegroundColor = HSSFColor.White.Index;
            cell.CellStyle = style;
            switch (cell.CellType)
            {
                case CellType.Error:
                    LogRecorder.Debug($"{row}行{column}列数据错误,使用默认值");
                    WriteCellState(row, column, "数据错误,使用默认值", false);
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
                case CellType.Formula: //公式列
                    var e = new XSSFFormulaEvaluator(Sheet.Workbook);
                    var vc = e.Evaluate(cell);
                    switch (vc.CellType)
                    {
                        case CellType.Error:
                            LogRecorder.Debug($"{row}行{column}列数据错误,使用默认值");
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
                    break;
                default:
                    cell.SetCellType(CellType.String);
                    value = cell.StringCellValue?.Trim();
                    break;
            }
            return true;
        }

        /// <summary>
        ///     设置字段值
        /// </summary>
        /// <param name="data">数据类对象</param>
        /// <param name="column">列号</param>
        /// <param name="value">读取出的文本值</param>
        /// <param name="row">当前行</param>
        protected virtual bool SetValue(TData data, int column, string value, IRow row)
        {
            var field = ColumnFields[column];
            try
            {
                data.SetValue(field, value);
                return true;
            }
            catch (Exception ex)
            {
                //LogRecorder.Exception(ex);
                LogRecorder.Debug($"{row}行{column}列值[{value}]无法写入({ex.Message})。");
                WriteCellState(row, column, $"值[{value}]无法写入({ex.Message})。", true);
                return false;
            }
        }

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
                //if (emptyCnt > 0)
                //{
                //    WriteCellState(row, column, "字段名字为空白,导入直接中止", true);
                //    return false;
                //}

                if (!FieldMap.TryGetValue(field.Trim(), out var innerFile))
                {
                    continue;
                    //WriteCellState(row, column, $"字段名字{field}映射关系不正确,导入直接中止", true);
                    //return false;
                }
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
                if (_drawingPatriarch == null)
                {
                    _drawingPatriarch = Sheet.CreateDrawingPatriarch();
                }
                var msg = $"{(isError ? "错误" : "警告")}\r\n{message}";
                var comment = _drawingPatriarch.CreateCellComment(new XSSFClientAnchor(0, 0, 1, 1, col, row.RowNum, col + 6, row.RowNum + 3));
                comment.String = new XSSFRichTextString(msg);
                comment.Column = col;
                comment.Row = row.RowNum;
                cell.CellComment = comment;
                cell.CellStyle.FillBackgroundColor = isError ? HSSFColor.Red.Index : HSSFColor.Yellow.Index;
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
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
            if (!succeed)
            {
                row.SetCellValue("错误", MaxColumn);
            }
            if (!string.IsNullOrEmpty(message))
            {
                row.SetCellValue(message, MaxColumn + 1);
            }
        }

        /// <summary>
        ///     得数字类型的值(日期或数字)
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="vl">要取的值</param>
        /// <returns>数字类型的值(日期或数字)</returns>
        protected static string GetNumericValue(ICell cell, double vl)
        {
            if (DateUtil.IsCellDateFormatted(cell))
            {
                return ExcelHelper.ExcelBaseDate.AddDays(vl).ToString("s");
            }
            return (decimal)vl == (int)vl ? ((int)vl).ToString() : vl.ToString("s");
        }

        #endregion
    }
}

namespace Agebull.EntityModel.Excel.MySql
{
    /// <summary>
    ///     Excel导入类
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <typeparam name="TAccess">数据类型对应的数据访问类</typeparam>
    /// <typeparam name="TDatabase">数据库对象</typeparam>
    public class ExcelImporter<TData, TAccess, TDatabase> : BusinessLogicBase<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        /// <summary>
        ///     分析出的字段名称(列号对应的字段)
        /// </summary>
        protected Dictionary<int, string> ColumnFields;

        /// <summary>
        ///     内部字段与Excel列的对照表
        /// </summary>
        public Dictionary<string, string> FieldMap;

        /// <summary>
        ///     当前导入的工作表
        /// </summary>
        protected ISheet Sheet;

        /// <summary>
        ///     导入Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <param name="map">内部字段与Excel列的对照表</param>
        /// <returns>导入数量</returns>
        public int ImportExcel(ISheet sheet, Dictionary<string, string> map)
        {
            Debug.WriteLine($"导入工作表名为:{sheet.SheetName}");

            FieldMap = map;
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
        ///     读取一条数据
        /// </summary>
        /// <param name="line"></param>
        /// <returns>是否成功读取</returns>
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
        ///     实体保存
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        protected virtual bool Save(IRow row, TData entity)
        {
            if (!entity.__EntityStatus.IsModified)
            {
                WriteRowState(row, false, "数据全为默认值,不导入");
                return false;
            }
            var vmsg = entity.Validate();
            if (!vmsg.Succeed)
            {
                WriteRowState(row, false, "数据不合格,不导入。" + "\r\n" + vmsg);
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
                WriteRowState(row, false, "保存失败。" + "\r\n" + ex);
            }
            return true;
        }

        /// <summary>
        ///     读取一行的各个字段
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        private void ReadRowFields(IRow row, TData entity)
        {
            foreach (var field in ColumnFields.Keys)
            {
                if (!GetCellValue(row, field, out var cell, out var value))
                    continue;

                if (value == "#REF!" || value == "#N/A")
                {
                    WriteCellState(row, field, "公式错误,使用默认值", false);
                }
                else
                {
                    SetValue(entity, cell.ColumnIndex, value, row);
                }
            }
        }
        /// <summary>
        /// 取一个单元格的值
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
                    WriteCellState(row, column, "数据错误,使用默认值", false);
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
                case CellType.Formula: //公式列
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
        ///     设置字段值
        /// </summary>
        /// <param name="data">数据类对象</param>
        /// <param name="column">列号</param>
        /// <param name="value">读取出的文本值</param>
        /// <param name="row">当前行</param>
        protected virtual void SetValue(TData data, int column, string value, IRow row)
        {
            var field = ColumnFields[column];
            try
            {
                data.SetValue(field, value);
            }
            catch
            {
                WriteCellState(row, column, $"值[{value}]写入到字段[{field}]失败。", true);
            }
        }

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
        protected virtual void Initiate()
        {
        }

        #region 列与字段对应分析

        /// <summary>
        ///     最大列
        /// </summary>
        protected int MaxColumn;

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
                    WriteCellState(row, column, "字段名字为空白,导入直接中止", true);
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
            var cell = row.SafeGetCell(col);
            if (_drawingPatriarch == null)
            {
                _drawingPatriarch = Sheet.CreateDrawingPatriarch();
            }
            var msg = $"{(isError ? "错误" : "警告")}\r\n{message}";
            var comment = _drawingPatriarch.CreateCellComment(new XSSFClientAnchor(0, 0, 1, 1, col, row.RowNum, col + 3, row.RowNum + 3));
            comment.String = new XSSFRichTextString(msg);
            comment.Column = col;
            comment.Row = row.RowNum;
            cell.CellComment = comment;
            cell.CellStyle.FillForegroundColor = isError ? HSSFColor.Red.Index : HSSFColor.Yellow.Index;
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
            if (!succeed)
            {
                row.SetCellValue("错误", MaxColumn);
            }
            if (!string.IsNullOrEmpty(message))
            {
                row.SetCellValue(message, MaxColumn + 1);
            }
        }

        /// <summary>
        ///     得数字类型的值(日期或数字)
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="vl">要取的值</param>
        /// <returns>数字类型的值(日期或数字)</returns>
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
