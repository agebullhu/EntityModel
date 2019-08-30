using System;
using Agebull.Common.Base;

namespace Agebull.Common.Context
{
    /// <summary>
    /// 管理模式范围
    /// </summary>
    public class ManageModeScope : ScopeBase
    {
        private readonly bool _preIs;

        private ManageModeScope()
        {
            _preIs = GlobalContext.Current.IsManageMode;
            if (_preIs)
                return;
            GlobalContext.Current.IsManageMode = true;
        }
        /// <summary>
        /// 生成范围
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            return new ManageModeScope();
        }

        /// <inheritdoc />
        protected override void OnDispose()
        {
            if (_preIs)
                return;
            GlobalContext.Current.IsManageMode = false;
        }
    }
}