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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    partial class DataAccess<TEntity>
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
        public async Task<List<T>> Group<T>(string group, Dictionary<string, string> colls, Expression<Func<TEntity, bool>> lambda, Action<DbDataReader, T> readAction)
            where T : class, new()
        {
            var groupF = Option.FieldMap[group];
            var code = new StringBuilder();
            code.Append($"SELECT {groupF} as {group}");

            foreach (var field in colls)
            {
                code.Append($",{field.Value}({Option.FieldMap[field.Key]}) AS {field.Key}");
            }
            var convert = SqlBuilder.Compile(lambda);
            code.AppendLine($" FROM {Option.ReadTableName} ");
            SqlBuilder.InjectionLoadCondition(code, convert.ConditionSql);
            code.Append($" GROUP BY {groupF};");

            var results = new List<T>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                using var cmd = connectionScope.CreateCommand(code.ToString(), convert.Parameters);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var t = new T();
                    readAction(reader, t);
                    results.Add(t);
                }
            }

            return results;
        }
    }
}