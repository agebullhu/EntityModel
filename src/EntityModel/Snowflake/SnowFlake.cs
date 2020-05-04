/*
 声明 : 本代码来自第三方,版权归原始作者所有
 */
using System;
using System.Globalization;
using System.Net;

namespace Agebull.Common
{
    /// <summary>  
    /// 动态生产有规律的ID  
    /// </summary>  
    public class SnowFlake
    {
        private static long _machineId;//机器ID  
        private static long _dataCenterId;//数据ID  
        private static long _sequence;//计数从零开始  

        private const long Twepoch = 0L; //唯一时间随机量

        private const long MachineIdBits = 5L; //机器码字节数  
        private const long DataCenterIdBits = 5L; //数据中心ID长度  
        private const long MaxMachineId = -1L ^ -1L << (int)MachineIdBits; //最大支持机器节点数0~31，一共32个  
        private const long MaxDataCenterId = -1L ^ (-1L << (int)DataCenterIdBits); //最大支持数据中心节点数0~31，一共32个 

        private const long SequenceBits = 12L; //序列号12位，12个字节用来保存计数码，机器节点左移12位        
        private const long MachineIdShift = SequenceBits; //机器码数据左移位数，就是后面计数器占用的位数，数据中心节点左移17位
        private const long DataCenterIdShift = SequenceBits + MachineIdBits;
        private const long TimestampLeftShift = SequenceBits + MachineIdBits + DataCenterIdBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数，时间毫秒数左移22位  
        private const long SequenceMask = -1L ^ -1L << (int)SequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微秒在进行生成，4095  
        private static long _lastTimestamp = -1L;//最后时间戳  

        private static readonly object SyncRoot = new object();//加锁对象  
        private static SnowFlake _snowflake;

        #region 初始化部分
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static SnowFlake Instance()
        {
            if (_snowflake == null) _snowflake = new SnowFlake();
            GetMachineId();
            return _snowflake;
        }
        /// <summary>
        /// 快捷使用的方法
        /// </summary>
        public static long NewId => Instance().GetSerialId();
        /// <summary>
        /// 构造
        /// </summary>
        public SnowFlake()
        {
            Snowflakes(0L, -1);
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="machineId"></param>
        public SnowFlake(long machineId)
        {
            Snowflakes(machineId, -1);
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="dataCenterId"></param>
        public SnowFlake(long machineId, long dataCenterId)
        {
            Snowflakes(machineId, dataCenterId);
        }
        #endregion


        #region 生成SerialID部分
        private static void GetMachineId()
        {
            // ReSharper disable once ConstantNullCoalescingCondition
            string hostName = Dns.GetHostName() ?? "XJXX-001";
            var idx = hostName.IndexOf("-", StringComparison.Ordinal) + 1;
            string machineId = hostName.Substring(idx, hostName.Length - idx);
            //_machineId = int.Parse(machineId);
            _machineId = int.TryParse(machineId, out var id) ? id : 1;
        }
        private static void Snowflakes(long machineId, long dataCenterId)
        {
            if (machineId >= 0)
            {
                if (machineId > MaxMachineId)
                {
                    throw new SystemException("机器码ID非法");
                }
                _machineId = machineId;
            }
            if (dataCenterId >= 0)
            {
                if (dataCenterId > MaxDataCenterId)
                {
                    throw new SystemException("数据中心ID非法");
                }
                _dataCenterId = dataCenterId;
            }
        }

        /// <summary>  
        /// 生成当前时间戳  
        /// </summary>  
        /// <returns>毫秒</returns>  
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>  
        /// 获取下一微秒时间戳  
        /// </summary>  
        /// <param name="lastTimestamp"></param>  
        /// <returns></returns>  
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            if (timestamp <= lastTimestamp)
            {
                timestamp = GetTimestamp();
            }
            return timestamp;
        }

        /// <summary>  
        /// 获取长整形的ID  
        /// </summary>  
        /// <returns></returns>  
        public long GetSerialId()
        {
            long id;
            lock (SyncRoot)
            {
                long timestamp = GetTimestamp();
                if (_lastTimestamp == timestamp)
                {
                    //同一微秒中生成ID , 用&运算计算该微秒内产生的计数是否已经到达上限  
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                    {
                        //一微秒内产生的ID计数已达上限，等待下一微秒 
                        timestamp = GetNextTimestamp(_lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID  
                    _sequence = 0L;
                }
                if (timestamp < _lastTimestamp)
                {
                    throw new SystemException("时间戳比上一次生成ID时时间戳还小，故异常");
                }
                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳  
                id = ((timestamp - Twepoch) << (int)TimestampLeftShift) |
                          (_dataCenterId << (int)DataCenterIdShift) |
                          (_machineId << (int)MachineIdShift) |
                          _sequence;
            }
            return id;
        }
        #endregion


        #region 解读SerialID部分
        /// <summary>
        /// 解读时间戳部分
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetSerialString(long serialId)
        {
            //取出时间戳
            return GetTime(serialId >> (int)TimestampLeftShift).ToString(CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// UNIX时间戳到日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public DateTime GetTime(long timeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
        #endregion
    }
}