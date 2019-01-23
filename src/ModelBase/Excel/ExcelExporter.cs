#if !NETCOREAPP
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Agebull.Common.DataModel.BusinessLogic.Report
{
    /// <summary>
    ///     Excel������
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    /// <typeparam name="TAccess">�������Ͷ�Ӧ�����ݷ�����</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class ExcelExporter<TData, TAccess, TDatabase> : BusinessLogicBase<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        /// <summary>
        /// ����������
        /// </summary>
        protected XSSFWorkbook _book;

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <returns>����</returns>
        public byte[] ExportExcel(string name,string path)
        {
            _book = new XSSFWorkbook(path);
            var sheet = _book.CreateSheet(name);
            ExportExcel(sheet);
            using (var mem = new MemoryStream())
            {
                _book.Write(mem);
                return mem.GetBuffer();
            }
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
            _book = new XSSFWorkbook();
            var sheet = _book.CreateSheet(name);
            ExportExcel(sheet, lambda);
            using (var mem = new MemoryStream())
            {
                _book.Write(mem);
                return mem.ToArray();
            }
        }


        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <param name="lambda">��ѯ����</param>
        /// <returns>����</returns>
        public void ExportExcel(ISheet sheet, LambdaItem<TData> lambda)
        {
            var datas = Access.All(lambda);
            OnDataLoaded?.Invoke(datas);
            WriteToSheet(sheet, datas);
        }
        /// <summary>
        /// ����������Զ��崦��
        /// </summary>
        public Action<List<TData>> OnDataLoaded;

        /// <summary>
        ///     ����Excel
        /// </summary>
        /// <param name="lambda">��ѯ����</param>
        /// <param name="name"></param>
        /// <returns>����</returns>
        public byte[] ExportExcel(Expression<Func<TData, bool>> lambda, string name = "Sheet1")
        {
            _book = new XSSFWorkbook();
            var sheet = _book.CreateSheet(name);
            ExportExcel(sheet, lambda);
            using (var mem = new MemoryStream())
            {
                _book.Write(mem);
                return mem.GetBuffer();
            }
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
        ///     ����Excel
        /// </summary>
        /// <param name="sheet">�������ڵĹ�����</param>
        /// <returns>����</returns>
        public void ExportExcel(ISheet sheet)
        {
            var datas = Access.All();
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
            var format = _book.CreateDataFormat();
            var styleint2 = _book.CreateCellStyle();
            styleint2.DataFormat = 1;
            var stylenum2 = _book.CreateCellStyle();
            stylenum2.DataFormat = 2;
            var styledate = _book.CreateCellStyle();
            styledate.DataFormat = 0x16;
            var styleint = _book.CreateCellStyle();
            styleint.DataFormat = 3;
            var stylenum = _book.CreateCellStyle();
            stylenum.DataFormat = 4;
            var stylestr = _book.CreateCellStyle();
            stylestr.DataFormat = format.GetFormat("@");


            int line = 0;
            var row = sheet.SafeGetRow(line);
            var importFields = datas[0].__Struct.Properties.Values.Where(p => p.CanExport).OrderBy(p=>p.Index).ToArray();
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
    }
}
#endif