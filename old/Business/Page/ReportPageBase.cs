// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-14
// // �޸�:2016-06-16
// // *****************************************************/

using System;
using System.IO;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic.Report;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     ����ҳ��Ļ���
    /// </summary>
    public abstract class ReportPageBase : MyPageBase
    {
        /// <summary>
        ///     ҳ������������
        /// </summary>
        protected sealed override void OnPageLoaded()
        {
            CreateReport();
        }

        /// <summary>
        ///     ��ǰ�����Ӧ�Ķ���
        /// </summary>
        protected string ReportAction { get; set; }

        /// <summary>
        ///     ��鶯���Ƿ�����
        /// </summary>
        protected override bool CheckCanDo()
        {
            return BusinessContext.Current.PowerChecker.CanDoAction(LoginUser, PageItem, ReportAction);
        }

        /// <summary>
        ///     ҳ�洦��ǰ׼��
        /// </summary>
        protected override void OnPrepare()
        {

        }

        /// <summary>
        ///     ҳ�洦�����
        /// </summary>
        protected override void OnResult()
        {

        }


        /// <summary>
        ///     ҳ������������
        /// </summary>
        protected abstract void CreateReport();

        /// <summary>
        ///     д��������
        ///     
        /// </summary>
        /// <param name="name">�ļ���</param>
        /// <param name="buffer">д��Ķ���������</param>
        protected void Write(string name, byte[] buffer)
        {
            if (Request?.UserAgent?.ToLower().IndexOf("firefox",StringComparison.OrdinalIgnoreCase) > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + name + "\"");
            }
            else
            {
                Response.AddHeader("Content-Disposition", $"attachment;filename={Server.UrlEncode(name)}");
            }
            //this.Response.AddHeader("Content-Disposition", "attachment;filename=" + name);
            //this.Response.AddHeader("Content-Disposition", $"attachment;filename={Server.UrlEncode(name)}");
            Response.AddHeader("Content-Length", buffer.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(buffer);
        }
    }

    /// <summary>
    ///     ����ҳ��Ļ���
    /// </summary>
    public abstract class ExportPageBase<TData, TAccess> : ReportPageBase
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData>, new()
    {
        /// <summary>
        ///     ҳ������������
        /// </summary>
        protected ExportPageBase()
        {
            ReportAction = "export";
        }
        /// <summary>
        /// ��ǰ����ɸѡ��
        /// </summary>
        /// <returns></returns>
        protected abstract LambdaItem<TData> GetFilter();
        /// <summary>
        /// ����������
        /// </summary>
        protected abstract string Name { get;}
        /// <summary>
        ///     ҳ������������
        /// </summary>
        protected override void CreateReport()
        {
            BusinessContext.Current.IsUnSafeMode = true;
            var exporter = new ExcelExporter<TData, TAccess>();
            var temp = Path.Combine(Request.MapPath("~"),"Report","excel.xlsx");
            var buffer = exporter.ExportExcel(GetFilter(), Name, temp);
            Write(Name + ".xlsx", buffer);
        }
    }
}