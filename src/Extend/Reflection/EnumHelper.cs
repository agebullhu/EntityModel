﻿// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if CLIENT
using System.Windows.Forms;
#endif
#endregion

namespace Agebull.Common.Reflection
{
    /// <summary>
    ///   枚举的帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        ///   得到枚举中文表示的值
        /// </summary>
        /// <returns> </returns>
        public static string GetCaption<T>(T value) where T : struct
        {
            var lValue = Convert.ToInt64(value);
            var t = typeof(T);
            var enumValues = GetEnumInfomation(t);

            if (t.GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0)
            {
                var vl = enumValues.FirstOrDefault(p => p.LValue == lValue);
                return vl == null
                               ? "无"
                               : vl.Caption;
            }
            var re = from v in enumValues
                     where (v.LValue & lValue) == v.LValue
                     select v.Caption;
            return re.LinkToString();
        }

        /// <summary>
        ///   得到枚举以中文表示的值
        /// </summary>
        /// <returns> </returns>
        public static List<EnumInfomation<T>> KeyValue<T>(bool keepNone = false) where T : struct
        {
            var t = typeof(T);
            var enumValues = t.GetEnumValues();
            return (from T value in enumValues
                    let lv = Convert.ToInt64(value)
                    let caption = t.GetFieldDescription(value.ToString())
                    where caption != null && (!keepNone || lv != 0)
                    select new EnumInfomation<T>
                    {
                        Value = value,
                        LValue = Convert.ToInt64(value),
                        Caption = caption
                    }).ToList();
        }

        /// <summary>
        ///   得到枚举以中文表示的值
        /// </summary>
        /// <returns> </returns>
        public static List<EnumInfomation> GetEnumFields(Type t, bool keepNone = false)
        {
            var enumValues = t.GetEnumValues();
            return (from object value in enumValues
                    let lv = Convert.ToInt64(value)
                    let caption = t.GetFieldDescription(value.ToString())
                    where caption != null && (!keepNone || lv != 0)
                    select new EnumInfomation
                               {
                                   Value = value,
                                   LValue = lv,
                                   Caption = caption
                               }).ToList();
        }

        /// <summary>
        ///   得到枚举以中文表示的值
        /// </summary>
        /// <returns> </returns>
        public static List<IEnumInfomation> GetEnumInfomation(Type type)
        {
            if (type == null)
            {
                return new List<IEnumInfomation>();
            }
            if (EnumInfomationMaps.ContainsKey(type))
            {
                return EnumInfomationMaps[type];
            }

            var enumFieldInfos = type.GetFields();
            var kvs = enumFieldInfos.Where(p => p.Name != "value__").Select(fieldInfo => (IEnumInfomation)new EnumInfomation
            {
                Caption = fieldInfo.GetDescription(),
                Value = Enum.Parse(type, fieldInfo.Name, true),
                LValue = Convert.ToInt64(Enum.Parse(type, fieldInfo.Name, true))
            }).ToList();

            EnumInfomationMaps.Add(type, kvs);

            return kvs;
        }

        /// <summary>
        ///   得到枚举以中文表示的值
        /// </summary>
        /// <returns> </returns>
        public static List<IEnumInfomation> GetEnumInfomation(string typeName)
        {
            return GetEnumInfomation(Type.GetType(typeName));
        }

        /// <summary>
        ///   枚举以中文表示的值的字典，以防止每次都需要做反序列化
        /// </summary>
        private static readonly Dictionary<Type, List<IEnumInfomation>> EnumInfomationMaps = new Dictionary<Type, List<IEnumInfomation>>();

    }

    /// <summary>
    ///   表示一个枚举的值文本对应表节点
    /// </summary>
    public interface IEnumInfomation
    {
        /// <summary>
        ///   文本
        /// </summary>
        string Caption { get; }

        /// <summary>
        ///   内容
        /// </summary>
        object Value { get; }

        /// <summary>
        ///   内容
        /// </summary>
        long LValue { get; }
    }

    /// <summary>
    ///   表示一个枚举的值文本对应表节点
    /// </summary>
    public class EnumInfomation : IEnumInfomation
    {
        /// <summary>
        ///   文本
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///   内容
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///   内容
        /// </summary>
        public long LValue { get; set; }

        /// <summary>
        ///   到文本
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            return Caption;
        }
    }

    /// <summary>
    ///   表示一个枚举的值文本对应表节点
    /// </summary>
    /// <typeparam name="T"> 枚举 </typeparam>
    public class EnumInfomation<T> : IEnumInfomation
            where T : struct
    {
        /// <summary>
        ///   文本
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///   内容
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        ///   内容
        /// </summary>
        public long LValue { get; set; }

        /// <summary>
        ///   内容
        /// </summary>
        object IEnumInfomation.Value => Value;

        /// <summary>
        ///   到文本
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            return Caption;
        }
    }
}
