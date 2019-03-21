// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace Agebull.EntityModel.Common.WebUI
{
    /// <summary>
    /// 支持EasyUi表格列表数据的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EasyUiGridData<T>
        where T : class
    {
        /// <summary>
        ///     返回的数据
        /// </summary>
        [JsonProperty("rows", Required = Required.Always)]
        public IList<T> Data { get; set; }

        /// <summary>
        ///     总行数
        /// </summary>
        [JsonProperty("total", Required = Required.Always)]
        public int Total { get; set; }


        /// <summary>
        ///     页脚
        /// </summary>
        [JsonProperty("footer", Required = Required.Default)]
        public IList<T> Footer { get; set; }
        
    }
}