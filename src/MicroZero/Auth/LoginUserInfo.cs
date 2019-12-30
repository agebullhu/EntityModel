using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        /// 数据有效期
        /// </summary>
        [JsonProperty("v", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Valid { get; set; }

        /// <summary>
        ///     全局用户数字标识
        /// </summary>
        [JsonProperty("gUserId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public long GlobalUserId { get; set; }

        /// <summary>
        ///     用户数字标识
        /// </summary>
        [JsonProperty("userId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public long UserId { get; set; }

        /// <summary>
        ///     用户开放标识
        /// </summary>
        [JsonProperty("openId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string OpenId { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        [JsonProperty("nickName",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string NickName { get; set; }

        /// <summary>
        ///     登录者的账号
        /// </summary>
        [JsonProperty("account",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [JsonProperty("state",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public UserStateType State { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        /// <remarks>
        ///     头像
        /// </remarks>
        [JsonProperty("avatarUrl",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     当前用户登录方式
        /// </summary>
        [JsonProperty("loginType",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public int LoginType { get; set; }

        /// <summary>
        ///     登录者的手机号
        /// </summary>
        [JsonProperty("phone",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        /// <summary>
        ///     登录设备的标识
        /// </summary>
        [JsonProperty("deviceId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        /// <summary>
        ///     身份令牌
        /// </summary>
        [JsonProperty("accessToken",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }


        /// <summary>
        ///     公司标识
        /// </summary>
        [JsonProperty("orgId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public long OrganizationId { get; set; }

        /// <summary>
        ///     职位
        /// </summary>
        /// <remarks>
        ///     职位
        /// </remarks>
        [JsonProperty("pos",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }

        /// <summary>
        ///     机构
        /// </summary>
        /// <remarks>
        ///     机构
        /// </remarks>
        [JsonProperty("org",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Organization { get; set; }

        /// <summary>
        ///     主要角色名称
        /// </summary>
        [JsonProperty("role",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        /// <summary>
        ///     主要角色标识
        /// </summary>
        [JsonProperty("roleId",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public long RoleId { get; set; }
        
        /// <summary>
        ///     用户角色标识
        /// </summary>
        [JsonProperty("allRoleIds",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public List<long> AllRoleIds { get; set; }

        /// <summary>
        ///     登录设备的操作系统
        /// </summary>
        [JsonProperty("os",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string Os { get; set; }

        /// <summary>
        ///     当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        [JsonProperty("app",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public string App { get; set; }

        /// <summary>
        ///     组织标识
        /// </summary>
        [JsonProperty("gid",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore)]
        public long GroupId { get; set; }

        /// <summary>
        ///     扩展信息
        /// </summary>
        [JsonProperty("ext",DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,NullValueHandling = NullValueHandling.Ignore)]
        public string ExtendValue { get; set; }

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
        public static LoginUserInfo CreateAnymouse(string did="*", string app = "*", string os = "*")
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
        ///     匿名用户(慎用)
        /// </summary>
        internal static LoginUserInfo Anymouse => new LoginUserInfo
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
        ///     系统用户(慎用)
        /// </summary>
        public static LoginUserInfo System => new LoginUserInfo
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