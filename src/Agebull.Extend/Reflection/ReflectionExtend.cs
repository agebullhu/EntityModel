using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq;

using Agebull.Common.Reflection;

namespace System
{
    /// <summary>
    ///   表示一个字段的重要程度与分类扩展属性
    /// </summary>
    public static class ReflectionExtend
    {
        /// <summary>
        ///   找到一个字段的显示文字
        /// </summary>
        public static string GetFieldDescription(this Type type, string field)
        {
            return type.GetField(field).GetDescription();
        }
        /// <summary>
        ///   找到一个字段的显示文字
        /// </summary>
        public static string GetPropertyDescription(this Type type, string field)
        {
            return type.GetProperty(field).GetDescription();
        }
        /// <summary>
        /// 得到一个类型中的SupperPropert特性
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns>找到则返回对象的SupperPropert特性,一个默认的对象</returns>
        public static string GetDescription(this MemberInfo field)
        {
            var b = field.GetAttribute<DescriptionAttribute>();
            return b == null ? field.Name : b.Description ?? field.Name;
        }
        /// <summary>
        ///   生成一个类型的实例
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object CreateObject(this Type type)
        {
            return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
        }

        /// <summary>
        /// 有否方法
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="fun"></param>
        /// <returns> 类型的实例 </returns>
        public static bool  HaseFun(this Type type, string fun)
        {
            var m =type.GetMethod(fun,
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance | //指定实例成员将包括在搜索中。
                BindingFlags.Public | //指定公共成员将包括在搜索中。
                BindingFlags.NonPublic); //指定非公共成员将包括在搜索中。)
            return m != null && m.DeclaringType == type;
        }
        /// <summary>
        /// 从文本构造(调用TryParse)
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="value"></param>
        /// <returns> 类型的实例 </returns>
        public static T TryParse<T>(this Type type, object value)
        {
            if (type == typeof(string))
                return (T)value;
            return (T)ReflectionHelper.TryParse(type, (string)value);
        }
        /// <summary>
        ///   生成一个类型的实例
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object Generate(this Type type)
        {
            return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
        }
        /// <summary>
        ///   生成一个类型的实例
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <returns> 类型的实例 </returns>
        public static T Generate<T>(this Type type) where T : class
        {
            return Generate(type) as T;
        }
        /// <summary>
        /// 包括指定方法吗
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static bool HaseMethod(this Type type, string fun)
        {
            if (type == null
                || fun == null)
                return false;
            return type.GetMethod(fun, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Public, Type.DefaultBinder, Type.EmptyTypes, null) != null;
        }
        /// <summary>
        /// 得到泛型参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericParameter(this Type type)
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (Type tParam in type.GetGenericArguments())
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.Append(ReflectionHelper.GetTypeName(tParam));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到一个类型是否有对   民性属应对错
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="attribute">属性对象 </param>
        /// <returns>找到则返回对象的SupperPropert特性,一个默认的对象</returns>
        public static bool HaseAttribute(this Type type, Type attribute)
        {
            return type.GetCustomAttributes(attribute, true).Length > 0;
        }
        /// <summary>
        /// 是否基本类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBaseType(this Type type)
        {
            return type.IsPrimitive || type.IsEnum || BaseTypes.Contains(type);
        }
        /// <summary>
        /// 所有基础类型
        /// </summary>
        public static readonly Type[] BaseTypes = new[]
            {
                    typeof (string),
                    typeof (decimal),
                    typeof (DateTime),
                    typeof (Guid),
                    typeof (Boolean),
                    typeof (Byte),
                    typeof (SByte),
                    typeof (Int16),
                    typeof (UInt16),
                    typeof (Int32),
                    typeof (UInt32),
                    typeof (Int64),
                    typeof (UInt64),
                    typeof (Byte),
                    typeof (IntPtr),
                    typeof (UIntPtr),
                    typeof (Char),
                    typeof (Double),
                    typeof (Single)
            };
        /// <summary>
        /// 确定类型是否实现或继承指定接口
        /// </summary>
        /// <param name="type">要确定的类型</param>
        /// <param name="faces">实现或继承的接口</param>
        /// <returns>true表达实现了这个接口</returns>
        public static bool IsSupperInterface(this Type type, Type faces)
        {
            return type == faces || type.GetInterface(faces.Name) != null;
        }
        /// <summary>
        /// 得到数据类型的接口
        /// </summary>
        /// <param name="type">要确定的类型</param>
        /// <returns>true表达实现了这个接口</returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }
        /// <summary>
        /// 得到数据类型的接口
        /// </summary>
        /// <param name="field">要确定的类型</param>
        /// <returns>true表达实现了这个接口</returns>
        public static T GetAttribute<T>(this MemberInfo field) where T : Attribute
        {
            return field.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }
        /// <summary>
        /// 得到数据类型的接口
        /// </summary>
        /// <param name="field">要确定的类型</param>
        /// <returns>true表达实现了这个接口</returns>
        public static T GetAttribute2<T>(this MemberInfo field) where T : Attribute
        {
            return field.GetCustomAttributes(true).LastOrDefault(p=>p is T) as T;
        }
    }
}