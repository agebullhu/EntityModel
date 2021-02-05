// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 委托析构
    /// </summary>
    public class AgentScope : IDisposable
    {
        /// <summary>
        /// 设置代为析构的对象
        /// </summary>
        public IDisposable Client { get; set; }

        void IDisposable.Dispose()
        {
            Client?.Dispose();
        }
    }
}