// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System.Collections.Generic;
using System.Text;
using Agebull.Common.Frame;

#endregion

namespace Agebull.Common.Security.Certificate.X509
{
    /// <summary>
    ///   反编译类，用于将证书序列号转化为一个十进制数字符串,后面通过比较该字符串来确定使用证书
    /// </summary>
    internal static class Asn1IntegerConverter
    {
        private static readonly char[] digitMap = new[]
        {
                '0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9'
        };

        private static readonly List<byte[]> powersOfTwo = new List<byte[]>(new[]
        {
                new byte[]
                {
                        1
                }
        });

        private static void AddSecondDecimalToFirst(IList<byte> first, byte[] second)
        {
            byte num = 0;
            if (first == null || second == null)
                return;
            for (int i = 0; (i < second.Length) || (i < first.Count); i++)
            {
                byte num3;
                if (i >= first.Count)
                {
                    first.Add(0);
                }
                if (i < second.Length)
                {
                    num3 = (byte)((first[i] + second[i]) + num);
                }
                else
                {
                    num3 = (byte)(first[i] + num);
                }
                first[i] = (byte)(num3 % 10);
                num = (byte)(num3 / 10);
            }
            if (num > 0)
            {
                first.Add(num);
            }
        }

        public static string Asn1IntegerToDecimalString(byte[] asn1)
        {
            if (asn1 == null || asn1.Length == 0)
            {
                return string.Empty;
            }
            byte num2;
            //if (asn1 == null)
            //{
            //    throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("asn1");
            //}
            //if (asn1.Length == 0)
            //{
            //    throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("asn1", System.IdentityModel.SR.GetString("LengthOfArrayToConvertMustGreaterThanZero")));
            //}
            List<byte> first = new List<byte>((asn1.Length * 8) / 3);
            int n = 0;
            for (int i = 0; i < (asn1.Length - 1); i++)
            {
                num2 = asn1[i];
                for (int k = 0; k < 8; k++)
                {
                    if ((num2 & 1) == 1)
                    {
                        AddSecondDecimalToFirst(first, TwoToThePowerOf(n));
                    }
                    n++;
                    num2 = (byte)(num2 >> 1);
                }
            }
            num2 = asn1[asn1.Length - 1];
            for (int j = 0; j < 7; j++)
            {
                if ((num2 & 1) == 1)
                {
                    AddSecondDecimalToFirst(first, TwoToThePowerOf(n));
                }
                n++;
                num2 = (byte)(num2 >> 1);
            }
            StringBuilder builder = new StringBuilder(first.Count + 1);
            List<byte> list2;
            if (num2 == 0)
            {
                list2 = first;
            }
            else
            {
                List<byte> list3 = new List<byte>(TwoToThePowerOf(n));
                SubtractSecondDecimalFromFirst(list3, first);
                list2 = list3;
                builder.Append('-');
            }
            int num6 = list2.Count - 1;
            while (num6 >= 0)
            {
                if (list2[num6] != 0)
                {
                    break;
                }
                num6--;
            }
            while (num6 >= 0)
            {
                builder.Append(digitMap[list2[num6--]]);
            }
            return builder.ToString();
        }

        private static void SubtractSecondDecimalFromFirst(IList<byte> first, IList<byte> second)
        {
            if (first == null || second == null)
            {
                return;
            }
            byte num = 0;
            for (int i = 0; i < second.Count; i++)
            {
                int num3 = (first[i] - second[i]) - num;
                if (num3 < 0)
                {
                    num = 1;
                    first[i] = (byte)(num3 + 10);
                }
                else
                {
                    num = 0;
                    first[i] = (byte)num3;
                }
            }
            if (num <= 0)
                return;
            for (int j = second.Count; j < first.Count; j++)
            {
                int num5 = (byte)(first[j] - num);
                if (num5 < 0)
                {
                    first[j] = (byte)(num5 + 10);
                }
                else
                {
                    first[j] = (byte)num5;
                    return;
                }
            }
        }

        private static byte[] TwoToThePowerOf(int n)
        {
            using (ThreadLockScope.Scope(powersOfTwo))
            {
                if (n >= powersOfTwo.Count)
                {
                    for (int i = powersOfTwo.Count; i <= n; i++)
                    {
                        List<byte> list = new List<byte>(powersOfTwo[i - 1]);
                        byte num2 = 0;
                        for (int j = 0; j < list.Count; j++)
                        {
                            byte num4 = (byte)((list[j] << 1) + num2);
                            list[j] = (byte)(num4 % 10);
                            num2 = (byte)(num4 / 10);
                        }
                        if (num2 > 0)
                        {
                            list.Add(num2);
                        }
                        powersOfTwo.Add(list.ToArray());
                    }
                }
                return powersOfTwo[n];
            }
        }
    }
}
