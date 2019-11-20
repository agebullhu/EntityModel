using System;
using System.Collections.Generic;
using Agebull.Common.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    /// IOC范围对象,内部框架使用
    /// </summary>
    public class IocScope : ScopeBase
    {
        private IServiceScope _scope;

        /// <summary>
        /// 生成一个范围
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            return new IocScope
            {
                _scope = IocHelper.CreateScope()
            };
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            if (DisposeFunc != null)
            {
                foreach (var func in DisposeFunc)
                {
                    try
                    {
                        func();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                _disposeFunc = null;
            }
            try
            {
                IocHelper.DisposeScope();
                _scope.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            GC.Collect();
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        [ThreadStatic] private static List<Action> _disposeFunc;

        /// <summary>
        /// 析构方法
        /// </summary>
        public static List<Action> DisposeFunc => _disposeFunc ?? (_disposeFunc = new List<Action>());
    }
}