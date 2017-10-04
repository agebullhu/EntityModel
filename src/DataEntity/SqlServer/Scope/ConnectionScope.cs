using System;
using MS.Lib.BusinessEntity;

namespace MS.Lib
{
    /// <summary>
    /// 连接范围对象
    /// </summary>
    public class ConnectionScope : IDisposable
    {
        private readonly MSConnectionManager _connectionManager;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="connection">连接对象</param>
        public ConnectionScope(MSConnectionManager connection)
        {
            _connectionManager = connection;
        }
        /// <summary>
        /// 是否已发生错误
        /// </summary>
        public bool IsFailed { get; set; }
        /// <summary>
        /// 析构
        /// </summary>
        void IDisposable.Dispose()
        {
            _connectionManager.Dispose();
        }
    }
}