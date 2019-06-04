// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-14
// // �޸�:2016-06-16
// // *****************************************************/
#region ����

using System.Collections.Generic;
using System.IO;
using Agebull.EntityModel.BusinessLogic.SqlServer;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.SqlServer;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

#endregion

namespace Agebull.EntityModel.Excel.SqlServer
{
    /// <summary>
    ///     Excel�������ɵĻ���
    /// </summary>
    /// <typeparam name="TData">��������</typeparam>
    /// <typeparam name="TAccess">�������Ͷ�Ӧ�����ݷ�����</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class ExcelReportBase<TData, TAccess, TDatabase> : BusinessLogicBase<TData, TAccess, TDatabase>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : SqlServerTable<TData, TDatabase>, new()
        where TDatabase : SqlServerDataBase
    {
        /// <summary>
        ///     �и����п�(��һ������Ϊ�п�,��һ������Ϊ�и�)
        /// </summary>
        protected readonly List<List<int>> matrix = new List<List<int>>();

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
            matrix.Clear();
            Styles.Clear();
            var l2 = new List<int>();
            matrix.Add(l2);
            for (var col = sc; col <= ec; col++)
            {
                l2.Add(sheet.GetColumnWidth(col));
            }
            l2 = new List<int>();
            matrix.Add(l2);

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
