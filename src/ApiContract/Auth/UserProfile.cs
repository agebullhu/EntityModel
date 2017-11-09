using Gboxt.Common.DataModel;
using Yizuan.Service.Api;

namespace GoodLin.OAuth.Api
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserProfile: IUserProfile
    {
        /// <summary>
        /// 用户数字标识
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户所在设备标识
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 加入系统时间
        /// </summary>
        public long JoinTime { get; set; }

        /// <summary>
        /// 当前服务器时间
        /// </summary>
        public long ApiServerTime { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public DataStateType State { get; set; }

        /// <summary>
        /// 请求者操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 请求者操作系统
        /// </summary>
        public string Browser { get; set; }
    }
}