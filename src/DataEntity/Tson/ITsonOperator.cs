namespace Agebull.Common.Tson
{
    /// <summary>
    /// TSON������
    /// </summary>
    /// <typeparam name="TData">��������������</typeparam>
    public interface ITsonOperator<in TData>
    {
        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void ToTson(ITsonSerializer serializer, TData value);

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void FromTson(ITsonDeserializer serializer, TData value);
    }
}