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
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        long NewID { get; }

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
}
