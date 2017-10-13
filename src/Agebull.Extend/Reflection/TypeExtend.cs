using System.Linq.Expressions;

using Agebull.Common.Reflection;

namespace System
{
    /// <summary>
    /// 类型扩展
    /// </summary>
    public static class TypeExtend
    {
        /// <summary>
        ///   得到对象的可读类型名字
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string GetTypeName(this object value)
        {
            return value == null ? null : ReflectionHelper.GetTypeName(value);
        }

        /// <summary>
        ///   得到对象的可读类型名字
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string[] GetTypeNameSpace(this object value)
        {
            return value == null ? null : ReflectionHelper.GetTypeNameSpace(value);
        }

        #region Lambda表达式支持

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName(Expression<Delegate> expression)
        {
            return ReflectionHelper.GetName(expression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName<TResult>(Expression<Func<TResult>> expression)
        {
            return ReflectionHelper.GetName(expression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName<TArg, TResult>(Expression<Func<TArg, TResult>> expression)
        {
            return ReflectionHelper.GetName(expression);
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName(MemberExpression expression)
        {
            return ReflectionHelper.GetName(expression);
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<TResult> GetFunc<TResult>(Expression<Func<TResult>> expression)
        {
            return ReflectionHelper.GetFunc(expression);
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<TArg, TResult> GetFunc<TArg, TResult>(Expression<Func<TArg, TResult>> expression)
        {
            return ReflectionHelper.GetFunc(expression);
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object GetValue(Expression expression)
        {
            return ReflectionHelper.GetValue(expression);
        }
        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static TResult GetValue<TResult>(Expression<Func<TResult>> expression)
        {
            return ReflectionHelper.GetValue(expression);
        }

        /// <summary>
        ///     取得方法委托
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object GetValue<TArg, TResult>(Expression<Func<TArg, TResult>> expression, TArg arg)
        {
            return ReflectionHelper.GetValue(expression, arg);
        }

        #endregion
    }
}
