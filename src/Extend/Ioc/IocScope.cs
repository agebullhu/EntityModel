using System;
using System.Collections.Generic;
using Agebull.Common.Base;

namespace Agebull.Common.Ioc
{
    /// <summary>
    /// IOC范围对象,内部框架使用
    /// </summary>
    public class IocScope : ScopeBase
    {
        /// <summary>
        /// 生成一个范围
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            IocHelper.CreateScope();
            return new IocScope();
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            foreach (var func in DisposeFunc)
                func();
            IocHelper.DisposeScope();
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        public static List<Action> DisposeFunc = new List<Action>();
    }
}