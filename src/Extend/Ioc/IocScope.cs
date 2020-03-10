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
    /// IOC范围对象,内部框架使用
    /// </summary>
    public class IocScope : ScopeBase
    {
        private IServiceScope _scope;
        /// <summary>
        /// 生成一个范围
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
        /// 活动实例
        /// </summary>
        internal static readonly AsyncLocal<LocalValueType> Local = new AsyncLocal<LocalValueType>();

        /// <summary>
        /// 析构方法
        /// </summary>
        static LocalValueType LocalValue => Local.Value ?? (Local.Value = new LocalValueType("Scope", new DependencyObjects(), new List<Action>(), IocHelper.Create<ILoggerFactory>().CreateLogger("Log")));

        /// <summary>
        /// 范围名称
        /// </summary>
        public static string Name
        {
            get => LocalValue.Item1;
            set => Local.Value = new LocalValueType(value, LocalValue.Item2, LocalValue.Item3, Logger);
        }

        /// <summary>
        /// 日志记录器
        /// </summary>
        public static ILogger Logger
        {
            get => LocalValue.Item4;
            set => Local.Value = new LocalValueType(Name, LocalValue.Item2, LocalValue.Item3, value);
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        public static List<Action> DisposeFunc => LocalValue.Item3;

        /// <summary>
        /// 附件内容
        /// </summary>
        public static DependencyObjects Dependency => LocalValue.Item2;
    }
}