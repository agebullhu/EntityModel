using Agebull.Common.Base;
using Agebull.Common.Ioc;
using Microsoft.Extensions.Logging;
using System;

namespace Agebull.Common.Logging
{
    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class EmptyScope : ScopeBase
    {
        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
        }
    }

    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class MonitorScope : ScopeBase
    {
        private bool _isStep;
        private bool _isScope;
        /// <summary>
        /// ���ɷ�Χ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MonitorScope CreateScope(string name)
        {
            if (!LogRecorder.LogMonitor)
                return new MonitorScope();
            var scope = new MonitorScope
            {
                _isScope = true,
                _isStep = LogRecorder.MonitorItem.InMonitor
            };
            if (LogRecorder.MonitorItem.InMonitor)
                LogRecorder.BeginStepMonitor(name);
            else
            {
                IocScope.Logger = IocHelper.Create<ILoggerFactory>().CreateLogger(name);
                LogRecorder.BeginMonitor(name);
            }
            return scope;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            if (!_isScope)
                return;
            if (_isStep)
                LogRecorder.EndStepMonitor();
            else
                LogRecorder.EndMonitor();
        }
    }

    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    [Obsolete]
    public class MonitorStepScope : MonitorScope
    {

    }
}