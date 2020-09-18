using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     ���ݸ��´�����
    /// </summary>
    public interface IDataUpdateTrigger
    {
        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        void OnPrepareSave(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        void OnDataSaved(EditDataObject entity, DataOperatorType operatorType);

        /// <summary>
        ///     �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="entityId">ʵ������ID</param>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        void OnOperatorExecuting(int entityId, string condition, IEnumerable<MySqlParameter> args,
            DataOperatorType operatorType);

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="entityId">ʵ������ID</param>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        void OnOperatorExecutd(int entityId, string condition, IEnumerable<MySqlParameter> args,
            DataOperatorType operatorType);
    }
}