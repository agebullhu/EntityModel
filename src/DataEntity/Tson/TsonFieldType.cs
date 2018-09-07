namespace Agebull.Common.Tson
{
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum TsonDataType : byte
    {
        /// <summary>
        /// 求知(错误)
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
        /// 日期时间
        /// </summary>
        DateTime,
        /// <summary>
        /// 对象
        /// </summary>
        Object = 0xF0,
        /// <summary>
        /// 数组
        /// </summary>
        Array = 0xF1,
        /// <summary>
        /// 文本
        /// </summary>
        String = 0xF2,
        /// <summary>
        /// 空值(文本\数组)
        /// </summary>
        Empty = 0xFE,
        /// <summary>
        /// 无值
        /// </summary>
        Nil = 0xFF
    };
}