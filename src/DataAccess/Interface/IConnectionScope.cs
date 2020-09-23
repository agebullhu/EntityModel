
using System;
using System.Data.Common;

namespace Agebull.EntityModel.Common
{

    /// <summary>
    /// 数据库连接范围
    /// </summary>
    public interface IConnectionScope : IAsyncDisposable, IDisposable
    {
        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction Transaction { get; }

        /// <summary>
        /// 连接对象
        /// </summary>
        DbConnection Connection { get; }

    }

}