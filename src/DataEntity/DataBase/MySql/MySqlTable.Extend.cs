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
using MySql.Data.MySqlClient;
using System.Linq;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    partial class MySqlTable<TData>
    {
        #region 数据库

        /// <summary>
        ///     构造一个缺省可用的数据库对象
        /// </summary>
        /// <returns></returns>
        protected abstract MySqlDataBase CreateDefaultDataBase();

        /// <summary>
        /// 按修改更新
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        ///     表的唯一标识
        /// </summary>
        public abstract int TableId { get; }

        #endregion

        #region 数据结构

        /// <summary>
        ///     全表读取的SQL语句
        /// </summary>
        protected abstract string FullLoadFields { get; }

        /// <summary>
        ///     表名
        /// </summary>
        protected abstract string ReadTableName { get; }
        /// <summary>
        ///     表名
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected virtual string DeleteSqlCode => $@"DELETE FROM `{WriteTableName}`";

        /// <summary>
        ///     插入的SQL语句
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     全部更新的SQL语句
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     基本查询条件
        /// </summary>
        public string BaseCondition { get; set; }

        #endregion

        #region 字段字典

        /// <summary>
        ///     设计时的主键字段
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     字段字典
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     字段字典(设计时)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return _fieldMap ?? (_fieldMap = Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     所有字段(设计时)
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     字段字典(动态覆盖)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }



        #endregion


        #region 动态上下文扩展


        /// <summary>
        ///     动态读取的字段
        /// </summary>
        internal string _contextReadFields;

        /// <summary>
        ///     动态读取的字段
        /// </summary>
        protected string ContextLoadFields
        {
            get { return _contextReadFields ?? FullLoadFields; }
            set { _contextReadFields = string.IsNullOrWhiteSpace(value) ? null : value; }
        }
        /// <summary>
        ///     当前上下文读取的表名
        /// </summary>
        protected virtual string ContextReadTable => _dynamicReadTable ?? ReadTableName;

        /// <summary>
        /// 当前上下文的读取器
        /// </summary>
        public Action<MySqlDataReader, TData> ContentLoadAction { get; set; }

        /// <summary>
        ///     动态读取的表
        /// </summary>
        protected string _dynamicReadTable;

        /// <summary>
        ///     取得实际设置的ContextReadTable动态读取的表
        /// </summary>
        /// <returns>之前的动态读取的表名</returns>
        public string SetDynamicReadTable(string table)
        {
            var old = _dynamicReadTable;
            _dynamicReadTable = string.IsNullOrWhiteSpace(table) ? null : table;
            return old;
        }

        #endregion

        #region 简单读取

        /// <summary>
        /// 简单读取SQL语句
        /// </summary>
        public virtual string SimpleFields => FullLoadFields;


        /// <summary>
        /// 简单读取载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public virtual void SimpleLoad(MySqlDataReader reader, TData entity)
        {
            LoadEntity(reader, entity);
        }
        #endregion

        #region 纯虚方法

        /// <summary>
        ///     设置更新数据的命令
        /// </summary>
        protected virtual void SetUpdateCommand(TData entity, MySqlCommand cmd)
        {
        }

        /// <summary>
        ///     设置插入数据的命令
        /// </summary>
        /// <returns>返回真说明要取主键</returns>
        protected virtual bool SetInsertCommand(TData entity, MySqlCommand cmd)
        {
            return false;
        }

        /// <summary>
        ///     载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        protected virtual void LoadEntity(MySqlDataReader reader, TData entity)
        {
        }

        #endregion

        #region 操作扩展


        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///    更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        protected virtual void OnOperatorExecutd(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        #endregion
    }
}