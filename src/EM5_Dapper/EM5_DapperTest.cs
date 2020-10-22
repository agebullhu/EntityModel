using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.MySql;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zeroteam.MessageMVC.EventBus;
using Zeroteam.MessageMVC.EventBus.DataAccess;

namespace EM5_Dapper
{
    internal class EM5_DapperTest
    {
        internal static async Task Test()
        {
            DependencyHelper.AddScoped<EventBusDb>();
            DependencyHelper.Flush();

            await EntityModelPrepare();
            await DapperPrepare();
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
            {

                Console.Write("【EntityModel Write】  ");
                count = 0;
                var list = new Task[taskCnt];
                var start = DateTime.Now;
                for (var idx = 0; idx < list.Length; idx++)
                    list[idx] = EntityModelWrite();

                Task.WaitAll(list);
                var end = DateTime.Now;
                var time = (end - start).TotalSeconds;
                Console.WriteLine($"☆ {end}( { count / time}/s = {count } / {time}s)");
            }
            {
                Console.Write("【Dapper Write】  ");
                count = 0;
                var list = new Task[taskCnt];
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
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.FirstAsync(p=>p.Id == 2);
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
                for (int i = 0; i < 100; i++)
                {
                    await access.LoadByPrimaryKeyAsync(2);
                    //await access.FirstAsync(p => p.Id == 2);
                }
                Interlocked.Add(ref count, 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static async Task EntityModelWrite()
        {
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventDefaultEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.FirstAsync(p => p.Id == 2);
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
        static string loadSqlCode = @"SELECT 
tb_event_default_test.`id` AS `id`
,`event_name` AS `event_name`
FROM tb_event_default_test
where id=2";


        /// <summary>
        /// 更新的字段
        /// </summary>
        public const string UpdateSqlCode = @"Update tb_event_default_test set
       `event_name` = ?EventName
where id=?id";

        static async Task DapperPrepare()
        {
            Console.Write("【Dapper Prepare】");
            using var scope = DependencyScope.CreateScope();
            try
            {
                await using var connection = await MySqlDataBase.OpenConnection(ConfigurationHelper.GetConnectionString("EventBusDb"));
                {
                    var data = await connection.QueryFirstAsync<EventDefaultEntity>(loadSqlCode);
                    var cnt = await connection.ExecuteAsync(UpdateSqlCode, data);
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
                    await using var connection = await MySqlDataBase.OpenConnection(ConfigurationHelper.GetConnectionString("EventBusDb"));
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
                await using var connection = await MySqlDataBase.OpenConnection(ConfigurationHelper.GetConnectionString("EventBusDb"));
                var data = await connection.QueryFirstAsync<EventDefaultEntity>(loadSqlCode);
                for (int i = 0; i < 100; i++)
                {
                    await connection.ExecuteAsync(UpdateSqlCode, data);
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