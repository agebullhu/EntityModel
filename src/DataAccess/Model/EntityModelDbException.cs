// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:CF_WeiYue
// // ����:2016-07-22
// // �޸�:2016-07-27
// // *****************************************************/

#region ����

using System.Data.Common;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ���ݿ���������쳣
    /// </summary>
    public class EntityModelDbException : DbException
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="message"></param>
        public EntityModelDbException(string message) : base(message)
        {

        }
    }
}