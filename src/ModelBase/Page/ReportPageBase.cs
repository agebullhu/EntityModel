// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-14
// // 修改:2016-06-16
// // *****************************************************/

using System;
using System.IO;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic.Report;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     报表页面的基类
    /// </summary>
    public abstract class ReportPageBase : MyPageBase
    {
        /// <summary>
        ///     页面操作处理入口
        /// </summary>
        protected sealed override void OnPageLoaded()
        {
            CreateReport();
        }

        /// <summary>
        ///     当前报表对应的动作
        /// </summary>
        protected string ReportAction { get; set; }

        /// <summary>
        ///     检查动作是否允许
        /// </summary>
        protected override bool CheckCanDo()
        {
            return BusinessContext.Current.PowerChecker.CanDoAction(LoginUser, PageItem, ReportAction);
        }

        /// <summary>
        ///     页面处理前准备
        /// </summary>
        protected override void OnPrepare()
        {

        }

        /// <summary>
        ///     页面处理结束
        /// </summary>
        protected override void OnResult()
        {

        }


        /// <summary>
        ///     页面操作处理入口
        /// </summary>
        protected abstract void CreateReport();

        /// <summary>
        ///     写入数据流
        ///     
        /// </summary>
        /// <param name="name">文件名</param>
        /// <param name="buffer">写入的二进制内容</param>
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
    ///     报表页面的基类
    /// </summary>
    public abstract class ExportPageBase<TData, TAccess> : ReportPageBase
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData>, new()
    {
        /// <summary>
        ///     页面操作处理入口
        /// </summary>
        protected ExportPageBase()
        {
            ReportAction = "export";
        }
        /// <summary>
        /// 当前数据筛选器
        /// </summary>
        /// <returns></returns>
        protected abstract LambdaItem<TData> GetFilter();
        /// <summary>
        /// 导出表名称
        /// </summary>
        protected abstract string Name { get;}
        /// <summary>
        ///     页面操作处理入口
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