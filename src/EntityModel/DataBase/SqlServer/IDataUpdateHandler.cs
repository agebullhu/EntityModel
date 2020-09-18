// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

using System.Data.SqlClient;

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     ���ݸ��²�����
    /// </summary>
    public interface IDataUpdateHandler<TData>
        where TData : EditDataObject, new()
    {
        /// <summary>
        /// �Ƿ��ڷ���������֮�󴥷�������Ϊ�����ڷ��������ط���֮�������������ڷ��������ط���֮ǰ����
        /// </summary>
        bool AfterInner { get; }

        /// <summary>
        ///     ����ǰ����(Insert��Update)
        /// </summary>
        /// <param name="access">�����¼������ݷ��ʶ���</param>
        /// <param name="entity">����Ķ���</param>
        /// <param name="subsist">��ǰʵ������״̬</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        void PrepareSave(SqlServerTable<TData> access,TData entity, EntitySubsist subsist);

        /// <summary>
        ///     ������ɺ��ڴ���(Insert��Update)
        /// </summary>
        /// <param name="access">�����¼������ݷ��ʶ���</param>
        /// <param name="entity"></param>
        /// <param name="subsist"></param>
        void EndSaved(SqlServerTable<TData> access, TData entity, EntitySubsist subsist);

        /// <summary>
        ///     ִ��SQL���ǰ����
        /// </summary>
        /// <param name="access">�����¼������ݷ��ʶ���</param>
        /// <param name="cmd">ִ�е��������</param>
        /// <param name="entity">����Ķ���</param>
        /// <param name="subsist">��ǰʵ������״̬</param>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        void PrepareExecSql(SqlServerTable<TData> access, SqlCommand cmd, TData entity, EntitySubsist subsist);
    }
}