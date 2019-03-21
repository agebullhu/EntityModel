// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-29 21:44
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Agebull.Common.Help
{
    /// <summary>
    /// CSV文件转换对象
    /// </summary>
    public class CSVConvert
    {
        #region 导入

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public static List<T> Import<T>(string value, Action<T, string, string> setValue) where T : class,new()
        {
            var lines = Split(value);
            if (lines.Count <= 1)
                return null;
            var results = new List<T>();
            var line = new List<string>();
            foreach (var field in lines[0])
            {
                line.Add(field.Trim().MulitReplace2("", " ", "　", "\t"));
            }
            for (var i = 1; i < lines.Count; i++)
            {
                var tv = new T();
                var vl = lines[i];
                for (var cl = 0; cl < vl.Count; cl++)
                {
                    setValue(tv, line[cl], vl[cl]);
                }
                results.Add(tv);
            }
            return results;
        }

        /// <summary>
        /// CSV内容分解
        /// </summary>
        /// <param name="values">CSV内容</param>
        /// <returns>分解后的分行列的文本</returns>
        private static List<List<string>> Split(string values)
        {
            var result = new List<List<string>>();
            List<string> line = null;
            var sb = new StringBuilder();
            var inQuotation = false; //在引号中
            var preQuotation = false; //前一个也是引号
            foreach (var c in values)
            {
                switch (c)
                {
                    case ',':
                        if (!inQuotation || preQuotation)
                        {
                            preQuotation = false;
                            inQuotation = false;
                            if (line == null)
                            {
                                line = new List<string>();
                                result.Add(line);
                            }
                            line.Add(sb.ToString());
                            sb.Clear();
                        }
                        else
                        {
                            sb.Append('，');
                        }
                        continue;
                    case '\"':
                        if (inQuotation)
                        {
                            if (preQuotation)
                            {
                                sb.Append('\"');
                                preQuotation = false; //连续引号当成正常的引号
                                continue;
                            }
                            //否则得看下一个，如果还是引号则认为正常引号，是引号当成引号来使用，其它情况不符合CVS的文件标准
                            preQuotation = true;
                            continue;
                        }
                        inQuotation = true;
                        continue;
                    case '\r':
                    case '\n':
                        if (!inQuotation || preQuotation)
                        {
                            if (line != null)
                            {
                                line.Add(sb.ToString());
                            }
                            sb.Clear();
                            line = null;
                            inQuotation = false;
                            preQuotation = false;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        continue;
                    default:
                        if (preQuotation)
                        {
                            sb.Append('\"');
                            preQuotation = false;
                            continue;
                        }
                        break;
                }
                sb.Append(c);
            }
            return result;
        }

        /// <summary>
        /// 导入文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ImportFile(string file)
        {
            string s;
            try
            {
                s = IOHelper.ReadString(file);
            }
            catch
            {
                throw new AgebullSystemException("文件无法打开");
            }

            var lines = Split(s);
            var datas = new List<Dictionary<string, string>>();
            if (lines.Count <= 1)
            {
                return datas;
            }
            var head = new List<string>();
            foreach (var field in lines[0])
            {
                head.Add(field.Trim().MulitReplace2("", " ", "　", "\t"));
            }
            for (var i = 1; i < lines.Count; i++)
            {
                var row = new Dictionary<string, string>();
                var ds = lines[i];
                foreach (var t in head)
                {
                    var v = ds[i];
                    row.Add(t, String.IsNullOrEmpty(v)
                                    ? null
                                    : v);
                }
                datas.Add(row);
            }
            return datas;
        }

        #endregion

        #region 导出

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="heads"></param>
        /// <param name="datas"></param>
        /// <param name="rFunc"></param>
        /// <returns></returns>
        public static string Export<T>(IEnumerable<string> heads, IEnumerable<T> datas, Func<T, string> rFunc) where T : class
        {
            var sb = new StringBuilder();
            sb.AppendLine(heads.LinkByFormat2(p => String.Format("\"{0}\"", p.MulitReplace(',', '，', '\"', '＂'))));
            foreach (var value in datas)
            {
                sb.AppendLine(rFunc(value));
            }
            return sb.ToString();
        }
        #endregion
    }
}
