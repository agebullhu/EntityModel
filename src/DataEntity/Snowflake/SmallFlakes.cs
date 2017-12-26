// Copyright (c) 2013 - Jeremiah Peschka
//
// This file is provided to you under the Apache License,
// Version 2.0 (the "License"); you may not use this file
// except in compliance with the License.  You may obtain
// a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Yizuan.Service.Fundtion
{
    /// <summary>
    /// 模仿Twitter的SnowFlakes算法，根据开源代码更改的适用于本框架的生成一个根据时间递增，带有机器号和一个本地计数器的生成52位整型数的分布式Id生成器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SmallFlakes<T>
    {
        private static readonly long _epochTicks;

        //16000+ ids per second for each type

        //Cell's size cannot exceed 256 workers.

        //We can provider id for about 34 years

        //总共52位，额外一位用来标记紧急客户端生成的顺序Id

        private static int _counter;

        public static string MachineName { get; private set; }
        public static int WorkerId { get; private set; }

        static SmallFlakes()
        {
            InitMachineName();

            var epoch = new DateTime(2013, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            _epochTicks = epoch.Ticks;

            _counter = 0;

            Identifier = (ushort)WorkerId;
        }

        private static void InitMachineName()
        {
            MachineName = Guid.NewGuid().ToString();

            WorkerId = MachineName.GetHashCode() % (2 << 10);
        }

        /// <summary>
        /// 机器识别号，处于生成的Id中端位置，一个不长于10位的整型数
        /// </summary>
        public static ushort Identifier { get; set; }

        /// <summary>
        /// 获取一个新Id
        /// </summary>
        /// <returns></returns>
        public static long Oxidize()
        {
            var ct = CurrentTimeCounter();

            var counter = Interlocked.Increment(ref _counter);

            if ((ushort)_counter == 0)
            {
                ct = WaitForNextTimeCounter((uint)ct);

                counter = Interlocked.Increment(ref _counter);
            }

            var result = ((ct << SmallFlakes.TimestampShift) + ((long)Identifier << SmallFlakes.IdentifierShift) + (uint)counter);

            return result & SmallFlakes.Mask;
        }



        private static uint WaitForNextTimeCounter(uint ct)
        {
            while (true)
            {
                var timeCounter = CurrentTimeCounter();

                if (timeCounter != ct)
                {
                    return (uint)timeCounter;
                }
            }
        }

        private static long CurrentTimeCounter()
        {
            var utcNowTicks = DateTime.UtcNow.Ticks;

            //右移24位等于除以8388608，约等于每秒Ticks数10000000L
            return (uint)((utcNowTicks - _epochTicks) >> 23);
        }
    }

    internal static class SmallFlakes
    {
        internal static readonly int EncodingTimestampLength = (int)Math.Ceiling(Math.Log(Math.Pow(2, TimestampLength), 36));
        internal static readonly int EncodingWorkerLength = (int)Math.Ceiling(Math.Log(Math.Pow(2, WorkerLength), 36));
        internal static readonly int EncodingCounterLength = (int)Math.Ceiling(Math.Log(Math.Pow(2, CounterLength), 36));
        internal const int CounterLength = 14;
        internal const int WorkerLength = 8;
        internal const int TimestampLength = 30;
        internal const int TotalLength = CounterLength + WorkerLength + TimestampLength;
        internal const int IdentifierShift = CounterLength;
        internal const int TimestampShift = IdentifierShift + WorkerLength;
        internal const long Mask = 0xFFFFFFFFFFFFF;

        internal static readonly string[] elements =
           Enumerable.Range(0, 10).Select(number => number.ToString()).Union(
               Enumerable.Range(0, 26).Select(index => (char)('A' + (char)index)).Select(c => c.ToString())
               ).ToArray();

        internal static readonly int newBaseCount = elements.Length;

        internal static readonly string elementsTalbe = elements.Aggregate((s, s1) => s + s1);
    }
}
