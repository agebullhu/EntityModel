// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
namespace Agebull.Common
{
    /// <summary>
    ///   对于BYTE型的扩展
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        public static byte[] ToByte(ulong i)
        {
            var b = new byte[4] ;
            b[0] = (byte) (i << 16 >> 24) ;
            b[1] = (byte) (i << 24 >> 24) ;
            b[2] = (byte) (i << 8 >> 24) ;
            b[3] = (byte) (i >> 24) ;
            return b ;
        }

        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        public static byte[] ToByte(int i)
        {
            var b = new byte[4] ;
            b[0] = (byte) (i << 16 >> 24) ;
            b[1] = (byte) (i << 24 >> 24) ;
            b[3] = (byte) (i >> 24) ;
            b[2] = (byte) (i << 8 >> 24) ;
            return b ;
        }

        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="i"> </param>
        /// <returns> </returns>
        public static byte[] ToByte(short i)
        {
            var b = new byte[2] ;
            b[0] = (byte) (i >> 8) ;
            b[1] = (byte) (i << 8 >> 8) ;
            return b ;
        }

        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> </returns>
        public static byte[] ToByte(ushort number)
        {
            var b = new byte[2] ;
            b[0] = (byte) (number >> 8) ;
            b[1] = (byte) (number << 8 >> 8) ;
            return b ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> </returns>
        public static short ToShort(byte[] number)
        {
            return (short) (number[1] | (number[0] << 8)) ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> </returns>
        public static ushort ToUshort(byte[] number)
        {
            return (ushort) (number[1] | (number[0] << 8)) ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="number"> </param>
        /// <returns> </returns>
        public static int ToInt(byte[] number)
        {
            return number[1] + ((number[0]) << 8) + ((number[3]) << 16) + ((number[2]) << 24) ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="b"> </param>
        /// <param name="idx"> </param>
        /// <returns> </returns>
        public static ushort ToUshort(byte[] b , int idx)
        {
            return (ushort) (b[idx + 1] | (b[idx] << 8)) ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="b"> </param>
        /// <param name="idx"> </param>
        /// <returns> </returns>
        public static int ToInt(byte[] b , int idx)
        {
            return b[idx + 1] + ((b[idx]) << 8) + ((b[idx + 3]) << 16) + ((b[idx + 2]) << 24) ;
        }

        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="i"> </param>
        /// <param name="b"> </param>
        /// <param name="idx"> </param>
        public static void ToByte(int i , byte[] b , int idx)
        {
            b[idx] = (byte) (i << 16 >> 24) ;
            b[idx + 1] = (byte) (i << 24 >> 24) ;
            b[idx + 3] = (byte) (i >> 24) ;
            b[idx + 2] = (byte) (i << 8 >> 24) ;
        }

        /// <summary>
        ///   转换为BYTE
        /// </summary>
        /// <param name="i"> </param>
        /// <param name="b"> </param>
        /// <param name="idx"> </param>
        public static void ToByte(ushort i , byte[] b , int idx)
        {
            b[idx] = (byte) (i >> 8) ;
            b[idx + 1] = (byte) (i << 8 >> 8) ;
        }

        /// <summary>
        ///   转换为为数字
        /// </summary>
        /// <param name="b"> </param>
        /// <returns> </returns>
        public static ulong ToNumber(byte[] b)
        {
            return b[3] + (((ulong) b[2]) << 8) + (((ulong) b[1]) << 16) + (((ulong) b[0]) << 24) ;
        }
    }
}
