
using Agebull.Common.Base;
using Agebull.EntityModel.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Excel
{
    /// <summary>
    ///     Excel导出类
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TAccess">数据类型对应的数据访问类</typeparam>
    public class ExcelExporter<TData,TPrimaryKey, TAccess> : ScopeBase
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, new()
        where TAccess : class, IDataAccess<TData>, new()
    {



        private TAccess _access;

        /// <summary>
        ///     数据访问对象
        /// </summary>
        public TAccess Access => _access ??= CreateAccess();

        /// <summary>
        ///     数据访问对象
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            return new TAccess();
        }

        /// <summary>
        /// 工作簿对象
        /// </summary>
        protected XSSFWorkbook Book { get; private set; }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <returns>数据</returns>
        public byte[] ExportExcel(string name, string path)
        {
            Book = new XSSFWorkbook(path);
            var sheet = Book.CreateSheet(name);
            ExportExcel(sheet);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="lambda">查询条件</param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns>数据</returns>
        public byte[] ExportExcel(LambdaItem<TData> lambda, string name, string path)
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            ExportExcel(sheet, lambda).Wait();
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.ToArray();
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="lambda">查询条件</param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns>数据</returns>
        public async Task<byte[]> ExportExcelAsync(LambdaItem<TData> lambda, string name, string path)
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            await ExportExcel(sheet, lambda);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.ToArray();
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <param name="lambda">查询条件</param>
        /// <returns>数据</returns>
        public async Task ExportExcel(ISheet sheet, LambdaItem<TData> lambda)
        {
            var datas = await Access.AllAsync(lambda);
            OnDataLoad?.Invoke(datas);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="lambda">查询条件</param>
        /// <param name="name"></param>
        /// <returns>数据</returns>
        public byte[] ExportExcel(Expression<Func<TData, bool>> lambda, string name = "Sheet1")
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            ExportExcel(sheet, lambda);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <param name="lambda">查询条件</param>
        /// <returns>数据</returns>
        public void ExportExcel(ISheet sheet, Expression<Func<TData, bool>> lambda)
        {
            var datas = Access.All(lambda);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        /// 数据载入的处理
        /// </summary>
        public Action<List<TData>> OnDataLoad;

        /// <summary>
        ///     导出Excel
        /// </summary>
        /// <param name="sheet">导入所在的工作表</param>
        /// <returns>数据</returns>
        public void ExportExcel(ISheet sheet)
        {
            var datas = Access.All();
            OnDataLoad?.Invoke(datas);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="datas"></param>
        protected virtual void WriteToSheet(ISheet sheet, List<TData> datas)
        {
            WriteToSheetByConfig(sheet, datas);
        }

        /// <summary>
        /// 写入数据到工作表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="datas"></param>
        protected void WriteToSheetByConfig(ISheet sheet, List<TData> datas)
        {
            if (datas == null || datas.Count == 0)
                return;
            var format = Book.CreateDataFormat();
            var styleint2 = Book.CreateCellStyle();
            styleint2.DataFormat = 1;
            var stylenum2 = Book.CreateCellStyle();
            stylenum2.DataFormat = 2;
            var styledate = Book.CreateCellStyle();
            styledate.DataFormat = 0x16;
            var styleint = Book.CreateCellStyle();
            styleint.DataFormat = 3;
            var stylenum = Book.CreateCellStyle();
            stylenum.DataFormat = 4;
            var stylestr = Book.CreateCellStyle();
            stylestr.DataFormat = format.GetFormat("@");


            int line = 0;
            var row = sheet.SafeGetRow(line);
            var importFields = datas[0].__Struct.Properties.Values.Where(p => p.CanExport).OrderBy(p => p.Index).ToArray();
            int idx = 0;
            foreach (var field in importFields)
            {
                row.SetCellValue(field.Title ?? field.PropertyName, idx++, stylestr);
            }
            foreach (var data in datas)
            {
                idx = 0;
                row = sheet.SafeGetRow(++line);
                foreach (var field in importFields)
                {
                    var vl = data.GetValue(field.PropertyName);
                    if (vl != null)
                    {
                        if (field.PropertyType == typeof(int))
                            row.SetNumberValue((int)vl, idx, styleint);
                        else if (field.PropertyType == typeof(decimal))
                            row.SetCellMoney((decimal)vl, idx, stylenum);
                        else if (field.PropertyType == typeof(DateTime))
                            row.SetDateCellValue((DateTime)vl, idx, styledate);
                        else
                            row.SetCellValue(vl.ToString(), idx, stylestr);
                    }
                    idx++;
                }
            }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            //Book.Dispose();
        }
    }
}
