using System.Threading;

using Agebull.Common.Base;

namespace Agebull.Common.Frame
{
    /// <summary>
    /// 线程锁范围
    /// </summary>
    /// <remarks>
    /// 利用Monitor在线程内可重复锁定一个对象和排斥其它线程使用这个对象的特点设计的锁范围
    /// 在线程内可以多次锁定这个对象而不出错,而其它线程却需要等待锁定对象的线程结束范围后方可使用这个对象,
    /// 用于保证锁定对象单元操作时的稳定性(不被意外地修改)
    /// 注意:锁定的对象不能是值类型,因为值类型是被复制而不是被引用,这会导致锁定失败
    /// </remarks>
    public class ThreadLockScope : ScopeBase
    {
        /// <summary>
        /// 锁定对象
        /// </summary>
        private readonly object _lockObject;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="lockObject">要锁定的一或多个对象</param>
        public static ThreadLockScope Scope(object lockObject)
        {
            return new ThreadLockScope(lockObject);
        }

        /// <summary>
        /// 是否正确锁定
        /// </summary>
        public readonly bool isLocked;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="lockObject">要锁定的一或多个对象</param>
        public ThreadLockScope(object lockObject)
        {
            _lockObject = lockObject;
            isLocked = Monitor.TryEnter(lockObject);
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void OnDispose()
        {
            if (!isLocked)
                return;
            Monitor.Exit(_lockObject);
        }
    }
}