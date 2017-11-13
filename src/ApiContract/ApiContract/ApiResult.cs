using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// 表示API返回数据
    /// </summary>
    public interface IApiResultData
    {
    }

    /// <summary>
    /// API返回基类
    /// </summary>
    public interface IApiResult
    {
        /// <summary>
        /// 成功或失败标记
        /// </summary>
        bool Result { get; set; }

        /// <summary>
        /// API执行状态（为空表示状态正常）
        /// </summary>
        IApiStatusResult Status { get; }
    }
    /// <summary>
    /// API返回基类
    /// </summary>
    public interface IApiResult<TData> : IApiResult
        where TData : IApiResultData
    {
        /// <summary>
        /// 返回值
        /// </summary>
        TData ResultData { get; set; }
    }

    /// <summary>
    /// API状态返回（一般在出错时发生）
    /// </summary>
    public interface IApiStatusResult
    {
        /// <summary>
        /// 错误码（系统定义）
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// 对应HTTP错误码（参考）
        /// </summary>
        string HttpCode { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 内部提示信息
        /// </summary>
        string InnerMessage { get; set; }
    }
    /// <summary>
    /// API状态返回接口实现
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiStatsResult : IApiStatusResult
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public ApiStatsResult()
        {

        }
        /// <summary>
        /// 默认构造
        /// </summary>
        public ApiStatsResult(int code, string messgae)
        {
            ErrorCode = code;
            Message = messgae;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 对应HTTP错误码（参考）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HttpCode { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        /// <summary>
        /// 内部提示信息
        /// </summary>
#if DEBUG
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
#else
        [JsonIgnore]
#endif
        public string InnerMessage { get; set; }
    }

    /// <summary>
    /// API返回基类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// 成功或失败标记
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Result { get; set; }

        /// <summary>
        /// API执行状态（为空表示状态正常）
        /// </summary>
        [JsonIgnore]
        IApiStatusResult IApiResult.Status => Status;

        /// <summary>
        /// API执行状态（为空表示状态正常）
        /// </summary>
        [JsonProperty("ResponseStatus", NullValueHandling = NullValueHandling.Ignore)]
        public ApiStatsResult Status { get; set; }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiResult Error(int errCode)
        {
            return new ApiResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = ErrorCode.GetMessage(errCode)
                }
            };
        }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult Error(int errCode, string message)
        {
            return new ApiResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message
                }
            };
        }
        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <returns></returns>
        public static ApiResult ErrorResult(int errCode, string message, string innerMessage)
        {
            return new ApiResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message,
                    InnerMessage = innerMessage
                }
            };
        }
        /// <summary>
        /// 生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult Succees()
        {
            return new ApiResult
            {
                Result = true
            };
        }
    }

    /// <summary>
    /// API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiValueResult<TData> : ApiResult
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData ResultData { get; set; }

        /// <summary>
        /// 生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiValueResult<TData> Succees(TData data)
        {
            return new ApiValueResult<TData>
            {
                Result = true,
                ResultData = data
            };
        }
        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = ErrorCode.GetMessage(errCode)
                }
            };
        }
        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode, string message)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message
                }
            };
        }

        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode, string message, string innerMessage)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message,
                    InnerMessage = innerMessage
                }
            };
        }
    }

    /// <summary>
    /// API返回数据泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult<TData> : ApiResult, IApiResult<TData>
        where TData : IApiResultData
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData ResultData { get; set; }

        /// <summary>
        /// 生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Succees(TData data)
        {
            return new ApiResult<TData>
            {
                Result = true,
                ResultData = data
            };
        }
        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns></returns>
        public static ApiResult<TData> ErrorResult(int errCode)
        {
            return new ApiResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = ErrorCode.GetMessage(errCode)
                }
            };
        }
        /// <summary>
        /// 生成一个包含错误码的标准返回
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<TData> ErrorResult(int errCode, string message)
        {
            return new ApiResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message
                }
            };
        }
    }

    /// <summary>
    /// 表示API返回的列表
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ApiList<TData> : List<TData>, IApiResultData
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="list"></param>
        public ApiList(IList<TData> list)
        {
            if (list == null)
                return;
            AddRange(list);
        }
    }

    /// <summary>
    /// API返回数组泛型类
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiArrayResult<TData> : IApiResult<ApiList<TData>>
    {
        /// <summary>
        /// 成功或失败标记
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Result { get; set; }

        /// <summary>
        /// API执行状态（为空表示状态正常）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IApiStatusResult Status { get; set; }

        private ApiList<TData> _datas;

        /// <summary>
        /// 返回值
        /// </summary>
        [JsonIgnore]
        ApiList<TData> IApiResult<ApiList<TData>>.ResultData
        {
            get
            {
                if (_datas != null)
                    return _datas;
                _datas = ResultData as ApiList<TData>;
                if (_datas != null)
                    return _datas;
                return _datas = new ApiList<TData>(ResultData);
            }
            set { ResultData = value; }
        }

        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TData> ResultData { get; set; }

    }
    /// <summary>
    /// API返回数组泛型类
    /// </summary>
    public class ApiPageResult : ApiResult<ApiPage>
    {
    }

    /// <summary>
    /// API返回数组泛型类
    /// </summary>
    public class ApiPageResult<TData> : ApiResult<ApiPageData<TData>>
    {
    }

    /// <summary>
    /// API返回分页信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPage : IApiResultData
    {
        /// <summary>
        /// 当前页号（从1开始）
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页行数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageCount { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int RowCount { get; set; }
    }

    /// <summary>
    /// API返回分布页数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPageData<TData> : ApiPage
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [JsonProperty("Rows", NullValueHandling = NullValueHandling.Ignore)]
        public List<TData> Rows { get; set; }
    }
}
