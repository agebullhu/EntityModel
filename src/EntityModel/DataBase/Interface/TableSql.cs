// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using Agebull.EntityModel.Events;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// ���ݱ���Ϣ
    /// </summary>
    public class DataTableInfomation
    {
        /// <summary>
        ///     ������
        /// </summary>
        public string ReadTableName { get; set; }

        /// <summary>
        ///     д����
        /// </summary>
        public string WriteTableName { get; set; }

        /// <summary>
        ///     �����SQL���
        /// </summary>
        public string InsertSqlCode { get; }

        /// <summary>
        ///     ȫ�����µ�SQL���
        /// </summary>
        public string UpdateSqlCode { get; }

        /// <summary>
        ///     ȫ���ȡ��SQL���
        /// </summary>
        public string FullLoadFields { get; set; }

        /// <summary>
        ///     ������ѯ����
        /// </summary>
        public string BaseCondition { get; set; }

        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     �����ֶ�(���ʱ)
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        ///     �ֶ��ֵ�
        /// </summary>
        private Dictionary<string, string> _fieldMap;

        /// <summary>
        ///     �ֶ��ֵ�(���ʱ)
        /// </summary>
        public Dictionary<string, string> FieldMap => _fieldMap ??= Fields.ToDictionary(p => p, p => p);

    }

}