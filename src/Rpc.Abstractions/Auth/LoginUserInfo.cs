using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Agebull.Common.OAuth
{
    /// <summary>
    ///     当前登录的用户信息
    /// </summary>
    [DataContract]
    [Category("上下文")]
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginUserInfo : ILoginUserInfo
    {
        /// <summary>
        ///     用户数字标识
        /// </summary>
        [JsonProperty("userId")]
        public long UserId { get; set; }

        /// <summary>
        ///     用户角色标识
        /// </summary>
        [JsonProperty("roleId")]
        public long RoleId { get; set; }

        /// <summary>
        ///     集团(医联体)标识
        /// </summary>
        [JsonProperty("groupId")]
        public long GroupId { get; set; }

        /// <summary>
        ///     公司(医院)标识
        /// </summary>
        [JsonProperty("orgId")]
        public long OrganizationId { get; set; }

        /// <summary>
        ///     部门(科室)标识
        /// </summary>
        [JsonProperty("depId")]
        public long DepartmentId { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        [JsonProperty("nickName")]
        public string NickName { get; set; }

        /// <summary>
        ///     登录者的账号
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [JsonProperty("state")]
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
        [JsonProperty("loginType")]
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
        ///     登录设备的操作系统
        /// </summary>
        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public string Os { get; set; }

        /// <summary>
        ///     当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        [JsonProperty("app", NullValueHandling = NullValueHandling.Ignore)]
        public string App { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        [JsonProperty("post", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        /// <summary>
        ///     机构级别
        /// </summary>
        [JsonProperty("dep", NullValueHandling = NullValueHandling.Ignore)]
        public int DepartmentLevel { get; set; }

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
        ///     部门
        /// </summary>
        /// <remarks>
        ///     部门
        /// </remarks>
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

        /// <summary>
        ///     集团
        /// </summary>
        [JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        #region 预定义

        /// <summary>
        ///     匿名用户
        /// </summary>
        public static LoginUserInfo CreateAnymouse(string did, string app, string os)
        {
            return new LoginUserInfo
            {
                UserId = -2,
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
            UserId = -1,
            Account = "anymouse",
            DeviceId = "%",
            App = "sys",
            Os = "sys",
            LoginType = 0,
            State = UserStateType.None
        };

        /// <summary>
        ///     系统用户
        /// </summary>
        public static LoginUserInfo System { get; } = new LoginUserInfo
        {
            UserId = -1,
            Account = "system",
            DeviceId = "***system***",
            App = "sys",
            Os = "sys",
            LoginType = 0,
            State = UserStateType.Enable
        };

        #endregion
    }
}