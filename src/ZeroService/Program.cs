using Agebull.Common;
using Agebull.Common.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.Http;
using ZeroTeam.MessageMVC.Kafka;
using ZeroTeam.MessageMVC.RedisMQ;

namespace ZeroTeam.MessageMVC.ConfigSync
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseMessageMVC(services =>
                {
                    services.AddMessageMvcRedis();
                    services.AddMessageMvcKafka();
                    services.AddMessageMvcHttpClient();
                    services.AddSingleton<IIdGenerator, SnowFlakeIdGenerator>();
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                .ConfigureLogging(builder => builder.AddConfiguration(ConfigurationHelper.Root.GetSection("Logging")))
                .UseKestrel((ctx, opt) =>
                {
                    webBuilder.UseUrls(ctx.Configuration.GetSection("AspNetCore:Url").Value);
                    opt.Configure(ctx.Configuration.GetSection("AspNetCore:Kestrel"));
                })
                .Configure(app => app.UseMessageMVC()));
        }
    }
}