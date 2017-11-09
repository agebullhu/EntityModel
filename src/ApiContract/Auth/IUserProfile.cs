
namespace Yizuan.Service.Api
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserProfile : IApiResultData
    {
        /// <summary>
        /// 用户数字标识
        /// </summary>
        long UserId { get; set; }

        /// <summary>
        /// 用户所在设备标识
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        string NickName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        string PhoneNumber { get; set; }
        /// <summary>
        /// 加入系统时间
        /// </summary>
        long JoinTime { get; set; }
        /// <summary>
        /// 当前服务器时间
        /// </summary>
        long ApiServerTime { get; set; }

        /// <summary>
        /// 请求者操作系统
        /// </summary>
        string Os { get; set; }

        /// <summary>
        /// 请求者操作系统
        /// </summary>
        string Browser { get; set; }
    }

}