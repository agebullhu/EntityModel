using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     当前登录的用户信息
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginUserInfo : ILoginUserInfo
    {
        /// <summary>
        ///     用户数字标识
        /// </summary>
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public long UserId { get; set; }

        /// <summary>
        ///     用户开放标识
        /// </summary>
        [JsonProperty("openId", NullValueHandling = NullValueHandling.Ignore)]
        public string OpenId { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        [JsonProperty("nickName", NullValueHandling = NullValueHandling.Ignore)]
        public string NickName { get; set; }

        /// <summary>
        ///     登录者的账号
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public UserStateType State { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        /// <remarks>
        ///     头像
        /// </remarks>
        [JsonProperty("avatarUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     当前用户登录方式
        /// </summary>
        [JsonProperty("loginType", NullValueHandling = NullValueHandling.Ignore)]
        public int LoginType { get; set; }

        /// <summary>
        ///     登录者的手机号
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        /// <summary>
        ///     登录设备的标识
        /// </summary>
        [JsonProperty("deviceId", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        /// <summary>
        ///     身份令牌
        /// </summary>
        [JsonProperty("accessToken", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }


        /// <summary>
        ///     公司标识
        /// </summary>
        [JsonProperty("orgId", NullValueHandling = NullValueHandling.Ignore)]
        public long OrganizationId { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        [JsonProperty("post", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        [JsonProperty("org", NullValueHandling = NullValueHandling.Ignore)]
        public string Organization { get; set; }

        /// <summary>
        ///     角色
        /// </summary>
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        /// <summary>
        ///     用户角色标识
        /// </summary>
        [JsonProperty("roleId", NullValueHandling = NullValueHandling.Ignore)]
        public long RoleId { get; set; }


        /// <summary>
        ///     登录设备的操作系统
        /// </summary>
        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public string Os { get; set; }

        /// <summary>
        ///     当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        [JsonProperty("app", NullValueHandling = NullValueHandling.Ignore)]
        public string App { get; set; }

        #region 预定义

        /// <summary>
        /// 匿名用户ID
        /// </summary>
        public const long AnymouseUserId = -2;

        /// <summary>
        /// 系统用户ID
        /// </summary>
        public const long SystemUserId = -1;

        /// <summary>
        ///     匿名用户
        /// </summary>
        public static LoginUserInfo CreateAnymouse(string did, string app, string os)
        {
            return new LoginUserInfo
            {
                UserId = AnymouseUserId,
                Account = "anymouse",
                App = app,
                Os = os,
                DeviceId = did,
                State = UserStateType.None
            };
        }
        /// <summary>
        ///     匿名用户
        /// </summary>
        public static LoginUserInfo Anymouse { get; } = new LoginUserInfo
        {
            UserId = AnymouseUserId,
            Account = "anymouse",
            DeviceId = "%anymouse",
            App = "*",
            Os = "*",
            LoginType = 0,
            State = UserStateType.None
        };

        /// <summary>
        ///     系统用户
        /// </summary>
        public static LoginUserInfo System { get; } = new LoginUserInfo
        {
            UserId = SystemUserId,
            Account = "system",
            DeviceId = "%system",
            App = "sys",
            Os = "sys",
            LoginType = 0,
            State = UserStateType.Enable
        };

        #endregion
    }
}