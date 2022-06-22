using System;
using System.Collections.Generic;
using System.Threading;
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
            Local.Value = new List<Action>();
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
                Local.Value = null;
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
        /// �ʵ��
        /// </summary>
        internal static readonly AsyncLocal<List<Action>> Local = new AsyncLocal<List<Action>>();

        /// <summary>
        /// ��������
        /// </summary>
        public static List<Action> DisposeFunc => Local.Value ?? (Local.Value = new List<Action>());
    }
}