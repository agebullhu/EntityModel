// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:14
namespace System.Text
{
    /// <summary>
    ///   全角中文的操作类
    /// </summary>
    public static class QuanJiaoChinese
    {
        ///<summary>
        ///  是否半角文字 判断字符是否英文半角字符或标点 32 空格 33-47 标点 48-57 0~9 58-64 标点 65-90 A~Z 91-96 标点 97-122 a~z 123-126 标点
        ///</summary>
        ///<param name="c"> </param>
        ///<returns> </returns>
        public static bool IsBjChar(char c)
        {
            int i = c ;
            return i >= 32 && i <= 126 ;
        }

        /// <summary>
        ///   判断字符是否全角字符或标点 全角字符 - 65248 = 半角字符 全角空格例外
        /// </summary>
        /// <param name="c"> </param>
        /// <returns> </returns>
        public static bool IsQjChar(char c)
        {
            if(c == '\u3000')
            {
                return true ;
            }
            int i = c - 65248 ;
            return i >= 32 && IsBjChar((char) i) ;
        }

        /// <summary>
        ///   将字符串中的全角字符转换为半角
        /// </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static string ConverToAscii(this string s)
        {
            return ToBj(s) ;
        }


        /// <summary>
        ///   将字符串中的全角字符转换为半角
        /// </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static string ToBj(string s)
        {
            if(string.IsNullOrWhiteSpace(s))
            {
                return s ;
            }
            s = s.Trim();
            StringBuilder sb = new StringBuilder(s.Length) ;
            foreach(char t in s)
            {
                switch(t)
                {
                case '\u3000' :
                    sb.Append('\u0020') ;
                    break ;
                default :
                    if(IsQjChar(t))
                    {
                        sb.Append((char) (t - 65248)) ;
                    }
                    else
                    {
                        sb.Append(t) ;
                    }
                    break ;
                }
            }
            return sb.ToString().Trim(new[]
            {
                    ' ' , '\r' , '\n' , '\t'
            }) ;
        }
    }
}
