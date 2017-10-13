namespace GoodLin.Common.Api
{
    /// <summary>
    /// 用户身份校验
    /// </summary>
    public interface IBearValidater
    {
        /// <summary>
        /// 检查调用的ServiceKey（来自内部调用）
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        IApiResult ValidateServiceKey(string token);
        /// <summary>
        /// 检查AT(来自登录用户)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IApiResult<IUserProfile> VerifyAccessToken(string token);
        /// <summary>
        /// 检查设备标识（来自未登录用户）
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        IApiResult<IUserProfile> ValidateDeviceId(string token);

        /// <summary>
        /// 检查设备标识（来自未登录用户）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        IApiResult<IUserProfile> GetUserProfile(long uid);
    }
}