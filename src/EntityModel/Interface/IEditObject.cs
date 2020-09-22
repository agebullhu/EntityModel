// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     �༭����
    /// </summary>
    public interface IEditObject : IDataObject
    {
        /// <summary>
        ///     �����޸�
        /// </summary>
        void AcceptChanged();

        /// <summary>
        /// ����Ϊδ�޸�
        /// </summary>
        void RejectChanged();

        /// <summary>
        ///     �Ƿ��޸�
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        ///     �Ƿ���ɾ��
        /// </summary>
        bool IsDelete { get; }

        /// <summary>
        ///     �Ƿ�����
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        ///     �Ƿ��޸�
        /// </summary>
        /// <param name="propertyIndex"> �ֶε���� </param>
        bool FieldIsModified(int propertyIndex);

        /// <summary>
        ///     ����Ϊ�Ǹı�
        /// </summary>
        /// <param name="propertyIndex"> �ֶε���� </param>
        void SetUnModify(int propertyIndex);

        /// <summary>
        ///     ����Ϊ�ı�
        /// </summary>
        /// <param name="propertyIndex"> �ֶε���� </param>
        void SetModify(int propertyIndex);

        /// <summary>
        ///     �����޸ĵĺ��ڴ���(�����)
        /// </summary>
        /// <remarks>
        ///     �Ե�ǰ��������Եĸ���,�����б���,���򽫶�ʧ
        /// </remarks>
        void LaterPeriodByModify();
    }
}