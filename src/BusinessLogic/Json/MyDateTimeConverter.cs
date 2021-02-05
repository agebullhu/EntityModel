// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-13
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     自定义的日期JSON转换器(用于Newtonsoft.Json)
    /// </summary>
    public class MyDateTimeConverter : DateTimeConverterBase
    {
        private static readonly IsoDateTimeConverter DtConverter = new IsoDateTimeConverter();

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        ///     The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            if (date <= new DateTime(1901, 1, 1))
            {
                writer.WriteNull();
            }
            else
            {
                DtConverter.WriteJson(writer, value, serializer);
            }
        }
    }
}