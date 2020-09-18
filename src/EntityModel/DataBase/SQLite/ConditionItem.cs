using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    /// 表示访问条件和参数
    /// </summary>
    public sealed class ConditionItem
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string ConditionSql
        {
            get;
            set;
        }
        /// <summary>
        /// 参数列表(调用时生成数组,请只读调用,可以设置)
        /// </summary>
        public SQLiteParameter[] Parameters
        {
            get
            {
                return _parameters.Values.ToArray();
            }
            set
            {
                _parameters.Clear();
                foreach (var par in value)
                {
                    AddParameter(par);
                }
            }
        }

        readonly Dictionary<string, SQLiteParameter> _parameters = new Dictionary<string, SQLiteParameter>();

        /// <summary>
        /// 加入或替换参数
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(SQLiteParameter parameter)
        {
            string name = parameter.ParameterName;
            if (name == null)
            {
                name = string.Format("p_{0}", _parameters.Count + 1);
            }
            else if (!this._parameters.ContainsKey(name))
            {
                _parameters[name] = parameter;
                return;
            }
            _parameters.Add(name, parameter);
        }


        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="value">参数值</param>
        /// <returns>参数名称</returns>
        public string AddParameter(object value)
        {
            string name = string.Format("p_{0}", _parameters.Count + 1);
            _parameters.Add(name, SqliteDataBase.CreateParameter(name, value));
            return name;
        }
        /// <summary>
        /// AddParameter
        /// </summary>
        /// <param name="name">参数名(无名称使用序号)</param>
        /// <param name="value">参数值</param>
        /// <returns>True表示设置到旧参数，False表示增加了一个参数</returns>
        public bool SetParameterValue(string name, object value)
        {
            if (name == null)
            {
                name = string.Format("p_{0}", _parameters.Count + 1);
                _parameters.Add(name, SqliteDataBase.CreateParameter(name, value));
                return false;
            }
            SQLiteParameter parameter;
            if (this._parameters.TryGetValue(name, out parameter))
            {
                parameter.Value = value;
                return true;
            }
            _parameters.Add(name,SqliteDataBase.CreateParameter(name,value));
            return false;
        }
    }
}