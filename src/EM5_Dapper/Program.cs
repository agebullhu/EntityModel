using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
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
            //await Tester.Test();
            await EntityModelWrite();
        }

        static async Task EntityModelWrite()
        {
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Flush();
            using var scope = DependencyScope.CreateScope();
            try
            {
                var filter = new LambdaItem<EventDefaultEntity>();
                var ids = new long[] { 1,2,3};
                filter.Root = p => ids.Contains(p.Id);
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();

                var data = await access.FirstAsync(filter);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
