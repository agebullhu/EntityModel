using Agebull.Common.Configuration;
using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.MySql;
using Dapper;
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
            string cnt;
            do
            {
                Console.Write("请输入并行数或Q结束：");
                cnt = Console.ReadLine();
                if (cnt == "Q" || cnt == "q")
                    return;
            }
            while (!int.TryParse(cnt, out taskCnt) || taskCnt <= 0);
            Console.Write("输入循环数(10)：");
            var ic = Console.ReadLine();
            if (!int.TryParse(ic, out forLen) || forLen < 1 || forLen > 100000)
                forLen = 10;
            taskCnt /= 2;
            _ = Task.Run(DapperRead);
            _ = Task.Run(EntityModelRead);
            _ = Task.Run(EntityModelRead);
            _ = Task.Run(DapperRead);
            Console.ReadLine();
            Console.WriteLine($"【Dapper Read】  ☆ ( { daCount / daTime}/s = {daCount } / {daTime}s)");
            Console.WriteLine($"【EntityModel Read】  ☆ ( { emCount / emTime}/s = {emCount } / {emTime}s)");
        }

        static void EntityModelRead()
        {
            emCount = 0;
            var list = new Task[taskCnt];
            var start = DateTime.Now;
            for (var idx = 0; idx < list.Length; idx++)
                list[idx] = EntityModelLoad();

            Task.WaitAll(list);
            var end = DateTime.Now;
            emTime = (end - start).TotalSeconds;
            Console.WriteLine($"【EntityModel】  ☆ {end}");
        }
        static void DapperRead()
        {
            daCount = 0;
            var list = new Task[taskCnt];
            var start = DateTime.Now;
            for (var idx = 0; idx < list.Length; idx++)
                list[idx] = DapperLoad();

            Task.WaitAll(list);
            var end = DateTime.Now;
            daTime= (end - start).TotalSeconds;
            Console.WriteLine($"【Dapper Read】  ☆ {end}");
        }/*
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
            }*/
        static long emCount = 0;
        static long daCount = 0;
        static double emTime = 0;
        static double daTime = 0;
        static int forLen = 100;
        static int taskCnt;
        static async Task EntityModelPrepare()
        {
            Console.Write("【EntityModel Prepare】");
            using var scope = DependencyScope.CreateScope();
            try
            {
                var access = EventBusDb.CreateEventSubscribeEntityAccess(false);
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.LoadByPrimaryKeyAsync(21);
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                //var cnt = await access.UpdateAsync(data);
                //Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));

                //Console.WriteLine($"update {cnt} records");
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
                var access = EventBusDb.CreateEventSubscribeEntityAccess(false);
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                for (int i = 0; i < forLen; i++)
                {
                    for (int j = 0; j < 2; j++)
                        await access.FirstAsync(p=>p.Id == 21);
                    //var list = await access.AllAsync();
                    //foreach (var ls in list)
                    //{
                    //    ls.Id = 0;
                    //}
                }
                Interlocked.Add(ref emCount, forLen * 2);
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
                var access = DependencyHelper.ServiceProvider.CreateDataAccess<EventSubscribeEntity>();
                await using var connectionScope = await access.DataBase.CreateConnectionScope();
                var data = await access.FirstAsync(p => p.Id == 2);
                for (int i = 0; i < forLen; i++)
                {
                    await access.UpdateAsync(data);
                }
                Interlocked.Add(ref emCount, forLen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static string loadSqlCode = @"SELECT 
`tb_event_subscribe`.`id` AS `id`
,`tb_event_subscribe`.`event_id` AS `eventid`
,`tb_event_subscribe`.`service` AS `service`
,`tb_event_subscribe`.`is_look_up` AS `islookup`
,`tb_event_subscribe`.`api_name` AS `apiname`
,`tb_event_subscribe`.`memo` AS `memo`
,`tb_event_subscribe`.`target_name` AS `targetname`
,`tb_event_subscribe`.`target_type` AS `targettype`
,`tb_event_subscribe`.`target_description` AS `targetdescription`
,`tb_event_subscribe`.`is_freeze` AS `isfreeze`
,`tb_event_subscribe`.`data_state` AS `datastate`
,`tb_event_subscribe`.`latest_updated_date` AS `latestUpdatedDate`
,`tb_event_subscribe`.`latest_updated_user_id` AS `latestUpdatedUserId`
,`tb_event_subscribe`.`latest_updated_user` AS `latestUpdatedUser`
,`tb_event_subscribe`.`created_user_id` AS `createdUserId`
,`tb_event_subscribe`.`created_user` AS `createdUser`
,`tb_event_subscribe`.`created_date` AS `createdDate`
FROM tb_event_subscribe limit 1";//where id = 21


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
                    var data = await connection.QueryFirstAsync<EventSubscribeEntity>(loadSqlCode);
                    //var cnt = await connection.ExecuteAsync(UpdateSqlCode, data);
                    Console.WriteLine($"records,data:\n{JsonConvert.SerializeObject(data, Formatting.Indented)}");
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
                await using var connection = await MySqlDataBase.OpenConnection(ConfigurationHelper.GetConnectionString("EventBusDb"));
                for (int i = 0; i < forLen; i++)
                {
                    for (int j = 0; j < 2; j++)
                        await connection.QueryFirstAsync<EventSubscribeEntity>(loadSqlCode);

                    //var list = await connection.QueryAsync<EventSubscribeEntity>(loadSqlCode);
                    //foreach (var ls in list)
                    //{
                    //    ls.Id = 0;
                    //}
                }
                Interlocked.Add(ref daCount, forLen* 2);
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
                var data = await connection.QueryFirstAsync<EventSubscribeEntity>(loadSqlCode);
                for (int i = 0; i < forLen; i++)
                {
                    await connection.ExecuteAsync(UpdateSqlCode, data);
                }
                Interlocked.Add(ref daCount, forLen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

