using Agebull.Common.OAuth;
namespace Agebull.Common.Context
{
    /// <summary>
    /// 上下文的辅助对象
    /// </summary>
    public interface IGlobalContextHelper
    {
        /// <summary>
        /// 生成一个用户对象
        /// </summary>
        /// <param name="type">0 无要求即只构造，1 构造一个匿名用户，2 构造一个系统用户</param>
        /// <returns></returns>
        ILoginUserInfo CreateUserObject(int type);


        /// <summary>
        /// 生成一个组织对象
        /// </summary>
        /// <returns></returns>
        IOrganizational CreateOrganizationalObject();
    }
}