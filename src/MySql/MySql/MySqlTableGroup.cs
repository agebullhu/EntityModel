// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;

#endregion

namespace Agebull.EntityModel.MySql
{
    partial class MySqlTable<TData, TMySqlDataBase>
    {
        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="group"></param>
        /// <param name="colls"></param>
        /// <param name="lambda"></param>
        /// <param name="readAction"></param>
        /// <returns></returns>
        public List<T> Group<T>(string group, Dictionary<string, string> colls, Expression<Func<TData, bool>> lambda, Action<MySqlDataReader, T> readAction)
            where T : class, new()
        {
            var groupF = FieldDictionary[group];
            var code = new StringBuilder();
            code.Append($"SELECT {groupF} as {group}");

            foreach (var field in colls)
            {
                code.Append($",{field.Value}({FieldDictionary[field.Key]}) AS {field.Key}");
            }
            var convert = Compile(lambda);
            code.AppendLine($" FROM {ContextReadTable} ");
            code.AppendLine(ContitionSqlCode(convert.ConditionSql));
            code.Append($" GROUP BY {groupF};");



            var results = new List<T>();
            using (var cmd = DataBase.CreateCommand(code.ToString(), convert.Parameters))
            {
                var task = cmd.ExecuteReaderAsync();
                task.Wait();
                using (var reader = (MySqlDataReader)task.Result)
                {
                    while (true)
                    {
                        var task2 = reader.ReadAsync();
                        task2.Wait();
                        if (!task2.Result)
                            break;
                        var t = new T();
                        readAction(reader,t);
                        results.Add(t);
                    }
                }
            }
            return results;
        }

    }
}