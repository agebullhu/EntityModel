// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    partial class MySqlTable<TData>
    {
        #region ���ݿ�

        /// <summary>
        ///     ����һ��ȱʡ���õ����ݿ����
        /// </summary>
        /// <returns></returns>
        protected abstract MySqlDataBase CreateDefaultDataBase();

        /// <summary>
        /// ���޸ĸ���
        /// </summary>
        public bool UpdateByMidified { get; set; }

        /// <summary>
        ///     ���Ψһ��ʶ
        /// </summary>
        public abstract int TableId { get; }

        #endregion

        #region ���ݽṹ

        /// <summary>
        ///     ȫ���ȡ��SQL���
        /// </summary>
        protected abstract string FullLoadFields { get; }

        /// <summary>
        ///     ����
        /// </summary>
        protected abstract string ReadTableName { get; }
        /// <summary>
        ///     ����
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected virtual string DeleteSqlCode => $@"DELETE FROM `{WriteTableName}`";

        /// <summary>
        ///     �����SQL���
        /// </summary>
        protected abstract string InsertSqlCode { get; }

        /// <summary>
        ///     ȫ�����µ�SQL���
        /// </summary>
        protected abstract string UpdateSqlCode { get; }

        /// <summary>
        ///     ������ѯ����
        /// </summary>
        public string BaseCondition { get; set; }

        #endregion

        #region �ֶ��ֵ�

        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        protected abstract string PrimaryKey { get; }

        /// <summary>
        ///     �ֶ��ֵ�
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     �ֶ��ֵ�(���ʱ)
        /// </summary>
        public virtual Dictionary<string, string> FieldMap
        {
            get { return _fieldMap ?? (_fieldMap = Fields.ToDictionary(p => p, p => p)); }
        }

        /// <summary>
        ///     �����ֶ�(���ʱ)
        /// </summary>
        public abstract string[] Fields { get; }

        /// <summary>
        ///     �ֶ��ֵ�(��̬����)
        /// </summary>
        public Dictionary<string, string> OverrideFieldMap { get; set; }



        #endregion


        #region ��̬��������չ


        /// <summary>
        ///     ��̬��ȡ���ֶ�
        /// </summary>
        internal string _contextReadFields;

        /// <summary>
        ///     ��̬��ȡ���ֶ�
        /// </summary>
        protected string ContextLoadFields
        {
            get { return _contextReadFields ?? FullLoadFields; }
            set { _contextReadFields = string.IsNullOrWhiteSpace(value) ? null : value; }
        }
        /// <summary>
        ///     ��ǰ�����Ķ�ȡ�ı���
        /// </summary>
        protected virtual string ContextReadTable => _dynamicReadTable ?? ReadTableName;

        /// <summary>
        /// ��ǰ�����ĵĶ�ȡ��
        /// </summary>
        public Action<MySqlDataReader, TData> ContentLoadAction { get; set; }

        /// <summary>
        ///     ��̬��ȡ�ı�
        /// </summary>
        protected string _dynamicReadTable;

        /// <summary>
        ///     ȡ��ʵ�����õ�ContextReadTable��̬��ȡ�ı�
        /// </summary>
        /// <returns>֮ǰ�Ķ�̬��ȡ�ı���</returns>
        public string SetDynamicReadTable(string table)
        {
            var old = _dynamicReadTable;
            _dynamicReadTable = string.IsNullOrWhiteSpace(table) ? null : table;
            return old;
        }

        #endregion

        #region �򵥶�ȡ

        /// <summary>
        /// �򵥶�ȡSQL���
        /// </summary>
        public virtual string SimpleFields => FullLoadFields;


        /// <summary>
        /// �򵥶�ȡ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <param name="entity">��ȡ���ݵ�ʵ��</param>
        public virtual void SimpleLoad(MySqlDataReader reader, TData entity)
        {
            LoadEntity(reader, entity);
        }
        #endregion

        #region ���鷽��

        /// <summary>
        ///     ���ø������ݵ�����
        /// </summary>
        protected virtual void SetUpdateCommand(TData entity, MySqlCommand cmd)
        {
        }

        /// <summary>
        ///     ���ò������ݵ�����
        /// </summary>
        /// <returns>������˵��Ҫȡ����</returns>
        protected virtual bool SetInsertCommand(TData entity, MySqlCommand cmd)
        {
            return false;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <param name="entity">��ȡ���ݵ�ʵ��</param>
        protected virtual void LoadEntity(MySqlDataReader reader, TData entity)
        {
        }

        #endregion

        #region ������չ


        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnDataSaved(DataOperatorType operatorType, TData entity)
        {

        }


        /// <summary>
        ///    �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecuting(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        protected virtual void OnOperatorExecutd(DataOperatorType operatorType, string condition, IEnumerable<MySqlParameter> args)
        {
        }

        #endregion
    }
}