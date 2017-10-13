using Agebull.Common.Logging;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yizuan.Service.Fundtion.Interceptor
{

    /// <summary>
    /// 自定义日志拦截器
    /// </summary>
    public class LogInterceptor : IInterceptor
    {
        /// <summary>
        /// 拦截器实现
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
           
                 BeforeProceed(invocation);
                 AfterProceed(invocation);
                 AfterProceed(invocation);
         
        }

        /// <summary>
        /// 方法执行开始，记录日志
        /// </summary>
        /// <param name="invocation"></param>
        protected  void BeforeProceed(IInvocation invocation)
        {

            LogRecorder.MonitorTrace($"请求类名称：{invocation.InvocationTarget}");
            LogRecorder.MonitorTrace($"请求方法名称：{invocation.Method.Name}");
            LogRecorder.MonitorTrace($"请求参数：{ JsonConvert.SerializeObject(invocation.Arguments)}");
        }
        /// <summary>
        /// 方法执行异常记录日志
        /// </summary>
        /// <param name="invocation"></param>
        protected void AfterProceed(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();//异常拦截
            }
            catch (Exception ex)
            {
                LogRecorder.MonitorTrace($"异常信息：{JsonConvert.SerializeObject(ex)}");
              
            }

        }
        /// <summary>
        /// 方法执行完毕记录日志
        /// </summary>
        /// <param name="invocation"></param>
        protected void ExceptionProceed(IInvocation invocation)
        {
            LogRecorder.MonitorTrace($"返回结果：{ JsonConvert.SerializeObject(invocation.ReturnValue)}");
          
        }


    }
}
