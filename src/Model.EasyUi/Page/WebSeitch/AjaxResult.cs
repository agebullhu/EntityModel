// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     AJAX的标准返回
    /// </summary>
    [DataContract]
    public class AjaxResult
    {
        /// <summary>
        ///     结果状态
        /// </summary>
        [DataMember, JsonProperty("state")]
        public int State { get; set; }

        /// <summary>
        ///     结果状态
        /// </summary>
        [DataMember, JsonProperty("succeed")]
        public bool Succeed { get; set; }

        /// <summary>
        ///     操作消息
        /// </summary>
        [DataMember, JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     操作消息
        /// </summary>
        [DataMember, JsonProperty("message2")]
        public string Message2 { get; set; }
    }

    /// <summary>
    ///     AJAX的标准返回
    /// </summary>
    [DataContract]
    public class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        ///     结果值
        /// </summary>
        [DataMember, JsonProperty("value")]
        public T Value { get; set; }
    }
}