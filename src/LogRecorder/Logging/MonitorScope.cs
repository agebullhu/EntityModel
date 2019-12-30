using Agebull.Common.Base;
using Agebull.Common.Ioc;

namespace Agebull.Common.Logging
{
    /// <summary>
    /// 根据步骤范围
    /// </summary>
    public class MonitorScope : ScopeBase
    {
        private bool _isStep;
        /// <summary>
        /// 生成范围
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MonitorScope CreateScope(string name)
        {
            var item = IocHelper.Create<MonitorItem>();
            var scope= new MonitorScope
            {
                _isStep = item.InMonitor
            };
            if (item.InMonitor)
                LogRecorderX.BeginStepMonitor(name);
            else
                LogRecorderX.BeginMonitor(name);
            return scope;
        }

        /// <summary>
        /// 清理资源
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
    /// 根据步骤范围
    /// </summary>
    public class MonitorStepScope : MonitorScope
    {

    }
}