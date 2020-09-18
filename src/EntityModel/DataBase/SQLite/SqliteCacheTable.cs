using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

using Agebull.Common.DataModel;

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    /// �������SQLiteʵ�������
    /// </summary>
    /// <typeparam name="TEntity">ʵ��</typeparam>
    public abstract class SqliteCacheTable<TEntity> : SqliteTable<TEntity>
        where TEntity : EditEntityObject, new()
    {

        /// <summary>
        /// ȫ�ֻ���
        /// </summary>
        private static readonly Dictionary<object, TEntity> cache = new Dictionary<object, TEntity>();

        /// <summary>
        /// ȫ�ֻ���
        /// </summary>
        public static Dictionary<object, TEntity> Cache
        {
            get
            {
                return cache;
            }
        }

        #region �ڲ�����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <returns>��ȡ���ݵ�ʵ��</returns>
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
        ///     ��������
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
        ///     ɾ��
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