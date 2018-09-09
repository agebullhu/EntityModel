using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Gboxt.Common.DataModel;

namespace Agebull.Common.WebApi
{
	/// <summary>
	/// API返回专用的ResponseMessage
	/// </summary>
	public class ApiResponseMessage : HttpResponseMessage
	{
		/// <summary>
		/// 默认的成功消息
		/// </summary>
		protected static readonly string SuccessMessage;

		static ApiResponseMessage()
		{
			SuccessMessage = JsonConvert.SerializeObject(ApiResult.Succees());
		}

		/// <summary>
		/// 默认构造（状态码为
		/// </summary>
		public ApiResponseMessage()
			: base(HttpStatusCode.OK)
		{
			Content = new StringContent(SuccessMessage);
		}

		/// <summary>
		/// 状态构造
		/// </summary>
		/// <param name="statusCode">状态</param>
		public ApiResponseMessage(HttpStatusCode statusCode)
			: base(statusCode)
		{
			Content = new StringContent(SuccessMessage);
		}

		/// <summary>
		/// 状态构造
		/// </summary>
		/// <param name="statusCode">状态</param>
		/// <param name="result"></param>
		public ApiResponseMessage(HttpStatusCode statusCode, ApiResult result)
			: base(statusCode)
		{
			Content = ((result != null) ? new StringContent(JsonConvert.SerializeObject(result)) : new StringContent(SuccessMessage));
		}

		/// <summary>
		/// 状态构造
		/// </summary>
		/// <param name="statusCode">状态</param>
		/// <param name="result"></param>
		public ApiResponseMessage(HttpStatusCode statusCode, IApiResult result)
			: base(statusCode)
		{
			Content = ((result != null) ? new StringContent(JsonConvert.SerializeObject(result)) : new StringContent(SuccessMessage));
	    }
	    /// <summary>
	    /// 状态构造
	    /// </summary>
	    /// <param name="result">数据</param>
	    /// <param name="statusCode">状态</param>
	    public ApiResponseMessage(string result, HttpStatusCode statusCode = HttpStatusCode.OK)
	        : base(statusCode)
	    {
	        Content = new StringContent(result);
	    }
    }
	/// <summary>
	/// API返回专用的ResponseMessage
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class ApiResponseMessage<TResult> : ApiResponseMessage where TResult : IApiResult
	{
		/// <summary>
		/// 默认构造（状态码为
		/// </summary>
		public ApiResponseMessage()
			: base(HttpStatusCode.OK)
		{
			Content = new StringContent(SuccessMessage);
		}

		/// <summary>
		/// 状态构造
		/// </summary>
		/// <param name="statusCode">状态</param>
		public ApiResponseMessage(HttpStatusCode statusCode)
			: base(statusCode)
		{
			Content = new StringContent(SuccessMessage);
		}

	    /// <summary>
	    /// 状态构造
	    /// </summary>
	    /// <param name="result">数据</param>
	    /// <param name="statusCode">状态</param>
	    public ApiResponseMessage(TResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
	        : base(statusCode)
	    {
	        Content = ((result != null) ? new StringContent(JsonConvert.SerializeObject(result)) : new StringContent(SuccessMessage));
	    }
    }
}
