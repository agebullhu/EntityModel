// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-12
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Linq;
using Gboxt.Common.SystemModel.DataAccess;

#endregion

namespace YHXBank.BussinessSystem.Common
{
    /// <summary>
    ///     数据字典的读写对象
    /// </summary>
    public class DataDictionary
    {
        #region 读值

        /// <summary>
        ///     取值对象文本值
        /// </summary>
        /// <param name="name">字典名称</param>
        /// <returns>文本值</returns>
        public static string GetValue(string name)
        {
            var lg = new DataDictionaryDataAccess();
            return lg.Get(name);
        }

        /// <summary>
        ///     取值对象
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="name">字典名称</param>
        /// <param name="parse">文本转类型方法</param>
        /// <returns>对象类型</returns>
        public static T GetValue<T>(string name, Func<string, T> parse)
        {
            var value = GetValue(name);
            return string.IsNullOrWhiteSpace(value) ? default(T) : parse(value);
        }

        /// <summary>
        ///     取得整数字典对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetInt(string name)
        {
            return GetValue(name, int.Parse);
        }

        private static readonly object LockObj = new object();

        /// <summary>
        ///     取得一个数字后将这个数字自增1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int Addr(string name)
        {
            lock (LockObj)
            {
                var id = GetValue(name, int.Parse);
                SetValue(name, id + 1);
                return id + 1;
            }
        }

        /// <summary>
        ///     取得整数字典对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="parse">数据转化方法</param>
        /// <returns></returns>
        public static List<T> GetArray<T>(string name, Func<string, T> parse)
        {
            var value = GetValue(name);
            if (string.IsNullOrEmpty(value))
            {
                return new List<T>();
            }
            var array = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            return array.Length == 0 ? null : array.Select(parse).ToList();
        }

        /// <summary>
        ///     取得长整数字典对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static long GetLong(string name)
        {
            return GetValue(name, long.Parse);
        }

        /// <summary>
        ///     取得小数字典对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string name)
        {
            return GetValue(name, decimal.Parse);
        }

        #endregion

        #region 写值

        /// <summary>
        ///     写对象文本值
        /// </summary>
        /// <param name="name">字典名称</param>
        /// <param name="value">值</param>
        /// <returns>文本值</returns>
        public static void SetValue(string name, string value)
        {
            var lg = new DataDictionaryDataAccess();
            lg.Save(name, value);
        }

        /// <summary>
        ///     写对象值
        /// </summary>
        /// <param name="name">字典名称</param>
        /// <param name="value">值</param>
        public static void SetValue<T>(string name, List<T> value)
        {
            SetValue(name, value == null || value.Count == 0 ? null : string.Join(",", value));
        }

        /// <summary>
        ///     写对象值
        /// </summary>
        /// <param name="name">字典名称</param>
        /// <param name="value">值</param>
        public static void SetValue<T>(string name, T value)
        {
            SetValue(name, Equals(value, default(T)) ? null : value.ToString());
        }

        #endregion
    }
}