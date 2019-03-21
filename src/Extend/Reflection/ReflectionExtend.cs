using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq;

using Agebull.Common.Reflection;

namespace System
{
    /// <summary>
    ///   ��ʾһ���ֶε���Ҫ�̶��������չ����
    /// </summary>
    public static class ReflectionExtend
    {
        /// <summary>
        ///   �ҵ�һ���ֶε���ʾ����
        /// </summary>
        public static string GetFieldDescription(this Type type, string field)
        {
            return type.GetField(field).GetDescription();
        }
        /// <summary>
        ///   �ҵ�һ���ֶε���ʾ����
        /// </summary>
        public static string GetPropertyDescription(this Type type, string field)
        {
            return type.GetProperty(field).GetDescription();
        }
        /// <summary>
        /// �õ�һ�������е�SupperPropert����
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <returns>�ҵ��򷵻ض����SupperPropert����,һ��Ĭ�ϵĶ���</returns>
        public static string GetDescription(this MemberInfo field)
        {
            var b = field.GetAttribute<DescriptionAttribute>();
            return b == null ? field.Name : b.Description ?? field.Name;
        }
        /// <summary>
        ///   ����һ�����͵�ʵ��
        /// </summary>
        /// <param name="type"> ���� </param>
        /// <returns> ���͵�ʵ�� </returns>
        public static object CreateObject(this Type type)
        {
            return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
        }

        /// <summary>
        /// �з񷽷�
        /// </summary>
        /// <param name="type"> ���� </param>
        /// <param name="fun"></param>
        /// <returns> ���͵�ʵ�� </returns>
        public static bool  HaseFun(this Type type, string fun)
        {
            var m =type.GetMethod(fun,
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance | //ָ��ʵ����Ա�������������С�
                BindingFlags.Public | //ָ��������Ա�������������С�
                BindingFlags.NonPublic); //ָ���ǹ�����Ա�������������С�)
            return m != null && m.DeclaringType == type;
        }
        /// <summary>
        /// ���ı�����(����TryParse)
        /// </summary>
        /// <param name="type"> ���� </param>
        /// <param name="value"></param>
        /// <returns> ���͵�ʵ�� </returns>
        public static T TryParse<T>(this Type type, object value)
        {
            if (type == typeof(string))
                return (T)value;
            return (T)ReflectionHelper.TryParse(type, (string)value);
        }
        /// <summary>
        ///   ����һ�����͵�ʵ��
        /// </summary>
        /// <param name="type"> ���� </param>
        /// <returns> ���͵�ʵ�� </returns>
        public static object Generate(this Type type)
        {
            return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
        }
        /// <summary>
        ///   ����һ�����͵�ʵ��
        /// </summary>
        /// <param name="type"> ���� </param>
        /// <returns> ���͵�ʵ�� </returns>
        public static T Generate<T>(this Type type) where T : class
        {
            return Generate(type) as T;
        }
        /// <summary>
        /// ����ָ��������
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
        /// �õ����Ͳ���
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericParameter(this Type type)
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var tParam in type.GetGenericArguments())
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
        /// �õ�һ�������Ƿ��ж�   ������Ӧ�Դ�
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="attribute">���Զ��� </param>
        /// <returns>�ҵ��򷵻ض����SupperPropert����,һ��Ĭ�ϵĶ���</returns>
        public static bool HaseAttribute(this Type type, Type attribute)
        {
            return type.GetCustomAttributes(attribute, true).Length > 0;
        }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBaseType(this Type type)
        {
            return type.IsPrimitive || type.IsEnum || BaseTypes.Contains(type);
        }
        /// <summary>
        /// ���л�������
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
        /// ȷ�������Ƿ�ʵ�ֻ�̳�ָ���ӿ�
        /// </summary>
        /// <param name="type">Ҫȷ��������</param>
        /// <param name="faces">ʵ�ֻ�̳еĽӿ�</param>
        /// <returns>true���ʵ��������ӿ�</returns>
        public static bool IsSupperInterface(this Type type, Type faces)
        {
            return type == faces || type.GetInterface(faces.Name) != null;
        }
        /// <summary>
        /// �õ��������͵Ľӿ�
        /// </summary>
        /// <param name="type">Ҫȷ��������</param>
        /// <returns>true���ʵ��������ӿ�</returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }
        /// <summary>
        /// �õ��������͵Ľӿ�
        /// </summary>
        /// <param name="field">Ҫȷ��������</param>
        /// <returns>true���ʵ��������ӿ�</returns>
        public static T GetAttribute<T>(this MemberInfo field) where T : Attribute
        {
            return field.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }
        /// <summary>
        /// �õ��������͵Ľӿ�
        /// </summary>
        /// <param name="field">Ҫȷ��������</param>
        /// <returns>true���ʵ��������ӿ�</returns>
        public static T GetAttribute2<T>(this MemberInfo field) where T : Attribute
        {
            return field.GetCustomAttributes(true).LastOrDefault(p=>p is T) as T;
        }
    }
}