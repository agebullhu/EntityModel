using System;

namespace Agebull.MicroZero
{
    /// <summary>
    ///     API��������
    /// </summary>
    [Flags]
    public enum ApiAccessOption
    {
        /// <summary>
        ///     ���ɷ���
        /// </summary>
        None,

        /// <summary>
        ///     ��������(������Ȩ)
        /// </summary>
        Public = 0x1,

        /// <summary>
        ///     �ڲ�����(��Ҫ��Ȩ)
        /// </summary>
        Internal = 0x2,

        /// <summary>
        ///     �ο�
        /// </summary>
        Anymouse = 0x4,

        /// <summary>
        ///     �ͻ�
        /// </summary>
        Customer = 0x10,

        /// <summary>
        ///     �ڲ�Ա��
        /// </summary>
        Employe = 0x20,

        /// <summary>
        ///     �̼��û�
        /// </summary>
        Business = 0x40,

        /*// <summary>
        ///     ��չ�û�����3
        /// </summary>
        User1 = 0x80,

        /// <summary>
        ///     ��չ�û�����2
        /// </summary>
        User2 = 0x100,

        /// <summary>
        ///     ��չ�û�����3
        /// </summary>
        User3 = 0x200,

        /// <summary>
        ///     ��չ�û�����4
        /// </summary>
        User4 = 0x400,

        /// <summary>
        ///     ��չ�û�����5
        /// </summary>
        User5 = 0x800,

        /// <summary>
        ///     ��չ�û�����6
        /// </summary>
        User6 = 0x1000,

        /// <summary>
        ///     ��չ�û�����7
        /// </summary>
        User7 = 0x4000,

        /// <summary>
        ///     ��չ�û�����8
        /// </summary>
        User8 = 0x8000,*/

        /// <summary>
        ///     ��������Ϊnull
        /// </summary>
        ArgumentCanNil = 0x10000,

        /// <summary>
        ///     ������Ϊ����,��ʵ��ʹ��
        /// </summary>
        ArgumentIsDefault = 0x20000
    }
}