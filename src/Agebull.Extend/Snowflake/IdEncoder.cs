using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yizuan.Service.Fundtion
{
    public static class IdEncoder
    {
        /// <summary>
        /// 将一个52位整型数专为36进制编码的字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Encoding(long id)
        {
            var sb = new StringBuilder();

            var uId = (ulong)id;

            var timstampSegment = uId << 64 - SmallFlakes.TimestampLength - SmallFlakes.WorkerLength - SmallFlakes.CounterLength >> 64 - SmallFlakes.TimestampLength;

            Encoding((long)timstampSegment, SmallFlakes.EncodingTimestampLength, sb);

            var workerSegment = uId << 64 - SmallFlakes.WorkerLength - SmallFlakes.CounterLength >> 64 - SmallFlakes.WorkerLength;

            Encoding((long)workerSegment, SmallFlakes.EncodingWorkerLength, sb);

            var localSegment = uId << 64 - SmallFlakes.CounterLength >> 64 - SmallFlakes.CounterLength;

            Encoding((long)localSegment, SmallFlakes.EncodingCounterLength, sb);

            return sb.ToString();
        }

        /// <summary>
        /// 将上述方法得到的36进制编码专为对应的52位整型数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static long Decoding(string code)
        {
            string timestampSegment = code.Substring(0, SmallFlakes.EncodingTimestampLength);

            string workerSegment = code.Substring(SmallFlakes.EncodingTimestampLength, SmallFlakes.EncodingWorkerLength);

            string localSegment = code.Substring(SmallFlakes.EncodingTimestampLength + SmallFlakes.EncodingWorkerLength, SmallFlakes.EncodingCounterLength);

            long result = DecodingSubcode(timestampSegment);

            result = result << SmallFlakes.WorkerLength;

            result += DecodingSubcode(workerSegment);

            result = result << SmallFlakes.CounterLength;

            result += DecodingSubcode(localSegment);

            return result;
        }

        private static void Encoding(long segment, int maxLength, StringBuilder stringBuilder)
        {
            var buildStack = new Stack<string>();

            while (true)
            {
                var index = segment % SmallFlakes.elements.Length;

                buildStack.Push(SmallFlakes.elements[index]);

                segment = segment / SmallFlakes.elements.Length;

                if (segment == 0)
                {
                    break;
                }
            }

            foreach (var padLeft in Enumerable.Repeat("0", maxLength - buildStack.Count))
            {
                buildStack.Push(padLeft);
            }

            foreach (var element in buildStack)
            {
                stringBuilder.Append(element);
            }
        }

        private static long DecodingSubcode(string subSegment)
        {
            long result = 0;

            var buildStack = new Stack<char>();

            foreach (var number in subSegment.ToCharArray())
            {
                buildStack.Push(number);
            }

            int length = buildStack.Count;

            for (int i = 0; i < length; i++)
            {
                result += SmallFlakes.elementsTalbe.IndexOf(buildStack.Pop()) * (long)Math.Pow(SmallFlakes.newBaseCount, i);
            }

            return result;
        }
    }
}