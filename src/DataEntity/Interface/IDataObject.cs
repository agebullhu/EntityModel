// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     ������һ�����ݶ���
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        ///     ����ֵ
        /// </summary>
        /// <param name="source">���Ƶ�Դ�ֶ�</param>
        void CopyValue(IDataObject source);

        /// <summary>
        ///     �õ��ֶε�ֵ
        /// </summary>
        /// <param name="field"> �ֶε����� </param>
        /// <returns> �ֶε�ֵ </returns>
        object GetValue(string field);

        /// <summary>
        ///     �����ֶε�ֵ
        /// </summary>
        /// <param name="field"> �ֶε����� </param>
        /// <param name="value"> �ֶε�ֵ </param>
        void SetValue(string field, object value);
    }
}