// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-14
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    /// 支持Easy combobox的键值对象
    /// </summary>
    public class EasyComboValues
    {
        public EasyComboValues()
        {
        }
        
        public EasyComboValues(object key, object value, object extend = null, object tag = null)
        {
            Key = key;
            Value = value;
            Extend = extend?.ToString();
            Tag = tag?.ToString();
        }

        public static readonly EasyComboValues Empty = new EasyComboValues
        {
            Key = 0,
            Value = "-"
        };

        /// <summary>
        ///     键
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public object Key { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public object Value { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [JsonProperty("extend")]
        public string Extend { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}