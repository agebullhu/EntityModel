/*****************************************************
(c)2016-2021 by ZeroTeam
����: ����ˮ
����: Agebull.EntityModel.CoreAgebull.DataModel
����:2016-06-07
�޸�: -
*****************************************************/

namespace Agebull.EntityModel.Common
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