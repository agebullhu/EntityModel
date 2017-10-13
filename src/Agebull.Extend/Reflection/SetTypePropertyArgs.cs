using System;
using System.Reflection;

namespace Agebull.Common.Reflection
{
    /// <summary>
    ///   表示为一个类型实例配置值的参数
    /// </summary>
    /// <typeparam name="A"> </typeparam>
    /// <typeparam name="B"> </typeparam>
    public class SetTypePropertyArgs<A, B>
    {
        /// <summary>
        ///   实例对象
        /// </summary>
        public A Object { get; set; }

        /// <summary>
        ///   属性
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///   值
        /// </summary>
        public B Value { get; set; }

        /// <summary>
        ///   配置值
        /// </summary>
        /// <param name="value"> </param>
        public void SetProperty(B value)
        {
            // Call a method.
            typeof(A).InvokeMember(this.Property,
                                   BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                   null,
                                   this.Object,
                                   new object[]
                                       {
                                               value
                                       });
        }

        /// <summary>
        ///   配置值
        /// </summary>
        public void SetProperty()
        {
            // Call a method.
            typeof(A).InvokeMember(this.Property,
                                   BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                   null,
                                   this.Object,
                                   new object[]
                                       {
                                               this.Value
                                       });
        }
    }

    /// <summary>
    ///   表示为一个类型实例配置值的参数
    /// </summary>
    public class SetTypePropertyArgs
    {
        /// <summary>
        ///   类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///   实例对象
        /// </summary>
        public Object Object { get; set; }

        /// <summary>
        ///   属性
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///   值
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        ///   配置值
        /// </summary>
        /// <param name="value"> </param>
        public void SetProperty(Object value)
        {
            // Call a method.
            this.Type.InvokeMember(this.Property,
                                   BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                   null,
                                   this.Object,
                                   new[]
                                       {
                                               value
                                       });
        }

        /// <summary>
        ///   配置值
        /// </summary>
        public void SetProperty()
        {
            // Call a method.
            this.Type.InvokeMember(this.Property,
                                   BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                                   null,
                                   this.Object,
                                   new[]
                                       {
                                               this.Value
                                       });
        }
    }
}