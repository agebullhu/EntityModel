using System.Collections.Generic;

namespace Agebull.Common
{
    /// <summary>
    ///   ��ʾһ��ԭ��������
    /// </summary>
    public interface IAtomLock
    {
        /// <summary>
        ///   ���ڱ�����,Ӧ����AtomLockNames�Ƿ�Ϊ������ʾ�������д���
        /// </summary>
        bool IsAtomLock { get ; }

        /// <summary>
        ///   ���ڱ��������ּ���
        /// </summary>
        List<string> AtomLockNames { get ; set ; }
    }
}