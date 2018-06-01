using System;
using System.Collections.Generic;

namespace Agebull.Common.Ioc
{
    /// <summary>
    ///  简单的依赖注入器
    /// </summary>
    public class IocHelper
    {
        /// <summary>
        /// 已注册对象
        /// </summary>
        public static Dictionary<Type, Func<object>> InterfaceDictionary = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 注册接口实现
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TClass"></typeparam>
        public static void Regist<TInterface, TClass>()
            where TClass : class, new()
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                InterfaceDictionary[typeof(TInterface)] = () => new TClass();
            else
                InterfaceDictionary.Add(typeof(TInterface), () => new TClass());
        }

        /// <summary>
        /// 生成接口实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                return InterfaceDictionary[typeof(TInterface)]() as TInterface;
            throw new Exception($"接口{typeof(TInterface)}，所需要的实现没有注册");
        }
    }
}