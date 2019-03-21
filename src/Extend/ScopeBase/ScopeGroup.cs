using System;

using Agebull.Common.Base;

namespace Agebull.Common
{
    /// <summary>
    /// 有效范围组
    /// </summary>
    public class ScopeGroup : ScopeBase
    {
        private readonly IDisposable[] Scopes;
        /// <summary>
        /// 构造，此时改变上下文，析构时自动还原
        /// </summary>
        /// <param name="scopes"></param>
        public static ScopeGroup CreateScope(params IDisposable[] scopes)
        {
            return new ScopeGroup(scopes);
        }
        /// <summary>
        /// 构造，此时改变上下文，析构时自动还原
        /// </summary>
        /// <param name="scopes"></param>
        public ScopeGroup(IDisposable[] scopes)
        {
            Scopes = scopes;
        }

        /// <summary>
        /// 执行析构的事情
        /// </summary>
        protected override void OnDispose()
        {
            foreach (var scope in Scopes)
                scope.Dispose();
        }
    }
}