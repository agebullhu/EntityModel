using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;

namespace EM5_Dapper
{
    internal class LambdaTest
    {
        internal static Task Test()
        {
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Flush();
            return SelectFieldTest();
        }

        static async Task SelectFieldTest()
        {
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Flush();
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
                access.SelectField("id", "IsFreeze");
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                await access.FirstAsync(p => p.Id.Equals(12));
                await access.FirstAsync(p => 12.Equals(p.Id));
                await access.FirstAsync(p => Equals(p.Id, 12));
                await access.FirstAsync(p => p.Id == 12);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task EntityModelWrite()
        {
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Flush();
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                await access.FirstAsync(p => p.Id.Equals(12));
                await access.FirstAsync(p => 12.Equals(p.Id));
                await access.FirstAsync(p => Equals(p.Id, 12));
                await access.FirstAsync(p => p.Id == 12);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}


/*
        static async Task EntityModelLoad()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var option = new EventDefaultDataAccessOption();
                var access = option.CreateDataAccess(DependencyHelper.ServiceProvider);
                //await using var connectionScope = await access.DataBase.CreateConnectionScope();

                //Console.WriteLine("【ExistAsync】");
                //{
                //    var s = DateTime.Now;
                //    var ex = await access.ExistAsync();
                //    var end = DateTime.Now;
                //    var time = (end - s).TotalMilliseconds;
                //    Console.WriteLine($" ☆ {ex}({time}ms)");
                //}
                //Console.WriteLine("【CountAsync】");
                //{
                //    var s = DateTime.Now;
                //    var cn = await access.CountAsync();
                //    var end = DateTime.Now;
                //    var time = (end - s).TotalMilliseconds;
                //    Console.WriteLine($" ☆ {time}ms");
                //}
                //Console.WriteLine("【FirstOrDefaultAsync】");
                //{
                //    var s = DateTime.Now;
                //    var data = await access.FirstOrDefaultAsync();
                //    var end = DateTime.Now;
                //    var time = (end - s).TotalMilliseconds;
                //    Console.WriteLine($" ☆ {time}ms");
                //    //Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                //}

                //Console.WriteLine("【AllAsync】");
                //{
                //    var s = DateTime.Now;
                //    var datas = await access.AllAsync(p => p.Id > 0);
                //    var end = DateTime.Now;
                //    var time = (end - s).TotalMilliseconds;
                //    Console.WriteLine($" ☆ {time}ms");
                //    //Console.WriteLine(JsonConvert.SerializeObject(datas, Formatting.Indented));
                //}

                ////Console.WriteLine("【InsertAsync】");
                //foreach (var da in datas)
                //{
                //    bool su = await access.InsertAsync(da);
                //    //Console.WriteLine($"{da.Id}:{su}");
                //}

                ////Console.WriteLine("【UpdateAsync】");
                //foreach (var da in datas)
                //{
                //    bool su = await access.UpdateAsync(da);
                //    //Console.WriteLine($"{da.Id}:{su}");
                //}
                //Console.WriteLine($"【InsertAsync】{DateTime.Now}");
                {
                    //await using var cxt1 = await access.BeginInsert();
                    //await using var cxt2 = await access.BeginUpdate(connectionScope);
                    //await using var cxt3 = await access.BeginDelete(connectionScope);
                    //EventDefaultData data = await access.FirstAsync();
                    //var s = DateTime.Now;
                    //await access.InsertAsync(cxt1, data);
                    for (int i = 0; i < 100; i++)
                    {
                        var data = await access.FirstAsync();
                        //await access.UpdateAsync(data);
                        {
                            //await access.InsertAsync(data);
                            //await access.UpdateAsync(data);
                            //FlowTracer.BeginMonitor("InsertAsync");
                            //await cxt1.Command.ExecuteNonQueryAsync();
                            //var step = FlowTracer.EndMonitor();
                            //DependencyScope.Logger.TraceMonitor(step);
                            //Console.WriteLine(JsonConvert.SerializeObject(step, Formatting.Indented));
                            //await access.UpdateAsync(cxt2, data);
                            //await access.DeleteAsync(cxt3, data.Id);
                        }
                    }
                    //var end = DateTime.Now;
                    //var len = (end - s).TotalSeconds;
                    Interlocked.Add(ref count, 100);

                    //Console.WriteLine($" ☆ {DateTime.Now}( { 100 / len}/s = 100 / {len}s)");
                }
                ////Console.WriteLine($"【UpdateAsync】{DateTime.Now}");
                //{
                //    await using var cxt = await access.BeginUpdate();
                //    foreach (var da in datas)
                //    {
                //        bool su = await access.UpdateAsync(cxt, da);
                //        //Console.WriteLine($"{da.Id}:{su}");
                //    }
                //}

                ////Console.WriteLine("【DeleteAsync】");
                //foreach (var da in datas)
                //{
                //    bool su = await access.DeleteAsync(da);
                //    //Console.WriteLine($"{da.Id}:{su}");
                //}
                ////Console.WriteLine("【DeleteAsync】");
                //{
                //    await using var cxt = await access.BeginDelete();
                //    foreach (var da in datas)
                //    {
                //        bool su = await access.DeleteAsync(cxt, da.Id);
                //        //Console.WriteLine($"{da.Id}:{su}");
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
*/