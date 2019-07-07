namespace Agebull.Common.DataModel
{
    /// <summary>
    /// 全局对象基类
    /// </summary>
    public abstract class GlobalBase
    {
        /// <summary>
        /// 开始
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// 更新缓存
        /// </summary>
        public abstract void FlushCache();

        /// <summary>
        /// 析构
        /// </summary>
        public abstract void Dispose();
    }
}