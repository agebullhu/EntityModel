
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
    ///     Excel������
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    /// <typeparam name="TAccess">�������Ͷ�Ӧ�����ݷ�����</typeparam>
    public class ExcelExporter<TData,TPrimaryKey, TAccess> : ScopeBase
        where TData : EditDataObject, IIdentityData<TPrimaryKey>, new()
        where TAccess : class, IDataAccess<TData>, new()
    {



        private TAccess _access;

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        public TAccess Access => _access ??= CreateAccess();

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            return new TAccess();
        }

        /// <summary>
        /// ����������
        /// </summary>
        protected XSSFWorkbook Book { get; private set; }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <returns>����</returns>
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
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns>����</returns>
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
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns>����</returns>
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
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <param name="lambda">��ѯ����</param>
        /// <returns>����</returns>
        public async Task ExportExcel(ISheet sheet, LambdaItem<TData> lambda)
        {
            var datas = await Access.AllAsync(lambda);
            OnDataLoad?.Invoke(datas);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <returns>����</returns>
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
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <param name="lambda">��ѯ����</param>
        /// <returns>����</returns>
        public void ExportExcel(ISheet sheet, Expression<Func<TData, bool>> lambda)
        {
            var datas = Access.All(lambda);
            WriteToSheet(sheet, datas);
        }

        /// <summary>
        /// ��������Ĵ���
        /// </summary>
        public Action<List<TData>> OnDataLoad;

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <returns>����</returns>
        public void ExportExcel(ISheet sheet)
        {
            var datas = Access.All();
            OnDataLoad?.Invoke(datas);
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
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            //Book.Dispose();
        }
    }
}
