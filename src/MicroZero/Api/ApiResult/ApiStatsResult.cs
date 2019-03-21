using Agebull.EntityModel.Common;
using Newtonsoft.Json;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     API状态返回接口实现
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiStatusResult : OperatorStatus
    {
        /// <summary>
        ///     默认构造
        /// </summary>
        public ApiStatusResult()
        {
        }

        /// <summary>
        ///     默认构造
        /// </summary>
        public ApiStatusResult(int code, string messgae)
            : base(code, messgae)
        {
        }
    }
}