namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ��ʾ��������֧�������㼶
    /// </summary>
    public interface IInnerTree
    {
        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <value>string</value>
        long ParentId { get; set; }

    }
}