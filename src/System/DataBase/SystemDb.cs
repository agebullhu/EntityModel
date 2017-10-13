// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-12
// // *****************************************************/

#region 引用

using System.Configuration;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.SystemModel.DataAccess
{
    /// <summary>
    ///     本地数据库
    /// </summary>
    public partial class SystemDb : MySqlDataBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize()
        {
            if (Default == null)
                DefaultDataBase = Default = this;
        }

        /// <summary>
        /// 生成缺省数据库
        /// </summary>
        public static void CreateDefault()
        {
            if (Default == null)
                Default = new SystemDb();
        }

        /// <summary>
        /// 缺省强类型数据库
        /// </summary>
        public static SystemDb Default
        {
            get;
            private set;
        }
        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected override string LoadConnectionStringSetting()
        {
            return ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
        }
    }
}