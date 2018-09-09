using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Agebull.Common.Rpc;
using Gboxt.Common.DataModel;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// API配置过滤器
    /// </summary>
    public class ApiAccessOptionFilterAttribute : ActionFilterAttribute
    {
		private readonly ApiAccessOption _option;

		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="option"></param>
		public ApiAccessOptionFilterAttribute(ApiAccessOption option)
		{
			_option = option;
		}

		/// <summary>
		/// 动作运行前
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(HttpActionContext filterContext)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			if (_option == ApiAccessOption.None && _option < ApiAccessOption.Anymouse)
			{
				throw new HttpResponseException(filterContext.Request.ToResponse(ApiResult.Error(-5)));
			}
			if (!_option.HasFlag(ApiAccessOption.Public))
			{
				if (!_option.HasFlag(ApiAccessOption.Internal))
				{
					throw new HttpResponseException(filterContext.Request.ToResponse(ApiResult.Error(-5)));
				}
				if (GlobalContext.RequestInfo == null|| GlobalContext.RequestInfo.ServiceKey == GlobalContext.ServiceKey)
                {
					throw new HttpResponseException(filterContext.Request.ToResponse(ApiResult.Error(-5)));
				}
			}
			if (!_option.HasFlag(ApiAccessOption.Anymouse) && _option.HasFlag(ApiAccessOption.Customer) && (GlobalContext.Customer == null || GlobalContext.Customer.UserId <= 0))
			{
				throw new HttpResponseException(filterContext.Request.ToResponse(ApiResult.Error(-5)));
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
