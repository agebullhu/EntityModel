// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;

#endregion

namespace Agebull.Common.DataModel.MySql
{
    /// <summary>
    ///     表示访问条件和参数
    /// </summary>
    public sealed class ConditionItem
    {
        private readonly Dictionary<string, MySqlParameter> _parameters = new Dictionary<string, MySqlParameter>();

        /// <summary>
        ///     参数列表(调用时生成数组,请只读调用,可以设置)
        /// </summary>
        public IEnumerable<MySqlParameter> MySqlParameter => _parameters.Values;

        /// <summary>
        ///     条件
        /// </summary>
        public string ConditionSql { get; set; }

        /// <summary>
        ///     参数列表(调用时生成数组,请只读调用,可以设置)
        /// </summary>
        public bool HaseParameters => _parameters.Count > 0;

        /// <summary>
        ///     参数列表(调用时生成数组,请只读调用,可以设置)
        /// </summary>
        public MySqlParameter[] Parameters
        {
            get => _parameters.Values.ToArray();
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
        /// 参数的索引号
        /// </summary>
        public int ParaIndex { get; set; }

        private string NewParameterName => $"p_{++ParaIndex}";
        /// <summary>
        ///     加入或替换参数
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(MySqlParameter parameter)
        {
            var name = parameter.ParameterName;
            if (name == null)
            {
                name = NewParameterName;
            }
            else if (!_parameters.ContainsKey(name))
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
        public void AddAndCondition(string condition, MySqlParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                ConditionSql = $"({ConditionSql}) AND ({condition})";
            }
            AddParameter(parameter);
        }

        /// <summary>
        ///     加入条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        public void AddOrCondition(string condition, IEnumerable<MySqlParameter> parameters)
        {
            if (string.IsNullOrWhiteSpace(ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                ConditionSql = $"({ConditionSql}) OR ({condition})";
            }
            AddParameter(parameters);
        }


        /// <summary>
        ///     加入条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        public void AddAndCondition(string condition, IEnumerable<MySqlParameter> parameters)
        {
            if (string.IsNullOrWhiteSpace(ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                ConditionSql = $"({ConditionSql}) AND ({condition})";
            }
            AddParameter(parameters);
        }

        /// <summary>
        ///     加入条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameter"></param>
        public void AddOrCondition(string condition, MySqlParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(ConditionSql))
            {
                ConditionSql = condition;
            }
            else if (!string.IsNullOrWhiteSpace(condition))
            {
                ConditionSql = $"({ConditionSql}) Or ({condition})";
            }
            AddParameter(parameter);
        }

        /// <summary>
        ///     加入或替换参数
        /// </summary>
        /// <param name="parameters"></param>
        public void AddParameter(IEnumerable<MySqlParameter> parameters)
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
            var name = NewParameterName;
            _parameters.Add(name, MySqlDataBase.CreateParameter(name, value));
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
                name = NewParameterName;
                _parameters.Add(name, MySqlDataBase.CreateParameter(name, value));
                return false;
            }
            MySqlParameter parameter;
            if (_parameters.TryGetValue(name, out parameter))
            {
                parameter.Value = value;
                return true;
            }
            _parameters.Add(name, MySqlDataBase.CreateParameter(name, value));
            return false;
        }
    }
}