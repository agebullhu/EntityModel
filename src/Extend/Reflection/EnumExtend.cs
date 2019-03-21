using System.Linq;

namespace System
{
    /// <summary>
    /// 枚举静态扩展
    /// </summary>
    public static class EnumExtend
    {
        /// <summary>
        ///   包括枚举的全部值吗
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="values"> </param>
        /// <returns> </returns>
        public static bool Equals(this Enum source, params Enum[] values)
        {
            return values.All(source.Equals);
        }

        /// <summary>
        ///   包括枚举的全部值吗
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="values"> </param>
        /// <returns> </returns>
        public static bool EqualsSome(this Enum source, params Enum[] values)
        {
            return values.Contains(source);
        }

        /// <summary>
        ///   包括枚举的全部值吗
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="values"> </param>
        /// <returns> </returns>
        public static bool HasFlags(this Enum source, params Enum[] values)
        {
            return values.All(source.HasFlag);
        }

        /// <summary>
        ///   包含枚举的一或多个值吗
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="values"> </param>
        /// <returns> </returns>
        public static bool HasSomeFlags(this Enum source, params Enum[] values)
        {
            return values.Any(source.HasFlag);
        }
    }
}