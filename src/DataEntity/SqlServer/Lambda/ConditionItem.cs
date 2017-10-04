// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:YhxBank.FundsManagement
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     表示访问条件和参数
    /// </summary>
    public sealed class ConditionItem
    {
        private readonly Dictionary<string, SqlParameter> _parameters = new Dictionary<string, SqlParameter>();

        /// <summary>
        ///     条件
        /// </summary>
        public string ConditionSql { get; set; }

        /// <summary>
        ///     参数列表(调用时生成数组,请只读调用,可以设置)
        /// </summary>
        public SqlParameter[] Parameters
        {
            get { return _parameters.Values.ToArray(); }
            set
            {
                _parameters.Clear();
                foreach (var par in value)
                {
                    AddParameter(par);
                }
            }
        }

        /// <summary>
        ///     加入或替换参数
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(SqlParameter parameter)
        {
            var name = parameter.ParameterName;
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
        ///     加入条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameter"></param>
        public void AddAndCondition(string condition, SqlParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(this.ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                this.ConditionSql = string.Format("({0}) AND ({1})", this.ConditionSql, condition);
            }
            AddParameter(parameter);
        }

        /// <summary>
        ///     加入条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameter"></param>
        public void AddOrCondition(string condition, SqlParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(this.ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                this.ConditionSql = string.Format("({0}) Or ({1})", this.ConditionSql, condition);
            }
            AddParameter(parameter);
        }

        /// <summary>
        ///     加入或替换参数
        /// </summary>
        /// <param name="parameters"></param>
        public void AddParameters(IEnumerable<SqlParameter> parameters)
        {
            foreach (var p in parameters)
            {
                AddParameter(p);
            }
        }

        /// <summary>
        ///     AddParameter
        /// </summary>
        /// <param name="value">参数值</param>
        /// <returns>参数名称</returns>
        public string AddParameter(object value)
        {
            if (value is Enum)
            {
                value = Convert.ToInt32(value);
            }
            var name = string.Format("p_{0}", _parameters.Count + 1);
            _parameters.Add(name, SqlServerDataBase.CreateParameter(name, value));
            return name;
        }

        /// <summary>
        ///     AddParameter
        /// </summary>
        /// <param name="name">参数名(无名称使用序号)</param>
        /// <param name="value">参数值</param>
        /// <returns>True表示设置到旧参数，False表示增加了一个参数</returns>
        public bool SetParameterValue(string name, object value)
        {
            if (name == null)
            {
                name = string.Format("p_{0}", _parameters.Count + 1);
                _parameters.Add(name, SqlServerDataBase.CreateParameter(name, value));
                return false;
            }
            SqlParameter parameter;
            if (this._parameters.TryGetValue(name, out parameter))
            {
                parameter.Value = value;
                return true;
            }
            _parameters.Add(name, SqlServerDataBase.CreateParameter(name, value));
            return false;
        }
    }
}