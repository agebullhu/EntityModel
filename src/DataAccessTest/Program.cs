using Agebull.Common.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ZeroTeam.MessageMVC.ConfigSync
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseConfiguration(ConfigurationHelper.Root)
                        .UseUrls(ConfigurationHelper.Root.GetSection("Http:Url").Value)
                        .UseKestrel((ctx, opt) =>
                        {
                            opt.Configure(ctx.Configuration.GetSection("Http:Kestrel"));
                        })
                        .UseStartup<Startup>();
                });
    }
}