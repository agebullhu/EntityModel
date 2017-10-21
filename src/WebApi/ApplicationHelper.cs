using System.Globalization;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Web.Http.Cors;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// WebApi应用辅助类
    /// </summary>
    public static class ApplicationHelper
    {

        /// <summary>
        /// Application_Start时调用
        /// </summary>
        public static void OnApplicationStart()
        {
            AreaRegistration.RegisterAllAreas();
            RegistHandler();
            //RegistFormatter();
            GlobalConfiguration.Configure(Regist);
            
        }

        /// <summary>
        /// Application_Start时调用(内部使用)
        /// </summary>
        public static void OnApplicationStartInner()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpIoLogHandler());
            RegistFormatter();
            GlobalConfiguration.Configure(Regist);
        }

        /// <summary>
        /// 注册
        /// </summary>
        public static void Regist(HttpConfiguration config)
        {
            RegistFilter(config);
            RegistCors(config);
        }
        /// <summary>
        /// 跨域支持
        /// </summary>
        /// <param name="config"></param>
        public static void RegistCors(HttpConfiguration config)
        {
            var allowOrigins = ConfigurationManager.AppSettings["cors_allowOrigins"] ?? "*";
            var allowHeaders = ConfigurationManager.AppSettings["cors_allowHeaders"] ?? "*";
            var allowMethods = ConfigurationManager.AppSettings["cors_allowMethods"] ?? "*";
            var globalCors = new EnableCorsAttribute(allowOrigins, allowHeaders, allowMethods);
            config.EnableCors(globalCors);
        }
        /// <summary>
        /// 注册过滤器
        /// </summary>
        public static void RegistFilter(HttpConfiguration config)
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
        /// <summary>
        /// 注册格式化器
        /// </summary>
        public static void RegistFormatter()
        {
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json  
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            json.SerializerSettings.Culture = CultureInfo.GetCultureInfo("zh-cn");
            json.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
        }

        /// <summary>
        /// 基础MessageHandler注册
        /// </summary>
        public static void RegistHandler()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BearerHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpIoLogHandler());
        }
    }
}
