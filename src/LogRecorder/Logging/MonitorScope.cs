using Agebull.Common.Base;
using Agebull.Common.Ioc;

namespace Agebull.Common.Logging
{
    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class MonitorScope : ScopeBase
    {
        private bool _isStep;
        /// <summary>
        /// ���ɷ�Χ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MonitorScope CreateScope(string name)
        {
            var scope= new MonitorScope
            {
                _isStep = LogRecorderX.MonitorItem.InMonitor
            };
            if (LogRecorderX.MonitorItem.InMonitor)
                LogRecorderX.BeginStepMonitor(name);
            else
                LogRecorderX.BeginMonitor(name);
            return scope;
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            if (_isStep)
                LogRecorderX.EndStepMonitor();
            else
                LogRecorderX.EndMonitor();
        }
    }

    /// <summary>
    /// ���ݲ��跶Χ
    /// </summary>
    public class MonitorStepScope : MonitorScope
    {

    }
}