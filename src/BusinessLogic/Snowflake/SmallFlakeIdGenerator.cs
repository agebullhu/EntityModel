using System.Collections.Generic;
using System.Linq;

namespace Agebull.Common
{
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
        /// 快捷使用的方法
        /// </summary>
        public long NewID => SmallFlakes<long>.Oxidize();

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
