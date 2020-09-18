namespace Agebull.Common.Tson
{
    /// <summary>
    /// �ֶ�����
    /// </summary>
    public enum TsonDataType : byte
    {
        /// <summary>
        /// ��֪(����)
        /// </summary>
        None,
        /// <summary>
        /// bool
        /// </summary>
        Boolean,
        /// <summary>
        /// byte
        /// </summary>
        Byte,
        /// <summary>
        /// sbyte
        /// </summary>
        SByte,
        /// <summary>
        /// short
        /// </summary>
        Short,
        /// <summary>
        /// ushort
        /// </summary>
        UShort,
        /// <summary>
        /// int
        /// </summary>
        Int,
        /// <summary>
        /// uint
        /// </summary>
        UInt,
        /// <summary>
        /// long
        /// </summary>
        Long,
        /// <summary>
        /// ulong
        /// </summary>
        ULong,
        /// <summary>
        /// decimal
        /// </summary>
        Decimal,
        /// <summary>
        /// float
        /// </summary>
        Float,
        /// <summary>
        /// double
        /// </summary>
        Double,
        /// <summary>
        /// GUID
        /// </summary>
        Guid,
        /// <summary>
        /// ����ʱ��
        /// </summary>
        DateTime,
        /// <summary>
        /// ����
        /// </summary>
        Object = 0xF0,
        /// <summary>
        /// ����
        /// </summary>
        Array = 0xF1,
        /// <summary>
        /// �ı�
        /// </summary>
        String = 0xF2,
        /// <summary>
        /// ��ֵ(�ı�\����)
        /// </summary>
        Empty = 0xFE,
        /// <summary>
        /// ��ֵ
        /// </summary>
        Nil = 0xFF
    };
}