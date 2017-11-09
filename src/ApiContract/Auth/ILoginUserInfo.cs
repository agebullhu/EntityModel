using Gboxt.Common.DataModel;

namespace Yizuan.Service.Api.OAuth
{
    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public interface IUser : IStateData
    {
        /// <summary>
        /// 用户数字标识
        /// </summary>
        long UserId { get; set; }
    }

    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public interface IPerson : IUser
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        /// <remarks>
        /// 头像
        /// </remarks>
        string AvatarUrl
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public interface ILoginUserInfo : IPerson, IApiResultData
    {
        /// <summary>
        /// 当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        string LoginSystem { get; set; }

        /// <summary>
        /// 当前用户登录方式
        /// </summary>
        int LoginType { get; set; }

        /// <summary>
        /// 登录者的手机号
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// 登录者的账号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 登录设备的标识
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// 登录设备的操作系统
        /// </summary>
        string Os { get; set; }

        /// <summary>
        /// 登录设备的浏览器
        /// </summary>
        string Browser { get; set; }
    }

    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public class LoginUserInfo : ILoginUserInfo
    {
        /// <summary>
        /// 用户数字标识
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        /// <remarks>
        /// 头像
        /// </remarks>
        public string AvatarUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 当前用户登录到哪个系统（预先定义的系统标识）
        /// </summary>
        public string LoginSystem { get; set; }

        /// <summary>
        /// 当前用户登录方式
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// 登录者的手机号
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// 登录者的账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登录设备的标识
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 登录设备的操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 登录设备的浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        ///     数据状态
        /// </summary>
        public DataStateType DataState { get; set; }

        /// <summary>
        ///     数据是否已冻结，如果是，则为只读数据
        /// </summary>
        /// <value>bool</value>
        public bool IsFreeze { get; set; }
    }
}