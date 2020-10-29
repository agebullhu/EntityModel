using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 字段选择范围控制
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DynamicOptionScope<TEntity> : IDisposable
        where TEntity : class, new()
    {
        private readonly IDataOperator<TEntity> backup;

        /// <summary>
        /// 基本条件
        /// </summary>
        public DynamicOption DynamicOption => Query.Option.DynamicOption;

        /// <summary>
        /// 查询对象
        /// </summary>
        public DataQuery<TEntity> Query { get; }

        internal DynamicOptionScope(DataQuery<TEntity> query)
        {
            Query = query;
            backup = query.DataOperator;
            Query.Provider.DataOperator = new DataOperator<TEntity>
            {
                Provider = query.Provider
            };
            if (Query.Option.DynamicOption == Query.Option.TableOption)
                Query.Option.DynamicOption = Query.Option.TableOption.Copy();
        }

        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public void Select(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return;
            SelectFields(fields);
        }

        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public void Select(IEnumerable<string> fields)
        {
            if (fields == null || !fields.Any())
                return;
            SelectFields(fields);
        }
        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public void OrderBy(Dictionary<string, bool> orderbys)
        {
            if (orderbys == null || orderbys.Count == 0)
                return;
            var code = new StringBuilder();
            bool first = true;
            foreach (var orderby in orderbys)
            {
                if (first) first = false;
                else code.Append(',');
                code.Append(Query.SqlBuilder.OrderCode(orderby.Key, orderby.Value));
            }
            DynamicOption.OrderbyFields = code.ToString();
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="orderbys"></param>
        public void OrderBy(string orderField, bool asc=true)
        {
            DynamicOption.OrderbyFields = Query.SqlBuilder.OrderCode(orderField, asc);
        }

        void IDisposable.Dispose()
        {
            Query.Provider.DataOperator = backup;
            Query.Option.DynamicOption = Query.Option.TableOption;
        }


        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        void SelectFields(IEnumerable<string> fields)
        {
            DynamicOption.ReadProperties = new List<EntityProperty>();
            foreach (var field in fields)
            {
                if (Query.Option.PropertyMap.TryGetValue(field, out var property))
                    Query.Option.ReadProperties.Add(property);
            }
            DynamicOption.LoadFields = Query.SqlBuilder.BuilderLoadFields();
        }

    }
}