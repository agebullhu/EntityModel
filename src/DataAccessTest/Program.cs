using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.MySql;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;

namespace DataAccessTest
{
    class Program
    {
        class aaa { }

        static async Task Main(string[] args)
        {
            ConfigurationHelper.Flush();
            DependencyHelper.AddScoped<EventBusDb>();
            //DependencyHelper.ServiceCollection.AddTransient(typeof(IOperatorInjection<>),typeof(OperatorInjection<>));

            DependencyHelper.Reload();


            LoggerExtend.LogDataSql = false;
            await EntityModelPrepare();
            await DapperPrepare();
            Console.Write("请输入并行数：");
            var taskCnt = Console.ReadLine();
            {
                Console.Write("【EntityModel Read】  ");
                count = 0;
                var list = new Task[int.Parse(taskCnt)];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = EntityModelLoad();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
            {
                Console.Write("【Dapper Read】  ");
                count = 0;
                var list = new Task[int.Parse(taskCnt)];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = DapperLoad();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
            {

                Console.Write("【EntityModel Write】  ");
                count = 0;
                var list = new Task[int.Parse(taskCnt)];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = EntityModelTest();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
            {
                Console.Write("【Dapper Write】  ");
                count = 0;
                var list = new Task[int.Parse(taskCnt)];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = DapperTest();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
        }
        static long count = 0;
        static async Task EntityModelPrepare()
        {
            Console.Write("【EntityModel Prepare】");
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventSubscribeData, EventBusDb, EventSubscribeDataOperator>(EventSubscribeDataOperator.OperatorOption);
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.FirstAsync();
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                var cnt = await access.UpdateAsync(data);
                Console.WriteLine($"update {cnt} records,data:\n{JsonConvert.SerializeObject(data, Formatting.Indented)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static async Task EntityModelLoad()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventSubscribeData, EventBusDb, EventSubscribeDataOperator>(EventSubscribeDataOperator.OperatorOption);
                await using var connectionScope = await access.DataBase.CreateConnectionScope();

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
                    //EventSubscribeData data = await access.FirstAsync();
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

        static async Task EntityModelTest()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventSubscribeData, EventBusDb, EventSubscribeDataOperator>(EventSubscribeDataOperator.OperatorOption);
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.FirstAsync();
                for (int i = 0; i < 100; i++)
                {
                    await access.UpdateAsync(data);
                }
                Interlocked.Add(ref count, 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task DapperPrepare()
        {
            Console.Write("【Dapper Prepare】");
            using var scope = DependencyScope.CreateScope();
            try
            {
                await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                {
                    var data = await connection.QueryFirstAsync<EventSubscribeData>(EventSubscribeDataOperator.LoadSqlCode);
                    var cnt = await connection.ExecuteAsync(EventSubscribeDataOperator.UpdateSqlCode, data);
                    Console.WriteLine($" update {cnt} records,data:\n{JsonConvert.SerializeObject(data, Formatting.Indented)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static async Task DapperLoad()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                    await connection.QueryFirstAsync<EventSubscribeData>(EventSubscribeDataOperator.LoadSqlCode);
                }
                Interlocked.Add(ref count, 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static async Task DapperTest()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                var data = await connection.QueryFirstAsync<EventSubscribeData>(EventSubscribeDataOperator.LoadSqlCode);
                for (int i = 0; i < 100; i++)
                {
                    await connection.ExecuteAsync(EventSubscribeDataOperator.UpdateSqlCode, data);
                }
                Interlocked.Add(ref count, 100);
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
                var option = new EventSubscribeDataAccessOption();
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
                    //EventSubscribeData data = await access.FirstAsync();
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