using System;
using Agebull.Common.Logging;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Gboxt.Common.DataModel;
using Agebull.Common.WebApi.Auth;
using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Rpc;

namespace Agebull.Common.WebApi
{
    /// <summary>
    /// WebApi应用辅助类
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        public static bool IsInitialize
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化，必须先调用
        /// </summary>
        public static void InitializeBase(bool bear)
        {
            if (IsInitialize)
                return;

            IocHelper.Update();
            IocHelper.AddScoped<GlobalContext, BusinessContext>();
            IocHelper.AddSingleton<IZeroPublisher, ZeroNetBridge>();
            IocHelper.AddSingleton<IGlobalContext, BusinessContext>(p => BusinessContext.Context);
            IocHelper.Update(); 

            Environment.CurrentDirectory = ConfigurationManager.BasePath = HttpContext.Current.Server.MapPath("~");

            LogRecorder.Initialize();
            LogRecorder.SystemLog("启动");
            LogRecorder.GetRequestIdFunc = () => RandomOperate.Generate(8);
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes(new CustomDirectRouteProvider());
            HttpHandler.Handlers.Add(new CorsHandler());
            HttpHandler.Handlers.Add(new HttpIoLogHandler());
            if (bear)
                HttpHandler.Handlers.Add(new BearerHandler());
            IsInitialize = true;
        }

        /// <summary>
        /// 初始化，必须先调用
        /// </summary>
        public static void Initialize()
        {
            InitializeBase(true);
        }

        /// <summary>
        /// 初始化，必须先调用
        /// </summary>
        public static void InitializeNoBearer()
        {
            InitializeBase(false);
        }

        /// <summary>
        /// 注册系统处理器
        /// </summary>
        /// <param name="handler"></param>
        public static void RegistSystemHandler(IHttpSystemHandler handler)
        {
            if (!IsInitialize)
            {
                Initialize();
            }
            if (handler != null && !HttpHandler.Handlers.Contains(handler))
            {
                HttpHandler.Handlers.Add(handler);
            }
        }

        /// <summary>
        /// Application_Start时调用
        /// </summary>
        public static void OnApplicationStart()
        {
            RegistFormatter();
            global::System.Web.Mvc.AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(RegistFilter);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpHandler());
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        /// <summary>
        /// 注册过滤器
        /// </summary>
        public static void RegistFilter(HttpConfiguration config)
        {
            global::System.Web.Mvc.GlobalFilters.Filters.Add(new global::System.Web.Mvc.HandleErrorAttribute());
        }

        /// <summary>
        /// 注册格式化器
        /// </summary>
        public static void RegistFormatter()
        {
            GlobalConfiguration.Configuration.Formatters.XmlFormatter
                .SupportedMediaTypes
                .Clear();
            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Formatting = (0);
            jsonFormatter.SerializerSettings.ContractResolver = (new CamelCasePropertyNamesContractResolver());
            jsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            jsonFormatter.SerializerSettings.Culture = (CultureInfo.GetCultureInfo("zh-cn"));
            jsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
        }
    }
}
