using Agebull.Common.OAuth;
using Gboxt.Common.DataModel;
using System.Configuration;

namespace Agebull.Common.WebApi.Auth
{
    /// <summary>
    /// 身份验证服务API(内网访问实现)
    /// </summary>
    public class BearValidaterProxy : IBearValidater
    {
        /// <summary>
        /// 调用器
        /// </summary>
        public WebApiCaller Caller = new WebApiCaller
        {
            Host = ConfigurationManager.AppSettings["AuthUrl"]
        };

        /// <summary>
        /// 检查调用的ServiceKey（来自内部调用）
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        ApiResult IBearValidater.ValidateServiceKey(string token)
        {
            return Caller.Post<LoginUserInfo>("v1/verify/sk", $"Token={token}");
        }

        /// <summary>
        /// 检查AT(来自登录用户)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> IBearValidater.VerifyAccessToken(string token)
        {
            return Caller.Post<LoginUserInfo>("v1/verify/at", $"Token={token}");
        }

        /// <summary>
        /// 检查设备标识（来自未登录用户）
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> IBearValidater.ValidateDeviceId(string token)
        {
            return Caller.Post<LoginUserInfo>("v1/verify/did", $"Token={token}");
        }

        /// <summary>
        /// 检查设备标识（来自未登录用户）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> IBearValidater.GetUserProfile(long uid)
        {
            return Caller.Post<LoginUserInfo>("v1/oauth/user", $"Value={uid}");
        }

        /// <summary>
        ///     取得用户信息
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>用户信息</returns>
        ApiResult<LoginUserInfo> IBearValidater.GetLoginUser(string token)
        {
            return null;
        }
    }
}
