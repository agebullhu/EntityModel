using System.Collections.Generic;

namespace Agebull.Common
{
    /// <summary>
    ///   表示一个原子锁对象
    /// </summary>
    public interface IAtomLock
    {
        /// <summary>
        ///   正在被锁定,应该用AtomLockNames是否为空来表示，或自行处理
        /// </summary>
        bool IsAtomLock { get ; }

        /// <summary>
        ///   正在被锁定名字集合
        /// </summary>
        List<string> AtomLockNames { get ; set ; }
    }
}