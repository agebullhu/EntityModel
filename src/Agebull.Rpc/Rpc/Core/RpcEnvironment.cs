using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Agebull.Zmq.Rpc
{
    //网络状态
    //网络命令
    using NET_COMMAND = UInt16;
    //网络命令状态 -0x10之后为自定义错误
    using COMMAND_STATE = Int32;

    /// <summary>
    ///     网络命令事件处理Handler
    /// </summary>
    /// <param name="arg"></param>
    public delegate void CommandHandler(CommandArgument sender, CommandArgument arg);

    /// <summary>
    /// 命令的环境相关的定义
    /// </summary>
    public class RpcEnvironment
    {
        #region 命令标识

        //系统通知
        public const ushort NET_COMMAND_SYSTEM_NOTIFY = 0x1;
        //业务通知
        public const ushort NET_COMMAND_BUSINESS_NOTIFY = 0x2;
        //命令请求(有返回)
        public const ushort NET_COMMAND_CALL = 0x3;
        //命令请求(有返回)
        public const ushort NET_COMMAND_RESULT = 0x4;

        #endregion
        #region 常量定义

        /// <summary>
        /// 序列化头大小（长度4+类型4+版本1+类型1）
        /// </summary>
        public const int SERIALIZE_HEAD_LEN = 10;
        /// <summary>
        /// 序列化基本大小（长度4+类型4+版本1+类型1+结束4）
        /// </summary>
        public const int SERIALIZE_BASE_LEN = 14;

        //数据修改通知
        public const NET_COMMAND NET_COMMAND_DATA_CHANGED = (0xFFFE);
        //数据推送
        public const NET_COMMAND NET_COMMAND_DATA_PUSH = (0xFFFF);

        //网络命令状态 -0x10之后为自定义错误
        //数据
        public const COMMAND_STATE NET_COMMAND_STATE_DATA = 0x0;
        //命令发送中
        public const COMMAND_STATE NET_COMMAND_STATE_SENDING = 0x7;
        //命令已发送
        public const COMMAND_STATE NET_COMMAND_STATE_SENDED = 0x8;
        //命令已在服务端排队
        public const COMMAND_STATE NET_COMMAND_STATE_WAITING = 0x9;
        //命令已执行完成
        public const COMMAND_STATE NET_COMMAND_STATE_SUCCEED = 0xA;
        //命令发送出错
        public const COMMAND_STATE NET_COMMAND_STATE_NETERROR = 0x6;
        //未知错误(未收到回执)
        public const COMMAND_STATE NET_COMMAND_STATE_UNKNOW = 0x5;
        //服务器未知错误(系统异常)
        public const COMMAND_STATE NET_COMMAND_STATE_SERVER_UNKNOW = 0x4;
        //本地未知错误(系统异常)
        public const COMMAND_STATE NET_COMMAND_STATE_CLIENT_UNKNOW = 0x3;
        //数据重复处理
        public const COMMAND_STATE NET_COMMAND_STATE_DATA_REPEAT = 0x2;
        //命令不允许执行
        public const COMMAND_STATE NET_COMMAND_STATE_CANNOT = 0x1;

        //参数错误
        public const COMMAND_STATE NET_COMMAND_STATE_ARGUMENT_INVALID = -1;
        //逻辑错误
        public const COMMAND_STATE NET_COMMAND_STATE_LOGICAL_ERROR = -2;
        //CRC错误
        public const COMMAND_STATE NET_COMMAND_STATE_CRC_ERROR = -3;
        //未知数据
        public const COMMAND_STATE NET_COMMAND_STATE_UNKNOW_DATA = -4;
        //已达最大重试次数
        public const COMMAND_STATE NET_COMMAND_STATE_RETRY_MAX = -5;
        //已超时
        public const COMMAND_STATE NET_COMMAND_STATE_TIME_OUT = -6;


        /// <summary>
        /// 最大重试次数
        /// </summary>
        public const int NET_COMMAND_RETRY_MAX = 5;

        //令牌字段长度
        public const int GUID_LEN = 34;
        //命令的网络头长度(不包含CRC字段)
        public const int NETCOMMAND_BODY_LEN = 76;
        //命令的网络头长度
        public const int NETCOMMAND_HEAD_LEN = 80;
        //命令的网络数据长度
        public const int NETCOMMAND_LEN = 84;

        /// <summary>
        /// 取得命令对象的实际长度
        /// </summary>
        /// <param name="cmd_call"></param>
        /// <returns></returns>
        public static int get_cmd_len(CommandArgument cmd_call)
        {
            return cmd_call.dataLen == 0 ? NETCOMMAND_LEN : (cmd_call.dataLen + NETCOMMAND_HEAD_LEN);
        }

        #endregion
        #region 运行状态

        #region 系统状态

        /// <summary>
        ///     当前系统处于哪个流程中
        /// </summary>
        public static RpcFlowType CurrentFlow { get; internal set; }

        /// <summary>
        ///     是否用户模式
        /// </summary>
        public static bool OnUserModel
        {
            get
            {
                return CurrentFlow > RpcFlowType.UserInitialized &&
                       CurrentFlow < RpcFlowType.EnforceProcessing;
            }
        }

        /// <summary>
        ///     用户是否可操作
        /// </summary>
        public static bool UserCanDo
        {
            get { return CurrentFlow == RpcFlowType.Browsing; }
        }

        #endregion
        /// <summary>
        ///     客户端用户标识
        /// </summary>
        /// <returns></returns>
        internal static byte[] Token { get; set; }

        /// <summary>
        ///     当前运行状态
        /// </summary>
        protected static volatile ZmqNetStatus m_netState = ZmqNetStatus.None;

        /// <summary>
        ///     运行状态
        /// </summary>
        /// <returns></returns>
        public static ZmqNetStatus NetState {get { return m_netState; } }

        /// <summary>
        ///     当前启动了多少命令线程
        /// </summary>
        protected static volatile int m_commandThreadCount;

        /// <summary>
        ///     当前启动了多少命令线程
        /// </summary>
        /// <returns></returns>
        public static int CommandThreadCount {get { return m_commandThreadCount; } }

        /// <summary>
        ///     登记线程开始
        /// </summary>
        public static void set_command_thread_start()
        {
            m_commandThreadCount++;
            //Console.WriteLine("网络处理线程数量%d.启动", command_thread_count);
        }

        /// <summary>
        ///     登记线程关闭
        /// </summary>
        public static void set_command_thread_end()
        {
            m_commandThreadCount--;
            //Console.WriteLine("网络处理线程数量%d.关闭", command_thread_count);
        }


        #endregion

        #region 命令缓存


        /// <summary>
        /// 命令缓存字典（RequstId，Argument）
        /// </summary>
        private static readonly Dictionary<string, CommandArgument> m_requestCommands =
            new Dictionary<string, CommandArgument>();

        /// <summary>
        /// 缓存命令
        /// </summary>
        /// <param name="command"></param>
        public static void CacheCommand(CommandArgument command)
        {
            command.TimeSpamp = DateTime.Now;
            if (!m_requestCommands.ContainsKey(command.token))
                m_requestCommands.Add(command.token, command);
        }

        /// <summary>
        /// 同步到缓存命令中
        /// </summary>
        /// <param name="command"></param>
        public static CommandArgument SyncCacheCommand(CommandArgument command)
        {
            if (!m_requestCommands.ContainsKey(command.token))
                return null;
            var old = m_requestCommands[command.token];
            old.cmdState = command.cmdState;
            old.Data = command.Data;
            command.TimeSpamp = DateTime.Now;
            return old;
        }

        /// <summary>
        /// 删除缓存命令
        /// </summary>
        /// <param name="command"></param>
        public static void RemoveCacheCommand(CommandArgument command)
        {
            m_requestCommands.Remove(command.token);
        }


        /// <summary>
        /// 删除缓存命令
        /// </summary>
        public static void ClearCacheCommand()
        {
            m_requestCommands.Clear();
        }

        /// <summary>
        ///     超时检测
        /// </summary>
        public static void TimeOutCheckTask()
        {
            set_command_thread_start();
            while (NetState <= ZmqNetStatus.Runing)
            {
                Thread.Sleep(10000);
                var date = DateTime.Now;
                var array = m_requestCommands.Values.ToArray();
                foreach (var cmd in array)
                {
                    if (cmd.cmdState == NET_COMMAND_STATE_TIME_OUT)
                        continue;
                    if ((date - cmd.TimeSpamp).Minutes < 1)
                        continue;
                    cmd.OnEnd?.Invoke(cmd);
                    cmd.cmdState = NET_COMMAND_STATE_TIME_OUT;
                    cmd.OnRequestStateChanged(cmd);
                }
            }
            set_command_thread_end();
        }
        #endregion
    }

}
