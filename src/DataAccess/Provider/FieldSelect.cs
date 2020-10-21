using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 字段选择范围控制
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class FieldSelectEx
    {
        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IDisposable SelectField<TEntity>(this DataQuery<TEntity> query, params string[] fields)
        where TEntity : class, new()
        {
            return new FieldSelect<TEntity>(query, fields);
        }

        /// <summary>
        /// 字段选择范围控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        class FieldSelect<TEntity> : IDisposable
            where TEntity : class, new()
        {

            private readonly IDataOperator<TEntity> backup;
            private readonly DataQuery<TEntity> dataQuery;

            internal FieldSelect(DataQuery<TEntity> query, params string[] fields)
            {
                dataQuery = query;
                backup = query.DataOperator;
                dataQuery.Provider.DataOperator = new DataOperator<TEntity>
                {
                    Provider = query.Provider
                };
                query.Option.Select(fields);
            }

            void IDisposable.Dispose()
            {
                dataQuery.Provider.DataOperator = backup;
                dataQuery.Option.SelectAll();
            }
        }
    }
}