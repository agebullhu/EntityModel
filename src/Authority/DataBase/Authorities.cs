/*design by:agebull designer date:2017/5/26 16:49:54*/

using System.Configuration;
using Gboxt.Common.DataModel.MySql;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    sealed partial class Authorities : MySqlDataBase
    {
        /// <summary>
        /// 生成缺省数据库
        /// </summary>
        public static void CreateDefault()
        {
            Default = new Authorities();
        }
        static Authorities _default;
        /// <summary>
        /// 缺省强类型数据库
        /// </summary>
        public static Authorities Default
        {
            get{ return _default;}
            set
            { 
                 DefaultDataBase =  _default = value;
            }
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