/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.Core
建立:2016-06-16
修改: -
*****************************************************/

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