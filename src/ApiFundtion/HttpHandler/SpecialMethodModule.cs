using System;
using System.Web;

namespace Agebull.Common.WebApi
{
    public class SpecialMethodModule : IHttpModule
    {
        public SpecialMethodModule() { }
        public void Init(HttpApplication app)
        {
            app.BeginRequest += new EventHandler(this.BeginRequest);
        }
        public void Dispose() { }
        public void BeginRequest(object resource, EventArgs e)
        {
            HttpApplication app = resource as HttpApplication;
            HttpContext context = app.Context;
            if (context.Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                context.Response.End();
            }
        }
    }
}