using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Agebull.Common
{
    /// <summary>
    /// 上下文辅助类
    /// </summary>
    public static class ContextHelper
    {
        /// <summary>
        /// 单元测试时使用的字典
        /// </summary>
        private static readonly Dictionary<string, object> Contexts = new Dictionary<string, object>();

        /// <summary>
        /// 得到调用逻辑上下文对象
        /// </summary>
        public static T LogicalGetData<T>(string name)
            where T : class
        {
            if (!InUnitTest)
                return CallContext.LogicalGetData(name) as T;
            object value;
            if (!Contexts.TryGetValue(name, out value))
                return null;
            return value as T;
        }

        /// <summary>
        /// 设置调用逻辑上下文对象
        /// </summary>
        public static void LogicalSetData<T>(string name, T value)
            where T : class
        {
            if (!InUnitTest)
            {
                CallContext.LogicalSetData(name, value);
                return;
            }
            if (Contexts.ContainsKey(name))
                Contexts[name] = value;
            else
                Contexts.Add(name, value);
        }

        /// <summary>
        /// 清除调用逻辑上下文对象
        /// </summary>
        public static void Remove(string name)
        {
            if (!InUnitTest)
            {
                CallContext.LogicalSetData(name, null);
                return;
            }
            if (Contexts.ContainsKey(name))
                Contexts.Remove(name);
        }

        /// <summary>
        /// 是否处于单元测试环境中（单元测试环境与CallContext不兼容）
        /// </summary>
        public static bool InUnitTest { get; set; }

    }
}