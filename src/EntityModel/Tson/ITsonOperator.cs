namespace Agebull.Common.Tson
{
    /// <summary>
    /// TSON������
    /// </summary>
    /// <typeparam name="TEntity">��������������</typeparam>
    public interface ITsonOperator<in TEntity>
    {
        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void ToTson(ITsonSerializer serializer, TEntity value);

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void FromTson(ITsonDeserializer serializer, TEntity value);
    }
}