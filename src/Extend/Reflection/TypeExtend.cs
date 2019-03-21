using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
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

        #region 动态调用支持


        /// <summary>
        /// 生成动态匿名调用内部方法（参数由TArg转为实际类型后调用，并将调用返回值转为TRes）
        /// </summary>
        /// <typeparam name="TArg">参数类型（接口）</typeparam>
        /// <typeparam name="TRes">返回值类型（接口）</typeparam>
        /// <param name="callInfo">调用对象类型</param>
        /// <param name="argInfo">原始参数类型</param>
        /// <param name="resInfo">原始返回值类型</param>
        /// <param name="methodName">原始调用方法</param>
        /// <returns>匿名委托</returns>
        public static Func<TArg, TRes> CreateFunc<TArg, TRes>(TypeInfo callInfo, string methodName, TypeInfo argInfo, TypeInfo resInfo)
        {
            var typeConstructor = resInfo.GetConstructor(Type.EmptyTypes);
            if (typeConstructor == null)
                throw new ArgumentException($"类型{callInfo.FullName}没有无参构造函数");
            var innerMethod = callInfo.GetMethod(methodName);
            if (innerMethod == null)
                throw new ArgumentException($"类型{callInfo.FullName}没有名称为{methodName}的方法");
            if (innerMethod.ReturnType != resInfo)
                throw new ArgumentException($"类型{callInfo.FullName}的方法{methodName}返回值不为{resInfo.FullName}");
            var args = innerMethod.GetParameters();
            if (args.Length != 1)
                throw new ArgumentException($"类型{callInfo.FullName}的方法{methodName}参数不是一个");
            if (args[0].ParameterType != argInfo)
                throw new ArgumentException($"类型{callInfo.FullName}的方法{methodName}唯一参数不为{argInfo.FullName}");
            //构造匿名方法
            var callMethod = new DynamicMethod(methodName, typeof(TRes), new[] { typeof(TArg) });
            //构造动态IL（方法内部实现）
            var il = callMethod.GetILGenerator();
            il.Emit(OpCodes.Nop);
            //1 参数类型转换
            il.Emit(OpCodes.Ldarg, 0);
            il.Emit(OpCodes.Castclass, argInfo);
            var arg = il.DeclareLocal(argInfo);
            il.Emit(OpCodes.Stloc, arg);
            //2 调用对象构造
            il.Emit(OpCodes.Newobj, typeConstructor);
            var call = il.DeclareLocal(callInfo);
            il.Emit(OpCodes.Stloc, call);
            //3 方法调用
            il.Emit(OpCodes.Ldloc, call);
            il.Emit(OpCodes.Ldloc, arg);
            il.Emit(OpCodes.Callvirt, innerMethod);
            var ret = il.DeclareLocal(innerMethod.ReturnType);
            //4 返回值转换
            il.Emit(OpCodes.Stloc, ret);
            il.Emit(OpCodes.Ldloc, ret);
            il.Emit(OpCodes.Castclass, typeof(TRes).GetTypeInfo());
            var res = il.DeclareLocal(resInfo);
            //5 返回
            il.Emit(OpCodes.Stloc, res);
            il.Emit(OpCodes.Ldloc, res);
            il.Emit(OpCodes.Ret);
            //返回动态委托
            return callMethod.CreateDelegate(typeof(Func<TArg, TRes>)) as Func<TArg, TRes>;
        }

        /// <summary>
        /// 生成动态匿名调用内部方法（无参，调用返回值转为TRes）
        /// </summary>
        /// <typeparam name="TRes">返回值类型（接口）</typeparam>
        /// <param name="callInfo">调用对象类型</param>
        /// <param name="resInfo">原始返回值类型</param>
        /// <param name="methodName">原始调用方法</param>
        /// <returns>匿名委托</returns>
        public static Func<TRes> CreateFunc<TRes>(TypeInfo callInfo, string methodName, TypeInfo resInfo)
        {
            var typeConstructor = resInfo.GetConstructor(Type.EmptyTypes);
            if (typeConstructor == null)
                throw new ArgumentException($"类型{callInfo.FullName}没有无参构造函数");
            var innerMethod = callInfo.GetMethod(methodName);
            if (innerMethod == null)
                throw new ArgumentException($"类型{callInfo.FullName}没有名称为{methodName}的方法");
            if (innerMethod.ReturnType != resInfo)
                throw new ArgumentException($"类型{callInfo.FullName}的方法{methodName}返回值不为{resInfo.FullName}");
            var args = innerMethod.GetParameters();
            if (args.Length > 0)
                throw new ArgumentException($"类型{callInfo.FullName}的方法{methodName}参数不为空");
            //构造匿名方法
            var callMethod = new DynamicMethod(methodName, typeof(TRes), null);
            //构造动态IL（方法内部实现）
            var il = callMethod.GetILGenerator();
            il.Emit(OpCodes.Nop);
            //1 调用对象构造
            il.Emit(OpCodes.Newobj, typeConstructor);
            var call = il.DeclareLocal(callInfo);
            il.Emit(OpCodes.Stloc, call);
            //3 方法调用
            il.Emit(OpCodes.Ldloc, call);
            il.Emit(OpCodes.Callvirt, innerMethod);
            var ret = il.DeclareLocal(innerMethod.ReturnType);
            //4 返回值转换
            il.Emit(OpCodes.Stloc, ret);
            il.Emit(OpCodes.Ldloc, ret);
            il.Emit(OpCodes.Castclass, typeof(TRes).GetTypeInfo());
            var res = il.DeclareLocal(resInfo);
            //5 返回
            il.Emit(OpCodes.Stloc, res);
            il.Emit(OpCodes.Ldloc, res);
            il.Emit(OpCodes.Ret);
            //返回动态委托
            return callMethod.CreateDelegate(typeof(Func<TRes>)) as Func<TRes>;
        }


        #endregion
    }
}
