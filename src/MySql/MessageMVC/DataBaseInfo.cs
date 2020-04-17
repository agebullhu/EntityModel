using System.Collections.Concurrent;

namespace Agebull.EntityModel.MySql
{
    internal class DataBaseInfo
    {
        internal string ConnectionString { get; set; }

        internal ConcurrentDictionary<int, ConnectionInfo> ActiveConnections = new ConcurrentDictionary<int, ConnectionInfo>();
    }
}