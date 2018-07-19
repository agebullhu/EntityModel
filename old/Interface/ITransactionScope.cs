using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     表示访问条件和参数
    /// </summary>
    public interface ITransactionScope:IDisposable
    {
        /// <summary>
        ///     设置操作状态
        /// </summary>
        /// <param name="succeed">是否成功</param>
        void SetState(bool succeed);
    }
}