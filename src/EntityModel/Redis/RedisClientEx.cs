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
        /// ����������
        /// </summary>
        public string LastError;

        /// <summary>
        /// �Ƿ��ڴ���״̬(�ɼ���������)
        /// </summary>
        public bool IsFailed
        {
            get { return Status != 0; }
        }

        /// <summary>
        /// �Ƿ��ڴ���״̬
        /// </summary>
        public bool IsError
        {
            get { return Status != 0; }
        }

        /// <summary>
        /// �Ƿ��ڴ���״̬(���ɼ���������)
        /// </summary>
        public bool IsBad
        {
            get { return Status < 0; }
        }

        /// <summary>
        /// ״̬����,��0Ϊ����,����Ϊ�߼�����,����Ϊ��������
        /// -1 δ֪�ڲ�����
        /// -2 �����ݴ���
        /// -3 ������δ��ȷ����
        /// -4 �����������
        /// 1 ������
        /// 2 �������ʹ���(��ʶ)
        /// 3 �������ʹ���(ת��)
        /// </summary>
        public int Status;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="host">����</param>
        /// <param name="port">�˿�</param>
        /// <param name="password">����</param>
        /// <param name="db">ʹ���ĸ�DB</param>

        public RedisClientEx(string host, int port = 6379, string password = null, long db = 0L)
            : base(host, port, password, db)
        {
        }

        #region Set��չ
        /// <summary>
        /// ����һ�����ݵ�Set
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
        /// ����һ�����ݵ�Set
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
        /// Setɨ�践��ֵ
        /// </summary>
        public class SetScanResult
        {
            /// <summary>
            /// ��һ�ε��α�,Ϊ0��ʾ����
            /// </summary>
            public int NextCursor;

            /// <summary>
            /// ��ȡ��������,���н���
            /// </summary>
            public readonly List<byte[]> Values = new List<byte[]>();
            /// <summary>
            /// �Ƿ�������
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
        /// ɨ��Set
        /// </summary>
        /// <param name="setId">Key</param>
        /// <param name="cursor">�α�</param>
        /// <param name="count">����</param>
        /// <returns>��ǰ���ص�����</returns>
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

        #region SortedSet��չ

        /// <summary>
        /// ����ߵ�λ��ϵķ�ֵ
        /// </summary>
        /// <param name="setId">Key�Ķ�����</param>
        /// <param name="dataId">Id</param>
        /// <param name="h">���صĸ�λ</param>
        /// <param name="l">���صĵ�λ</param>
        /// <returns>true��ʾ�ɹ�ȡ��ֵ</returns>
        /// <remarks>l��hֵ�������0xFFFFFF�����ض�</remarks>>
        public long ZAdd(byte[] setId, long dataId, long h, long l)
        {
            return ZAdd(setId, dataId.ToUtf8Bytes(), h, l);
        }

        /// <summary>
        /// ����ߵ�λ��ϵķ�ֵ
        /// </summary>
        /// <param name="setId">Key�Ķ�����</param>
        /// <param name="dataId">Id</param>
        /// <param name="h">���صĸ�λ</param>
        /// <param name="l">���صĵ�λ</param>
        /// <returns>true��ʾ�ɹ�ȡ��ֵ</returns>
        /// <remarks>l��hֵ�������0xFFFFFF�����ض�</remarks>>
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
        /// ���ߵ�λ����������Ϸ�Χȡ��SortedSet�е�ID
        /// </summary>
        /// <param name="setId">Key�Ķ�����</param>
        /// <param name="h">���صĸ�λ</param>
        /// <param name="l">���صĵ�λ</param>
        /// <returns>true��ʾ�ɹ�ȡ��ֵ</returns>
        /// <remarks>l��hֵ�������0xFFFFFF�����ض�</remarks>>
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
        /// ��ȡSortedSet�ķ�ֵ(�ߵ�λ��������)
        /// </summary>
        /// <param name="setId">Key�Ķ�����</param>
        /// <param name="dataId">Id�Ķ�����</param>
        /// <param name="h">���صĸ�λ</param>
        /// <param name="l">���صĵ�λ</param>
        /// <returns>true��ʾ�ɹ�ȡ��ֵ</returns>
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

        #region ��ȡ����ֵ��չ

        /// <summary>
        /// ������
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
        /// ���ı�
        /// </summary>
        /// <returns>�ı�</returns>
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
            this.ReadLine();//����
            return this.ReadLine();//ֵ
        }

        /// <summary>
        /// �з���ֵ
        /// </summary>
        public class LineResult
        {
            /// <summary>
            /// ����
            /// -1 ������
            /// -2 ϵͳ����
            /// -3 ���ʹ���
            /// 0 ԭʼ�ı�
            /// 1 �����ı�
            /// 2 byte����
            /// </summary>
            public int type;

            /// <summary>
            /// �ı�ֵ
            /// </summary>
            public string sValue;

            /// <summary>
            /// byte����ֵ
            /// </summary>
            public byte[] bValue;
        }

        /// <summary>
        /// ��һ�з���ֵ
        /// </summary>
        /// <returns>��¼����ֵ�Ķ���</returns>
        private LineResult ReadSingleLine()
        {
            LineResult result = new LineResult();
            ReadSingleLine(result);
            return result;
        }

        /// <summary>
        /// ��һ�з���ֵ
        /// </summary>
        /// <param name="result">��¼����ֵ�Ķ���</param>
        /// <returns>false��ʾ��������</returns>
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