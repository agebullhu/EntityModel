using Agebull.Common.OAuth;

namespace Agebull.Common.Context
{
    /// <inheritdoc />
    /// <summary>
    /// 默认上下文的辅助对象
    /// </summary>
    public class DefaultGlobalContextHelper : IGlobalContextHelper
    {
        /// <inheritdoc />
        /// <summary>
        /// 生成一个用户对象
        /// </summary>
        /// <param name="type">0 无要求即只构造，1 构造一个匿名用户，2 构造一个系统用户</param>
        /// <returns></returns>
        ILoginUserInfo IGlobalContextHelper.CreateUserObject(int type)
        {
            switch (type)
            { 
                case 1:
                    return new LoginUserInfo
                    {
                        UserId = LoginUserInfo.AnymouseUserId,
                        Account = "anymouse",
                        DeviceId = "%anymouse",
                        App = "*",
                        Os = "*",
                        LoginType = 0,
                        State = UserStateType.None
                    };
                case 2:
                    return new LoginUserInfo
                    {
                        UserId = LoginUserInfo.SystemUserId,
                        Account = "system",
                        DeviceId = "%system",
                        App = "sys",
                        Os = "sys",
                        LoginType = 0,
                        State = UserStateType.Enable
                    };
                default:
                    return new LoginUserInfo();
            }
        }


        /// <inheritdoc />
        /// <summary>
        /// 生成一个组织对象
        /// </summary>
        /// <returns></returns>
        IOrganizational IGlobalContextHelper.CreateOrganizationalObject() => OrganizationalInfo.System;
    }
}