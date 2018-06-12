// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
using System.Linq;

using Agebull.Common.Frame;

#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   表示内部原子操作(不可重入)范围
    /// </summary>
    public sealed class AtomLockScope : IDisposable
    {
        /// <summary>
        ///   锁对象
        /// </summary>
        private IAtomLock AtomLock;

        /// <summary>
        ///   构造
        /// </summary>
        /// <param name="al"> 对象 </param>
        /// <param name="name"> 对象锁的名字 </param>
        /// <returns> 锁定范围，如果之前已锁定则为空 </returns>
        public static AtomLockScope CreateLock(object al, string name = null)
        {
            string ln = $"{al.GetType().FullName}_{al.GetHashCode()}_{name}";
            if (AtomLocks.Contains(ln))
            {
                return null;
            }
            AtomLockScope lockScope = new AtomLockScope
            {
                LockName = ln
            };
            using (ThreadLockScope.Scope(AtomLocks))
            {
                AtomLocks.Add(ln);
            }
            return lockScope;
        }

        /// <summary>
        ///   是否已锁定
        /// </summary>
        /// <param name="al"> 对象 </param>
        /// <param name="name"> 对象锁的名字 </param>
        /// <returns> 锁定范围，如果之前已锁定则为空 </returns>
        /// <remarks>
        ///   名称是大小写敏感的
        /// </remarks>
        public static bool IsLock(IAtomLock al, string name)
        {
            return al.AtomLockNames != null && al.AtomLockNames.Contains(name);
        }

        /// <summary>
        ///   是否已锁定
        /// </summary>
        /// <param name="al"> 对象 </param>
        /// <param name="name"> 对象锁的名字 </param>
        /// <returns> 锁定范围，如果之前已锁定则为空 </returns>
        /// <remarks>
        ///   名称是大小写敏感的
        /// </remarks>
        public static bool IsLocked(object al, string name)
        {
            string ln = string.Format("{0}_{1}_{2}", al.GetType().FullName, al.GetHashCode(), name);
            return AtomLocks.Contains(ln);
        }
        /// <summary>
        ///   构造
        /// </summary>
        /// <param name="al"> 对象 </param>
        /// <returns> 锁定范围，如果之前已锁定则为空 </returns>
        /// <remarks>
        ///   名称是大小写敏感的
        /// </remarks>
        public static AtomLockScope CreateLock(IAtomLock al)
        {
            if (al.AtomLockNames == null)
            {
                al.AtomLockNames = new List<string>();
            }
            else if (al.AtomLockNames.Count > 0)
            {
                return null;
            }
            using (ThreadLockScope.Scope(al))
            {
                al.AtomLockNames.Add("AtomLockScope");
            }
            return new AtomLockScope
            {
                AtomLock = al
            };
        }

        /// <summary>
        ///   构造
        /// </summary>
        /// <param name="al"> 对象 </param>
        /// <param name="name"> 对象锁的名字 </param>
        /// <param name="elimination"> 排斥的名称，即如果有这些名称的锁定将不能再次锁定 </param>
        /// <returns> 锁定范围，如果之前已锁定则为空 </returns>
        /// <remarks>
        ///   名称是大小写敏感的
        /// </remarks>
        public static AtomLockScope CreateLock(IAtomLock al, string name, params string[] elimination)
        {
            if (al.AtomLockNames == null)
            {
                al.AtomLockNames = new List<string>();
            }
            else if (al.AtomLockNames.Contains(name))
            {
                return null;
            }
            else if (elimination != null && elimination.Length > 0)
            {
                if (elimination.Any(e => al.AtomLockNames.Contains(e)))
                {
                    return null;
                }
            }

            using (ThreadLockScope.Scope(al))
            {
                al.AtomLockNames.Add(name);
            }
            return new AtomLockScope
            {
                AtomLock = al,
                LockName = name
            };
        }

        /// <summary>
        ///   锁定表
        /// </summary>
        private static readonly List<string> AtomLocks = new List<string>();

        /// <summary>
        ///   当前范围作用的对象
        /// </summary>
        private string LockName;

        /// <summary>
        ///   构造
        /// </summary>
        private AtomLockScope()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (AtomLock != null)
            {
                using (ThreadLockScope.Scope(AtomLock))
                {
                    if (LockName == null)
                        AtomLock.AtomLockNames.Clear();
                    else
                        AtomLock.AtomLockNames.Remove(LockName);
                }
            }
            else
            {
                using (ThreadLockScope.Scope(AtomLocks))
                {
                    AtomLocks.Remove(LockName);
                }
            }
        }
    }
}
