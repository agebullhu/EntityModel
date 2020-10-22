using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;

namespace EM5_Dapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DependencyHelper.ServiceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.Services.AddScoped(provider => ConfigurationHelper.Root);
                var level = ConfigurationHelper.Root.GetValue<LogLevel>("Logging:LogLevel:Default");
                builder.SetMinimumLevel(level);
                builder.AddConsole();
            }));

            //await EM5_DapperTest.Test();
            await LambdaTest.Test();
        }

    }
}
