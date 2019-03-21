﻿// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-14
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Newtonsoft.Json;

#endregion

namespace Agebull.EntityModel.EasyUI
{
    /// <summary>
    /// 支持Easy combobox的键值对象
    /// </summary>
    public class EasyComboValues
    {
        /// <summary>
        /// 构造
        /// </summary>
        public EasyComboValues()
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="extend"></param>
        /// <param name="tag"></param>
        public EasyComboValues(object key, object value, object extend = null, object tag = null)
        {
            Key = key;
            Value = value;
            Extend = extend?.ToString();
            Tag = tag?.ToString();
        }
        /// <summary>
        /// 空内容
        /// </summary>
        public static readonly EasyComboValues Empty = new EasyComboValues
        {
            Key = 0,
            Value = "-"
        };

        /// <summary>
        ///     键
        /// </summary>
        [JsonProperty("id")]
        public object Key { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [JsonProperty("text")]
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