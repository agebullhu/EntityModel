namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 全局对象基类
    /// </summary>
    public abstract class GlobalBase
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static GlobalBase Signle { get; set; }

        /// <summary>
        /// 构造上下文
        /// </summary>
        public abstract void CreateContext();

        /// <summary>
        /// 依赖内容注册
        /// </summary>
        public abstract void DependantRegist();

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