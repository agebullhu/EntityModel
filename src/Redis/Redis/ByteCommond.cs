using System;
using System.IO;
using System.Text;

namespace Agebull.Common.DataModel
{
    /// <summary>
    /// 数字与BYTE的转换辅助对象
    /// </summary>
    public static class ByteCommond
    {
        #region 二进制

        /// <summary>
        ///     跳过空数据
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        /// <param name="type">数据类型</param>
        /// <returns>否表明数据已损坏,应该中止读取</returns>
        public static bool SkipEmpty(BinaryReader reader, byte type)
        {
            switch ((int)type)
            {
                case 0:
                    return false;
                case 255:
                    reader.ReadChar();
                    break;
                case 254:
                    reader.ReadString();
                    break;
                default:
                    reader.BaseStream.Position += type;
                    break;
            }
            return true;
        }
        #endregion
        #region 转为BYTE
        /// <summary>
        ///     文本变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this string value)
        {
            return String.IsNullOrWhiteSpace(value) ? new byte[0] : Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this int value)
        {
            var buffer = new byte[4];
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 0x10);
            buffer[3] = (byte)(value >> 0x18);
            return buffer;
        }

        /// <summary>
        ///     小数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this decimal value)
        {
            var bits = decimal.GetBits(value);
            var buffer = new byte[16];
            buffer[0] = (byte)bits[0];
            buffer[1] = (byte)(bits[0] >> 8);
            buffer[2] = (byte)(bits[0] >> 0x10);
            buffer[3] = (byte)(bits[0] >> 0x18);
            buffer[4] = (byte)bits[1];
            buffer[5] = (byte)(bits[1] >> 8);
            buffer[6] = (byte)(bits[1] >> 0x10);
            buffer[7] = (byte)(bits[1] >> 0x18);
            buffer[8] = (byte)bits[2];
            buffer[9] = (byte)(bits[2] >> 8);
            buffer[10] = (byte)(bits[2] >> 0x10);
            buffer[11] = (byte)(bits[2] >> 0x18);
            buffer[12] = (byte)bits[3];
            buffer[13] = (byte)(bits[3] >> 8);
            buffer[14] = (byte)(bits[3] >> 0x10);
            buffer[15] = (byte)(bits[3] >> 0x18);
            return buffer;
        }
        /// <summary>
        ///     长整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByte(this long value)
        {
            var buffer = new byte[8];
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 0x10);
            buffer[3] = (byte)(value >> 0x18);
            buffer[4] = (byte)(value >> 0x20);
            buffer[5] = (byte)(value >> 40);
            buffer[6] = (byte)(value >> 0x30);
            buffer[7] = (byte)(value >> 0x38);
            return buffer;
        }
        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe byte[] ToByte(this float value)
        {
            var buffer = new byte[4];
            uint num = *((uint*)&value);
            buffer[0] = (byte)num;
            buffer[1] = (byte)(num >> 8);
            buffer[2] = (byte)(num >> 0x10);
            buffer[3] = (byte)(num >> 0x18);
            return buffer;
        }
        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe byte[] ToByte(this double value)
        {
            var buffer = new byte[8];
            ulong num = *((ulong*)&value);
            buffer[0] = (byte)num;
            buffer[1] = (byte)(num >> 8);
            buffer[3] = (byte)(num >> 0x18);
            buffer[4] = (byte)(num >> 0x20);
            buffer[5] = (byte)(num >> 40);
            buffer[6] = (byte)(num >> 0x30);
            buffer[7] = (byte)(num >> 0x38);
            return buffer;
        }

        /// <summary>
        ///     文本变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] StringBytes(string value)
        {
            return ToByte( value);
        }

        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] IntBytes(int value)
        {
            return ToByte( value);
        }
        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] DecimalBytes(decimal value)
        {
            return ToByte( value);
        }

        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] LongToBytes(long value)
        {
            return ToByte( value);
        }

        /// <summary>
        ///     整数变为字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] DoubleToBytes(double value)
        {
            return ToByte( value);
        }
        #endregion
        #region BYTE反转

        /// <summary>
        ///     字节变为文本
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            return Encoding.UTF8.GetString(bytes );
        }

        /// <summary>
        ///     字节变为小数
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static decimal BytesToDecimal(this byte[] buffer)
        {
            if (buffer == null || buffer.Length < 16)
                return 0M;
            return new decimal(new[]
            {
                ((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18),
                ((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18),
                ((buffer[8] | (buffer[9] << 8)) | (buffer[10] << 0x10)) | (buffer[11] << 0x18),
                ((buffer[12] | (buffer[13] << 8)) | (buffer[14] << 0x10)) | (buffer[15] << 0x18)
            });

        }

        /// <summary>
        ///     字节变为长整数
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static long BytesToLong(this byte[] buffer)
        {
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            long num2 = buffer.Length == 4 ? 0L : (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));
            return (num2 << 0x20) | num;
        }

        /// <summary>
        ///     字节变为整数
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int BytesToInt(this byte[] buffer)
        {
            return (((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
        }

        /// <summary>
        ///     字节变为整数
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static unsafe double BytesToDouble(this byte[] buffer)
        {
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            ulong num2 = (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));
            ulong num3 = (num2 << 0x20) | num;
            return *(((double*)&num3));
        }
        /// <summary>
        /// 转换到Bool
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static bool BytesToBoolean(this byte[] buffer)
        {
            return (buffer[0] != 0);
        }

        /// <summary>
        /// 转换到Short
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static short BytesToShort(this byte[] buffer)
        {
            return (short)(buffer[0] | (buffer[1] << 8));
        }
        /// <summary>
        /// 转换到Float
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static unsafe float BytesSingle(this byte[] buffer)
        {
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            return *(((float*)&num));
        }

        /// <summary>
        /// 转到ushort
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ushort BytesUInt16(this byte[] buffer)
        {
            return (ushort)(buffer[0] | (buffer[1] << 8));
        }

        /// <summary>
        /// 转到Uint
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static uint BytesUInt32(this byte[] buffer)
        {
            return (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
        }

        /// <summary>
        /// 转到uint64
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static ulong BytesUInt64(this byte[] buffer)
        {
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            ulong num2 = (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));
            return ((num2 << 0x20) | num);
        }
        #endregion


    }
}
