
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
    ///     Excel������
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public class ExcelExporter<TData, TPrimaryKey>
            where TData : class, new()
    {
        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        public DataQuery<TData> DataQuery { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        protected XSSFWorkbook Book { get; private set; }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <returns>����</returns>
        public async Task<byte[]> ExportExcel(string name, string path)
        {
            Book = new XSSFWorkbook(path);
            var sheet = Book.CreateSheet(name);
            await ExportExcel(sheet);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns>����</returns>
        public async Task<byte[]> ExportExcel(LambdaItem<TData> lambda, string name, string path)
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            await ExportExcel(sheet, lambda);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.ToArray();
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <returns>����</returns>
        public async Task<byte[]> ExportExcelAsync(LambdaItem<TData> lambda, string name)
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            await ExportExcel(sheet, lambda);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.ToArray();
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <param name="lambda">��ѯ����</param>
        /// <returns>����</returns>
        public async Task ExportExcel(ISheet sheet, LambdaItem<TData> lambda)
        {
            var datas = await DataQuery.AllAsync(lambda);
            OnDataLoad?.Invoke(datas);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <returns>����</returns>
        public async Task<byte[]> ExportExcel(Expression<Func<TData, bool>> lambda, string name = "Sheet1")
        {
            Book = new XSSFWorkbook();
            var sheet = Book.CreateSheet(name);
            await ExportExcel(sheet, lambda);
            using var mem = new MemoryStream();
            Book.Write(mem);
            return mem.GetBuffer();
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <param name="lambda">��ѯ����</param>
        /// <returns>����</returns>
        public async Task ExportExcel(ISheet sheet, Expression<Func<TData, bool>> lambda)
        {
            var datas = await DataQuery.AllAsync(lambda);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        /// ��������Ĵ���
        /// </summary>
        public Func<List<TData>, Task> OnDataLoad;

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <returns>����</returns>
        public async Task ExportExcel(ISheet sheet)
        {
            var datas = await DataQuery.AllAsync();
            if (OnDataLoad != null)
                await OnDataLoad(datas);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        /// д�����ݵ�������
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="datas"></param>
        protected virtual void WriteToSheet(ISheet sheet, List<TData> datas)
        {
            WriteToSheetByConfig(sheet, datas);
        }

        /// <summary>
        /// д�����ݵ�������
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
            var importFields = DataQuery.Option.Properties.Where(p => p.CanExport).OrderBy(p => p.Index).ToArray();
            int idx = 0;
            foreach (var field in importFields)
            {
                row.SetCellValue(field.Caption, idx++, stylestr);
            }
            foreach (var data in datas)
            {
                idx = 0;
                row = sheet.SafeGetRow(++line);
                foreach (var field in importFields)
                {
                    var vl = DataQuery.Provider.EntityOperator.GetValue(data, field.PropertyName);
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

    }
}
