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

#endregion

namespace YHXBank.BussinessSystem.Common
{
    /// <summary>
    ///     工作日帮助类
    /// </summary>
    public static class WorkDateHelper
    {
        private static readonly Dictionary<int, List<int>> YearDictionary = new Dictionary<int, List<int>>();

        /// <summary>
        ///     取对应工作日后的日期
        /// </summary>
        /// <param name="date">起始日期</param>
        /// <param name="workDay">工作日数量</param>
        /// <returns>对应工作日后的日期</returns>
        public static DateTime GetWorkDate(this DateTime date, int workDay)
        {
            if (workDay <= 0)
            {
                return date;
            }
            var year = date.Year;
            var array = GetYear(year);
            var count = 0;
            var idx = date.DayOfYear - 1;
            while (true)
            {
                if (idx == array.Count)
                {
                    array = GetYear(++year);
                    idx = 0;
                }
                if (array[idx] == 0)
                {
                    if (count == workDay)
                    {
                        return new DateTime(year, 1, 1).AddDays(idx);
                    }
                    count++;
                }
                idx++;
            }
        }

        public static List<int> GetYear(int year)
        {
            if (YearDictionary.ContainsKey(year))
            {
                return YearDictionary[year];
            }
            var name = "*_S_WorkDate_" + year;
            var value = DataDictionary.GetArray(name, int.Parse);
            if (value.Count == 0)
            {
                var s = new DateTime(year, 1, 1);
                while (s.Year == year)
                {
                    switch (s.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            value.Add(1);
                            break;
                        default:
                            value.Add(0);
                            break;
                    }
                    s = s.AddDays(1);
                }
                DataDictionary.SetValue(name, value);
            }
            YearDictionary.Add(year, value);
            return value;
        }
    }
}