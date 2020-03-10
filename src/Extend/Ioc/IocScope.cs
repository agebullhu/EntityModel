using System;
using System.Collections.Generic;
using System.Threading;
using Agebull.Common.Base;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LocalValueType = System.Tuple<string, Agebull.EntityModel.Common.DependencyObjects, System.Collections.Generic.List<System.Action>, Microsoft.Extensions.Logging.ILogger>;
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
        public static IDisposable CreateScope(string name = null)
        {
            Local.Value = new LocalValueType(name ?? "Scope", new DependencyObjects(), new List<Action>(), IocHelper.Create<ILoggerFactory>().CreateLogger(name ?? "Log"));
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
        internal static readonly AsyncLocal<LocalValueType> Local = new AsyncLocal<LocalValueType>();

        /// <summary>
        /// ��������
        /// </summary>
        static LocalValueType LocalValue => Local.Value ?? (Local.Value = new LocalValueType("Scope", new DependencyObjects(), new List<Action>(), IocHelper.Create<ILoggerFactory>().CreateLogger("Log")));

        /// <summary>
        /// ��Χ����
        /// </summary>
        public static string Name
        {
            get => LocalValue.Item1;
            set => Local.Value = new LocalValueType(value, LocalValue.Item2, LocalValue.Item3, Logger);
        }

        /// <summary>
        /// ��־��¼��
        /// </summary>
        public static ILogger Logger
        {
            get => LocalValue.Item4;
            set => Local.Value = new LocalValueType(Name, LocalValue.Item2, LocalValue.Item3, value);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public static List<Action> DisposeFunc => LocalValue.Item3;

        /// <summary>
        /// ��������
        /// </summary>
        public static DependencyObjects Dependency => LocalValue.Item2;
    }
}