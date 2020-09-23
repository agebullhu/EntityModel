using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;
using Zeroteam.MessageMVC.EventBus.Zeroteam.MessageMVC.EventBus;

namespace DataAccessTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationHelper.Flush();
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Reload();
            LoggerExtend.LogDataSql = true;
            await Test();
        }

        static async Task Test()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = new DataAccess<EventSubscribeData>(new EventSubscribeDataAccessOption());

                Console.WriteLine("【ExistAsync】");
                var ex = await access.ExistAsync();
                Console.WriteLine(ex);

                Console.WriteLine("【CountAsync】");
                var cn = await access.CountAsync();
                Console.WriteLine(cn);

                Console.WriteLine("【FirstOrDefaultAsync】");
                var data = await access.FirstOrDefaultAsync();
                Console.WriteLine(JsonConvert.SerializeObject(data));

                Console.WriteLine("【AllAsync】");
                var datas = await access.AllAsync(p => p.Id > 0);
                Console.WriteLine(JsonConvert.SerializeObject(datas));

                Console.WriteLine("【InsertAsync】");
                foreach (var da in datas)
                {
                    bool su = await access.InsertAsync(da);
                    Console.WriteLine($"{da.Id}:{su}");
                }

                Console.WriteLine("【UpdateAsync】");
                foreach (var da in datas)
                {
                    bool su = await access.UpdateAsync(da);
                    Console.WriteLine($"{da.Id}:{su}");
                }

                Console.WriteLine("【DeleteAsync】");
                foreach (var da in datas)
                {
                    bool su = await access.DeleteAsync(da);
                    Console.WriteLine($"{da.Id}:{su}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
