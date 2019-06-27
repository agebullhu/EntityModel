// ���ڹ��̣�Agebull.EntityModel
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:35
// ����ʱ�䣺2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
using System.Linq;

using Agebull.Common.Frame;

#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   ��ʾ�ڲ�ԭ�Ӳ���(��������)��Χ
    /// </summary>
    public sealed class AtomLockScope : IDisposable
    {
        /// <summary>
        ///   ������
        /// </summary>
        private IAtomLock AtomLock;

        /// <summary>
        ///   ����
        /// </summary>
        /// <param name="al"> ���� </param>
        /// <param name="name"> ������������ </param>
        /// <returns> ������Χ�����֮ǰ��������Ϊ�� </returns>
        public static AtomLockScope CreateLock(object al, string name = null)
        {
            var ln = $"{al.GetType().FullName}_{al.GetHashCode()}_{name}";
            if (AtomLocks.Contains(ln))
            {
                return null;
            }
            var lockScope = new AtomLockScope
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
        ///   �Ƿ�������
        /// </summary>
        /// <param name="al"> ���� </param>
        /// <param name="name"> ������������ </param>
        /// <returns> ������Χ�����֮ǰ��������Ϊ�� </returns>
        /// <remarks>
        ///   �����Ǵ�Сд���е�
        /// </remarks>
        public static bool IsLock(IAtomLock al, string name)
        {
            return al.AtomLockNames != null && al.AtomLockNames.Contains(name);
        }

        /// <summary>
        ///   �Ƿ�������
        /// </summary>
        /// <param name="al"> ���� </param>
        /// <param name="name"> ������������ </param>
        /// <returns> ������Χ�����֮ǰ��������Ϊ�� </returns>
        /// <remarks>
        ///   �����Ǵ�Сд���е�
        /// </remarks>
        public static bool IsLocked(object al, string name)
        {
            var ln = $"{al.GetType().FullName}_{al.GetHashCode()}_{name}";
            return AtomLocks.Contains(ln);
        }

        /// <summary>
        ///   ����
        /// </summary>
        /// <param name="al"> ���� </param>
        /// <returns> ������Χ�����֮ǰ��������Ϊ�� </returns>
        /// <remarks>
        ///   �����Ǵ�Сд���е�
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
        ///   ����
        /// </summary>
        /// <param name="al"> ���� </param>
        /// <param name="name"> ������������ </param>
        /// <param name="elimination"> �ų�����ƣ����������Щ���Ƶ������������ٴ����� </param>
        /// <returns> ������Χ�����֮ǰ��������Ϊ�� </returns>
        /// <remarks>
        ///   �����Ǵ�Сд���е�
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
        ///   ������
        /// </summary>
        private static readonly List<string> AtomLocks = new List<string>();

        /// <summary>
        ///   ��ǰ��Χ���õĶ���
        /// </summary>
        private string LockName;

        /// <summary>
        ///   ����
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
