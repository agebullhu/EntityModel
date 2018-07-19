// /*****************************************************
// (c)2008-2013 Copy right www.Gboxt.com
// 作者:bull2
// 工程:CodeRefactor-Agebull.Common.SimpleDataAccess
// 建立:2014-12-03
// 修改:2014-12-07
// *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Agebull.Common.DataModel;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;

#endregion

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    ///     SQLite实体访问类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract class SqliteTable<TEntity>
            where TEntity : EditDataObject, new()
    {
        #region Lambda表达式支持
        /// <summary>
        /// 载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity First()
        {
            return LoadFirst();
        }

        /// <summary>
        /// 载入首行
        /// </summary>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault()
        {
            return LoadFirst();
        }

        /// <summary>
        /// 载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity First(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        /// 载入首行
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            return LoadFirst(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public bool Any()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.ExistInner();
            }
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public bool Any(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.ExistInner(convert.ConditionSql, convert.Parameters);
            }
        }


        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public long Count(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.CountInner(convert.ConditionSql, convert.Parameters);
            }
        }

        /// <summary>
        ///  读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public List<TEntity> Select()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner();
            }
        }

        /// <summary>
        ///  读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TEntity> Select(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner(convert.ConditionSql, convert.Parameters);
            }
        }

        /// <summary>
        ///  读取数据
        /// </summary>
        /// <returns>是否存在数据</returns>
        public List<TEntity> All()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner();
            }
        }


        /// <summary>
        ///  读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TEntity> All(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner(convert.ConditionSql, convert.Parameters);
            }
        }
        /// <summary>
        ///  读取数据
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <returns>是否存在数据</returns>
        public List<TEntity> Where(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = PredicateConvert.Convert(Fields, lambda);

            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner(convert.ConditionSql, convert.Parameters);
            }
        }

        #endregion

        #region 数据库

        private SqliteDataBase _dataBase;

        /// <summary>
        ///     自动数据连接对象
        /// </summary>
        public SqliteDataBase DataBase
        {
            get
            {
                return this._dataBase ?? SqliteDataBase.DefaultDataBase;
            }
            set
            {
                this._dataBase = value;
            }
        }

        #endregion

        #region 数据结构

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        protected abstract string FullLoadSql
        {
            get;
        }

        /// <summary>
        ///     表名
        /// </summary>
        protected abstract string TableName
        {
            get;
        }

        /// <summary>
        ///     主键
        /// </summary>
        public abstract string PrimaryKey
        {
            get;
        }

        /// <summary>
        /// 插入的SQL语句
        /// </summary>
        protected abstract string InsertSql
        {
            get;
        }

        /// <summary>
        ///  所有字段
        /// </summary>
        public abstract string[] Fields
        {
            get;
        }

        #endregion

        #region 数据是否存在或总数

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist()
        {
            return this.Count() > 0;
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        public bool Exist(string condition, params SQLiteParameter[] args)
        {
            return this.Count(condition, args) > 0;
        }

        /// <summary>
        ///     主键数据是否存在
        /// </summary>
        public bool ExistPrimaryKey(object key)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.ExistInner(this.PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
            }
        }

        /// <summary>
        ///     总数
        /// </summary>
        public long Count()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.CountInner();
            }
        }

        /// <summary>
        ///     总数
        /// </summary>
        public long Count(string condition, params SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.CountInner(condition, args);
            }
        }

        #endregion

        #region 数据读取

        /// <summary>
        ///     全表读取
        /// </summary>
        public List<TEntity> LoadData()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner();
            }
        }

        /// <summary>
        ///     条件读取
        /// </summary>
        public List<TEntity> LoadData(string condition, params SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner(condition, args);
            }
        }


        /// <summary>
        ///     主键读取
        /// </summary>
        public TEntity LoadByPrimaryKey(object key)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadFirstInner(this.PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
            }
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string condition = null)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadFirstInner(condition);
            }
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public Object LoadValue(string field, string condition, params SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadValueInner(field, condition, args);
            }
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string condition, params SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadFirstInner(condition, args);
            }
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public TEntity LoadFirst(string foreignKey, object key)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadFirstInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
            }
        }

        /// <summary>
        ///     如果存在的话读取首行
        /// </summary>
        public List<TEntity> LoadByForeignKey(string foreignKey, object key)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                return this.LoadDataInner(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, key));
            }
        }

        /// <summary>
        ///     重新读取
        /// </summary>
        public void ReLoad(TEntity entity)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.ReLoadInner(entity);
            }
        }

        #endregion

        #region 数据操作

        /// <summary>
        ///     保存值
        /// </summary>
        public void SaveValue(string field, object value, string[] conditionFiles, object[] values)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                this.SaveValueInner(field, value, conditionFiles, values);
            }
        }

        /// <summary>
        ///     条件读取
        /// </summary>
        public void Save(IEnumerable<TEntity> entities)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                foreach (var entity in entities)
                {
                    this.SaveInner(entity);
                    this.ReLoadInner(entity);
                    //entity.RaiseStatusChanged(NotificationStatusType.Saved);
                }
            }
        }

        /// <summary>
        ///     保存
        /// </summary>
        public void Save(TEntity entity)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.SaveInner(entity);
            }
        }

        /// <summary>
        ///     更新
        /// </summary>
        public void Update(TEntity entity)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.UpdateInner(entity);
                this.ReLoadInner(entity);
            }
            //entity.RaiseStatusChanged(NotificationStatusType.Saved);
        }

        /// <summary>
        ///     更新
        /// </summary>
        public void Update(IEnumerable<TEntity> entities)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                foreach (var entity in entities)
                {
                    this.UpdateInner(entity);
                    this.ReLoadInner(entity);
                    //entity.RaiseStatusChanged(NotificationStatusType.Saved);
                }
            }
        }

        /// <summary>
        ///     新增
        /// </summary>
        public void Insert(TEntity entity)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.InsertInner(entity);
                this.ReLoadInner(entity);
            }
            //entity.RaiseStatusChanged(NotificationStatusType.Saved);
        }

        /// <summary>
        ///     新增
        /// </summary>
        public void Insert(IEnumerable<TEntity> entities)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                foreach (var entity in entities)
                {
                    this.InsertInner(entity);
                    this.ReLoadInner(entity);
                    //entity.RaiseStatusChanged(NotificationStatusType.Saved);
                }
            }
        }

        /// <summary>
        ///     删除
        /// </summary>
        public void Delete(IEnumerable<TEntity> entities)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                foreach (var entity in entities)
                {
                    this.DeleteInner(entity);
                    //entity.RaiseStatusChanged(NotificationStatusType.Deleted);
                }
            }
        }

        /// <summary>
        ///     删除
        /// </summary>
        public void Delete(TEntity entity)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.DeleteInner(entity);
            }
            //entity.RaiseStatusChanged(NotificationStatusType.Deleted);
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public int DeletePrimaryKey(object key)
        {
            return this.Delete(this.PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     删除所有数据
        /// </summary>
        public void Clear()
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                this.DeleteInner();
                return;
            }
        }

        /// <summary>
        ///     条件删除
        /// </summary>
        public int Delete(string condition, params SQLiteParameter[] args)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentException(@"删除条件不能为空,因为不允许执行全表删除", "condition");
            }
            using (SqliteDataBaseScope.CreateScope(this.DataBase, true))
            {
                return this.DeleteInner(condition, args);
            }
        }

        #endregion

        #region 内部方法

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>读取数据的实体</returns>
        protected virtual TEntity LoadEntity(SQLiteDataReader reader)
        {
            var entity = new TEntity();
            this.LoadEntity(reader, entity);
            return entity;
        }

        /// <summary>
        ///     重新载入
        /// </summary>
        protected virtual void ReLoadInner(TEntity entity)
        {
            var cmd = CreateLoadCommand(this.PrimaryKeyConditionSQL, CreatePimaryKeyParameter(entity));
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    this.LoadEntity(reader, entity);
            }
        }

        /// <summary>
        ///     读取值
        /// </summary>
        protected object LoadValueInner(string field, string condition, params SQLiteParameter[] args)
        {
            string sql = string.IsNullOrWhiteSpace(condition)
                    ? string.Format(@"SELECT {0} FROM {1};", field, this.TableName)
                    : string.Format(@"SELECT {0} FROM {1} WHERE {2};", field, this.TableName, condition);

            return DataBase.ExecuteScalar(sql, args);
        }

        /// <summary>
        ///     读取首行
        /// </summary>
        protected TEntity LoadFirstInner(string condition = null, SQLiteParameter args = null)
        {
            return LoadFirstInner(condition, args == null ? new SQLiteParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取首行
        /// </summary>
        protected TEntity LoadFirstInner(string condition, SQLiteParameter[] args)
        {
            var cmd = CreateLoadCommand(condition, args);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                //lock (this.DataBase)
                {
                    return this.LoadEntity(reader);
                }
            }
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TEntity> LoadDataInner(string condition = null, SQLiteParameter args = null)
        {
            return LoadDataInner(condition, args == null ? new SQLiteParameter[0] : new[] { args });
        }

        /// <summary>
        ///     读取全部
        /// </summary>
        protected List<TEntity> LoadDataInner(string condition, SQLiteParameter[] args)
        {
            var results = new List<TEntity>();
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                var cmd = CreateLoadCommand(condition, args);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //lock (DataBase)
                        {
                            results.Add(this.LoadEntity(reader));
                        }
                    }
                }
            }
            return results;
        }
        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition = null, SQLiteParameter args = null)
        {
            return ExistInner(condition, args == null ? new SQLiteParameter[0] : new[] { args });
        }

        /// <summary>
        ///     是否存在数据
        /// </summary>
        protected bool ExistInner(string condition, SQLiteParameter[] args)
        {
            return this.CountInner(condition, args) > 0;
        }

        /// <summary>
        ///     总数据量
        /// </summary>
        protected long CountInner(string condition = null, SQLiteParameter args = null)
        {
            return CountInner(condition, args == null ? new SQLiteParameter[0] : new[] { args });
        }
        /// <summary>
        ///     总数据量
        /// </summary>
        protected long CountInner(string condition, SQLiteParameter[] args)
        {
            using (SqliteDataBaseScope.CreateScope(this.DataBase))
            {
                var sql = string.IsNullOrWhiteSpace(condition)
                        ? string.Format(@"SELECT COUNT(*) FROM [{0}];", this.TableName)
                        : string.Format(@"SELECT COUNT(*) FROM [{0}] WHERE {1};", this.TableName, condition);
                return (long)this.DataBase.ExecuteScalar(sql, args);
            }
        }

        /// <summary>
        ///     保存
        /// </summary>
        private void SaveInner(TEntity entity)
        {
            if (!this.ExistPrimaryKey(entity.GetValue(this.PrimaryKey)))
            {
                this.InsertInner(entity);
            }
            else
            {
                if (entity.__EntityStatus.IsDelete)
                {
                    this.DeleteInner(entity);
                }
                else
                {
                    this.UpdateInner(entity);
                }
            }
        }

        /// <summary>
        ///     保存值
        /// </summary>
        protected void SaveValueInner(string field, object value, string[] conditionFiles, object[] values)
        {
            var args = CreateFieldsParameters(conditionFiles, values);
            var condition = FieldConditionSQL(true, conditionFiles);
            if (Exist(condition, args))
            {
                var cmd = DataBase.Connection.CreateCommand();
                cmd.Parameters.Add(new SQLiteParameter(field + "_n", this.GetDbType(field))
                {
                    Value = value
                });
                cmd.Parameters.AddRange(args);
                cmd.CommandText = string.Format(@"UPDATE [{0}] SET [{1}] = ${1}_n WHERE {2}", TableName, field, condition);
                // LogRecorder.RecordDataLog(cmd.CommandText);
                lock (DataBase)
                {
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                var entity = new TEntity();

                for (int idx = 0; idx < values.Length; idx++)
                    entity.SetValue(conditionFiles[idx], values[idx]);
                entity.SetValue(field, value);
                InsertInner(entity);
            }
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        /// <param name="entity">更新数据的实体</param>
        protected void InsertInner(TEntity entity)
        {
            var cmd = DataBase.Connection.CreateCommand();
            if (SetInsertCommand(entity, cmd))
            {
                // LogRecorder.RecordDataLog(cmd.CommandText);
                var key = cmd.ExecuteScalar();
                entity.SetValue(PrimaryKey, key);
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
            entity.AcceptChanged();
            this.ReLoadInner(entity);
            //entity.RaiseStatusChanged(NotificationStatusType.Saved);
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="entity">插入数据的实体</param>
        protected void UpdateInner(TEntity entity)
        {
            if (!entity.__EntityStatus.IsModified)
                return;
            var cmd = DataBase.Connection.CreateCommand();
            SetUpdateCommand(entity, cmd);

            // LogRecorder.RecordDataLog(cmd.CommandText);
            lock (DataBase)
            {
                cmd.ExecuteNonQuery();
            }
            entity.AcceptChanged();
            this.ReLoadInner(entity);
            //entity.RaiseStatusChanged(NotificationStatusType.Saved);
        }

        /// <summary>
        ///     删除
        /// </summary>
        protected virtual void DeleteInner(TEntity entity)
        {
            this.DeleteInner(this.PrimaryKeyConditionSQL, this.CreatePimaryKeyParameter(entity));
        }

        /// <summary>
        ///     删除
        /// </summary>
        protected int DeleteInner(string condition = null, SQLiteParameter args = null)
        {
            return DeleteInner(condition, args == null ? new SQLiteParameter[0] : new[] { args });
        }

        /// <summary>
        ///     删除
        /// </summary>
        protected int DeleteInner(string condition, SQLiteParameter[] args)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                return this.DataBase.Execute(string.Format(@"DELETE FROM [{0}] WHERE {1};", this.TableName, condition), args);
            }
            this.DataBase.Clear(this.TableName);
            return int.MaxValue;
        }

        #endregion

        #region 纯虚方法

        /// <summary>
        ///     设置更新数据的命令
        /// </summary>
        protected abstract void SetUpdateCommand(TEntity entity, SQLiteCommand cmd);

        /// <summary>
        ///     设置插入数据的命令
        /// </summary>
        /// <returns>返回真说明要取主键</returns>
        protected abstract bool SetInsertCommand(TEntity entity, SQLiteCommand cmd);

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected abstract void LoadEntity(SQLiteDataReader reader, TEntity entity);

        #endregion

        #region 字段的参数帮助
        /// <summary>
        ///     生成命令
        /// </summary>
        protected SQLiteCommand CreateLoadCommand(string condition, params SQLiteParameter[] args)
        {
            var cmd = this.DataBase.Connection.CreateCommand();
            cmd.CommandText = string.IsNullOrWhiteSpace(condition)
                    ? this.FullLoadSql
                    : string.Format(@"{0} WHERE {1};", this.FullLoadSql, condition);
            // LogRecorder.RecordDataLog(cmd.CommandText);
            if (args != null && args.Length > 0)
            {
                cmd.Parameters.AddRange(args);
            }
            return cmd;
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        protected SQLiteCommand CreateCommand(string sql, SQLiteParameter[] args)
        {
            var cmd = this.DataBase.Connection.CreateCommand();
            cmd.CommandText = sql;
            if (args != null && args.Length > 0)
            {
                cmd.Parameters.AddRange(args);
            }
            return cmd;
        }

        /// <summary>
        ///     得到字段的SqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        protected virtual DbType GetDbType(string field)
        {
            return DbType.String;
        }


        private string _primaryConditionSQL;

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string PrimaryKeyConditionSQL
        {
            get
            {
                return this._primaryConditionSQL ?? (this._primaryConditionSQL = this.FieldConditionSQL(this.PrimaryKey));
            }
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        public SQLiteParameter[] CreateFieldsParameters(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成参数", "fields");
            }
            return fields.Select(field => new SQLiteParameter(field, this.GetDbType(field))).ToArray();
        }

        /// <summary>
        ///     生成多个字段的参数
        /// </summary>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        public SQLiteParameter[] CreateFieldsParameters(string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成参数", "fields");
            }
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException(@"没有值用于生成参数", "values");
            }
            if (values.Length != fields.Length)
            {
                throw new ArgumentException(@"值的长度和字段长度必须一致", "values");
            }
            var res = fields.Select(field => new SQLiteParameter(field, this.GetDbType(field))).ToArray();
            for (int i = 0; i < fields.Length; i++)
            {
                res[i].Value = values[i];
            }
            return res;
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        public SQLiteParameter CreateFieldParameter(string field)
        {
            return new SQLiteParameter(field, this.GetDbType(field));
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="value">值</param>
        public SQLiteParameter CreateFieldParameter(string field, object value)
        {
            return new SQLiteParameter(field, this.GetDbType(field))
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        public SQLiteParameter CreateFieldParameter(string field, TEntity entity)
        {
            return new SQLiteParameter(field, this.GetDbType(field))
            {
                Value = entity.GetValue(field)
            };
        }

        /// <summary>
        ///     生成字段的参数
        /// </summary>
        /// <param name="field">生成参数的字段</param>
        /// <param name="entity">取值的实体</param>
        /// <param name="entityField">取值的字段</param>
        public SQLiteParameter CreateFieldParameter(string field, TEntity entity, string entityField)
        {
            return new SQLiteParameter(field, this.GetDbType(field))
            {
                Value = entity.GetValue(entityField)
            };
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        public SQLiteParameter CreatePimaryKeyParameter()
        {
            return new SQLiteParameter(this.PrimaryKey, this.GetDbType(this.PrimaryKey));
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="value">主键值</param>
        public SQLiteParameter CreatePimaryKeyParameter(object value)
        {
            return new SQLiteParameter(this.PrimaryKey, this.GetDbType(this.PrimaryKey))
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成主键字段的参数
        /// </summary>
        /// <param name="entity">取值的实体</param>
        public SQLiteParameter CreatePimaryKeyParameter(TEntity entity)
        {
            return new SQLiteParameter(this.PrimaryKey, this.GetDbType(this.PrimaryKey))
            {
                Value = entity.GetValue(this.PrimaryKey)
            };
        }

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        public string FieldConditionSQL(string field)
        {
            return string.Format(@"[{0}] = ${0}", field);
        }

        /// <summary>
        ///     生成命令对象
        /// </summary>
        /// <returns></returns>
        public SQLiteCommand CreateCommand(string sql)
        {
            var cmd = this.DataBase.Connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        /// <summary>
        ///     连接条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="conditions">条件</param>
        public string JoinConditionSQL(bool isAnd, params string[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
            {
                throw new ArgumentException(@"没有条件用于组合", "conditions");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", conditions[0]);
            for (var idx = 1; idx < conditions.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", conditions[idx]);
            }
            return sql.ToString();
        }

        /// <summary>
        ///     连接字段条件SQL
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        public string FieldConditionSQL(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            var sql = new StringBuilder();
            sql.AppendFormat(@"({0})", FieldConditionSQL(fields[0]));
            for (var idx = 1; idx < fields.Length; idx++)
            {
                sql.AppendFormat(@" {0} ({1}) ", isAnd ? "AND" : "OR", FieldConditionSQL(fields[idx]));
            }
            return sql.ToString();
        }

        /// <summary>
        ///     连接字段条件
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, params string[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields)
            };
        }

        /// <summary>
        ///     连接字段条件
        /// </summary>
        /// <param name="isAnd">是否用AND组合</param>
        /// <param name="fields">生成参数的字段</param>
        /// <param name="values">生成参数的值(长度和字段长度必须一致)</param>
        /// <returns>ConditionItem</returns>
        public ConditionItem CreateConditionItem(bool isAnd, string[] fields, object[] values)
        {
            if (fields == null || fields.Length == 0)
            {
                throw new ArgumentException(@"没有字段用于生成组合条件", "fields");
            }
            return new ConditionItem
            {
                ConditionSql = FieldConditionSQL(isAnd, fields),
                Parameters = CreateFieldsParameters(fields, values)
            };
        }
        #endregion
    }
}
