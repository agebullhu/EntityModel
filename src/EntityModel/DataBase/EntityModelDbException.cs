// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:CF_WeiYue
// // 建立:2016-07-22
// // 修改:2016-07-27
// // *****************************************************/

#region 引用

using System.Data.Common;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     数据库访问其它异常
    /// </summary>
    public class EntityModelDbException : DbException
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="message"></param>
        public EntityModelDbException(string message) : base(message)
        {

        }
    }
}