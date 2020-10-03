// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.Common.Ioc;
using Agebull.Common.Logging;
using Agebull.EntityModel.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     用于MySql的Lambda表达式解析器(仅支持查询条件)
    /// </summary>
    public sealed class PredicateConvert
    {
        #region 参数

        /// <summary>
        ///     关联字段
        /// </summary>
        private readonly DataAccessOption _option;

        /// <summary>
        ///     结果条件节点
        /// </summary>
        private ConditionItem _condition;

        /// <summary>
        ///     与上一次解释的条件用AND方式组合(否则为OR组合)
        /// </summary>
        private bool _mergeByAnd;

        /// <summary>
        /// 具体数据库对象
        /// </summary>
        public IParameterCreater ParameterCreater { get; set; }

        #endregion
        #region Cotr

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="option">设置</param>
        /// <param name="parameter">关联字段</param>
        PredicateConvert(IParameterCreater parameter, DataAccessOption option)
        {
            ParameterCreater = parameter;
            _option = option;
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="parameter"></param>
        /// <param name="option">设置</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(IParameterCreater parameter, DataAccessOption option, Expression<T> predicate)
        {
            var convert = new PredicateConvert(parameter, option);
            return convert.Convert(predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="parameter"></param>
        /// <param name="option">设置</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="condition">之前已解析的条件,可为空</param>
        /// <param name="mergeByAnd">与前面的条件(condition中已存在的)是用与还是或组合</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static void Convert<T>(IParameterCreater parameter, DataAccessOption option, Expression<T> predicate, ConditionItem condition, bool mergeByAnd = true)
        {
            var convert = new PredicateConvert(parameter, option)
            {
                _condition = condition,
                _mergeByAnd = mergeByAnd
            };
            convert.Convert(predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="parameter"></param>
        /// <param name="option">设置</param>
        /// <param name="filter">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(IParameterCreater parameter, DataAccessOption option, LambdaItem<T> filter)
        {
            var condition = new ConditionItem { ParameterCreater = parameter };
            Convert(parameter, option, filter, condition, true);
            return condition;
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="parameter"></param>
        /// <param name="option">设置</param>
        /// <param name="filter">Lambda表达式</param>
        /// <param name="condition">之前已解析的条件,可为空</param>
        /// <param name="mergeByAnd">与前面的条件(condition中已存在的)是用与还是或组合</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        static ConditionItem Convert<T>(IParameterCreater parameter, DataAccessOption option, LambdaItem<T> filter, ConditionItem condition, bool mergeByAnd)
        {
            var root = new PredicateConvert(parameter, option)
            {
                ParameterCreater = parameter,
                _condition = condition,
                _mergeByAnd = mergeByAnd
            };
            if (filter.Root != null)
            {
                root.Convert(filter.Root);
            }
            foreach (var ch in filter.Roots)
            {
                Convert(parameter, option, ch, condition, true);
            }

            ConditionItem item = new ConditionItem
            {
                ParameterCreater = parameter,
                ParaIndex = root._condition.ParaIndex
            };
            foreach (var ch in filter.Ands)
            {
                Convert(parameter, option, ch, item, true);
            }
            foreach (var ch in filter.Ors)
            {
                Convert(parameter, option, ch, item, false);
            }
            root._condition.ParaIndex = item.ParaIndex;
            root._condition.AddAndCondition(item.ConditionSql, item.Parameters);
            return root._condition;
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public ConditionItem Convert<T>(Expression<T> predicate)
        {
            if (_condition == null)
            {
                _condition = new ConditionItem { ParameterCreater = ParameterCreater };
            }
            var old = _condition.ConditionSql;
            var sql = ExpressionSql(predicate.Body);
            if (sql != null)
            {
                if (string.IsNullOrEmpty(old))
                {
                    _condition.ConditionSql = sql;
                }
                else
                {
                    _condition.ConditionSql = $"({old}) {LogicLink} ({sql})";
                }
            }
            return _condition;
        }
        #endregion
        #region 转换表达式

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private (bool toArg, object value) ConvertExpression(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                return (false, Convert(binaryExpression));
            }

            if (expression is UnaryExpression unary)
            {
                return (false, Convert(unary));
            }

            if (expression is MethodCallExpression call)
            {
                return (false, Convert(call));
            }

            if (expression is MemberExpression memberExpression)
            {
                return (false, Convert(memberExpression));
            }

            if (expression is ConstantExpression constantExpression)
            {
                return Convert(constantExpression);
            }
            if (expression.NodeType == ExpressionType.IsTrue)
            {
                return (false, 1);
            }
            if (expression.NodeType == ExpressionType.IsFalse)
            {
                return (false, 0);
            }
            return (false, CheckDynamicValue(GetValue(expression)));
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">二元Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(BinaryExpression expression)
        {
            //left
            var (_, left) = ConvertExpression(expression.Left);
            //right
            var (rToArg, right) = ConvertExpression(expression.Right);
            //body
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    return left == null && right == null
                        ? null
                            : left == null
                            ? right.ToString()
                                : right == null
                                ? left.ToString()
                                : $"({left}) AND ({right})";
                case ExpressionType.OrElse:
                    return left == null && right == null
                        ? null
                            : left == null
                            ? right.ToString()
                                : right == null
                                ? left.ToString()
                                : $"({left}) OR ({right})";
            };

            if (right is string rt && left is string lt)
            {
                if (string.Equals(rt, "NULL", StringComparison.OrdinalIgnoreCase))
                {
                    return expression.NodeType switch
                    {
                        ExpressionType.Equal => $"{lt} IS NULL",
                        ExpressionType.NotEqual => $"{lt} IS NOT NULL",
                        _ => CheckDynamicValue(GetValue(expression))//("Invalid lambda expression"),
                    };
                }
                if (string.Equals(lt, "NULL", StringComparison.OrdinalIgnoreCase))
                {
                    return expression.NodeType switch
                    {
                        ExpressionType.Equal => $"{rt} IS NULL",
                        ExpressionType.NotEqual => $"{rt} IS NOT NULL",
                        _ => CheckDynamicValue(GetValue(expression))//("Invalid lambda expression"),
                    };
                }
            }

            var rs = ToSql((rToArg, right));
            //body
            return expression.NodeType switch
            {
                ExpressionType.Equal => $"{left} = {rs}",
                ExpressionType.NotEqual => $"{left} <> {rs}",
                ExpressionType.GreaterThanOrEqual => $"{left} >= {rs}",
                ExpressionType.GreaterThan => $"{left} > {rs}",
                ExpressionType.LessThanOrEqual => $"{left} <= {rs}",
                ExpressionType.LessThan => $"{left} < {rs}",
                _ => CheckDynamicValue(GetValue(expression))//("Invalid lambda expression"),
            };
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">一元Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
                return ExpressionSql(expression.Operand);
            if (expression.NodeType != ExpressionType.Not)
                return CheckDynamicValue(GetValue(expression));//("Invalid lambda expression");

            return (expression.Operand) switch
            {
                MemberExpression member => $"{GetField(member)} = 0",
                _ => $"NOT({ExpressionSql(expression.Operand)})"
            };
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">方法Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType == null)
            {
                return CheckDynamicValue(GetValue(expression));//($"不支持方法:{expression.Method.Name}");
            }

            if (expression.Method.IsStatic)
            {
                if (expression.Method.DeclaringType == typeof(Math))
                {
                    switch (expression.Method.Name)
                    {
                        case nameof(Math.Abs):
                            if (TryGetField(expression.Arguments[0], out var field2))
                                return $"ABS({field2})";
                            break;
                    };
                }
                return CheckDynamicValue(GetValue(expression));//($"不支持方法:{expression.Method.Name}");
            }
            if (expression.Method.DeclaringType == typeof(EntityProperty))
            {
                var property = GetValue<EntityProperty>(expression.Object);
                return expression.Method.Name switch
                {
                    nameof(EntityProperty.IsEquals) => $"`{property.FieldName}` = {ExpressionSql(expression.Arguments[0])}",
                    nameof(EntityProperty.Expression) => $"`{property.FieldName}` {GetValue(expression.Arguments[0])} {ExpressionSql(expression.Arguments[1])}",
                    _ => CheckDynamicValue(GetValue(expression))//($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (expression.Method.DeclaringType == typeof(Ex))
            {
                return expression.Method.Name switch
                {
                    nameof(Ex.In) => $"{GetField(expression.Arguments[0])} IN ({ExpressionSql(expression.Arguments[1])})",
                    nameof(Ex.Like) => $"{GetField(expression.Arguments[0])} LIKE CONCAT('%',{ExpressionSql(expression.Arguments[1])}, '%')",
                    nameof(Ex.LeftLike) => $"{GetField(expression.Arguments[0])} LIKE CONCAT({ExpressionSql(expression.Arguments[1])}, '%')",
                    nameof(Ex.FieldEquals) => $"{GetField(expression.Arguments[0])} = {ExpressionSql(expression.Arguments[1])}",
                    nameof(Ex.Expression) => $"{GetField(expression.Arguments[0])} {ExpressionValue(expression.Arguments[1])} {ExpressionValue(expression.Arguments[2])}",
                    nameof(Ex.Condition) => $"{GetValue<string>(expression.Arguments[0])}",
                    nameof(Ex.IsNotNull) => $"{GetField(expression.Arguments[0])} IS NOT NULL",
                    nameof(Ex.IsNull) => $"{GetField(expression.Arguments[0])} IS NULL",
                    _ => CheckDynamicValue(GetValue(expression))//($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (TryGetField(expression.Object, out var field))
            {
                if (expression.Method.Name == nameof(object.Equals))
                {
                    string value = GetArgument(expression.Arguments);
                    if (value == null)
                        return $"{field} IS NULL";
                    return $"{field} = {value}";
                }
                if (expression.Method.DeclaringType == typeof(string))
                {
                    return expression.Method.Name switch
                    {
                        nameof(string.ToUpper) => $"UPPER({field})",
                        nameof(string.Contains) => $"{field} LIKE CONCAT('%',{GetArguments(expression.Arguments)},'%')",
                        nameof(string.ToLower) => $"LOWER({field})",
                        nameof(string.Trim) => $"TRIM({field})",
                        nameof(string.TrimStart) => $"LTRIM({field})",
                        nameof(string.TrimEnd) => $"RTRIM({field})",
                        nameof(string.Replace) => $"REPLACE({field},{GetArguments(expression.Arguments)})",
                        _ => CheckDynamicValue(GetValue(expression))//($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                    };
                }
                if (expression.Method.DeclaringType == typeof(Enum))
                {
                    return expression.Method.Name switch
                    {
                        nameof(Enum.HasFlag) => string.Format("({0} & {1}) = {1}", field, GetValue<int>(expression.Arguments[0])),
                        _ => CheckDynamicValue(GetValue(expression))//($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                    };
                }
            }

            if (expression.Method.Name == nameof(string.Contains))
            {
                string value;
                string field1;
                if (expression.Object == null)//扩展方法
                {
                    field1 = GetField(expression.Arguments[1]);
                    value = ExpressionSql(expression.Arguments[0]);
                }
                else
                {
                    field1 = GetArguments(expression.Arguments);
                    value = ExpressionSql(expression.Object);
                }

                if (!string.IsNullOrWhiteSpace(field1) && !string.IsNullOrWhiteSpace(value))
                    return $"{field1} IN ({value})";
                return null;
            }
            return CheckDynamicValue(GetValue(expression));
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">属性或字段Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(MemberExpression expression)
        {
            if (expression.Expression is ParameterExpression)
            {
                return GetField(expression);
            }
            if (expression.Expression is MemberExpression par1 && par1.Expression is ParameterExpression)
            {
                if (par1.Type == typeof(string) && expression.Member.Name == "Length")
                    return $"LENGTH({GetField(par1)})";
                return CheckDynamicValue(GetValue(expression));//($"不支持属性:{expression.Member.DeclaringType.FullName}.{expression.Member.Name}");
            }
            return CheckDynamicValue(GetValue(expression));
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">常量Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private (bool toArg, object value) Convert(ConstantExpression expression)
        {
            if (expression.Value == null)
            {
                return (false, "NULL");
            }
            switch (expression.Type.Name)
            {
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                case "int":
                case "Int32":
                case "uint":
                case "IntPtr":
                case "UInt32":
                case "UIntPtr":
                case "float":
                case "Float":
                case "double":
                case "Double":
                case "decimal":
                case "Decimal":
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return (false, expression.Value);
                case "bool":
                case "Boolean":
                    return (false, (bool)expression.Value ? 1 : 0);
                case "char":
                case "Char":
                    return ((char)expression.Value) switch
                    {
                        '\t' => (false, "'\t'"),
                        '\r' => (false, "'\r'"),
                        '\n' => (false, "'\n'"),
                        _ => (false, $"'{expression.Value}'"),
                    };
            }
            return (true, expression.Value);
        }

        /// <summary>
        ///     取得方法对象的参数
        /// </summary>
        /// <param name="arguments">方法Lambda对象</param>
        /// <returns>参数文本</returns>
        private string GetArgument(IEnumerable<Expression> arguments)
        {
            var arg = arguments.FirstOrDefault();
            if (arg == null)
                return null;
            return ExpressionSql(arg);
        }

        /// <summary>
        ///     取得方法对象的参数
        /// </summary>
        /// <param name="arguments">方法Lambda对象</param>
        /// <returns>参数文本</returns>
        private string GetArguments(IEnumerable<Expression> arguments)
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var arg in arguments)
            {
                var (toArg, value) = ConvertExpression(arg);
                if (value == null)
                    continue;
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.Append(ToSql((toArg, value)));
            }
            return isFirst ? null : sb.ToString();
        }

        #endregion
        #region Helper

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression">字段或属性对象</param>
        /// <returns>名称</returns>
        private static string GetName(MemberExpression expression)
        {
            return expression.Member.Name;
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression">Lambda节点对象</param>
        /// <returns>计算结果值</returns>
        private static T GetValue<T>(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            try
            {
                dynamic func = lambda.Compile();
                return (T)func();
            }
            catch (Exception e)
            {
                DependencyScope.Logger.Exception(e, expression.ToString());
                return default;
            }
        }
        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression">Lambda节点对象</param>
        /// <returns>计算结果值</returns>
        private string GetField(Expression expression)
        {
            var field = expression is MemberExpression member
                ? member.Member.Name
                : expression is ConstantExpression constant
                    ? (string)Convert(constant).value
                    : GetValue<string>(expression);

            _option.FieldMap.TryGetValue(field, out field);
            return $"`{field}`";
        }

        /// <summary>
        ///     取得字段
        /// </summary>
        /// <param name="expression">Lambda节点对象</param>
        /// <param name="field">字段</param>
        /// <returns>计算结果值</returns>
        private bool TryGetField(Expression expression, out string field)
        {
            field = expression is MemberExpression member
                ? member.Member.Name
                : expression is ConstantExpression constant
                    ? (string)Convert(constant).value
                    : GetValue<string>(expression);
            if (!_option.FieldMap.TryGetValue(field, out field))
                return false;
            field = $"`{field}`";
            return true;
        }
        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="member">Lambda节点对象</param>
        /// <returns>计算结果值</returns>
        private string GetField(MemberExpression member)
        {
            var field = member.Member.Name;

            _option.FieldMap.TryGetValue(field, out field);
            return $"`{field}`";
        }
        string LogicLink => _mergeByAnd ? "AND" : "OR";
        string ToSql((bool toArg, object value) re)
        {
            if (re.value == null)
                return null;
            if (re.toArg)
            {
                var name = _condition.AddParameter(re.value);
                return $"?{name}";
            }
            else
            {
                return re.value?.ToString();
            }
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string ExpressionSql(Expression expression)
        {
            return ToSql(ConvertExpression(expression));
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private object ExpressionValue(Expression expression)
        {
            return ConvertExpression(expression).value;
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression">Lambda节点对象</param>
        /// <returns>计算结果值</returns>
        private static object GetValue(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            try
            {
                dynamic func = lambda.Compile();
                return func();
            }
            catch (Exception e)
            {
                DependencyScope.Logger.Exception(e, expression.ToString());
                return null;
            }
        }

        string CheckDynamicValue(object vl)
        {
            if (vl == null)
            {
                return "NULL";
            }
            if (vl is bool b)
            {
                return b ? "1" : "0";
            }
            if (vl is string)
            {
                return $"?{_condition.AddParameter(vl)}";
            }
            if (vl is DateTime)
            {
                return $"?{_condition.AddParameter(vl)}";
            }
            var vlType = vl.GetType();
            if (vlType.IsEnum)
            {
                if (!vlType.IsArray)
                    return $"'{(int)vl}'";
                if (!(vl is IEnumerable array))
                    return $"'{(int)vl}'";
                StringBuilder sb = new StringBuilder();
                sb.Append("'");
                bool first = true;
                foreach (var v in array)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append(',');
                    sb.Append($"'{(int)v}'");
                }
                return sb.ToString();
            }

            if (vl is IEnumerable enumerable)
            {
                return enumerable.LinkToString("'", "','", "'");
            }
            if (vlType.IsValueType && vlType.IsBaseType())
            {
                return $"'{vl}'";
            }
            return $"?{_condition.AddParameter(vl)}";
        }

        /// <summary>
        ///     不成文的表达式处理
        /// </summary>
        /// <param name="str">有可能不合格的SQL文本</param>
        /// <returns>正确合格的SQL文本</returns>
        private string CheckSingle(string str)
        {
            return str;//.Contains("(") ? str : $"{str} = 1";
        }
        #endregion
    }
}