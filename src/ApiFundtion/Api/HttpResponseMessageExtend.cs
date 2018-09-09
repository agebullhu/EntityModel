using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Gboxt.Common.DataModel;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// 基于HTTP协议的返回消息对象扩展
    /// </summary>
    public static class HttpResponseMessageExtend
    {
        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <param name="statusCode">HTTP状态码</param>
        public static ApiResponseMessage ToResponse(this HttpRequestMessage request, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponseMessage(statusCode)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <param name="statusCode">HTTP状态码</param>
        public static ApiResponseMessage<ApiValueResult<T>> ToResponse<T>(this HttpRequestMessage request, ApiValueResult<T> result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiValueResult<T>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiValueResult<T>>(result, statusCode)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>HttpResponseMessage对象</returns>
        public static ApiResponseMessage ToResponse(this HttpRequestMessage request, ApiResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponseMessage(statusCode, result)
            {
                RequestMessage = request
            };
        }


        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>HttpResponseMessage对象</returns>
        public static ApiResponseMessage ToResponse(this HttpRequestMessage request, ApiValueResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponseMessage(statusCode, result)
            {
                RequestMessage = request
            };
        }
        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>HttpResponseMessage对象</returns>
        public static ApiResponseMessage ToResponse(this HttpRequestMessage request, IApiResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponseMessage(statusCode, result)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>HttpResponseMessage对象</returns>
        public static ApiResponseMessage ToResponse(this HttpRequestMessage request, string result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponseMessage(result, statusCode)
            {
                RequestMessage = request
            };
        }
        /// <summary>
        /// 生成一个标准返回对象(没有返回值）
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="result">返回内容</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>HttpResponseMessage对象</returns>
        public static ApiResponseMessage<ApiResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, TResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiResult<TResult>>(ApiResult<TResult>.Succees(result), statusCode)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象(返回一般数据)
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <returns></returns>
        public static ApiResponseMessage<ApiResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, ApiResult<TResult> result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiResult<TResult>>(statusCode)
            {
                RequestMessage = request,
                Content = new StringContent(JsonConvert.SerializeObject(result))
            };
        }

        /// <summary>
        /// 生成一个标准返回对象(返回一般数据)
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <returns></returns>
        public static ApiResponseMessage<ApiResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, IApiResult<TResult> result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiResult<TResult>>(statusCode)
            {
                RequestMessage = request,
                Content = new StringContent(JsonConvert.SerializeObject(result))
            };
        }

        /// <summary>
        /// 生成一个标准返回对象（返回数组）
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <returns></returns>
        public static ApiResponseMessage<ApiArrayResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, List<TResult> result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiArrayResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiArrayResult<TResult>>(new ApiArrayResult<TResult>
            {
                ResultData = result
            }, statusCode)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象（返回数组）
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <returns></returns>
        public static ApiResponseMessage<ApiArrayResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, ApiArrayResult<TResult> result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiArrayResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiArrayResult<TResult>>(result, statusCode)
            {
                RequestMessage = request
            };
        }

        /// <summary>
        /// 生成一个标准返回对象（返回分页）
        /// </summary>
        /// <typeparam name="TResult">返回类型，基于ApiResult</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <param name="result">返回内容</param>
        /// <returns>HttpResponseMessage对象</returns>
        /// <returns></returns>
        public static ApiResponseMessage<ApiPageResult<TResult>> ToResponse<TResult>(this HttpRequestMessage request, ApiPageResult<TResult> result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            if (result == null)
            {
                return new ApiResponseMessage<ApiPageResult<TResult>>(statusCode)
                {
                    RequestMessage = request
                };
            }
            return new ApiResponseMessage<ApiPageResult<TResult>>(result, statusCode)
            {
                RequestMessage = request
            };
        }
    }
}
