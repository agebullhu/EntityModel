namespace Agebull.Common.Tson
{
    /// <summary>
    /// TSON操作器
    /// </summary>
    /// <typeparam name="TEntity">操作的数据类型</typeparam>
    public interface ITsonOperator<in TEntity>
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void ToTson(ITsonSerializer serializer, TEntity value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        void FromTson(ITsonDeserializer serializer, TEntity value);
    }
}