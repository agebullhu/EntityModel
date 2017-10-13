/*design by:agebull designer date:2017/6/9 11:34:09*/
using System.Configuration;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.Workflow.DataAccess
{
    /// <summary>
    /// 本地数据库
    /// </summary>
    sealed partial class WorkflowDataBase : MySqlDataBase
    {
        
        /// <summary>
        /// 初始化
        /// </summary>
        partial void Initialize()
        {
        }
        
        /// <summary>
        /// 生成缺省数据库
        /// </summary>
        public static void CreateDefault()
        {
            Default = new WorkflowDataBase();
        }
        static WorkflowDataBase _default;
        /// <summary>
        /// 缺省强类型数据库
        /// </summary>
        public static WorkflowDataBase Default
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
            return ConfigurationManager.ConnectionStrings["CbsBm"].ConnectionString;
        }
    }
}