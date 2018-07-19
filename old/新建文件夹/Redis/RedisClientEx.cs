using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.Redis;

namespace Agebull.Common.DataModel.Redis
{
    public class RedisClientEx : RedisClient
    {
        /// <summary>
        /// 最后操作错误
        /// </summary>
        public string LastError;

        /// <summary>
        /// 是否处于错误状态(可继续操作的)
        /// </summary>
        public bool IsFailed
        {
            get { return Status != 0; }
        }

        /// <summary>
        /// 是否处于错误状态
        /// </summary>
        public bool IsError
        {
            get { return Status != 0; }
        }

        /// <summary>
        /// 是否处于错误状态(不可继续操作的)
        /// </summary>
        public bool IsBad
        {
            get { return Status < 0; }
        }

        /// <summary>
        /// 状态代码,仅0为正常,正数为逻辑问题,负数为致命问题
        /// -1 未知内部错误
        /// -2 流数据错乱
        /// -3 流数据未正确结束
        /// -4 发送命令错误
        /// 1 空数据
        /// 2 数据类型错误(标识)
        /// 3 数据类型错误(转换)
        /// </summary>
        public int Status;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="password">密码</param>
        /// <param name="db">使用哪个DB</param>

        public RedisClientEx(string host, int port = 6379, string password = null, long db = 0L)
            : base(host, port, password, db)
        {
        }

        #region Set扩展
        /// <summary>
        /// 加入一个数据到Set
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long SAdd(byte[] setId, long value)
        {
            return this.SendExpectLong(new[]
            {
                Commands.SAdd, 
                setId,
                value.ToUtf8Bytes() 
            });
        }

        /// <summary>
        /// 加入一组数据到Set
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long SAdd(byte[] setId, IEnumerable<long> values)
        {
            var longs = values as long[];
            var array = longs ?? values.ToArray();

            byte[][] cmd = new byte[2 + array.Length][];
            cmd[0] = Commands.SAdd;
            cmd[1] = setId;
            for (int i = 0; i < array.Length; i++)
            {
                cmd[i + 2] = array[i].ToUtf8Bytes();
            }
            return this.SendExpectLong(cmd);
        }

        /// <summary>
        /// Set扫描返回值
        /// </summary>
        public class SetScanResult
        {
            /// <summary>
            /// 下一次的游标,为0表示结束
            /// </summary>
            public int NextCursor;

            /// <summary>
            /// 读取到的数据,自行解析
            /// </summary>
            public readonly List<byte[]> Values = new List<byte[]>();
            /// <summary>
            /// 是否有数据
            /// </summary>
            public bool HaseValue
            {
                get { return Values.Count > 0; }
            }
        }

        public static readonly byte[] cSScan = "SSCAN".ToUtf8Bytes();
        public static readonly byte[] cMatch = "MATCH".ToUtf8Bytes();
        public static readonly byte[] cAnyone = "*".ToUtf8Bytes();
        public static readonly byte[] cCount = "COUNT".ToUtf8Bytes();

        /// <summary>
        /// 扫描Set
        /// </summary>
        /// <param name="setId">Key</param>
        /// <param name="cursor">游标</param>
        /// <param name="count">总数</param>
        /// <returns>当前返回的数据</returns>
        public SetScanResult ScanSet(byte[] setId, long cursor, long count = 10L)
        {
            var cmd = new[] { cSScan, setId, cursor.ToUtf8Bytes(), cCount, count.ToUtf8Bytes() };//cMatch, cAnyone,
            if (!this.SendCommand(cmd))
            {
                Status = -1;
                LastError = string.Join(" ", "SSCAN", Encoding.UTF8.GetString(setId), cursor, count);
                return null;
            }
            SetScanResult result = new SetScanResult();

            LineResult line = new LineResult();

            if (!ReadSingleLine(line))
            {
                return result;
            }
#if DEBUG
            if (line.type != 1)
            {
                this.Status = 2;
                return result;
            }
            int lines;
            if (!int.TryParse(line.sValue, out lines) || lines <= 0)
            {
                this.Status = 2;
                return result;
            }
#endif
            result.NextCursor = int.Parse(this.ReadString());
            var cnt = ReadNumber();
            for (int i = 0; i < cnt; i++)
            {
                if (!ReadSingleLine(line))
                {
                    return result;
                }
#if DEBUG
                if (line.type != 2)
                {
                    this.Status = 3;
                    return result;
                }
#endif
                result.Values.Add(line.bValue);
            }
            return result;
        }
        #endregion

        #region SortedSet扩展

        /// <summary>
        /// 加入高低位组合的分值
        /// </summary>
        /// <param name="setId">Key的二进制</param>
        /// <param name="dataId">Id</param>
        /// <param name="h">返回的高位</param>
        /// <param name="l">返回的低位</param>
        /// <returns>true表示成功取到值</returns>
        /// <remarks>l或h值如果大于0xFFFFFF将被截断</remarks>>
        public long ZAdd(byte[] setId, long dataId, long h, long l)
        {
            return ZAdd(setId, dataId.ToUtf8Bytes(), h, l);
        }

        /// <summary>
        /// 加入高低位组合的分值
        /// </summary>
        /// <param name="setId">Key的二进制</param>
        /// <param name="dataId">Id</param>
        /// <param name="h">返回的高位</param>
        /// <param name="l">返回的低位</param>
        /// <returns>true表示成功取到值</returns>
        /// <remarks>l或h值如果大于0xFFFFFF将被截断</remarks>>
        public long ZAdd(byte[] setId, byte[] dataId, long h, long l)
        {
            return this.SendExpectLong(new[]
            {
                Commands.ZAdd, 
                setId, 
                string.Concat(h & 0xFFFFFF, '.', l & 0xFFFFFF).ToUtf8Bytes(),
                dataId
            });
        }

        /// <summary>
        /// 按高低位两个数字组合范围取出SortedSet中的ID
        /// </summary>
        /// <param name="setId">Key的二进制</param>
        /// <param name="h">返回的高位</param>
        /// <param name="l">返回的低位</param>
        /// <returns>true表示成功取到值</returns>
        /// <remarks>l或h值如果大于0xFFFFFF将被截断</remarks>>
        public List<long> ZRangeByScore(byte[] setId, long h, long l)
        {
            var vl = string.Concat(h & 0xFFFFFF, '.', l & 0xFFFFFF).ToUtf8Bytes();

            var command = new[] { Commands.ZRangeByScore, setId, vl, vl };
            var bytes = this.SendExpectMultiData(command);
            if (bytes == null || bytes.Length == 0)
                return new List<long>();
            return bytes.Select(bf => bf.Length == 4 ? ByteCommond.BytesToInt(bf) : ByteCommond.BytesToLong(bf)).ToList();
        }

        /// <summary>
        /// 读取SortedSet的分值(高低位两个数字)
        /// </summary>
        /// <param name="setId">Key的二进制</param>
        /// <param name="dataId">Id的二进制</param>
        /// <param name="h">返回的高位</param>
        /// <param name="l">返回的低位</param>
        /// <returns>true表示成功取到值</returns>
        public bool ZScore(byte[] setId, byte[] dataId, out int h, out int l)
        {
            if (setId == null)
            {
                throw new ArgumentNullException("setId");
            }
            var command = new[] { Commands.ZScore, setId, dataId };
            h = 0;
            if (!this.SendCommand(command))
            {
                Status = -1;
                LastError = string.Join(" ", "ZScore", Encoding.UTF8.GetString(setId), ByteCommond.BytesToLong(dataId));
                l = 0;
                return false;
            }
            var line = ReadSingleLine();
            if (line.type != 2)
            {
                Status = 2;
                l = 0;
                return false;
            }
            StringBuilder sb = new StringBuilder();
            foreach (char by in line.bValue)
            {
                if (by == '.')
                {
                    h = int.Parse(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(by);
                }
            }
            l = int.Parse(sb.ToString());
            return true;
        }
        #endregion

        #region 读取返回值扩展

        /// <summary>
        /// 读数字
        /// </summary>
        /// <returns></returns>
        private int ReadNumber()
        {
            int state = this.Bstream.ReadByte();
            if (state == -1)
            {
                Status = 1;
                return -1;
            }
            if (state == '*')
                return int.Parse(this.ReadLine());
            Status = 2;
            return -1;
        }

        /// <summary>
        /// 读文本
        /// </summary>
        /// <returns>文本</returns>
        private string ReadString()
        {
            int state = this.Bstream.ReadByte();
            if (state == -1)
            {
                Status = 1;
                return null;
            }
            if (state != '$')
            {
                Status = 2;
                return null;
            }
            this.ReadLine();//长度
            return this.ReadLine();//值
        }

        /// <summary>
        /// 行返回值
        /// </summary>
        public class LineResult
        {
            /// <summary>
            /// 类型
            /// -1 无数据
            /// -2 系统错误
            /// -3 类型错误
            /// 0 原始文本
            /// 1 数字文本
            /// 2 byte数组
            /// </summary>
            public int type;

            /// <summary>
            /// 文本值
            /// </summary>
            public string sValue;

            /// <summary>
            /// byte类型值
            /// </summary>
            public byte[] bValue;
        }

        /// <summary>
        /// 读一行返回值
        /// </summary>
        /// <returns>记录返回值的对象</returns>
        private LineResult ReadSingleLine()
        {
            LineResult result = new LineResult();
            ReadSingleLine(result);
            return result;
        }

        /// <summary>
        /// 读一行返回值
        /// </summary>
        /// <param name="result">记录返回值的对象</param>
        /// <returns>false表示发生错误</returns>
        private bool ReadSingleLine(LineResult result)
        {
            result.type = this.Bstream.ReadByte();
            if (result.type == -1)
            {
                Status = 1;
                return false;
            }
            switch ((char)result.type)
            {
                case ':':
                    result.type = 0;
                    result.sValue = this.ReadLine();
                    return true;
                case '-':
                    result.type = -2;
                    Status = 100;
                    LastError = this.ReadLine();
                    return false;
                case '*':
                    result.type = 1;
                    result.sValue = this.ReadLine();
                    return true;
                case '$':
                    result.type = 2;
                    {
                        int num;
                        result.sValue = this.ReadLine();
                        if (result.sValue[0] == '-')
                        {
                            return true;
                        }
                        if (!int.TryParse(result.sValue, out num))
                        {
                            Status = -2;
                            LastError = ("Invalid length");
                            return false;
                        }
                        result.bValue = new byte[num];
                        int offset = 0;
                        while (num > 0)
                        {
                            int num3 = this.Bstream.Read(result.bValue, offset, num);
                            if (num3 <= 0)
                            {
                                Status = -2;
                                LastError = ("Unexpected end of Stream");
                                return false;
                            }
                            offset += num3;
                            num -= num3;
                        }
                        if ((this.Bstream.ReadByte() != 13) || (this.Bstream.ReadByte() != 10))
                        {
                            Status = -3;
                            LastError = ("Invalid termination");
                            return false;
                        }
                        return true;
                    }
                default:
                    Status = -1;
                    result.type = -3;
                    LastError = "Unexpected reply: " + this.ReadLine();
                    return false;
            }
        }
        #endregion
    }
}