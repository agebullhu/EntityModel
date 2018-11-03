using System.Collections.Generic;
using System.Linq;

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    /// ���ݱ�����
    /// </summary>
    public abstract class DataTableOption
    {
        /// <summary>
        /// ��ʽ��Ϊ����SQL������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ToSqlName(string name) => $"{FieldLeftChar}{name}{FieldRightChar}";

        /// <summary>
        /// ��ʽ��Ϊ����SQL������
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string PropertyToSqlName(string property) => $"{FieldLeftChar}{FieldDictionary[property]}{FieldRightChar}";


        /// <summary>
        /// �ֶ�����ַ�
        /// </summary>
        public char FieldLeftChar => '[';

        /// <summary>
        /// �ֶ��Ҳ��ַ�
        /// </summary>
        public char FieldRightChar => ']';

        /// <summary>
        /// ����ǰ���ַ�
        /// </summary>
        public char ArgumentChar => '$';

        /// <summary>
        ///     ȫ���ȡ��SQL���
        /// </summary>
        protected abstract string FullLoadFields { get; }

        /// <summary>
        ///     ������
        /// </summary>
        protected abstract string ReadTableName { get; }
        /// <summary>
        ///     д����
        /// </summary>
        protected abstract string WriteTableName { get; }

        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected virtual string DeleteSqlCode => $@"DELETE FROM [{WriteTableName}]";

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

        #region ���ݽṹ

        /// <summary>
        ///     �ֶ��ֵ�(����ʱ)
        /// </summary>
        public Dictionary<string, string> FieldDictionary => OverrideFieldMap ?? FieldMap;

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        private string _keyField;

        /// <summary>
        ///     �����ֶ�(�ɶ�̬����PrimaryKey)
        /// </summary>
        public string KeyField
        {
            get
            {
                if (_keyField != null)
                    return _keyField;
                return _keyField = PrimaryKey;
            }
            set => _keyField = value;
        }

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
    }
}