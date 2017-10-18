using System;
using System.Collections.Generic;
using System.Threading;
using NetMQ;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    ///     ZMQ命令执行泵
    /// </summary>
    public abstract class ZmqCommandPump<TSocket> : IDisposable
        where TSocket : NetMQSocket
    {
        #region 基础流程
        /// <summary>
        /// 泵名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 运行起来
        /// </summary>
        public void Run()
        {
            m_state = 3;
            DoRun();
            m_state = 4;
        }
        /// <summary>
        /// 线程
        /// </summary>
        internal Thread thread;
        /// <summary>
        /// 启动泵任务
        /// </summary>
        protected virtual void DoRun()
        {
            thread = new Thread(RunPump)
            {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        /// 重启次数
        /// </summary>
        private int m_restart;

        /// <summary>
        /// 是否已析构
        /// </summary>
        public int RestartCount {get { return m_restart; } }
        /// <summary>
        /// 执行状态
        /// </summary>
        private int m_state;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized { get { return m_state > 0; } }
        

        /// <summary>
        /// 是否已析构
        /// </summary>
        public bool IsDisposed { get { return m_state > 4; } }

        /// <summary>
        /// 状态 0 初始状态,1 正在初始化,2 完成初始化,3 开始运行,4 运行中,5 正在析构 6 完成析构
        /// </summary>
        public int State {get { return m_state; } }

        /// <summary>
        ///     初始化客户端
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
                return;
            m_state = 1;
            m_queue = new Queue<CommandArgument>();
            m_mutex = new Mutex();
            DoInitialize();
            m_state = 2;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void DoInitialize();

        /// <summary>
        ///     清理
        /// </summary>
        public void Dispose()
        {
            m_state = 5;
            if (m_mutex != null)
            {
                m_mutex.Dispose();
                m_mutex = null;
            }
            DoDispose();
            m_state = 6;
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void DoDispose()
        {

        }

        #endregion

        #region 消息队列

        /// <summary>
        ///     C端的命令调用队列
        /// </summary>
        private Queue<CommandArgument> m_queue;

        /// <summary>
        ///     C端命令调用队列锁
        /// </summary>
        private Mutex m_mutex;

        /// <summary>
        ///     取得队列数据
        /// </summary>
        protected CommandArgument Pop()
        {
            if (!m_mutex.WaitOne(100))
            {
                return null;
            }
            if (m_queue.Count == 0)
            {
                m_mutex.ReleaseMutex();
                Thread.Sleep(1);
                return null;
            }
            var mdMsg = m_queue.Dequeue();
            m_mutex.ReleaseMutex();
            return mdMsg;
        }

        /// <summary>
        ///     数据写入队列
        /// </summary>
        /// <param name="cmdMsg"></param>
        protected void Push(CommandArgument cmdMsg)
        {
            m_mutex.WaitOne();
            m_queue.Enqueue(cmdMsg);
            m_mutex.ReleaseMutex();
        }

        #endregion

        #region 命令请求

        /// <summary>
        /// 默认超时设置
        /// </summary>
        protected readonly TimeSpan timeOut = new TimeSpan(0, 0, 0, 1);

        /// <summary>
        /// ZMQ服务地址
        /// </summary>
        public string ZmqAddress { get; set; }

        /// <summary>
        /// 配置Socket对象
        /// </summary>
        /// <param name="socket"></param>
        protected abstract void OptionSocktet(TSocket socket);

        /// <summary>
        /// 连接处理
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void OnConnected(TSocket socket)
        {

        }

        //private NetMQMonitor monitor;
        /// <summary>
        /// 执行工作
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>返回状态,其中-1会导致重连</returns>
        protected abstract int DoWork(TSocket socket);
        
        /// <summary>
        /// 生成SOCKET对象
        /// </summary>
        /// <remarks>版本不兼容导致改变</remarks>
        protected abstract TSocket CreateSocket();

        /// <summary>
        ///     执行处理泵
        /// </summary>
        /// <returns></returns>
        protected void RunPump()
        {
            //登记线程开始
            RpcEnvironment.set_command_thread_start();
            //monitor?.Stop();
            int state = 0;
            Console.WriteLine($"命令请求泵({ZmqAddress})正在启动");
            using (var socket = CreateSocket())
            {
                //monitor = new NetMQMonitor(socket, $"inproc://pump_{Guid.NewGuid()}.rep", SocketEvents.All);
                OptionSocktet(socket);
                socket.Connect(ZmqAddress);
                OnConnected(socket);

                Console.WriteLine($"命令请求泵({ZmqAddress})已启动");

                while (RpcEnvironment.NetState == ZmqNetStatus.Runing)
                {
                    state = DoWork(socket);
                    if (state == -1)
                        break;
                }
                socket.Disconnect(ZmqAddress);
            }
            //登记线程关闭
            RpcEnvironment.set_command_thread_end();

            OnTaskEnd(state);
        }

        /// <summary>
        /// 当任务结束时调用(基类实现为异常时重新执行)
        /// </summary>
        /// <param name="state">状态</param>
        protected virtual void OnTaskEnd(int state)
        {
            if (state == -1 && RpcEnvironment.NetState == ZmqNetStatus.Runing)
            {
                Thread.Sleep(10);
                m_restart++;
                DoRun();
            }
        }
        #endregion
    }
}