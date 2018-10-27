using Agebull.Common.Rpc;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     API返回基类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ApiResult()
        {
            RequestId = GlobalContext.CurrentNoLazy?.Request?.RequestId;
            OperatorId = GlobalContext.CurrentNoLazy?.Request?.LocalGlobalId;
        }

        /// <summary>
        ///     API请求标识
        /// </summary>
        [JsonProperty("rid")]
        public string RequestId { get; set; }


        /// <summary>
        ///     API操作标识
        /// </summary>
        [JsonProperty("oid")]
        public string OperatorId { get; set; }


        /// <summary>
        ///     执行状态
        /// </summary>
        /// <remarks>success为true时,可能是空值</remarks>
        [JsonProperty("status")]
        public OperatorStatus Status { get; set; }

        /// <summary>
        ///     成功或失败标记
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        
        [JsonIgnore]
        IOperatorStatus IApiResult.Status
        {
            get => Status;
            set => Status = value as OperatorStatus;
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode)
        {
            return new ApiResult
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode, string message)
        {
            return new ApiResult
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode, string message, string innerMessage)
        {
            return new ApiResult
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage
                }
            };
        }
        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode, string message, string innerMessage, string guide, string describe)
        {
            return new ApiResult
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode, string message, string innerMessage, string point, string guide, string describe)
        {
            return new ApiResult
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    Point = point,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Succees<TData>(TData data)
        {
            return new ApiResult<TData>
            {
                Success = true,
                ResultData = data
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>(int errCode)
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>(int errCode, string message)
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>(int errCode, string message, string innerMessage)
        {
            return new ApiResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>(int errCode, string message, string innerMessage, string guide, string describe)
        {
            return new ApiResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message">错误消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>(int errCode, string message, string innerMessage, string point, string guide, string describe)
        {
            return new ApiResult<TData>
            {
                Success = errCode == ErrorCode.Success,
                Status = new OperatorStatus
                {
                    ErrorCode = errCode,
                    ClientMessage = message ?? ErrorCode.GetMessage(errCode),
                    InnerMessage = innerMessage,
                    Point = point,
                    GuideCode = guide,
                    Describe = describe
                }
            };
        }
        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult Error()
        {
            return new ApiResult
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Error<TData>()
        {
            return new ApiResult<TData>
            {
                Success = false,
                Status = GlobalContext.Current.LastStatus
            };
        }
        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult Succees()
        {
            return new ApiResult
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Succees<TData>()
        {
            return new ApiResult<TData>
            {
                Success = true,
                Status = GlobalContext.Current.LastStatus
            };
        }

        #region 预定义

        /// <summary>
        ///     成功
        /// </summary>
        /// <remarks>成功</remarks>
        public static ApiResult Ok => Succees();

        /// <summary>
        ///     页面不存在
        /// </summary>
        public static ApiResult NoFind => Error(ErrorCode.NoFind, "*页面不存在*");

        /// <summary>
        ///     不支持的操作
        /// </summary>
        public static ApiResult NotSupport => Error(ErrorCode.NoFind, "*页面不存在*");

        /// <summary>
        ///     参数错误字符串
        /// </summary>
        public static ApiResult ArgumentError => Error(ErrorCode.LogicalError, "参数错误");

        /// <summary>
        ///     逻辑错误字符串
        /// </summary>
        public static ApiResult LogicalError => Error(ErrorCode.LogicalError, "逻辑错误");

        /// <summary>
        ///     拒绝访问
        /// </summary>
        public static ApiResult DenyAccess => Error(ErrorCode.DenyAccess);

        /// <summary>
        ///     服务器无返回值的字符串
        /// </summary>
        public static ApiResult RemoteEmptyError => Error(ErrorCode.RemoteError, "*服务器无返回值*");

        /// <summary>
        ///     服务器访问异常
        /// </summary>
        public static ApiResult NetworkError => Error(ErrorCode.NetworkError);

        /// <summary>
        ///     本地错误
        /// </summary>
        public static ApiResult LocalError => Error(ErrorCode.LocalError);

        /// <summary>
        ///     本地访问异常
        /// </summary>
        public static ApiResult LocalException => Error(ErrorCode.LocalException);

        /// <summary>
        ///     系统未就绪
        /// </summary>
        public static ApiResult NoReady => Error(ErrorCode.NoReady);

        /// <summary>
        ///     暂停服务
        /// </summary>
        public static ApiResult Pause => Error(ErrorCode.NoReady, "暂停服务");

        /// <summary>
        ///     未知错误
        /// </summary>
        public static ApiResult UnknowError => Error(ErrorCode.LocalError, "未知错误");

        /// <summary>
        ///     网络超时
        /// </summary>
        /// <remarks>调用其它Api时时抛出未处理异常</remarks>
        public static ApiResult TimeOut => Error(ErrorCode.NetworkError, "网络超时");

        /// <summary>
        ///     内部错误
        /// </summary>
        /// <remarks>执行方法时抛出未处理异常</remarks>
        public static ApiResult InnerError => Error(ErrorCode.LocalError, "内部错误");

        /// <summary>
        ///     服务不可用
        /// </summary>
        public static ApiResult Unavailable => Error(ErrorCode.Unavailable, "服务不可用");


        #endregion

        #region JSON

        /// <summary>
        ///     成功的Json字符串
        /// </summary>
        /// <remarks>成功</remarks>
        public static string SucceesJson => JsonConvert.SerializeObject(Ok);

        /// <summary>
        ///     页面不存在的Json字符串
        /// </summary>
        public static string NoFindJson => JsonConvert.SerializeObject(NoFind);

        /// <summary>
        ///     系统不支持的Json字符串
        /// </summary>
        public static string NotSupportJson => JsonConvert.SerializeObject(NotSupport);

        /// <summary>
        ///     参数错误字符串
        /// </summary>
        public static string ArgumentErrorJson =>JsonConvert.SerializeObject(ArgumentError);

        /// <summary>
        ///     逻辑错误字符串
        /// </summary>
        public static string LogicalErrorJson =>JsonConvert.SerializeObject(LogicalError);

        /// <summary>
        ///     拒绝访问的Json字符串
        /// </summary>
        public static string DenyAccessJson => JsonConvert.SerializeObject(DenyAccess);

        /// <summary>
        ///     服务器无返回值的字符串
        /// </summary>
        public static string RemoteEmptyErrorJson =>JsonConvert.SerializeObject(RemoteEmptyError);

        /// <summary>
        ///     服务器访问异常
        /// </summary>
        public static string NetworkErrorJson => JsonConvert.SerializeObject(NetworkError);

        /// <summary>
        ///     本地错误
        /// </summary>
        public static string LocalErrorJson => JsonConvert.SerializeObject(LocalError);

        /// <summary>
        ///     本地访问异常的Json字符串
        /// </summary>
        public static string LocalExceptionJson => JsonConvert.SerializeObject(LocalException);

        /// <summary>
        ///     系统未就绪的Json字符串
        /// </summary>
        public static string NoReadyJson => JsonConvert.SerializeObject(NoReady);

        /// <summary>
        ///     暂停服务的Json字符串
        /// </summary>
        public static string PauseJson => JsonConvert.SerializeObject(Pause);

        /// <summary>
        ///     未知错误的Json字符串
        /// </summary>
        public static string UnknowErrorJson => JsonConvert.SerializeObject(UnknowError);

        /// <summary>
        ///     网络超时的Json字符串
        /// </summary>
        /// <remarks>调用其它Api时时抛出未处理异常</remarks>
        public static string TimeOutJson => JsonConvert.SerializeObject(TimeOut);

        /// <summary>
        ///     内部错误的Json字符串
        /// </summary>
        /// <remarks>执行方法时抛出未处理异常</remarks>
        public static string InnerErrorJson => JsonConvert.SerializeObject(InnerError);

        /// <summary>
        ///     服务不可用的Json字符串
        /// </summary>
        public static string UnavailableJson => JsonConvert.SerializeObject(Unavailable);


        #endregion

    }


    /// <summary>
    ///     API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult<TData> : ApiResult, IApiResult<TData>
    {
        /// <summary>
        ///     返回值
        /// </summary>
        [JsonProperty("data")]
        public TData ResultData { get; set; }

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Succees(TData data)
        {
            return new ApiResult<TData>
            {
                Success = true,
                ResultData = data
            };
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiResult<TData> ErrorResult(int errCode)
        {
            return Error<TData>(errCode);
        }

        /// <summary>
        ///     生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<TData> ErrorResult(int errCode, string message)
        {
            return Error<TData>(errCode, message);
        }
    }
}