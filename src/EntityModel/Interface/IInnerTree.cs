namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ��ʾ��������֧�������㼶
    /// </summary>
    public interface IInnerTree<TPrimaryKey>
    {
        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <value>string</value>
        TPrimaryKey ParentId { get; set; }

    }

    /// <summary>
    ///     ��ʾ��������֧�������㼶
    /// </summary>
    public interface IInnerTree : IInnerTree<long>
    {

    }
}