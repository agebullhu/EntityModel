using System;
using Agebull.Common.Base;
using Agebull.Common.OAuth;

namespace Agebull.Common.Rpc
{
    /// <summary>
    /// 系统模式范围
    /// </summary>
    public class SystemModelScope : ScopeBase
    {
        private readonly bool _preIs;
        private readonly ILoginUserInfo _preUser;

        SystemModelScope()
        {
            _preIs = GlobalContext.Current.IsSystemMode;
            if (_preIs) return;
            _preUser = GlobalContext.Current.User;
            GlobalContext.Current._user = LoginUserInfo.System;
            GlobalContext.Current.IsSystemMode = true;
        }
        /// <summary>
        /// 生成范围
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            return new SystemModelScope();
        }

        /// <inheritdoc />
        protected override void OnDispose()
        {
            if (!_preIs)
                GlobalContext.Current._user = _preUser;
        }
    }
}