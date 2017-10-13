// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-29
// // *****************************************************/

#region 引用

using Gboxt.Common.WebUI;
using Gboxt.Common.SystemModel.DataAccess;

#endregion

namespace Gboxt.Common.SystemModel.DataDictionaryPage
{
    /// <summary>
    /// 数据字典AJAX访问的API类
    /// </summary>
    public partial class DataDictionaryPageAction : ApiPageBaseEx<DataDictionaryData, DataDictionaryDataAccess>
    {
        public DataDictionaryPageAction()
        {
            IsPublicPage = true;
        }
        /// <summary>
        ///     读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected override void ReadFormData(DataDictionaryData data, FormConvert convert)
        {
            data.Name = convert.ToString("Name", false);
            data.Value = convert.ToString("Value", false);
            data.State = convert.ToLong("State");
            data.Feature = convert.ToString("Feature", true);
            data.Memo = convert.ToString("Memo", true);
        }
    }
}