using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

using Agebull.Common.DataModel;

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    /// 带缓存的SQLite实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract class SqliteCacheTable<TEntity> : SqliteTable<TEntity>
        where TEntity : EditEntityObject, new()
    {

        /// <summary>
        /// 全局缓存
        /// </summary>
        private static readonly Dictionary<object, TEntity> cache = new Dictionary<object, TEntity>();

        /// <summary>
        /// 全局缓存
        /// </summary>
        public static Dictionary<object, TEntity> Cache
        {
            get
            {
                return cache;
            }
        }

        #region 内部方法

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>读取数据的实体</returns>
        protected sealed override TEntity LoadEntity(SQLiteDataReader reader)
        {
            var key = reader[this.PrimaryKey];
            TEntity entity;
            lock (Cache)
            {
                if (Cache.TryGetValue(key, out entity))
                    return entity;
                entity = new TEntity();
                this.LoadEntity(reader, entity);
                cache.Add(key, entity);
            }
            return entity;
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        protected sealed override void ReLoadInner(TEntity entity)
        {
            var cmd = this.DataBase.Connection.CreateCommand();
            cmd.CommandText = string.Format(@"{0} WHERE {1};", this.FullLoadSql, this.PrimaryKeyConditionSQL);

            cmd.Parameters.Add(CreatePimaryKeyParameter(entity));
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                this.LoadEntity(reader, entity);
            }
            var key = entity.GetValue(PrimaryKey);
            if (!Cache.ContainsKey(key))
                cache.Add(key, entity);
        }

        /// <summary>
        ///     删除
        /// </summary>
        protected sealed override void DeleteInner(TEntity entity)
        {
            var key = entity.GetValue(PrimaryKey);
            if (Cache.ContainsKey(key))
                Cache.Remove(key);
            base.DeleteInner(entity);
        }
        #endregion
    }
}