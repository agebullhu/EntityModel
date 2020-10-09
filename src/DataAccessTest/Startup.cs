using Agebull.Common;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Zeroteam.MessageMVC.EventBus.DataAccess;
using ZeroTeam.MessageMVC.Context;
using ZeroTeam.MessageMVC.Http;
using ZeroTeam.MessageMVC.Messages;

namespace ZeroTeam.MessageMVC.ConfigSync
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.BindingMessageMvc();

            services.AddTransient<ISerializeProxy, NewtonJsonSerializeProxy>();
            services.AddTransient<IJsonSerializeProxy, NewtonJsonSerializeProxy>();

            services.AddScoped(typeof(IOperatorInjection<>), typeof(OperatorInjection<>));
            services.AddScoped<EventBusDb>();

            services.AddMessageMvcHttp();
            services.AddMessageMvc();
        }


        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="_"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            app.Use(async (ctx,next)=>
            {
                if (string.Equals(ctx.Request.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET", "POST" });
                    ctx.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "x-requested-with", "content-type", "authorization", "*" });
                    ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    //ctx.Response.Headers.Add("Access-Control-Allow-Origin", ctx.Request.IsHttps
                    //    ? $"https://{ctx.Request.Host.Value}"
                    //    : $"http://{ctx.Request.Host.Value}");
                }
                await next();
            });
            
            app.UseStaticFiles();
            app.UseFileServer();
            app.UseDefaultFiles("/index.html");
            app.UseMessageMVC(true);
        }

    }
}