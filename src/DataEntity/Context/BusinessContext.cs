// /*****************************************************
// (c)2016-2018 Copy right www.gboxt.com
// 作者:Agebull
// 工程:Agebull.DataModel
// 建立:2018.01.16
// 说明：全局业务对象，主要提供后期注入承载点
// *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using Agebull.Common.DataModel;
using Agebull.Common.Logging;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     为业务处理全局对象
    /// </summary>
    public static class BusinessGlobal
    {
        /// <summary>
        /// 后期注入的实体事件处理对象
        /// </summary>
        public static IEntityEventProxy EntityEventProxy { get; set; }
    }
}