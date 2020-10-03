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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;

namespace DataAccessTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationHelper.Flush();
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.ServiceCollection
                .AddTransient(typeof(IOperatorInjection<>), typeof(OperatorInjection<>))
                .AddSingleton<ISqlInjection, DataInterfaceFeatureInjection>();

            DependencyHelper.Reload();

            await PredicateConvertTest();

            //await EntityModelPrepare();
            LoggerExtend.LogDataSql = false;
            int taskCnt;
            string cnt;
            do
            {
                Console.Write("请输入并行数或Q结束：");
                cnt = Console.ReadLine();
                if (cnt == "Q" || cnt == "q")
                    return;
            }
            while (!int.TryParse(cnt, out taskCnt) || taskCnt <= 0);
            {
                Console.Write("【EntityModel Read】  ");
                count = 0;
                var list = new Task[taskCnt];
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
                var list = new Task[taskCnt];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = DapperLoad();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
            //await DapperPrepare();
            //{

            //    Console.Write("【EntityModel Write】  ");
            //    count = 0;
            //    var list = new Task[taskCnt];
            //    var start = DateTime.Now;
            //    for (var idx = 0; idx < list.Length; idx++)
            //        list[idx] = EntityModelTest();

            //    Task.WaitAll(list);
            //    var end = DateTime.Now;
            //    var time = (end - start).TotalSeconds;
            //    Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            //}
            //{
            //    Console.Write("【Dapper Write】  ");
            //    count = 0;
            //    var list = new Task[taskCnt];
            //    var start = DateTime.Now;
            //    for (var idx = 0; idx < list.Length; idx++)
            //        list[idx] = DapperTest();

            //    Task.WaitAll(list);
            //    var end = DateTime.Now;
            //    var time = (end - start).TotalSeconds;
            //    Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            //}
        }
        static long count = 0;
        static async Task PredicateConvertTest()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataQuery<EventSubscribeEntity>();
                Console.WriteLine("【IN】");
                var id = new List<string> { "d" };
                await access.FirstAsync(p => id.Contains(p.Service));
                await access.FirstAsync(p => new List<string> { "d" }.Contains(p.Service));
                await access.FirstAsync(p => new long[] { 1, 2, 3 }.Contains(p.Id));
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                {
                    Console.WriteLine("【EntityProperty method】");
                    var pro = access.Option.PropertyMap["EventId"];
                    await access.FirstAsync(p => pro.IsEquals(1) && pro.Expression(">", 1));
                }
                Console.WriteLine("【Ex method】");
                await access.FirstAsync(p => Ex.Condition("event_id > 0") || p.ApiName.IsNotNull() || "event_id".Expression("=", "0") || "event_id".FieldEquals(0));
                await access.FirstAsync(p => p.ApiName.IsNull() || "service".LeftLike("0"));
                await access.FirstAsync(p => p.Id.In(1, 2, 3));
                Console.WriteLine("【BinaryExpression】");
                await access.FirstAsync(p => p.ApiName == null || p.EventId > 1);
                Console.WriteLine("【UnaryExpression】");
                await access.FirstAsync(p => !(p.ApiName == null) || !p.IsLookUp);
                Console.WriteLine("【MemberExpression】");
                await access.FirstAsync(p => p.ApiName.Length == 2 && p.AddDate == DateTime.Now);
                Console.WriteLine("【Enum】");
                await access.FirstAsync(p => p.DataState != DataStateType.Delete || p.DataState.HasFlag(DataStateType.Enable));
                Console.WriteLine("【MethodCallExpression】");
                var str = " 1 ";
                await access.FirstAsync(p => p.IsFreeze);
                await access.FirstAsync(p => p.IsFreeze && p.IsFreeze == true && p.Service.ToUpper() == str.Trim() || Math.Abs(p.Id) == 0 || p.Service.Equals("rr"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static async Task EntityModelPrepare()
        {
            Console.Write("【EntityModel Prepare】");
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventSubscribeEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var pro = access.Option.PropertyMap["EventId"];
                var data = await access.FirstAsync();
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                await access.InsertAsync(data);
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                var cnt = await access.UpdateAsync(data);
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));

                Console.WriteLine($"update {cnt} records");
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
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
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

        static async Task EntityModelTest()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
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
                var loadSqlCode = EventDefaultEntityDataOperator.Option.SqlBuilder.CreateLoadSql();
                await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                {
                    var data = await connection.QueryFirstAsync<EventDefaultEntity>(loadSqlCode);
                    var cnt = await connection.ExecuteAsync(EventDefaultEntityDataOperator.Option.UpdateSqlCode, data);
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
                var loadSqlCode = EventDefaultEntityDataOperator.Option.SqlBuilder.CreateLoadSql();
                for (int i = 0; i < 100; i++)
                {
                    await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                    await connection.QueryFirstAsync<EventDefaultEntity>(loadSqlCode);
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
                var loadSqlCode = EventDefaultEntityDataOperator.Option.SqlBuilder.CreateLoadSql();

                await using var connection = await MySqlDataBase.OpenConnection("EventBusDb");
                var data = await connection.QueryFirstAsync<EventDefaultEntity>(loadSqlCode);
                for (int i = 0; i < 100; i++)
                {
                    await connection.ExecuteAsync(EventDefaultEntityDataOperator.Option.UpdateSqlCode, data);
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