using System.Collections.Generic;
using System.Linq;

namespace Yizuan.Service.Fundtion
{
    public interface IIdGenerator
    {
        long GetId<T>();// where T : EntityBase;

        /// <summary>
        /// 获取一批Id
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="count">Id数量</param>
        /// <returns>起始Id</returns>
        IEnumerable<long> GetSomeIds<T>(int count);

        long OwnerId { get; set; }

        long? OrgId { get; set; }
    }

    public class SmallFlakeIdGenerator : IIdGenerator
    {
        public long GetId<T>()// where T : EntityBase
        {
            return SmallFlakes<T>.Oxidize();
        }

        public IEnumerable<long> GetSomeIds<T>(int count)// where T : EntityBase
        {
            return Enumerable.Range(0, count).Select(index => SmallFlakes<T>.Oxidize());
        }

        public long OwnerId { get; set; }
        public long? OrgId { get; set; }
    }
}
