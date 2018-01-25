using System.Collections.Generic;
using System.Linq;

namespace Agebull.Common
{
    /// <summary>
    /// 随机ID生成
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        long GetId<T>();// where T : EntityBase;

        /// <summary>
        /// 获取一批Id
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="count">Id数量</param>
        /// <returns>ID集合</returns>
        IEnumerable<long> GetSomeIds<T>(int count);
        /// <summary>
        /// 所有者标识
        /// </summary>
        long OwnerId { get; set; }
        /// <summary>
        /// 组织标识
        /// </summary>
        long? OrgId { get; set; }
    }
    /// <summary>
    /// ID生成
    /// </summary>
    public class SmallFlakeIdGenerator : IIdGenerator
    {
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public long GetId<T>()// where T : EntityBase
        {
            return SmallFlakes<T>.Oxidize();
        }
        /// <summary>
        /// 生成一批ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<long> GetSomeIds<T>(int count)// where T : EntityBase
        {
            return Enumerable.Range(0, count).Select(index => SmallFlakes<T>.Oxidize());
        }

        /// <summary>
        /// 所有者标识
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// 组织标识
        /// </summary>
        public long? OrgId { get; set; }
    }
}
