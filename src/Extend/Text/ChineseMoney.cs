// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:13

#region

using System.Collections.Generic ;

#endregion

namespace System.Text
{
    /// <summary>
    ///   中文金额 该类重载的 ToString() 方法返回的是大写金额字符串
    /// </summary>
    public static class ChineseMoneyHelper
    {
        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="money"> </param>
        /// <returns> </returns>
        public static string ToChineseMoney(this decimal money)
        {
            return ChineseMoney.InvoiceString(money);
        }
        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="money"> </param>
        /// <returns> </returns>
        public static string ToPrintChineseMoney(this decimal money)
        {
            return ChineseMoney.InvoicePrintString(money);
        }
        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="money"> </param>
        /// <returns> </returns>
        public static string ToChineseNumber(this int money)
        {
            return ChineseNumber.NumberString(money);
        }
    }


    /// <summary>
    ///   中文金额 该类重载的 ToString() 方法返回的是大写金额字符串
    /// </summary>
    public static class ChineseNumber
    {
        private static readonly char[] Digit =
                {
                        '零' , '壹' , '贰' , '叁' , '肆' , '伍' , '陆' , '柒' , '捌' , '玖'
                }; // 大写数字

        private static readonly char[] Wei =
                {
                        ' ' , '拾' , '佰' , '仟' , '万' , '拾' , '佰' , '仟' , '亿' , '拾' , '佰' , '仟' , '万' , '拾' , '佰' , '仟'
                }; // 大写数字

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> </returns>
        public static string NumberString(int number)
        {
            if (number == 0)
            {
                return "零";
            }
            var sc = new List<string>();
            GetString(Math.Abs(number), sc, 0);
            var sb = new StringBuilder();
            if (number < 0)
            {
                sb.Append("负");
            }
            foreach (var s in sc)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }

        private static void GetString(long data, List<string> sc, int pos)
        {
            if (data == 0)
            {
                return;
            }
            sc.Insert(0, string.Format("{0}{1}", Digit[Convert.ToInt32(data % 10)], Wei[pos]));
            GetString(data / 10, sc, pos + 1);
        }

    }
    /// <summary>
    ///   中文金额 该类重载的 ToString() 方法返回的是大写金额字符串
    /// </summary>
    public static class ChineseMoney
    {
        private static readonly char[] Digit =
                {
                        '零' , '壹' , '贰' , '叁' , '肆' , '伍' , '陆' , '柒' , '捌' , '玖'
                } ; // 大写数字

        private static readonly char[] Wei =
                {
                        '分' , '角' , '元' , '拾' , '佰' , '仟' , '万' , '拾' , '佰' , '仟' , '亿' , '拾' , '佰' , '仟' , '万' , '拾' , '佰' , '仟'
                } ; // 大写数字

        /// <summary>
        ///   构造函数
        /// </summary>
        /// <param name="money"> </param>
        /// <returns> </returns>
        public static string InvoiceString(decimal money)
        {
            // 金额*100，即以“分”为单位的金额
            var money100 = Convert.ToInt64(money * 100m) ;
            if(money100 == 0)
            {
                return "零元" ;
            }
            var sc = new List<string>() ;
            GetString(Math.Abs(money100) , sc , 0) ;
            var sb = new StringBuilder() ;
            if(money100 < 0)
            {
                sb.Append("负") ;
            }
            foreach(var s in sc)
            {
                sb.Append(s) ;
            }
            return sb.ToString() ;
        }

        private static void GetString(long data , List<string> sc , int pos)
        {
            if(data == 0)
            {
                return ;
            }
            sc.Insert(0 , string.Format("{0}{1}" , Digit[Convert.ToInt32(data % 10)] , Wei[pos])) ;
            GetString(data / 10 , sc , pos + 1) ;
        }

        /// <summary>
        ///   转换为套打用的文本
        /// </summary>
        /// <param name="money"> </param>
        /// <returns> </returns>
        public static string InvoicePrintString(decimal money)
        {
            var sc = new List<string>() ;
            // 金额*100，即以“分”为单位的金额
            var money100 = Convert.ToInt64(money * 100m) ;
            if(money100 == 0)
            {
                return "" ;
            }
            while(money100 > 0)
            {
                sc.Insert(0 , Digit[Convert.ToInt32(money100 % 10)].ToString()) ;
                money100 /= 10 ;
            }
            while(sc.Count < 8)
            {
                sc.Insert(0 , "零") ;
            }
            var sb = new StringBuilder() ;
            foreach(var s in sc)
            {
                sb.Append("　　" + s) ;
            }
            return sb.ToString() ;
        }
    }
}
