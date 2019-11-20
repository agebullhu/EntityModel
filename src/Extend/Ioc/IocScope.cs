using System;
using System.Collections.Generic;
using Agebull.Common.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    /// IOC��Χ����,�ڲ����ʹ��
    /// </summary>
    public class IocScope : ScopeBase
    {
        private IServiceScope _scope;

        /// <summary>
        /// ����һ����Χ
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
        /// ������Դ
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
        /// ��������
        /// </summary>
        [ThreadStatic] private static List<Action> _disposeFunc;

        /// <summary>
        /// ��������
        /// </summary>
        public static List<Action> DisposeFunc => _disposeFunc ?? (_disposeFunc = new List<Action>());
    }
}