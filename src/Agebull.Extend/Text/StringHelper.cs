// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:14

/******************修改记录************************
 2014年12月8日 agebull 增加单词转到复数形式的扩展
 
**************************************************/
#region

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#endregion
namespace System
{
    /// <summary>
    /// 值格式化扩展
    /// </summary>
    public static class ValueToStringHelper
    {
        /// <summary>
        /// 拆分到单词(每个标点或大写字母作为分隔符)
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式</returns>
        public static string[] SpliteWord(this string word)
        {
            return StringHelper.SpliteWord(word);
        }

        /// <summary>
        /// 一个字符器转为名称
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="toFirstUpper">是否首字母大写</param>
        /// <returns>字符器</returns>
        public static string ToName(this string word, bool toFirstUpper = true)
        {
            return StringHelper.ToName(word, toFirstUpper);
        }

        /// <summary>
        /// 到一个单词的复数形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式</returns>
        public static string ToPluralism(this string word)
        {
            return StringHelper.ToPluralism(word);
        }

        /// <summary>
        ///   数据是不是相等
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <returns> </returns>
        public static bool IsEquals(this string a, string b)
        {
            bool aNull = string.IsNullOrWhiteSpace(a);
            bool bNull = string.IsNullOrWhiteSpace(b);
            return aNull && bNull || (aNull == bNull && a.Equals(b, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 到用空文本替换的文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        public static string ToNullString(this string str, string nullString = null)
        {
            return String.IsNullOrWhiteSpace(str) ? nullString : str;
        }

        /// <summary>
        /// 到百分比显示
        /// </summary>
        /// <param name="d"></param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <returns></returns>
        public static string ToPercent(this decimal d, string nullString)
        {
            return d == 0 ? (nullString ?? "0") : String.Format("{0:F2}%", d * 100.0M);
        }

        /// <summary>
        /// 到百分比显示
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToPercent(this decimal d)
        {
            return String.Format("{0:F2}%", d * 100.0M);
        }

        /// <summary>
        /// 到没有多余小数位的文本
        /// </summary>
        /// <param name="d"></param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <param name="dia">最长小数位(默认为10)</param>
        /// <returns></returns>
        public static string ToNullString(this decimal d, int dia, string nullString = null)
        {
            if (d == 0M)
                return nullString ?? "0";
            if (dia <= 0)
                dia = 10;
            return d.ToString(String.Format("F{0}", dia)).TrimEnd('0').TrimEnd('.');
        }

        /// <summary>
        /// 到没有多余小数位的文本
        /// </summary>
        /// <param name="d"></param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <param name="dia">最长小数位(默认为10)</param>
        /// <returns></returns>
        public static string ToNullString(this decimal? d, int dia, string nullString = null)
        {
            if (d == null
                || d.Value == 0M)
                return nullString;
            if (dia <= 0)
                dia = 10;
            return d.Value.ToString(String.Format("F{0}", dia)).TrimEnd('0').TrimEnd('.');
        }

        /// <summary>
        /// 到可为空的时间格式化
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fmt">格式化</param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <returns></returns>
        public static string ToNullString(this DateTime v, string fmt, string nullString)
        {
            return Equals(v, DateTime.MinValue) ? nullString : v.ToString(fmt);
        }

        /// <summary>
        /// 到可为空的时间格式化
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fmt">格式化</param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <returns></returns>
        public static string ToNullString(this DateTime? v, string fmt, string nullString)
        {
            return v == null || Equals(v.Value, DateTime.MinValue) ? nullString : v.Value.ToString(fmt);
        }
        /// <summary>
        /// 到缺省值用空文本表示的值
        /// </summary>
        /// <param name="v"></param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <returns></returns>
        public static string ToNullString<T>(this T v, string nullString = null) where T : struct
        {
            return Equals(v, default(T)) ? (nullString ?? v.ToString()) : v.ToString();
        }

        /// <summary>
        /// 到没有多余小数位的文本
        /// </summary>
        /// <param name="v"></param>
        /// <param name="nullString">为0时表示的文本</param>
        /// <returns></returns>
        public static string ToNullString<T>(this T? v, string nullString = null) where T : struct
        {
            return v == null || Equals(v.Value, default(T)) ? (nullString ?? v.ToString()) : v.Value.ToString();
        }
        /// <summary>
        /// 到固定长度显示
        /// </summary>
        /// <param name="d">小数</param>
        /// <param name="len">总长度</param>
        /// <param name="dit">小数位</param>
        /// <returns></returns>
        public static string ToFixLenString(this decimal d, int len, int dit)
        {
            string s = d.ToString("F" + dit);
            StringBuilder sb = new StringBuilder();
            int l = len - s.Length;
            if (l > 0)
                sb.Append(' ', l);
            sb.Append(s);
            return sb.ToString();
        }
        /// <summary>
        /// 到固定长度显示
        /// </summary>
        /// <param name="d">小数</param>
        /// <param name="len">总长度</param>
        /// <param name="dit">小数位</param>
        /// <returns></returns>
        public static string ToFixLenString(this float d, int len, int dit)
        {
            string s = d.ToString("F" + dit);
            StringBuilder sb = new StringBuilder();
            int l = len - s.Length;
            if (l > 0)
                sb.Append(' ', l);
            sb.Append(s);
            return sb.ToString();
        }
        /// <summary>
        /// 到固定长度显示
        /// </summary>
        /// <param name="d">小数</param>
        /// <param name="len">总长度</param>
        /// <param name="dit">小数位</param>
        /// <returns></returns>
        public static string ToFixLenString(this double d, int len, int dit)
        {
            string s = d.ToString("F" + dit);
            StringBuilder sb = new StringBuilder();
            int l = len - s.Length;
            if (l > 0)
                sb.Append(' ', l);
            sb.Append(s);
            return sb.ToString();
        }
        /// <summary>
        /// 到固定长度显示
        /// </summary>
        /// <param name="d">对象</param>
        /// <param name="len">总长度</param>
        /// <returns></returns>
        public static string ToFixLenString(this int d, int len)
        {
            string s = d.ToString(CultureInfo.InvariantCulture);
            StringBuilder sb = new StringBuilder();
            int l = len - s.Length;
            if (l > 0)
                sb.Append(' ', l);
            sb.Append(s);
            return sb.ToString();
        }
        /// <summary>
        /// 到固定长度显示
        /// </summary>
        /// <param name="d">对象</param>
        /// <param name="len">总长度</param>
        /// <returns></returns>
        public static string ToFixLenString<T>(this T d, int len)
        {
            string s = d.ToString();
            StringBuilder sb = new StringBuilder();
            int l = len - s.Length;
            if (l > 1)
                sb.Append(' ', l / 2);
            sb.Append(s);
            if (l > 0)
                sb.Append(' ', l - l / 2);
            return sb.ToString();
        }
    }
}
namespace System.Text
{
    /// <summary>
    ///   文本帮助类
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 拆分到单词(每个标点或大写字母作为分隔符)
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式</returns>
        public static string[] SpliteWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return null;
            List<string> words = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var ch in word)
            {
                switch (ch)
                {
                    case '_':
                    case '-':
                    case '\\':
                    case '/':
                    case '~':
                    case '!':
                    case '@':
                    case '$':
                    case '%':
                    case '&':
                    case '*':
                    case '|':
                    case '^':
                    case '.':
                    case ',':
                    case '?':
                    case '+':
                    case '=':
                    case '[':
                    case ']':
                    case '(':
                    case ')':
                    case '>':
                    case '<':
                    case '}':
                    case '{':
                        if (sb.Length > 0)
                        {
                            words.Add(sb.ToString());
                            sb.Clear();
                        }
                        continue;
                }
                if (ch >= 'A' && ch <= 'Z' && sb.Length > 0)
                {
                    words.Add(sb.ToString());
                    sb.Clear();
                }

                sb.Append(ch);
            }
            if (sb.Length > 0)
            {
                words.Add(sb.ToString());
            }
            return words.ToArray();
        }

        /// <summary>
        /// 一个字符器转为名称
        /// </summary>
        /// <param name="word">单词</param>
        /// <param name="toFirstUpper">是否首字母大写</param>
        /// <returns>字符器</returns>
        public static string ToName(string word, bool toFirstUpper = true)
        {
            if (string.IsNullOrEmpty(word))
                return null;
            var txts = SpliteWord(word);

            StringBuilder sb = new StringBuilder();

            foreach (string t in txts)
            {
                sb.Append(toFirstUpper ? ToWord(t) : t);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 到一个单词的复数形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式</returns>
        public static string ToPluralism(string word)
        {
            if (string.IsNullOrEmpty(word))
                return null;
            var txts = SpliteWord(word);

            StringBuilder sb = new StringBuilder();
            int index = 0;
            for (; index < txts.Length - 1; index++)
            {
                sb.Append(txts[index]);
            }
            sb.Append(ToPluralismInner(txts[index]));
            return sb.ToString();
        }
        /// <summary>
        /// 到一个单词的复数形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式</returns>
        private static string ToPluralismInner(string word)
        {
            switch (word.ToLower())
            {
                case "child":
                case "children":
                    return "Children";
                case "brother":
                case "brethren":
                    return "Brethren";
                case "ox":
                case "oxen":
                    return "Oxen";
                case "man":
                case "men":
                    return "Men";
                case "women":
                case "woman":
                    return "Women";
            }

            if (word.Length >= 3)
            {
                switch (word.Substring(word.Length - 2).ToLower())
                {
                    case "is":
                        return string.Format("{0}es", word.Substring(0, word.Length - 2));
                    case "fe":
                        return string.Format("{0}ves", word.Substring(0, word.Length - 1));
                    case "ch":
                    case "sh":
                        return string.Format("{0}es", word);
                }
            }
            switch (word[word.Length - 1])
            {
                case 's':
                case 'z':
                case 'x':
                case 'o':
                    return string.Format("{0}es", word);
                case 'f':
                    return string.Format("{0}ves", word);
                case 'y':
                    return string.Format("{0}ies", word.Substring(0, word.Length - 1));
            }

            return string.Format("{0}s", word);
        }
        /// <summary>
        ///   到首字母大写的文本
        /// </summary>
        /// <param name="word"> </param>
        /// <returns> </returns>
        public static string ToWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return string.Empty;
            }
            return word.Length == 1
                           ? word.ToUpper()
                           : string.Format("{0}{1}", ToUpper(word[0]), word.Substring(1));
        }
        /// <summary>
        ///   到首字母小写的文本
        /// </summary>
        /// <param name="word"> </param>
        /// <returns> </returns>
        public static string ToWord2(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return string.Empty;
            }
            return word.Length == 1
                           ? word.ToLower()
                           : string.Format("{0}{1}", ToLower(word[0]), word.Substring(1));
        }

        /// <summary>
        ///   将字母转为大写
        /// </summary>
        /// <param name="c"> 字母 </param>
        /// <returns> 转换后的字母 </returns>
        private static char ToUpper(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (char)(c - ('a' - 'A'));
            }
            return c;
        }

        /// <summary>
        ///   将字母转为小写
        /// </summary>
        /// <param name="c"> 字母 </param>
        /// <returns> 转换后的字母 </returns>
        private static char ToLower(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + ('a' - 'A'));
            }
            return c;
        }

        /// <summary>
        ///   列表转为以一个字符分隔的文本
        /// </summary>
        /// <param name="ls"> 列表 </param>
        /// <param name="dot"> 分隔符 </param>
        /// <returns> 文本 </returns>
        public static string ListToString(IEnumerable ls, char dot = ',')
        {
            StringBuilder sb = new StringBuilder();
            if (ls != null)
            {
                bool first = true;
                foreach (object o in from object o in ls where o != null select o)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(dot);
                    }
                    sb.Append(o);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///   列表转为以一个字符分隔的文本
        /// </summary>
        /// <param name="ls"> 列表 </param>
        /// <returns> 文本 </returns>
        public static string ToString(Dictionary<string, string> ls)
        {
            StringBuilder sb = new StringBuilder();
            if (ls != null)
            {
                bool first = true;
                foreach (var kv in ls)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(";");
                    }
                    sb.Append(string.Format("{0},{1}", kv.Key, kv.Value));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///   列表转为以一个字符分隔的文本
        /// </summary>
        /// <param name="value"> 列表 </param>
        /// <returns> 文本 </returns>
        public static Dictionary<string, string> FromString(string value)
        {
            Dictionary<string, string> ls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (value != null)
            {
                foreach (var s in value.Split(';').Select(kv => kv.Split(',')).Where(s => s.Length > 1))
                {
                    ls.Add(s[0], s[1]);
                }
            }
            return ls;
        }
    }

    /// <summary>
    ///   文本帮助类(String类扩展)
    /// </summary>
    public static class StringHelper2
    {

        /// <summary>
        /// 替换多个内容对
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rs">成对的替换,请保证是成对的</param>
        /// <returns></returns>
        public static string MulitReplace(this string str, params string[] rs)
        {
            if (str == null || rs == null || rs.Length == 0)
                return str;
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < rs.Length; i += 2)
                sb.Replace(rs[i], rs[i + 1]);
            return sb.ToString();
        }

        /// <summary>
        /// 替换多个内容为一个
        /// </summary>
        /// <param name="str"></param>
        /// <param name="last">替换到</param>
        /// <param name="org">被替换</param>
        /// <returns></returns>
        public static string MulitReplace2(this string str, string last, params string[] org)
        {
            if (str == null || org == null || org.Length == 0)
                return str;
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < org.Length; i += 1)
                sb.Replace(org[i], last);
            return sb.ToString();
        }
        /// <summary>
        /// 替换多个内容对
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rs">成对的替换,请保证是成对的</param>
        /// <returns></returns>
        public static string MulitReplace(this string str, params char[] rs)
        {
            if (str == null || rs == null || rs.Length == 0)
                return str;
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < rs.Length; i += 2)
                sb.Replace(rs[i], rs[i + 1]);
            return sb.ToString();
        }

        /// <summary>
        /// 替换多个内容为一个
        /// </summary>
        /// <param name="str"></param>
        /// <param name="last">替换到</param>
        /// <param name="org">被替换</param>
        /// <returns></returns>
        public static string MulitReplace2(this string str, char last, params char[] org)
        {
            if (str == null || org == null || org.Length == 0)
                return str;
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < org.Length; i += 1)
                sb.Replace(org[i], last);
            return sb.ToString();
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="str"></param>
        /// <param name="newLine"></param>
        public static void AppendSpaceText(this StringBuilder code, int space, string str, bool newLine = false)
        {
            if (newLine)
                code.AppendLine();
            code.Append(' ', space);
            code.Append(str);
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="newLine"></param>
        public static void AppendSpace(this StringBuilder code, int space, bool newLine = false)
        {
            if (newLine)
                code.AppendLine();
            code.Append(' ', space);
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceTextFormat(this StringBuilder code, int space, string fmt, params object[] args)
        {
            code.Append(' ', space);
            code.AppendFormat(fmt, args);
        }

        /// <summary>
        /// 得到相应长度的空文本
        /// </summary>
        /// <param name="len"></param>
        public static string SpaceString(this int len)
        {
            StringBuilder code = new StringBuilder();
            code.Append(' ', len);
            return code.ToString();
        }

        /// <summary>
        /// 得到布尔用于XAML的文字
        /// </summary>
        /// <param name="bl"></param>
        public static string XamlString(this bool bl)
        {
            return bl ? "True" : "False";
        }

        /// <summary>
        /// 加入XML属性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        public static void AppendAttrib(this StringBuilder code, string name, object value)
        {
            if (value != null)
                code.AppendFormat(@" {0}=""{1}""", name, value);
        }

        /// <summary>
        /// 加入XML属性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        public static void AppendAttrib(this StringBuilder code, string name, bool value)
        {
            code.AppendFormat(@" {0}=""{1}""", name, value ? "True" : "False");
        }

        /// <summary>
        /// 加入XML属性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">属性</param>
        /// <param name="head"></param>
        /// <param name="value">值</param>
        public static void AppendAttrib(this StringBuilder code, string name, string head, string value)
        {
            code.AppendFormat(@" {0}=""{1}{2}""", name, head, value);
        }

        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="newLine"></param>
        /// <param name="space"></param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceTextFormat(this StringBuilder code, bool newLine, int space, string fmt, params object[] args)
        {
            if (newLine)
                code.AppendLine();
            code.Append(' ', space);
            code.AppendFormat(fmt, args);
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="str"></param>
        /// <param name="newLine"></param>
        public static void AppendSpaceLineText(this StringBuilder code, int space, string str, bool newLine = false)
        {
            if (newLine)
                code.AppendLine();
            code.Append(SpaceLine(str, space));
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nameSpace"></param>
        public static void AppendUsing(this StringBuilder code, string nameSpace)
        {
            if (nameSpace == null)
                return;
            code.AppendFormat(@"using {0};", nameSpace);
            code.AppendLine();
        }
        /// <summary>
        /// 写入using语句
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nameSpaces"></param>
        public static void AppendUsing(this StringBuilder code, IEnumerable<string> nameSpaces)
        {
            if (nameSpaces == null)
                return;
            foreach (string nameSpace in nameSpaces.Where(p => p != null && p.IndexOf("System") == 0).Distinct().OrderBy(p => p))
            {
                code.AppendFormat(@"using {0};", nameSpace);
                code.AppendLine();
            }
            foreach (string nameSpace in nameSpaces.Where(p => p != null && p.IndexOf("System") != 0).Distinct().OrderBy(p => p))
            {
                code.AppendFormat(@"using {0};", nameSpace);
                code.AppendLine();
            }
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceLineFormat(this StringBuilder code, int space, string fmt, params object[] args)
        {
            code.Append(SpaceLine(string.Format(fmt, args), space));
        }

        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="newLine"> </param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceLineFormat(this StringBuilder code, int space, bool newLine, string fmt, params object[] args)
        {
            if (newLine)
                code.AppendLine();
            code.Append(SpaceLine(string.Format(fmt, args), space));
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="str"></param>
        /// <param name="newLine"></param>
        public static void AppendSpaceLineText2(this StringBuilder code, int space, string str, bool newLine = false)
        {
            if (newLine)
                code.AppendLine();
            code.Append(SpaceLine2(str, space));
        }
        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceLineFormat2(this StringBuilder code, int space, string fmt, params object[] args)
        {
            code.Append(SpaceLine2(string.Format(fmt, args), space));
        }

        /// <summary>
        /// 把空格放有每行前的格式化方式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="space"></param>
        /// <param name="newLine"> </param>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public static void AppendSpaceLineFormat2(this StringBuilder code, int space, bool newLine, string fmt, params object[] args)
        {
            if (newLine)
                code.AppendLine();
            code.Append(SpaceLine2(string.Format(fmt, args), space));
        }
        /// <summary>
        /// 使每行都缩进相同个数的空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static string SpaceLine2(this string str, int space)
        {
            if (str == null)
                return null;
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (string s in str.Replace("\r", "").Split('\n'))
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }
                if (isFirst)
                    isFirst = false;
                else
                    sb.AppendLine();
                sb.Append(' ', space);
                sb.Append(s.TrimEnd());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 使每行都缩进相同个数的空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="space"></param>
        /// <param name="head"> </param>
        /// <returns></returns>
        public static string SpaceLine(this string str, int space, string head = null)
        {
            if (str == null)
                return null;
            StringBuilder sb = new StringBuilder();
            bool empty = false;
            bool isFirst = true;
            foreach (string s in str.Trim().Replace("\r", "").Split('\n'))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        sb.AppendLine();
                    empty = false;
                    sb.Append(' ', space);
                    sb.Append(head);
                    sb.Append(s.Trim());
                }
                else
                {
                    if (empty)
                    {
                        continue;
                    }
                    empty = true;
                    if (isFirst)
                        isFirst = false;
                    else
                        sb.AppendLine();
                    sb.Append(head);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        ///   为空或是缺省文本
        /// </summary>
        /// <param name="word"> </param>
        /// <param name="def"> </param>
        /// <returns> </returns>
        public static bool IsNullOrDefault(this string word, string def = "无")
        {
            return string.IsNullOrWhiteSpace(word) || word.Equals(def, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   检测一个文字是否一个单词
        /// </summary>
        /// <param name="text"> </param>
        /// <returns> </returns>
        public static bool IsName(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            foreach (char c in text)
            {
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c > 255))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///   到首字母大写的文本
        /// </summary>
        /// <param name="word"> </param>
        /// <returns> </returns>
        public static string ToUWord(this string word)
        {
            if (String.IsNullOrWhiteSpace(word))
            {
                return String.Empty;
            }
            return word.Length == 1
                           ? word.ToUpper()
                           : String.Format("{0}{1}", ToUpper(word[0]), word.Substring(1));
        }

        /// <summary>
        ///   到首字母小写的文本
        /// </summary>
        /// <param name="word"> </param>
        /// <returns> </returns>
        public static string ToLWord(this string word)
        {
            if (String.IsNullOrWhiteSpace(word))
            {
                return String.Empty;
            }
            return word.Length == 1
                           ? word.ToUpper()
                           : String.Format("{0}{1}", ToLower(word[0]), word.Substring(1));
        }

        /// <summary>
        ///   将字母转为大写
        /// </summary>
        /// <param name="c"> 字母 </param>
        /// <returns> 转换后的字母 </returns>
        private static char ToUpper(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (char)(c - ('a' - 'A'));
            }
            return c;
        }

        /// <summary>
        ///   将字母转为小写
        /// </summary>
        /// <param name="c"> 字母 </param>
        /// <returns> 转换后的字母 </returns>
        private static char ToLower(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + ('a' - 'A'));
            }
            return c;
        }

        /// <summary>
        ///   到首字母大写的文本
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"></param>
        /// <returns> </returns>
        public static bool IsEquals(this string a, string b)
        {
            var aemp = string.IsNullOrWhiteSpace(a);
            var bemp = string.IsNullOrWhiteSpace(b);
            if (aemp && bemp)
                return false;
            if (!aemp)
                a = a.Trim();
            if (!bemp)
                b = b.Trim();
            return a == b;
        }
        /// <summary>
        ///   取得文本的长度(全角算两个)
        /// </summary>
        /// <param name="a"> </param>
        /// <returns> </returns>
        public static int GetLen(this string a)
        {
            if (string.IsNullOrWhiteSpace(a))
                return 0;
            return a.Length + a.Count(p => p > 255);
        }

        /// <summary>
        /// 拼接文本
        /// </summary>
        /// <param name="head">起头文本</param>
        /// <param name="splice">连接的中间文本</param>
        /// <param name="args">被连接的内容(如果为空,不拼接)</param>
        /// <returns></returns>
        public static string Append(this string head, string splice, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(head))
            {
                sb.Append(head);
                sb.Append(splice);
            }
            foreach (var arg in args.Where(p => p != null))
            {
                if (arg is string)
                {
                    sb.Append(arg);
                    sb.Append(splice);
                }
                else if (arg is IEnumerable)
                {
                    sb.Append(arg);
                    sb.Append(splice);
                }
            }

            return sb.ToString();
        }
        /// <summary>
        /// 拼接文本
        /// </summary>
        /// <param name="head">起头文本</param>
        /// <param name="splice">连接的中间文本</param>
        /// <param name="arg">被连接的内容(如果为空,不拼接)</param>
        /// <returns></returns>
        public static string Append(this string head, string splice, object arg)
        {
            if (arg == null)
            {
                return head;
            }
            return head == null
                ? arg.ToString()
                : string.Join(head, splice, arg);
        }

        /// <summary>
        /// 拼接文本
        /// </summary>
        /// <param name="head">起头文本</param>
        /// <param name="splice">连接的中间文本</param>
        /// <param name="fmt"></param>
        /// <param name="arg">被连接的内容(如果为空,不拼接)</param>
        /// <returns></returns>
        public static string AppendFormat(this string head, string splice, string fmt, params object[] arg)
        {
            return head == null
                ? string.Format(fmt, arg)
                : string.Join(head, splice, string.Format(fmt, arg));
        }
    }
}
