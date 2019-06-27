// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

#endregion

namespace System.Linq
{
    /// <summary>
    ///   集合的扩展
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        ///   安全加入(如已存在则改名加入)
        /// </summary>
        /// <param name="dictionary"> 集合本身 </param>
        /// <param name="k"> 格式化代码 </param>
        /// <param name="v"> 值 </param>
        /// <returns> </returns>
        public static string SafeAdd<TV>(this IDictionary<string, TV> dictionary, string k, TV v)
        {
            if (k == null)
                return null;
            var name = k;
            var idx = 1;
            while (dictionary.ContainsKey(name))
            {
                name = $"{k}{idx++}";
            }
            dictionary.Add(name, v);
            return name;
        }
        /// <summary>
        ///   加入格式化文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmt"> 格式化代码 </param>
        /// <param name="args"> 格式化参数 </param>
        /// <returns> </returns>
        public static string AddFormat(this IList<string> em, string fmt, params object[] args)
        {
            if (fmt == null)
                return null;
            var str = args == null ? fmt : string.Format(fmt, args);
            if (em == null)
            {
                return str;
            }
            em.Add(str);
            return str;
        }

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="parse"> </param>
        /// <returns> </returns>
        public static List<T> ParseToList<T>(this IEnumerable<string> source, Func<string, T> parse)
        {
            if (source == null)
            {
                return new List<T>();
            }

            var entityList = source as List<T>;
            if (entityList != null)
            {
                return entityList;
            }

            var list = new List<T>();
            foreach (var str in source)
            {
                list.Add(parse(str));
            }
            return list;
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <returns> </returns>
        public static string LinkToString(this IEnumerable em)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            foreach (var v in em)
            {
                if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                {
                    continue;
                }
                sb.Append(v);
            }
            return sb.ToString();
        }
        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="sp"> </param>
        /// <returns> </returns>
        public static string LinkToString(this IEnumerable em, char sp = ',')
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 文字长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StrLenght(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return -1;
            }
            var len = 0;
            foreach (var c in str)
            {
                len++;
                if (c > 128)
                {
                    len++;
                }
            }
            return len;
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="lt"> </param>
        /// <param name="rt"> </param>
        /// <returns> </returns>
        public static string LinkToSql(this IEnumerable<string> em, char lt = '[', char rt = ']')
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true; var maxLen = 0;
            var ll = new List<KeyValuePair<int, string>>();
            foreach (var str in em)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }
                var len = str.StrLenght();
                ll.Add(new KeyValuePair<int, string>(len, str));
                if (len > maxLen)
                {
                    maxLen = len;
                }
            }
            foreach (var kv in ll)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.Append($"{lt}{kv.Value}{rt}");
            }
            return sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="sp"> </param>
        /// <param name="maxCol"> </param>
        /// <param name="empty"> </param>
        /// <returns> </returns>
        public static string LinkToSql(this IEnumerable<string> em, string empty = " ", char sp = ',', int maxCol = 5)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            if (maxCol > 0)
            {
                var col = 0;
                var maxLen = 0;
                var ll = new List<KeyValuePair<int, string>>();
                foreach (var str in em)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        continue;
                    }
                    var len = str.StrLenght();
                    ll.Add(new KeyValuePair<int, string>(len, str));
                    if (len > maxLen)
                    {
                        maxLen = len;
                    }
                }
                var preLen = 0;
                foreach (var kv in ll)
                {
                    var str = kv.Value;
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(sp);
                        if (maxCol > 0 && ++col == maxCol)
                        {
                            col = 0;
                            sb.AppendLine();
                            sb.Append(empty);
                        }
                        else
                        {
                            var e = maxLen - preLen;
                            if (e > 0)
                            {
                                sb.Append(' ', e);
                            }
                        }
                    }
                    sb.Append(str);
                    preLen = kv.Key;
                }
            }
            else
            {
                foreach (var v in em)
                {
                    if (v == null || string.IsNullOrWhiteSpace(v.ToString(CultureInfo.InvariantCulture)))
                    {
                        continue;
                    }
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(sp);
                    }
                    sb.Append(v);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="sp"> </param>
        /// <returns> </returns>
        public static string LinkToString<T>(List<T> em, char sp = ',')
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   转换到文本列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="wh"> 条件 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static List<string> SelectFormat<T>(this IEnumerable<T> em, Func<T, bool> wh, string fm)
        {
            return em.Where(wh).Select(p => string.Format(fm, p)).ToList();
        }

        /// <summary>
        ///   转换到文本列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static List<string> SelectFormat<T>(this IEnumerable<T> em, string fm)
        {
            return em.Select(p => string.Format(fm, p)).ToList();
        }

        /// <summary>
        ///   转换到文本列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="valueFunc"> 取值的方法 </param>
        /// <param name="wh"> 条件 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static List<string> SelectFormat<T>(this IEnumerable<T> em, Func<T, string> valueFunc, Func<T, bool> wh, string fm)
        {
            return em.Where(wh).Select(p => string.Format(fm, valueFunc(p))).ToList();
        }

        /// <summary>
        ///   转换到文本列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="valueFunc"> 取值的方法 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static List<string> SelectFormat<T>(this IEnumerable<T> em, Func<T, string> valueFunc, string fm)
        {
            return em.Select(p => string.Format(fm, valueFunc(p))).ToList();
        }

        /// <summary>
        ///   加入格式化文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="valueFunc"> 取值的方法 </param>
        /// <param name="friend"> 要加入的集合 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        public static void AddRangeFormat<T>(this List<string> em, IEnumerable<T> friend, Func<T, string> valueFunc, string fm)
        {
            em.AddRange(friend.Select(p => string.Format(fm, p)));
        }

        /// <summary>
        ///   加入一批格式化文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="friend"> 要加入的集合 </param>
        /// <param name="wh"> 条件 </param>
        /// <param name="valueFunc"> 取值的方法 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        public static void AddRangeFormat<T>(this List<string> em, IEnumerable<T> friend, Func<T, string> valueFunc, Func<T, bool> wh, string fm)
        {
            em.AddRange(friend.Where(wh).Select(p => string.Format(fm, p)));
        }

        /// <summary>
        ///   加入格式化文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="friend"> 要加入的集合 </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static void AddRangeFormat<T>(this List<string> em, IEnumerable<T> friend, string fm)
        {
            em.AddRange(friend.Select(p => string.Format(fm, p)));
        }

        /// <summary>
        ///   加入格式化文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="friend"> 要加入的集合 </param>
        /// <param name="wh"> </param>
        /// <param name="fm"> 格式化代码(有且只能有{0}) </param>
        /// <returns> </returns>
        public static void AddRangeFormat<T>(this List<string> em, IEnumerable<T> friend, Func<T, bool> wh, string fm)
        {
            em.AddRange(friend.Where(wh).Select(p => string.Format(fm, p)));
        }

        /// <summary>
        ///   连接到文本(null和空白不加入且会消除空白)
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static void AddByNotNull(this List<string> em, string value)
        {
            if (em == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            em.Add(value.Trim());
        }

        /// <summary>
        ///   得到唯一文件列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="empty">空文本 </param>
        /// <returns> </returns>
        public static List<string> DistinctBy(this IEnumerable<string> em, string empty = null)
        {
            return em?.Where(p => p != empty).Distinct().ToList();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="sp"> </param>
        /// <param name="spToHead"></param>
        /// <returns> </returns>
        public static string LinkToString(this IEnumerable em, string sp, bool spToHead = false)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (spToHead)
                sb.Append(sp);
            var first = true;
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (v is string && string.IsNullOrWhiteSpace(v as string))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            return first ? null : sb.ToString();
        }

        /// <summary>
        ///   连接到文本(无值返回空)
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string Link(this List<string> em, string head, string sp, string food = null)
        {
            if (em == null || em.Count == 0)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(head))
            {
                sb.Append(head);
                sb.Append(' ');
            }
            var first = true;
            foreach (var v in em)
            {
                if (v == null || string.IsNullOrWhiteSpace(v))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            if (first)
                return null;
            if (!string.IsNullOrWhiteSpace(food))
            {
                sb.Append(' ');
                sb.Append(food);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="toString"> </param>
        /// <param name="sp"> </param>
        /// <returns> </returns>
        public static string LinkToString<T>(this IEnumerable<T> em, Func<T, string> toString, string sp = ",") where T : struct
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                var value = toString(v);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(value);
            }
            return first ? null : sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="toString"> </param>
        /// <param name="sp"> </param>
        /// <returns> </returns>
        public static string LinkToString2<T>(this IEnumerable<T> em, Func<T, string> toString, string sp = ",") where T : class
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                var value = toString(v);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(value);
            }
            return first ? null : sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="toString"> </param>
        /// <param name="sp"> </param>
        /// <returns> </returns>
        public static string LinkToString(this IEnumerable<string> em, Func<string, string> toString, string sp = ",")
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (string.IsNullOrWhiteSpace(v))
                {
                    continue;
                }
                var value = toString(v);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(value);
            }
            return first ? null : sb.ToString();
        }


        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmString"> 格式化的文本 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string ListToString(this IEnumerable em, string fmString, string sp = ",", string head = null, string food = null)
        {
            var sb = new StringBuilder();
            if (em != null)
            {
                if (head != null)
                {
                    sb.Append(head);
                }
                var first = true;
                foreach (var o in from object o in em where o != null select o)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(sp);
                    }
                    sb.Append(o);
                }
                if (first)
                    return null;
                if (food != null)
                {
                    sb.Append(food);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string LinkToString(this IEnumerable em, string head, string sp, string food = null)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            return first ? null : $"{head}{sb}{food}".Trim();
        }
        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <returns> </returns>
        public static string ListToString(this IEnumerable em, string sp = ",")
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (v is string && string.IsNullOrWhiteSpace(v as string))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(v);
            }
            return first ? null : sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmt"> 格式化方法</param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string ListToString(this IEnumerable em, Func<object, string> fmt, string sp = ",", string head = null, string food = null)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (head != null)
            {
                sb.Append(head);
            }
            var first = true;
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (v is string && string.IsNullOrWhiteSpace(v as string))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(fmt(v));
            }
            if (first)
                return null;
            if (food != null)
            {
                sb.Append(food);
            }
            return sb.ToString();
        }


        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmt"> 格式方法 </param>
        /// <param name="empty"> 为空时显示 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <returns> </returns>
        public static string LinkByFormat2<T>(this IEnumerable<T> em, Func<T, string> fmt, string sp = ",", string empty = null) where T : class
        {
            if (em == null)
            {
                return empty;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(fmt(v));
            }
            return first ? empty : sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmt"> 格式方法 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string LinkByFormat<T>(this IEnumerable<T> em, string fmt, string sp = ",", string head = null, string food = null) where T : class
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            if (head != null)
            {
                first = false;
                sb.Append(head);
                sb.Append(sp);
            }
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.AppendFormat(fmt, v);
            }
            if (first)
                return null;
            if (food != null)
            {
                sb.Append(sp);
                sb.Append(food);
            }
            return sb.ToString();
        }
        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmt"> 格式方法 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string LinkByFormat<T>(this IEnumerable<T> em, Func<T, string> fmt, string sp = ",", string head = null, string food = null) where T : class
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            if (head != null)
            {
                first = false;
                sb.Append(head);
                sb.Append(sp);
            }
            foreach (var v in em)
            {
                if (v == null)
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.Append(fmt(v));
            }
            if (first)
                return null;
            if (food != null)
            {
                sb.Append(sp);
                sb.Append(food);
            }
            return sb.ToString();
        }

        /// <summary>
        ///   连接到SQL文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <returns> </returns>
        public static string LinkToSql(this Dictionary<string, string> em)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            foreach (var value in em)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.AppendFormat("\r\n\t[{0}] = {1}", value.Key, value.Value);
            }
            return first ? null : sb.ToString();
        }

        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string LinkToString(this Dictionary<string, string> em, string sp = ",", string head = null, string food = null)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            var first = true;
            if (head != null)
            {
                first = false;
                sb.Append(head);
                sb.Append(sp);
            }
            foreach (var value in em)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.AppendFormat(@"{{""{0}"",""{1}""}}", value.Key, value.Value);
            }
            if (first)
                return null;
            if (food != null)
            {
                sb.Append(sp);
                sb.Append(food);
            }
            return sb.ToString();
        }
        /// <summary>
        ///   连接到文本
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="fmString"> 格式化的文本 </param>
        /// <param name="head"> 前缀 </param>
        /// <param name="sp"> 中间的连接字串 </param>
        /// <param name="food"> 后缀 </param>
        /// <returns> </returns>
        public static string LinkByFormat2(this IEnumerable em, string fmString, string sp = ",", string head = null, string food = null)
        {
            if (em == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            if (head != null)
            {
                sb.Append(head);
            }
            var first = true;
            foreach (var v in em)
            {
                if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                {
                    continue;
                }
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(sp);
                }
                sb.AppendFormat(fmString, v);
            }
            if (first)
                return null;
            if (food != null)
            {
                sb.Append(food);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 向ObservableCollection加入一批数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="range"></param>
        public static void AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> range)
        {
            if (range == null)
            {
                return;
            }
            foreach (var t in range)
            {
                list.Add(t);
            }
        }
        /// <summary>
        /// 不重复加入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="range"></param>
        public static void AddOnce<T>(this ICollection<T> list, IEnumerable<T> range)
        {
            if (range == null)
            {
                return;
            }
            foreach (var t in range)
            {
                if (!list.Contains(t))
                    list.Add(t);
            }
        }

        /// <summary>
        /// 不重复加入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="range"></param>
        public static void AddOnce<T>(this ICollection<T> list, params T[] range)
        {
            if (range == null)
            {
                return;
            }
            foreach (var t in range)
            {
                if (!list.Contains(t))
                    list.Add(t);
            }
        }

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static List<T> ToList2<T>(this IEnumerable source)
        {
            if (source == null)
            {
                return new List<T>();
            }

            var entityList = source as List<T>;
            if (entityList != null)
            {
                return entityList;
            }

            var list = new List<T>();
            foreach (T t in source)
            {
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable source)
        {
            if (source == null)
            {
                return new ObservableCollection<T>();
            }

            var entityList = source as ObservableCollection<T>;
            if (entityList != null)
            {
                return entityList;
            }

            var list = new ObservableCollection<T>();
            foreach (T t in source)
            {
                list.Add(t);
            }
            return list;
        }

#if CLIENT

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static ObservableCollection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return new ObservableCollection<T>();
            }

            ObservableCollection<T> entityList = source as ObservableCollection<T>;
            if (entityList != null)
            {
                return entityList;
            }

            ObservableCollection<T> list = new ObservableCollection<T>();
            foreach (T t in source)
            {
                list.Add(t);
            }
            return list;
        }
#else

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="convert"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select2<T>(this IEnumerable source, Func<object, T> convert)
        {
            if (source == null)
            {
                return new List<T>();
            }

            var entityList = source as List<T>;
            if (entityList != null)
            {
                return entityList;
            }

            var list = new List<T>();
            foreach (var t in source)
            {
                list.Add(convert(t));
            }
            return list;
        }
#endif

        /// <summary>
        ///   从 System.Collections.Generic.IEnumerable到List(并做到不为空和使用原来的对象)
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="convert"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> Select3<T, S>(this IEnumerable source, Func<S, T> convert)
        {
            if (source == null)
            {
                return new List<T>();
            }

            var entityList = source as List<T>;
            if (entityList != null)
            {
                return entityList;
            }

            var list = new List<T>();
            foreach (S t in source)
            {
                list.Add(convert(t));
            }
            return list;
        }

        /// <summary>
        ///   遍历
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="action"> 化方法</param>
        /// <param name="keepNull"> 跳过空值 </param>
        /// <returns> </returns>
        public static void ForEach(this IEnumerable em, Action<object> action, bool keepNull = true)
        {
            if (em == null)
                return;
            if (keepNull)
            {
                foreach (var t in em)
                {
                    if (!Equals(t, null))
                        action(t);
                }
            }
            else
            {
                foreach (var t in em)
                {
                    action(t);
                }
            }
        }

        /// <summary>
        ///   遍历
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="action"> 化方法</param>
        /// <param name="keepNull"> 跳过空值 </param>
        /// <returns> </returns>
        public static void Foreach<T>(this IEnumerable<T> em, Action<T> action, bool keepNull = true)
        {
            if (em == null)
                return;
            if (keepNull)
            {
                foreach (var t in em.Where(p => !Equals(p, default(T))))
                {
                    action(t);
                }
            }
            else
            {
                foreach (var t in em)
                {
                    action(t);
                }
            }
        }

        /// <summary>
        ///   到列表
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="action"> 化方法</param>
        /// <param name="keepNull"> 跳过空值 </param>
        /// <returns> </returns>
        public static List<TTarget> ToList<TTarget>(this IEnumerable em, Func<object, TTarget> action, bool keepNull = true)
        {
            var results = new List<TTarget>();
            if (em == null)
            {
                return results;
            }
            if (keepNull)
            {
                foreach (var t in em)
                {
                    if (!Equals(t, null))
                        results.Add(action(t));
                }
            }
            else
            {
                foreach (var t in em)
                {
                    results.Add(action(t));
                }
            }
            return results;
        }

        /// <summary>
        ///   遍历
        /// </summary>
        /// <param name="em"> 集合本身 </param>
        /// <param name="action"> 化方法</param>
        /// <param name="keepNull"> 跳过空值 </param>
        /// <returns> </returns>
        public static List<TTarget> Foreach<TTarget, TSource>(this IEnumerable<TSource> em, Func<object, TTarget> action, bool keepNull = true)
        {
            var results = new List<TTarget>();
            if (em == null)
            {
                return results;
            }
            if (keepNull)
            {
                foreach (var t in em.Where(p => !Equals(p, default(TSource))))
                {
                    results.Add(action(t));
                }
            }
            else
            {
                foreach (var t in em)
                {
                    results.Add(action(t));
                }
            }
            return results;
        }
        /// <summary>
        ///     如果数组中没有相同的,就增加它
        /// </summary>
        /// <param name="em"></param>
        /// <param name="value">内容</param>
        public static void AddOnce(this IList<string> em, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !em.Contains(value))
            {
                em.Add(value);
            }
        }

        /// <summary>
        ///     如果字典中没有就增加它
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">内容</param>
        public static void AddOnce<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        ///     如果字典中没有就增加它,有就替换它
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">内容</param>
        public static void AddOrSwitch<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        ///     如果字典中没有就增加它
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">内容</param>
        public static void OnlyAdd<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue value)
        {
            if (dictionary == null)
            {
                return;
            }
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }


        /// <summary>
        ///     合并(后一个如果有和前一个同键的,会覆盖它)
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static Dictionary<string, TValue> Merge<TValue>(params Dictionary<string, TValue>[] paras)
        {
            var re = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
            foreach (var dictionary in paras)
            {
                foreach (var k in dictionary)
                {
                    if (!re.ContainsKey(k.Key))
                    {
                        re.Add(k.Key, k.Value);
                    }
                    else
                    {
                        re[k.Key] = k.Value;
                    }
                }
            }
            return re;
        }


        /// <summary>
        /// 拼接文本
        /// </summary>
        /// <param name="head">起头文本</param>
        /// <param name="splice">连接的中间文本</param>
        /// <param name="args">被连接的内容(如果为空,不拼接)</param>
        /// <returns></returns>
        public static string Splice(this string head, string splice, params object[] args)
        {
            var sb = new StringBuilder();
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
                    sb.Append((arg as IEnumerable).LinkToString(splice));
                }
                else
                {
                    sb.Append(arg);
                    sb.Append(splice);
                }
            }

            return sb.ToString();
        }
    }
}
