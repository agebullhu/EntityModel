using Agebull.EntityModel.Common;
using System;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// Redis操作接口
    /// </summary>
    public interface IRedisBase : IDisposable
    {
        /// <summary>
        /// 配置
        /// </summary>
        RedisOption Option { get; set; }

        /// <summary>
        /// 配置变更
        /// </summary>
        /// <param name="option"></param>
        void OnOptionChange(RedisOption option);

        /// <summary>
        /// 当前数据库
        /// </summary>
        long CurrentDb { get; }

        /// <summary>
        /// 不关闭
        /// </summary>
        bool NoClose { get; }

        /// <summary>
        /// 原始对象,在不够用时扩展
        /// </summary>
        T Original<T>() where T : class;

        /// <summary>
        /// 更改Db
        /// </summary>
        void ChangeDb(int db);

    }
}