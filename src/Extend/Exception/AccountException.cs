// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
namespace Agebull.Common
{
    /// <summary>
    ///   表示用户登录方面的异常
    /// </summary>
    public class AccountException : ExceptionEx
    {
        /// <summary>
        ///   构造
        /// </summary>
        /// <param name="msg"> </param>
        public AccountException(string msg) : base(msg)
        {
        }
    }
}
