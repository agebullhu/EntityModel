// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:14

namespace System.Text
{
    /// <summary>
    ///   unicodeBase64编，解码
    /// </summary>
    public static class UnicodeBase64
    {
        /// <summary>
        ///   得到文字每个字的拼音的第一个
        /// </summary>
        /// <param name="code"> </param>
        /// <returns> 返回编码的值($SG$) </returns>
        public static string EncodeToBase64(this string code)
        {
            return EncodeBase64(code) ;
        }

        /// <summary>
        ///   对文本进行Base64编码 使用unicode编码
        /// </summary>
        /// <param name="code"> 要编码的文本 </param>
        /// <returns> 编码后的文本 </returns>
        public static string EncodeBase64(string code)
        {
            string encode ;
            byte[] bytes = Encoding.UTF8.GetBytes(code) ;
            try
            {
                encode = Convert.ToBase64String(bytes) ;
            }
            catch
            {
                encode = code ;
            }
            return encode ;
        }

        /// <summary>
        ///   对文本进行Base64解码
        /// </summary>
        /// <param name="code"> </param>
        /// <returns> 返回编码的值($SG$) </returns>
        public static string DecodeFromBase64(this string code)
        {
            return DecodeBase64(code) ;
        }

        /// <summary>
        ///   对文本进行Base64解码 使用unicode编码
        /// </summary>
        /// <param name="code"> 要解码的文本 </param>
        /// <returns> 解码后的文本 </returns>
        public static string DecodeBase64(string code)
        {
            string decode ;
            byte[] bytes = Convert.FromBase64String(code) ;
            try
            {
                decode = Encoding.UTF8.GetString(bytes , 0 , bytes.Length) ;
            }
            catch
            {
                decode = code ;
            }
            return decode ;
        }
    }
}
