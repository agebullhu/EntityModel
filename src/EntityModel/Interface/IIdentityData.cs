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
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IIdentityData
    {
        /// <summary>
        ///     ���ֱ�ʶ
        /// </summary>
        /// <value>int</value>
        long Id { get; set; }
    }

    /// <summary>
    ///     ��ʾʹ��ѩ����ķ�������������
    /// </summary>
    public interface ISnowFlakeId : IIdentityData
    {

    }
    /// <summary>
    ///     ��ʾ�б��������
    /// </summary>
    public interface ITitle
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <value>int</value>
        string Title { get; }
    }
    /// <summary>
    ///     ��ʾ��Ψһ���ֱ�ʶ������
    /// </summary>
    public interface IUnionUniqueEntity
    {
    }

}