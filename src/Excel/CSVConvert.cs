using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hpc.Project.ImportSkuCsv
{
    /// <summary>
    ///     CSV文件转换对象
    /// </summary>
    public class CSVConvert
    {
        #region 导出

        /// <summary>
        ///     导出
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string Export<T>(IList<T> datas)
            where T : EditDataObject
        {
            if (datas.Count == 0)
                return null;
            var sb = new StringBuilder();
            bool first = true;
            var fields = datas[0].__Struct.Properties.Values.Where(p => p.CanExport).ToArray();
            foreach (var pro in fields)
            {
                if (first)
                    first = false;
                else
                    sb.Append(',');
                sb.Append($"\"{pro.Caption}\"");
            }
            sb.AppendLine();
            foreach (var data in datas)
            {
                first = true;
                foreach (var pro in fields)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append(',');
                    var value = data.GetValue(pro.Name)?.ToString();
                    if (value != null && pro.PropertyType == typeof(string))
                        value = value.Replace("\"", "\"\"");
                    sb.Append($"\"{value}\"");
                }
                sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }

        #endregion

        #region 导入

        /// <summary>
        ///     导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public static List<T> Import<T>(string value, Action<T, string, string> setValue) where T : class, new()
        {
            var lines = Split(value);
            if (lines.Count <= 1) return null;

            var results = new List<T>();
            var colunms = new List<string>();
            foreach (var field in lines[0])
                colunms.Add(field.Trim().MulitReplace2("", " ", "　", "\t"));
            for (var i = 1; i < lines.Count; i++)
            {
                var tv = new T();
                var vl = lines[i];
                for (var cl = 0; cl < vl.Count; cl++)
                    setValue(tv, colunms[cl], vl[cl]);
                results.Add(tv);
            }
            return results;
        }

        /// <summary>
        ///     导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csv">CSV内容</param>
        /// <param name="setValue">列读入自定义处理</param>
        /// <param name="rowEnd">行结束时的处理方法</param>
        /// <returns></returns>
        public static int Import<T>(string csv, Action<T, string, string> setValue, Action<T> rowEnd) where T : class, new()
        {
            int cnt = 0;
            var lines = Split(csv);
            if (lines.Count <= 1)
                return 0;

            var colunms = new List<string>();
            foreach (var field in lines[0])
                colunms.Add(field.Trim().MulitReplace2("", " ", "　", "\t"));
            for (var i = 1; i < lines.Count; i++)
            {
                var tv = new T();
                var vl = lines[i];
                for (var cl = 0; cl < vl.Count; cl++)
                    setValue(tv, colunms[cl], vl[cl]);
                rowEnd(tv);
                cnt++;
            }

            return cnt;
        }

        /// <summary>
        ///     CSV内容分解
        /// </summary>
        /// <param name="values">CSV内容</param>
        /// <returns>分解后的分行列的文本</returns>
        private static List<List<string>> Split(string values)
        {
            var result = new List<List<string>>();
            var line = new List<string>();
            var sb = new StringBuilder();
            var inQuotation = false; //在引号中
            var preQuotation = false; //前一个也是引号
            var preSeparator = true; //前面是做字段分隔符号吗
            bool isClose = false;
            foreach (var c in values)
            {
                if (c == '\"')
                {
                    if (inQuotation)
                    {
                        if (preQuotation) //连续引号当成正常的引号
                        {
                            sb.Append('\"');
                            preQuotation = false;
                        }
                        else //否则得看下一个，如果还是引号则认为正常引号，是引号当成引号来使用，其它情况不符合CVS的文件标准
                        {
                            preQuotation = true;
                        }
                    }
                    else if (preSeparator) //分隔符后的引号者才是字段内容起止
                    {
                        inQuotation = true;
                        preSeparator = false;
                    }
                    else
                    {
                        sb.Append(c);
                    }

                    continue;
                }

                if (preQuotation) //可中止
                {
                    preQuotation = false;
                    inQuotation = false;
                    isClose = true;
                    line.Add(sb.ToString());
                    sb.Clear();
                }
                else if (inQuotation) //所有都是普通内容
                {
                    sb.Append(c);
                    continue;
                }

                switch (c)
                {
                    case ',':
                        if (isClose)
                        {
                            isClose = false;
                        }
                        else
                        {
                            if (sb.Length == 0)
                            {
                                line.Add(null);
                            }
                            else
                            {
                                line.Add(sb.ToString());
                                sb.Clear();
                            }
                        }
                        preSeparator = true;
                        continue;
                    case '\r':
                    case '\n':
                        if (isClose)
                        {
                            isClose = false;
                        }
                        else
                        {
                            if (sb.Length == 0)
                            {
                                line.Add(null);
                            }
                            else
                            {
                                line.Add(sb.ToString());
                                sb.Clear();
                            }
                        }
                        if (line.Count > 0)
                        {
                            result.Add(line);
                            line = new List<string>();
                        }
                        preSeparator = true;
                        continue;
                    case ' ':
                    case '\t':
                        break;
                    default:
                        if (preSeparator)
                            preSeparator = false;
                        break;
                }

                sb.Append(c);
            }

            if (sb.Length != 0)
            {
                line.Add(sb.ToString());
                sb.Clear();
                result.Add(line);
            }
            else if (line.Count > 0)
            {
                line.Add(null);
                result.Add(line);
            }

            return result;
        }

        /// <summary>
        ///     导入文件
        /// </summary>
        /// <returns></returns>
        public static void ImportFile(string file, Action<int, Dictionary<string, string>> action)
        {
            string s;
            try
            {
                s = File.ReadAllText(file);
            }
            catch
            {
                throw new FileLoadException("文件无法打开");
            }

            var lines = Split(s);
            if (lines.Count <= 1) return;
            var head = new List<string>();
            foreach (var field in lines[0])
                head.Add(field.Trim().MulitReplace2("", " ", "　", "\t"));

            for (var i = 1; i < lines.Count; i++)
            {
                var words = lines[i];
                var row = new Dictionary<string, string>();
                for (var index = 0; index < head.Count; index++)
                {
                    var t = head[index];
                    var v = index >= words.Count ? null : words[index];
                    row.Add(t, string.IsNullOrEmpty(v)
                        ? null
                        : v);
                }

                action(i, row);
            }
        }


        /// <summary>
        ///     导入文件
        /// </summary>
        /// <returns></returns>
        public static int ImportFile(string file, Action<List<string>, bool, int> action)
        {
            string s;
            try
            {
                s = File.ReadAllText(file);
            }
            catch
            {
                throw new FileLoadException("文件无法打开");
            }

            return Split(s, action);
        }


        /// <summary>
        /// CSV内容分解
        /// </summary>
        /// <param name="values">CSV内容</param>
        /// <param name="action">处理方法，第一个参数为解析出的行数组，第一个参数指是否为第一行即标题行</param>
        /// <returns>解析到的行数</returns>
        private static int Split(string values, Action<List<string>, bool, int> action)
        {
            var cnt = 0;
            var first = true;
            var line = new List<string>();
            var sb = new StringBuilder();
            var inQuotation = false; //在引号中
            var preQuotation = false; //前一个也是引号
            var preSeparator = true;//前面是做字段分隔符号吗
            int empty = 0;
            foreach (var c in values)
            {
                if (empty > 100)
                    break;
                if (c == '\"')
                {
                    empty = 0;
                    if (inQuotation)
                    {
                        if (preQuotation)//连续引号当成正常的引号
                        {
                            sb.Append('\"');
                            preQuotation = false;
                        }
                        else//否则得看下一个，如果还是引号则认为正常引号，是引号当成引号来使用，其它情况不符合CVS的文件标准
                        {
                            preQuotation = true;
                        }
                        continue;
                    }

                    if (preSeparator)//分隔符后的引号者才是字段内容起止
                    {
                        inQuotation = true;
                        preSeparator = false;
                        sb.Clear();
                        continue;
                    }
                }
                else if (preQuotation)//字段可中止
                {
                    preQuotation = false;
                    inQuotation = false;
                }
                else if (inQuotation)//所有都是普通内容
                {
                    sb.Append(c);
                    continue;
                }
                switch (c)
                {
                    case ',':
                        if (sb.Length == 0)
                        {
                            line.Add(null);
                        }
                        else
                        {
                            line.Add(sb.ToString());
                            sb.Clear();
                        }
                        preSeparator = true;
                        continue;
                    case '\r':
                        continue;
                    case '\n':
                        if (sb.Length != 0)
                        {
                            line.Add(sb.ToString());
                            sb.Clear();
                        }
                        if (line.Count > 0 && line.Any(p => p != null))
                        {
                            if (preSeparator)
                                line.Add(null);
                            action(line, first, ++cnt);
                            line = new List<string>();
                        }
                        else
                        {
                            ++empty;
                            continue;
                        }
                        first = false;
                        preSeparator = true;
                        empty = 0;
                        continue;
                }
                empty = 0;
                if (preSeparator && !char.IsWhiteSpace(c))
                {
                    preSeparator = false;
                }
                sb.Append(c);
            }
            if (sb.Length != 0)
            {
                line.Add(sb.ToString());
                sb.Clear();
            }
            if (line.Count > 0 && line.Any(p => p != null))
            {
                line.Add(null);
                action(line, first, ++cnt);
            }
            return cnt;
        }
        #endregion
    }
}