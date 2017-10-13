// /*****************************************************
// (c)2008-2013 Copy right www.Gboxt.com
// 作者:bull2
// 工程:Agebull.Common.Extend-Gboxt.Common.Reflection
// 建立:2014-04-13
// 修改:2014-10-13
// *****************************************************/

/***********************修改记录************************
2014年12月8日 agebull : 增加Lambda支持的两个方法GetName,GetValue
// *****************************************************/
#region 引用

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.CSharp;
using System.Runtime.Serialization.Formatters.Soap;
#if WINFORM
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

#endif

#endregion

namespace Agebull.Common.Reflection
{
    /// <summary>
    ///     反射的实例类
    /// </summary>
    public sealed class ReflectionHelper
    {
#if SILVERLIGHT
        /// <summary>
        ///   载入动态编译的结果并运行一个缺省的静态方法
        /// </summary>
        /// <param name="path"> 编译结果路径 </param>
        /// <param name="name"> 名称 </param>
        /// <param name="typeName"> 要运行的类型的全名 </param>
        /// <param name="staticMethod"> 要运行的静态方法 </param>
        /// <param name="args"> 要运行的静态方法的参数 </param>
        public static object LoadAndInvoke(Stream stream, string typeName, string staticMethod, params object[] args)
        {
            AssemblyPart assemblyPart = new AssemblyPart();
            Assembly assembly = assemblyPart.Load(stream);
            return InvokeStaticMethod(assembly.GetType(typeName), staticMethod, args);
        }

        /// <summary>
        ///   载入动态编译的结果并运行一个缺省的静态方法
        /// </summary>
        /// <param name="path"> 编译结果路径 </param>
        /// <param name="name"> 名称 </param>
        /// <param name="typeName"> 要运行的类型的全名 </param>
        /// <param name="staticMethod"> 要运行的静态方法 </param>
        /// <param name="args"> 要运行的静态方法的参数 </param>
        public static Assembly LoadAssembly(Stream stream)
        {
            AssemblyPart assemblyPart = new AssemblyPart();
            return assemblyPart.Load(stream);
        }
#else
        /// <summary>
        ///     动态编译
        /// </summary>
        /// <param name="path"> 代码所在的路径 </param>
        /// <param name="dllPaht"> 编译结果所在的位置 </param>
        /// <param name="dllName"> 不包括扩展名的名字,通常是根命名空间 </param>
        /// <returns> </returns>
        public static CompilerResults Compile(string path, string dllPaht, string dllName)
        {
            List<string> fields = IOHelper.GetAllField(path, "cs");
            CompilerParameters paras = new CompilerParameters()
            {
                IncludeDebugInformation = true,
                GenerateExecutable = false,
                GenerateInMemory = false,
                OutputAssembly = Path.Combine(dllPaht, dllName + ".dll")
            };
            paras.ReferencedAssemblies.Add("System.dll");
            paras.ReferencedAssemblies.Add("System.Configuration.dll");
            paras.ReferencedAssemblies.Add("System.Core.dll");
            paras.ReferencedAssemblies.Add("System.Data.dll");
            paras.ReferencedAssemblies.Add("System.Deployment.dll");
            paras.ReferencedAssemblies.Add("System.IdentityModel.dll");
            paras.ReferencedAssemblies.Add("System.IdentityModel.Selectors.dll");
            paras.ReferencedAssemblies.Add("System.Runtime.Serialization.dll");
            paras.ReferencedAssemblies.Add("System.Runtime.Serialization.Formatters.Soap.dll");
            paras.ReferencedAssemblies.Add("System.ServiceModel.dll");
            paras.ReferencedAssemblies.Add("System.Xml.dll");
            paras.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            paras.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                return provider.CompileAssemblyFromFile(paras, fields.ToArray());
            }
        }

        /// <summary>
        ///     动态编译
        /// </summary>
        /// <param name="path"> 代码所在的路径 </param>
        /// <param name="paras"> 编译参数 </param>
        /// <returns> </returns>
        public static CompilerResults Compile(string path, CompilerParameters paras)
        {
            paras.ReferencedAssemblies.Add("System.dll");
            paras.ReferencedAssemblies.Add("System.Configuration.dll");
            paras.ReferencedAssemblies.Add("System.Core.dll");
            paras.ReferencedAssemblies.Add("System.Data.dll");
            paras.ReferencedAssemblies.Add("System.Deployment.dll");
            paras.ReferencedAssemblies.Add("System.IdentityModel.dll");
            paras.ReferencedAssemblies.Add("System.IdentityModel.Selectors.dll");
            paras.ReferencedAssemblies.Add("System.Runtime.Serialization.dll");
            paras.ReferencedAssemblies.Add("System.Runtime.Serialization.Formatters.Soap.dll");
            paras.ReferencedAssemblies.Add("System.ServiceModel.dll");
            paras.ReferencedAssemblies.Add("System.Xml.dll");
            paras.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            paras.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            List<string> fields = IOHelper.GetAllField(path, "cs");
            CSharpCodeProvider provider = new CSharpCodeProvider();
            return provider.CompileAssemblyFromFile(paras, fields.ToArray());
        }

        /// <summary>
        ///     载入动态编译的结果并运行一个缺省的静态方法
        /// </summary>
        /// <param name="path"> 编译结果路径 </param>
        /// <param name="typeName"> 要运行的类型的全名 </param>
        /// <param name="staticMethod"> 要运行的静态方法 </param>
        /// <param name="args"> 要运行的静态方法的参数 </param>
        public static object LoadAndInvoke(string path, string typeName, string staticMethod, params object[] args)
        {
            Assembly assembly = LoadAssembly(path);
            return InvokeStaticMethod(assembly.GetType(typeName), staticMethod, args);
        }

        /// <summary>
        ///     已装入的程序集合
        /// </summary>
        private static readonly Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     装入程序集并准备进行方法调用
        /// </summary>
        /// <param name="path"> 路径 </param>
        /// <returns> 已装入的程序集 </returns>
        public static Assembly LoadAssembly(string path)
        {
            Assembly assembly;
            if (!loadedAssemblies.TryGetValue(Path.GetFileNameWithoutExtension(path), out assembly))
            {
                assembly = Assembly.LoadFrom(path);
                loadedAssemblies.Add(assembly.FullName, assembly);
            }
            return assembly;
        }

        //public static bool UnLoad(string name)
        //{
        //    if (!_Assembly.ContainsKey(name))
        //        return false;
        //    AppDomain currentDomain = AppDomain.CurrentDomain;
        //    currentDomain.
        //}
#endif

        private static readonly Dictionary<string, Type> unKnowTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     生成一个类型的实例
        /// </summary>
        /// <param name="typeName"> 类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object CreateObject(string typeName)
        {
            Type value;
            if (unKnowTypes.TryGetValue(typeName, out value))
            {
                return CreateObject(value);
            }
            foreach (Assembly asa in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    Type type = asa.GetType(typeName);
                    if (type == null)
                    {
                        continue;
                    }
                    unKnowTypes.Add(typeName, type);
                    if (typeName != type.FullName)
                    {
                        unKnowTypes.Add(type.FullName, type);
                    }
                    return CreateObject(type);
                }
                catch
                {
                }
            }
            return null;
        }

        /// <summary>
        ///     生成一个类型的实例
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object CreateObject(Type type)
        {
            return type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
        }

        /// <summary>
        ///     生成一个泛型类型的实例
        /// </summary>
        /// <param name="assembly"> 泛型所在的程序集(极其重要) </param>
        /// <param name="generic"> 空的泛型 </param>
        /// <param name="genericArguments"> 严格对应好的泛型的参数类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object CreateGenericObject(Assembly assembly, Type generic, params Type[] genericArguments)
        {
            if (generic == null || genericArguments == null || genericArguments.Length == 0)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(generic.FullName);
            sb.Append("[[");
            bool isFirst = true;
            foreach (Type ga in genericArguments)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append("] , [");
                }
                sb.Append(ga.AssemblyQualifiedName);
            }
            sb.Append("]]");
            Type type = assembly.GetType(sb.ToString());
            return type == null
                    ? null
                    : CreateObject(type);
        }

        /// <summary>
        ///     生成一个泛型类型的实例
        /// </summary>
        /// <param name="generic"> 空的泛型 </param>
        /// <param name="genericArguments"> 严格对应好的泛型的参数类型 </param>
        /// <returns> 类型的实例 </returns>
        public static object CreateGenericObject(Type generic, params Type[] genericArguments)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                object re = CreateGenericObject(assembly, generic, genericArguments);
                if (re != null)
                {
                    return re;
                }
            }
            return null;
        }


        /// <summary>
        ///     运行一个对象的静态方法
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 方法名 </param>
        /// <returns> 返回值 </returns>
        public static object InvokeStaticMethod(Type type, string name)
        {
            // Call a method.
            return type.InvokeMember(name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);
        }

        /// <summary>
        ///     运行一个对象的静态方法
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 方法名 </param>
        /// <param name="args"> 方法运行的参数 </param>
        /// <returns> 返回值 </returns>
        public static object InvokeStaticMethod(Type type, string name, params object[] args)
        {
            // Call a method.
            return type.InvokeMember(name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
        }

        /// <summary>
        ///     运行一个对象的方法
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 方法名 </param>
        /// <param name="obj"> 类的实例 </param>
        /// <param name="args"> 方法运行的参数 </param>
        /// <returns> 返回值 </returns>
        public static object InvokeMethod(Type type, string name, object obj, object[] args)
        {
            // Call a method.
            return type.InvokeMember(name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, args);
        }

        /// <summary>
        ///     得到一个对象的属性
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 属性名 </param>
        /// <param name="obj"> 类的实例,不能为空 </param>
        /// <returns> 返回值 </returns>
        public static object GetProperty(Type type, string name, object obj)
        {
            // Call a method.
            return type.InvokeMember("Get" + name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty, null, obj, null);
        }

        /// <summary>
        ///     得到一个对象的属性
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 属性名 </param>
        /// <param name="obj"> 类的实例,不能为空 </param>
        /// <returns> 返回值 </returns>
        public static object TryGetProperty(Type type, string name, object obj)
        {
            try
            {
                return GetProperty(type, name, obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     配置一个对象的属性
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 属性名 </param>
        /// <param name="obj"> 类的实例,不能为空 </param>
        /// <param name="arg"> 要配置的值 </param>
        /// <returns> 返回值 </returns>
        public static object SetProperty(Type type, string name, object obj, object arg)
        {
            // Call a method.
            return type.InvokeMember(name,
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null,
                    obj,
                    new[]
                    {
                        arg
                    });
        }

        /// <summary>
        ///     配置一个对象的属性
        /// </summary>
        /// <param name="args"> 要配置的值 </param>
        /// <returns> 返回值 </returns>
        public static object SetProperty(SetTypePropertyArgs args)
        {
            // Call a method.
            return args.Type.InvokeMember(args.Property,
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null,
                    args.Object,
                    new[]
                    {
                        args.Value
                    });
        }

        /// <summary>
        ///     类型
        /// </summary>
        public Type ThisType
        {
            get;
            protected set;
        }

        /// <summary>
        ///     实例
        /// </summary>
        public object ThisObject
        {
            get;
            protected set;
        }

        /// <summary>
        ///     用类型来构造
        /// </summary>
        /// <param name="type"> 类型 </param>
        public ReflectionHelper(Type type)
        {
            if (type == null)
            {
                return;
            }
            this.ThisType = type;
            this.ThisObject = CreateObject(type);
        }

        /// <summary>
        ///     构造
        /// </summary>
        protected ReflectionHelper()
        {
        }

        /// <summary>
        ///     读取或配置类型实例的属性
        /// </summary>
        /// <param name="property"> </param>
        /// <returns> </returns>
        public object this[string property]
        {
            get
            {
                return GetProperty(this.ThisType, property, this.ThisObject);
            }
            set
            {
                SetProperty(this.ThisType, property, this.ThisObject, value);
            }
        }

        /// <summary>
        ///     运行有参方法
        /// </summary>
        /// <param name="name"> 方法名 </param>
        /// <param name="args"> 参数 </param>
        /// <returns> 返回结果 </returns>
        public object Invoke(string name, object[] args)
        {
            return InvokeMethod(this.ThisType, name, this.ThisObject, args);
        }

        /// <summary>
        ///     运行无参方法
        /// </summary>
        /// <param name="name"> 方法名 </param>
        /// <returns> 返回结果 </returns>
        public object Invoke(string name)
        {
            return InvokeMethod(this.ThisType, name, this.ThisObject, null);
        }

        /// <summary>
        ///     运行值类型的TryParse方法,以进行文本到对象的转化
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="value"> 文本值 </param>
        /// <returns> 转换是否成功 </returns>
        internal static object TryParse(Type type, string value)
        {
            object[] pars =
            {
                value, CreateObject(type)
            };
            // Call a method.
            bool re =
                    (bool)
                            type.InvokeMember("TryParse", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, pars);
            if (re)
            {
                return pars[1];
            }
            throw new ArgumentException(string.Format("Argument[{0}] is bad!", value));
        }

        /// <summary>
        ///     通过MethodInfo对象来运行方法
        /// </summary>
        /// <param name="mi"> MethodInfo对象 </param>
        /// <param name="obj"> 对象实例 </param>
        /// <param name="args"> 参数 </param>
        /// <returns> 方法的返回值 </returns>
        private static object Invoke(MethodInfo mi, object obj, object[] args)
        {
            // Call a method.
            return mi.Invoke(obj, args);
        }

        /// <summary>
        ///     对值与类 类型进行到实例的反序列化
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="value"> 文本值 </param>
        /// <returns> 反序列化后的类实例 </returns>
        private static object Deserialize(Type type, string value)
        {
            value = "<?xml version='1.0' encoding='utf-8'?>\n" + value;
            // throw new Exception(type.ToString());
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader sr = new StringReader(value))
            {
                return serializer.Deserialize(sr);
            }
        }

        /// <summary>
        ///     构造反射调用时的参数
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="value"> 文本参数值 </param>
        /// <returns> 构造好的参数 </returns>
        private static object BuilderArg(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }
            return type.IsValueType
                    ? TryParse(type, value)
                    : Deserialize(type, value);
        }

        /// <summary>
        ///     构造反射调用时的参数
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <returns> 构造好的参数 </returns>
        public static bool IsStatic(Type type)
        {
            if (!type.IsSealed) //静态类都是密封类
            {
                return false;
            }
            if (type.BaseType != typeof(object)) //静态类都派生于object
            {
                return false;
            }
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return false; //靜态类不能有默认构造
            }
            if (type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
            {
                return false; //静态类没有实例构造
            }
            if (type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
            {
                return false; //静态类没有实例属性
            }
            if (type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
            {
                return false; //静态类没有实例字段
            }
            if (type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
            {
                return false; //静态类没有实例方法
            }
            return true;
        }

        /// <summary>
        ///     是否空的值类型
        /// </summary>
        /// <param name="info"> </param>
        /// <returns> </returns>
        public static bool IsNullableType(ITypeInfomation info)
        {
            return info.Type.IsValueType && info.GenericArguments.Count == 1 && info.GenericArguments[0].Type.IsValueType &&
                   info.TypeName == string.Format("Nullable<{0}>", info.GenericArguments[0].FullName);
        }

#if SILVERLIGHT
        /// <summary>
        ///   得到一个基类的所有已知类型
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static List<Type> GetKnowTypes(Type type, Assembly asm)
        {
            List<Type> types = new List<Type>();
            List<Type> knows = new List<Type>();
            knows.Add(type);
            GetKnowTypes(knows, asm.GetTypes(), type);
            return knows;
        }

        private static void GetKnowTypes(List<Type> knows, Type[] types, Type type)
        {
            foreach (Type tp in types.Where(p => !knows.Contains(p) && p.IsSubclassOf(type)))
            {
                if (!tp.IsGenericType)
                {
                    knows.Add(tp);
                }
                if (tp.BaseType.IsGenericType)
                {
                    knows.Add(tp.BaseType);
                }
                GetKnowTypes(knows, types, tp);
            }
        }
        
#else
        /// <summary>
        ///     得到一个基类的所有已知类型
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static List<Type> GetKnowTypes(Type type)
        {
            List<Type> types = new List<Type>();
            List<Type> knows = new List<Type>();
            //基类
            //找遍当前应用程序域中的所有程序集,如果找到此类派生类,也生成代码
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(asm.GetTypes());
            }
            knows.Add(type);
            GetKnowTypes(knows, types, type);
            return knows;
        }

        private static void GetKnowTypes(ICollection<Type> knows, IEnumerable<Type> types, Type type)
        {
            foreach (Type tp in types.Where(p => !knows.Contains(p) && p.IsSubclassOf(type)))
            {
                if (!tp.IsGenericType)
                {
                    knows.Add(tp);
                }
                if (tp.BaseType != null && tp.BaseType.IsGenericType)
                {
                    knows.Add(tp.BaseType);
                }
                GetKnowTypes(knows, types, tp);
            }
        }

        /// <summary>
        ///     检查一个类有无被继承
        /// </summary>
        /// <param name="knows"> </param>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static bool HaseSubclass(IEnumerable<Type> knows, Type type)
        {
            return knows.Any(p => !knows.Contains(p) && p.IsSubclassOf(type));
        }

        /// <summary>
        ///     将返回的对象序列化为可直接写到XML文件的XML文本
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="value"> 返回值 </param>
        /// <returns> 序列化后的文本 </returns>
        private static string ToResult(Type type, object value)
        {
            if (value == null)
            {
                return "null";
            }
            if (type == typeof(string))
            {
                return (string)value;
            }
            if (type.IsValueType)
            {
                return value.ToString();
            }
            XmlSerializer serializer = new XmlSerializer(type);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, value);
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            string xml = doc.LastChild.OuterXml;
            return xml;
        }

        private static string Execuest(object obj, Dictionary<string, string> args, MethodInfo mi)
        {
            ParameterInfo[] infos = mi.GetParameters();

            if (infos.Length <= 0)
            {
                return null;
            }
            object[] pars = new object[infos.Length];
            foreach (ParameterInfo info in infos)
            {
                try
                {
                    object o = BuilderArg(info.ParameterType, args[info.Name]);
                    pars[info.Position] = o;
                }
                catch (Exception err)
                {
                    throw new ArgumentException(SerializeException(err, "传递的参数有错误:" + info.ParameterType + ":" + mi.Name + ":" + info.Name));
                }
            }
            object re;
            try
            {
                re = Invoke(mi, obj, pars);
            }
            catch (Exception err)
            {
                throw new WarningException(SerializeException(err, "调用方法时发生错误 "));
            }
            try
            {
                if (mi.ReturnParameter == null || mi.ReturnParameter.ParameterType == typeof(void))
                {
                    return string.Empty;
                }
                return ToResult(mi.ReturnParameter.ParameterType, re);
            }
            catch (Exception err)
            {
                throw new WarningException(SerializeException(err, "生成返回值时发生错误 "));
            }
        }

        /// <summary>
        ///     根据对象实例运行它的方法并返回
        /// </summary>
        /// <param name="obj"> 对象实例 </param>
        /// <param name="name"> 方法名 </param>
        /// <param name="args"> 名称=值格式的参数表 </param>
        /// <returns> 返回值 </returns>
        /// <exception cref="ArgumentException">
        ///     <code>调用的参数有错误</code>
        ///     <code>调用的方法时发生内部错误</code>
        ///     <code>调用的方法不存在</code>
        /// </exception>
        public static string Execuest(object obj, string name, Dictionary<string, string> args)
        {
            Type[] types = obj.GetType().GetInterfaces();
            foreach (Type t in types)
            {
                MethodInfo mi = t.GetMethod(name);
                if (mi != null)
                {
                    return Execuest(obj, args, mi);
                }
            }
            throw new WarningException(SerializeException(null, "调用的方法不存在 "));
        }

        ///// <summary>
        ///// 以SOAP方式序列化未标记为序列化的对象
        ///// </summary>
        ///// <param name="o"></param>
        ///// <returns></returns>
        //public static string SerializeToSoap2(HttpCookieCollection o)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        SoapFormatter formatter = new SoapFormatter();
        //        try
        //        {
        //            formatter.Serialize(ms, o);
        //        }
        //        catch
        //        {
        //        }
        //        ms.Seek(0, SeekOrigin.Begin);
        //        TextReader reader = new StreamReader(ms);
        //        string lc = reader.ReadToEnd();
        //        return lc;
        //    }
        //}
        /// <summary>
        /// </summary>
        /// <param name="o"> </param>
        /// <returns> </returns>
        public static string Serialize(Object o)
        {
            if (o == null)
            {
                return null;
            }
            if (o is string)
            {
                return o.ToString();
            }
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, o);
            stream.Flush();
            return Convert.ToBase64String(stream.GetBuffer());
        }

        /// <summary>
        ///     以SOAP方式序列化
        /// </summary>
        /// <param name="o"> </param>
        /// <returns> </returns>
        public static string SerializeToSoap(Object o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SoapFormatter formatter = new SoapFormatter();
                try
                {
                    formatter.Serialize(ms, o);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
                ms.Seek(0, SeekOrigin.Begin);
                TextReader reader = new StreamReader(ms);
                string lc = reader.ReadToEnd();
                return lc;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="s"> </param>
        /// <returns> </returns>
        public static Object Deserialize(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }
            IFormatter formatter = new BinaryFormatter();
            byte[] buffer = Convert.FromBase64String(s);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///     生成一个类的已知类型代码
        /// </summary>
        /// <param name="type"> 类 </param>
        /// <param name="format"> 格式化代码 </param>
        /// <returns> 代码 </returns>
        public static string BuildKnowType(Type type, string format = "[KnownTypeAttribute(typeof({0}))]")
        {
            StringBuilder sb = new StringBuilder();
            List<Type> types = new List<Type>();
            List<Type> knows = new List<Type>();
            //基类
            //找遍当前应用程序域中的所有程序集,如果找到此类派生类,也生成代码
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(asm.GetTypes());
            }
            BuildKnowType(knows, types, type, sb, "\t" + format);
            return sb.ToString();
        }

        private static void BuildKnowType(ICollection<Type> knows, IEnumerable<Type> types, Type type, StringBuilder sb, string format)
        {
            foreach (Type tp in types.Where(p => !knows.Contains(p) && p.IsSubclassOf(type)))
            {
                knows.Add(tp);
                if (!tp.IsGenericType)
                {
                    sb.AppendFormat(format, GetTypeFullName(tp));
                }
                if (tp.BaseType != null && tp.BaseType.IsGenericType) // && !knows.Contains(tp.BaseType)
                {
                    sb.AppendLine();
                    sb.AppendFormat(format, GetTypeFullName(tp.BaseType));
                }
                sb.AppendLine();
                BuildKnowType(knows, types, tp, sb, "\t" + format);
            }
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static string GetTypeFullName(Type type)
        {
            return GetTypeName2(type, true);
        }

        /// <summary>
        ///     得到对象的可读类型名字
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string GetTypeName(object value)
        {
            return value == null
                    ? null
                    : GetTypeName2(value is Type ? (Type)value : value.GetType(), false);
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static string GetTypeName(Type type)
        {
            var name = GetTypeName2(type, false);
            return GetTypeShowName(name);
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="isFull"></param>
        /// <returns> </returns>
        private static string GetTypeName2(Type type, bool isFull)
        {
            StringBuilder sb = new StringBuilder();
            if (isFull && type.Namespace != null)
            {
                sb.Append(type.Namespace);
                sb.Append('.');
            }
            Type tp = type;
            if (type.IsByRef)
            {
                sb.Append("out ");
                tp = type.GetElementType();
            }
            else if (type.IsPointer)
            {
                sb.Append("ref ");
                tp = type.GetElementType();
            }
            if (type.IsArray)
            {
                tp = type.GetElementType();
            }
            if (tp.IsGenericType && tp.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                tp = tp.GetGenericArguments()[0];
            }
            sb.Append(GetTypeNameInner(tp, isFull));
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                sb.Append('?');
            }
            if (type.IsArray)
            {
                sb.Append('[');
                int mks = type.GetArrayRank();
                for (int i = 1; i < mks; i++)
                {
                    sb.Append(',');
                }
                sb.Append(']');
            }
            return sb.ToString();
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="isFull"></param>
        /// <returns> </returns>
        private static string GetTypeNameInner(Type type, bool isFull)
        {
            Type tp = type;
            if (type.IsGenericParameter)
            {
                if (type.DeclaringType != null)
                {
                    tp = type.DeclaringType;
                }
            }
            if (tp == typeof(void))
            {
                return "void";
            }
            if (tp == typeof(object))
            {
                return "object";
            }
            if (tp == typeof(bool))
            {
                return "bool";
            }
            if (tp == typeof(string))
            {
                return "string";
            }
            if (tp == typeof(Guid))
            {
                return @"Guid";
            }
            if (tp == typeof(DateTime))
            {
                return @"DateTime";
            }
            if (tp == typeof(decimal))
            {
                return @"decimal";
            }
            if (tp == typeof(float))
            {
                return @"float";
            }
            if (tp == typeof(double))
            {
                return @"double";
            }
            if (tp == typeof(int))
            {
                return @"int";
            }
            if (tp == typeof(uint))
            {
                return @"uint";
            }
            if (tp == typeof(short))
            {
                return @"short";
            }
            if (tp == typeof(ushort))
            {
                return @"ushort";
            }
            if (tp == typeof(long))
            {
                return @"long";
            }
            if (tp == typeof(ulong))
            {
                return @"ulong";
            }
            if (tp == typeof(char))
            {
                return @"char";
            }
            if (tp == typeof(byte))
            {
                return @"byte";
            }
            if (tp == typeof(sbyte))
            {
                return @"sbyte";
            }
            StringBuilder sb = new StringBuilder();
            if (!type.IsGenericType && !type.IsGenericParameter)
            {
                return type.Name;
            }
            sb.Append(type.IsGenericParameter
                    ? type.Name
                    : type.Name.Substring(0, type.Name.Length - 2));

            bool isFirst = true;
            foreach (Type tParam in type.GetGenericArguments())
            {
                if (isFirst)
                {
                    sb.Append('<');
                    isFirst = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.Append(GetTypeName2(tParam, isFull));
            }
            if (!isFirst)
            {
                sb.Append(">");
            }
            return sb.ToString();
        }

        /// <summary>
        ///     得到对象的可读类型名字
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string[] GetTypeNameSpace(object value)
        {
            return value == null
                    ? null
                    : GetTypeNameSpace(value.GetType());
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public static string[] GetTypeNameSpace(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }
            Type tp = type;
            if (type.IsGenericParameter)
            {
                if (type.DeclaringType != null)
                {
                    tp = type.DeclaringType;
                }
            }
            if (tp == typeof(byte[]))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(string))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(Guid))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(DateTime))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(decimal))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(float))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(double))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(float))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(int))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(uint))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(short))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(ushort))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(long))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(ulong))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(char))
            {
                return new[]
                {
                    "System"
                };
            }
            if (tp == typeof(byte))
            {
                return new[]
                {
                    "System"
                };
            }
            List<string> sps = new List<string>
            {
                type.Namespace
            };
            GetTypeNameSpace(tp, sps);
            return sps.Distinct().ToArray();
        }

        /// <summary>
        ///     得到类型的可读名字
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sps"> </param>
        /// <returns> </returns>
        private static void GetTypeNameSpace(Type type, List<string> sps)
        {
            sps.Add(type.Namespace);
            if (!type.IsGenericType
                && !type.IsGenericParameter)
            {
                return;
            }
            foreach (Type tParam in type.GetGenericArguments())
            {
                GetTypeNameSpace(tParam, sps);
            }
        }

        /// <summary>
        ///     得到类型的可读信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="root"> </param>
        /// <returns> </returns>
        public static void GetTypeInfomation<T>(Type type, T root) where T : class, ITypeInfomation, new()
        {
            root.Type = type;
            root.TypeName = GetTypeName(type);
            root.FullName = GetTypeFullName(type);
            root.NameSpace = type.Namespace;
            root.GenericArguments.Clear();
            if (!type.IsGenericType)
            {
                return;
            }
            foreach (Type tParam in type.GetGenericArguments())
            {
                T ch = new T();
                GetTypeInfomation(tParam, ch);
                root.GenericArguments.Add(ch);
            }
        }

        /// <summary>
        ///     得到类型的可读信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="creater">对象构造器 </param>
        /// <returns> </returns>
        public static T GetTypeInfomation<T>(Type type, Func<T> creater) where T : ITypeInfomation
        {
            T info = creater();
            info.Type = type;
            info.TypeName = GetTypeName(type);
            info.FullName = GetTypeFullName(type);
            info.NameSpace = type.Namespace;
            if (type.IsGenericType)
            {
                foreach (Type tParam in type.GetGenericArguments())
                {
                    info.GenericArguments.Add(GetTypeInfomation(tParam, creater));
                }
            }
            return info;
        }

        /// <summary>
        ///     生成一个类及派生类的复制代码
        /// </summary>
        /// <param name="type"> 类 </param>
        /// <returns> 代码 </returns>
        public static string BuildAllClassCopyCode(Type type)
        {
            StringBuilder sb = new StringBuilder();
            //基类
            sb.AppendLine(BuildClassCopyCode(type, type));
            //找遍当前应用程序域中的所有程序集,如果找到此类派生类,也生成代码
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type tp in asm.GetTypes())
                {
                    if (tp != type && type.IsSubclassOf(type))
                    {
                        sb.AppendLine(BuildClassCopyCode(tp, type));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        ///     生成一个类的复制代码
        /// </summary>
        /// <param name="type"> 派生类 </param>
        /// <param name="baseType"> 基类 </param>
        /// <returns> 代码 </returns>
        public static string BuildClassCopyCode(Type type, Type baseType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
==================================={0}====================================
        /// <summary>
        /// 从对象复制
        /// </summary>
        /// <param name=""obj"">同基类对象</param>
       public void {1} CopyFrom({0} obj)
        {{",
                    baseType,
                    baseType == type
                            ? "virtual"
                            : "override");
            if (baseType != type)
            {
                sb.AppendLine(@"
                if(obj == null)
                    return;
                base.CopyFrom(obj);");
            }
            sb.AppendFormat("               if(obj is {0})", type.Name);
            sb.AppendLine("             {");
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                if (pi.CanWrite)
                {
                    sb.AppendFormat("                   this.{0} = obj.{0};\n", pi.Name);
                }
            }
            sb.AppendLine("             }");
            sb.AppendLine("        }");
            Trace.WriteLine(sb.ToString());
            return sb.ToString();
        }
#endif

        #region 异常序列化

        /// <summary>
        ///     读取并格式化异常信息
        /// </summary>
        /// <param name="err"> 异常 </param>
        /// <param name="ti"> 缩进 </param>
        /// <returns> 格式化后的文本 </returns>
        public static string ReadExctption(Exception err, string ti)
        {
            StringBuilder sb = new StringBuilder();
            if (err != null)
            {
                Type tp = err.GetType();
                string type = tp.ToString();
                string message = err.Message;
#if !SILVERLIGHT
                string source = err.Source;
#else
                string source = string.Empty;
#endif
                string stack = err.StackTrace;
                Exception inner = err.InnerException;
                if (type.IndexOf("FaultException", StringComparison.Ordinal) >= 0)
                {
                    //直接处理SSO的异常,并中止递归
                    object innererr = TryGetProperty(tp, "Detail", err);
                    if (innererr != null)
                    {
                        object reason = TryGetProperty(tp, "Reason", err);
                        object errorCode = TryGetProperty(tp, "ErrorCode", err);
                        object errorMessage = TryGetProperty(tp, "ErrorMessage", err);
                        sb.AppendFormat(
#if DEBUG
"{4}Fault Type:{0}<br/>" +
#endif
 "{4}Message:{1}<br/>" + "{4}Error Code:{2}<br/>"
#if DEBUG
 + "{4}Error Info:<br/>{4}   {3}"
#endif
,
                                type,
                                reason == null
                                        ? ""
                                        : reason.ToString().Replace("<br/>", "<br/>" + ti),
                                errorCode == null
                                        ? ""
                                        : errorCode.ToString().Replace("<br/>", "<br/>" + ti),
                                errorMessage == null
                                        ? ""
                                        : errorMessage.ToString().Replace("<br/>", "<br/>" + ti),
                                ti);
                    }
                }
                //以递归方式去格式化异常(递归InnerException)
                sb.AppendFormat(
#if DEBUG
"{0}Exception:{1}<br/>" +
#endif
 "{0}Message:{2}<br/>" +
#if DEBUG
 "{0}Source:{3}<br/>" + "{0}StackTrace:<br/>{0}   {4}<br/>" +
#endif
 "{0}InnerException:<br/>{5}",
                        ti,
                        type,
                        string.IsNullOrWhiteSpace(message)
                                ? ""
                                : message.Replace("<br/>", "<br/>" + ti),
                        string.IsNullOrWhiteSpace(source)
                                ? ""
                                : source.Replace("<br/>", "<br/>" + ti),
                        string.IsNullOrWhiteSpace(stack)
                                ? ""
                                : stack.Replace("<br/>", "<br/>" + ti),
                        inner != null
                                ? ReadExctption(inner, ti + ti)
                                : "" //递归
                        );
            }
            return sb.ToString();
        }

        #region 名称检查

        /// <summary>
        /// 取得this属性的正确名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>his属性的正确名称</returns>
        public static string GetThisHead(string name)
        {
            if (name.IndexOf('.') < 0)
                return "this";
            var itns = GetTypeShowName(name).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Format("{0}.this", itns[itns.Length - 2]);
        }

        /// <summary>
        /// 取得短名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>短名称</returns>
        public static string GetTypeShowName(string name)
        {
            if (name.IndexOf('.') < 0)
                return name;
            var itns = name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (name.IndexOf('<') < 0)
            {
                return string.Format("{0}.{1}", itns[itns.Length - 2], itns[itns.Length - 1]);
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder word = new StringBuilder();
            for (int i = 0; i < itns.Length - 1; i++)
            {
                word.Clear();
                foreach (var ch in itns[i])
                {
                    switch (ch)
                    {
                        case '[':
                        case ']':
                            if (word.Length > 0)
                            {
                                sb.Append(CheckTypeString(word.ToString()));
                                word.Clear();
                            }
                            sb.Append(ch);
                            break;
                        case '<':
                        case '>':
                        case ',':
                            sb.Append(CheckTypeString(word.ToString()));
                            sb.Append(ch);
                            word.Clear();
                            break;
                        default:
                            word.Append(ch);
                            break;
                    }
                }
            }
            sb.Append('.');
            sb.Append(itns[itns.Length - 1]);
            return sb.ToString();
        }

        /// <summary>
        /// 检查文字的类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public static string CheckTypeString(string type)
        {
            switch (type)
            {
                case "Array":
                    return "array";

                case "Int16":
                    return "short";

                case "Int32":
                    return "int";

                case "Int64":
                    return "long";

                case "UInt16":
                    return "ushort";

                case "UInt32":
                    return "uint";

                case "UInt64":
                    return "ulong";

                case "Char":
                    return "char";

                case "Double":
                    return "double";

                case "Float":
                    return "float";

                case "Decimal":
                    return "decimal";

                case "Byte":
                    return "byte";

                case "UByte":
                    return "ubyte";

                case "String":
                    return "string";
                case "Object":
                    return "object";

            }
            return type;
        }
        #endregion
        /// <summary>
        ///     序列化异常到XML
        /// </summary>
        /// <param name="err"> 异常 </param>
        /// <returns> 序列化后的XML </returns>
        /// <param name="rootMessage"> 错误标题 </param>
        public static string SerializeException(Exception err, string rootMessage)
        {
            XElement xe = new XElement("Exceptions", new XElement("RootMessage", rootMessage));
            SerializeException(err, xe);
            return xe.ToString(SaveOptions.None);
        }

        /// <summary>
        ///     序列化异常(内部递归)
        /// </summary>
        /// <param name="err"> 异常 </param>
        /// <param name="par"> 上级节点 </param>
        public static void SerializeException(Exception err, XElement par)
        {
            if (err == null)
            {
                return;
            }
            par.Add(new XElement("ExceptionMessage", err.Message));
            if (err is AgebullSystemException)
            {
                AgebullSystemException serr = err as AgebullSystemException;
                par.Add(new XElement("InnerMessage", serr.InnerMessage));
                par.Add(new XElement("Extend", serr.Extend));
            }
            XElement xe = new XElement("Exception",
                    new XElement("Type", err.GetType().ToString()),
#if !SILVERLIGHT
 new XElement("Source", err.Source),
#endif
 new XElement("StackTrace", err.StackTrace));
            par.Add(xe);
            /*
            if (err is FaultException)
            {
                FaultException ex = err as FaultException;
                object innererr = TryGetProperty(err.GetType(), "Detail", err);

                string code = "未知", message = "未知";
                if (innererr != null)
                {
                    object a = TryGetProperty(innererr.GetType(), "ErrorCode", innererr);
                    if (a != null)
                    {
                        code = a.ToString();
                    }
                    object b = TryGetProperty(innererr.GetType(), "ErrorMessage", innererr);
                    if (b != null)
                    {
                        message = b.ToString();
                    }
                }
                xe.Add(new XElement("FaultException",
                        new XElement("Reason", ex.Reason),
                        new XElement("Code", ex.Code),
                        new XElement("Action", ex.Action),
                        new XElement("ErrorCode", code),
                        new XElement("ErrorMessage", message)));
                XElement data = new XElement("Datas");
                foreach (IDictionaryEnumerator a in ex.Data)
                {
                    data.Add(new XElement("Data", new XElement("Key", a.Key.ToString()), new XElement("Value", a.Value.ToString())));
                }
                xe.Add(data);
            }*/
            //递归下去,直到没有InnerException
            if (err.InnerException == null)
            {
                return;
            }
            XElement inner = new XElement("InnerException");
            xe.Add(inner);
            SerializeException(err.InnerException, inner);
        }

        #endregion

        #region Lambda表达式支持

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName(Expression<Delegate> expression)
        {
            return GetName(expression.Body as MemberExpression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName<TResult>(Expression<Func<TResult>> expression)
        {
            return GetName(expression.Body as MemberExpression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName<TArg, TResult>(Expression<Func<TArg, TResult>> expression)
        {
            return GetName(expression.Body as MemberExpression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName(MemberExpression expression)
        {
            return expression.Member.Name;
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<TResult> GetFunc<TResult>(Expression<Func<TResult>> expression)
        {
            return expression.Compile();
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<TArg, TResult> GetFunc<TArg, TResult>(Expression<Func<TArg, TResult>> expression)
        {
            return expression.Compile();
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object GetValue(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            dynamic func = lambda.Compile();
            return func();
        }
        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static TResult GetValue<TResult>(Expression<Func<TResult>> expression)
        {
            Func<TResult> func = expression.Compile();
            return func();
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object GetValue<TArg, TResult>(Expression<Func<TArg, TResult>> expression,TArg arg)
        {
            Func<TArg, TResult> func = expression.Compile();
            return func(arg);
        }

        /// <summary>
        ///     运行一个对象的静态方法(非反射执行)
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="typeName"> 类型名称 </param>
        /// <param name="methodName"> 方法名 </param>
        /// <remarks>
        ///     感谢博客园大牛鹤冲天,相关文章:http://www.cnblogs.com/ldp615/archive/2013/03/31/2991304.html
        /// </remarks>
        public static void InvokeStaticMethodByExpression(Assembly assembly, string typeName, string methodName)
        {
            Type type = assembly.GetType(typeName);
            InvokeStaticMethodByExpression(type, methodName);
        }

        /// <summary>
        ///     运行一个对象的静态方法(非反射执行)
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="name"> 方法名 </param>
        /// <remarks>
        ///     感谢博客园大牛鹤冲天,相关文章:http://www.cnblogs.com/ldp615/archive/2013/03/31/2991304.html
        /// </remarks>
        public static void InvokeStaticMethodByExpression(Type type, string name)
        {
            MethodInfo method = type.GetMethod(name);
            MethodCallExpression body = Expression.Call(method);
            Expression<Action> lambda = Expression.Lambda<Action>(body);
            Action func = lambda.Compile();
            func();
        }
        #endregion
    }
}
