// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-16
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

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
        /// <summary>
        ///     关联字段
        /// </summary>
        private readonly Dictionary<string, string> _columnMap;

        /// <summary>
        ///     结果条件节点
        /// </summary>
        private ConditionItem _condition;

        /// <summary>
        ///     与上一次解释的条件用AND方式组合(否则为OR组合)
        /// </summary>
        private bool _mergeByAnd;

        /// <summary>
        ///     构造
        /// </summary>
        public PredicateConvert()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="map">关联字段</param>
        public PredicateConvert(Dictionary<string, string> map)
        {
            _columnMap = map;
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression">字段或属性对象</param>
        /// <returns>名称</returns>
        public static string GetName(MemberExpression expression)
        {
            return expression.Member.Name;
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression">Lambda节点对象</param>
        /// <returns>计算结果值</returns>
        public static object GetValue(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            try
            {
                dynamic func = lambda.Compile();
                return func();
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e, expression.ToString());
                return null;
            }
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="columns">关联字段</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(string[] columns, Expression<T> predicate)
        {
            return Convert(columns.ToDictionary(p => p, p => p), predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="map">关联字段</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, Expression<T> predicate)
        {
            var convert = new PredicateConvert(map);
            return convert.Convert(predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="map">关联字段</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="condition">之前已解析的条件,可为空</param>
        /// <param name="mergeByAnd">与前面的条件(condition中已存在的)是用与还是或组合</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static void Convert<T>(Dictionary<string, string> map, Expression<T> predicate, ConditionItem condition, bool mergeByAnd = true)
        {
            var convert = new PredicateConvert(map)
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
        /// <param name="columns">关联字段</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="condition">之前已解析的条件,可为空</param>
        /// <param name="mergeByAnd">与前面的条件(condition中已存在的)是用与还是或组合</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert2<T>(string[] columns, Expression<Func<T, bool>> predicate, ConditionItem condition = null, bool mergeByAnd = true)
        {
            var convert = new PredicateConvert(columns.ToDictionary(p => p, p => p))
            {
                _condition = condition,
                _mergeByAnd = mergeByAnd
            };
            return convert.Convert(predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="map">关联字段</param>
        /// <param name="filter">Lambda表达式</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, LambdaItem<T> filter)
        {
            var condition = new ConditionItem(new MySqlDataBase_());
            Convert(map, filter, condition, true);
            return condition;
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="map">关联字段</param>
        /// <param name="filter">Lambda表达式</param>
        /// <param name="condition">之前已解析的条件,可为空</param>
        /// <param name="mergeByAnd">与前面的条件(condition中已存在的)是用与还是或组合</param>
        /// <returns>结果条件对象(SQL条件和参数)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, LambdaItem<T> filter, ConditionItem condition, bool mergeByAnd)
        {
            var root = new PredicateConvert(map)
            {
                _condition = condition,
                _mergeByAnd = mergeByAnd
            };
            if (filter.Root != null)
            {
                root.Convert(filter.Root);
            }
            foreach (var ch in filter.Roots)
            {
                Convert(map, ch, condition, true);
            }

            ConditionItem item = new ConditionItem(new MySqlDataBase_())
            {
                ParaIndex = root._condition.ParaIndex
            };
            foreach (var ch in filter.Ands)
            {
                Convert(map, ch, item, true);
            }
            foreach (var ch in filter.Ors)
            {
                Convert(map, ch, item, false);
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
                _condition = new ConditionItem(new MySqlDataBase_());
            }
            var old = _condition.ConditionSql;
            var sql = ConvertExpression(predicate.Body);
            var news = CheckSingle(sql);

            if (string.IsNullOrEmpty(old))
            {
                _condition.ConditionSql = news;
            }
            else if (!string.IsNullOrEmpty(news))
            {
                _condition.ConditionSql = $"({old}) {(_mergeByAnd ? "AND" : "OR")} ({news})";
            }
            return _condition;
        }

        /// <summary>
        ///     不成文的表达式处理
        /// </summary>
        /// <param name="str">有可能不合格的SQL文本</param>
        /// <returns>正确合格的SQL文本</returns>
        private string CheckSingle(string str)
        {
            return str.Contains("(") ? str : $"{str} = 1";
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string ConvertExpression(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                return Convert(binaryExpression);
            }

            if (expression is UnaryExpression unary)
            {
                return Convert(unary);
            }

            if (expression is MethodCallExpression call)
            {
                return Convert(call);
            }

            if (expression is MemberExpression memberExpression)
            {
                return Convert(memberExpression);
            }

            if (expression is ConstantExpression constantExpression)
            {
                return Convert(constantExpression);
            }

            if (expression is NewArrayExpression array)
            {
                var sb = new StringBuilder();
                bool first = true;
                foreach (var arg in array.Expressions)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append(',');
                    sb.Append(ConvertExpression(arg));
                }
                return sb.ToString();
            }
            if (expression.NodeType == ExpressionType.IsTrue)
            {
                return "1";
            }
            if (expression.NodeType == ExpressionType.IsFalse)
            {
                return "0";
            }
            throw new EntityModelDbException("Invalid lambda expression");
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">二元Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(BinaryExpression expression)
        {
            //left
            var lefttext = ConvertExpression(expression.Left);
            //right
            var righttext = ConvertExpression(expression.Right);
            if (lefttext == null)
            {
                return righttext;
            }
            if (righttext == null)
            {
                return lefttext;
            }
            if (string.Equals(righttext, "null", StringComparison.OrdinalIgnoreCase))
            {
                return expression.NodeType switch
                {
                    ExpressionType.Equal => $"({lefttext} IS NULL)",
                    ExpressionType.NotEqual => $"({lefttext} IS NOT NULL)",
                    _ => throw new EntityModelDbException("Invalid lambda expression"),
                };
            }
            if (string.Equals(lefttext, "null", StringComparison.OrdinalIgnoreCase))
            {
                return expression.NodeType switch
                {
                    ExpressionType.Equal => $"({righttext} IS NULL)",
                    ExpressionType.NotEqual => $"({righttext} IS NOT NULL)",
                    _ => throw new EntityModelDbException("Invalid lambda expression"),
                };
            }
            //body
            return expression.NodeType switch
            {
                ExpressionType.AndAlso => $"({CheckSingle(lefttext)} AND {CheckSingle(righttext)})",
                ExpressionType.OrElse => $"({CheckSingle(lefttext)} OR {CheckSingle(righttext)})",
                ExpressionType.Equal => $"({lefttext} = {righttext})",
                ExpressionType.NotEqual => $"({lefttext} <> {righttext})",
                ExpressionType.GreaterThanOrEqual => $"({lefttext} >= {righttext})",
                ExpressionType.GreaterThan => $"({lefttext} > {righttext})",
                ExpressionType.LessThanOrEqual => $"({lefttext} <= {righttext})",
                ExpressionType.LessThan => $"({lefttext} < {righttext})",
                _ => throw new EntityModelDbException("Invalid lambda expression"),
            };
        }

        /// <summary>
        ///     取得方法对象的参数
        /// </summary>
        /// <param name="expression">方法Lambda对象</param>
        /// <returns>参数文本</returns>
        private string GetArguments(MethodCallExpression expression)
        {
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var arg in expression.Arguments)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(',');
                }
                var ar = ConvertExpression(arg);
                sb.Append(ar ?? "NULL");
            }

            return isFirst ? null : sb.ToString();
        }

        /// <summary>
        ///     转换表达式
        /// </summary>
        /// <param name="expression">一元Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(UnaryExpression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Not => expression.Operand is ParameterExpression parameterExpression
                                       ? $"`{parameterExpression.Name}` == 0"
                                       : $"NOT({ConvertExpression(expression.Operand)})",
                ExpressionType.Convert => ConvertExpression(expression.Operand),
                ExpressionType.Decrement => $"({ConvertExpression(expression.Operand)}) - 1",
                ExpressionType.Increment => $"({ConvertExpression(expression.Operand)}) + 1",
                _ => throw new EntityModelDbException("Invalid lambda expression"),
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
                throw new EntityModelDbException($"不支持方法:{expression.Method.Name}");
            }
            if (expression.Method.Name == "Equals")
            {
                var left = ConvertExpression(expression.Object);
                var right = GetArguments(expression);
                var lnull = left == null;
                var rnull = right == null;
                if (lnull && rnull)
                    return "(1 = 1)";

                if (lnull)
                    return $"({right} IS NULL)";
                if (rnull)
                    return $"({left} IS NULL)";
                return $"({left} = {right})";
            }
            if (expression.Method.DeclaringType == typeof(string))
            {
                return expression.Method.Name switch
                {
                    "LeftLike" => $"({ConvertExpression(expression.Arguments[0])} Like concat({ConvertExpression(expression.Arguments[1])}, '%'))",
                    "ToUpper" => $"UPPER({ConvertExpression(expression.Object)})",
                    "Contains" => $"({ConvertExpression(expression.Object)} Like concat('%',{GetArguments(expression)},'%'))",
                    "ToLower" => $"LOWER({ConvertExpression(expression.Object)})",
                    "Trim" => $"TRIM({ConvertExpression(expression.Object)})",
                    "TrimStart" => $"LTRIM({ConvertExpression(expression.Object)})",
                    "TrimEnd" => $"RTRIM({ConvertExpression(expression.Object)})",
                    "Replace" => $"REPLACE({ConvertExpression(expression.Object)},{GetArguments(expression)})",
                    _ => throw new EntityModelDbException($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (expression.Method.DeclaringType == typeof(Math))
            {
                return expression.Method.Name switch
                {
                    "Abs" => $"ABS({GetArguments(expression)})",
                    _ => throw new EntityModelDbException($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (expression.Method.DeclaringType == typeof(Enum))
            {
                return expression.Method.Name switch
                {
                    "HasFlag" => string.Format("({0} & {1}) = {1}", ConvertExpression(expression.Object), GetArguments(expression)),
                    _ => throw new EntityModelDbException($"不支持方法:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }

            if (expression.Method.Name == "Contains")
            {
                string value;
                string field;
                if (expression.Object == null)//扩展方法
                {
                    value = ConvertExpression(expression.Arguments[0]);
                    field = ConvertExpression(expression.Arguments[1]);
                }
                else
                {
                    value = ConvertExpression(expression.Object);
                    field = GetArguments(expression);
                }
                if (!string.IsNullOrWhiteSpace(field) && !string.IsNullOrWhiteSpace(value))
                    return $"{field} IN ({value})";
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
                if (_columnMap.TryGetValue(expression.Member.Name, out var field))
                    return $"`{field}`";
                throw new EntityModelDbException($"字段不存在于数据库中.{expression.Member.Name}");
            }
            var par1 = expression.Expression as MemberExpression;
            if (!(par1?.Expression is ParameterExpression))
                return CheckDynamicValue(GetValue(expression));

            if (par1.Type == typeof(string))
            {
                switch (expression.Member.Name)
                {
                    case "Length":
                        return $"LENGTH(`{par1.Member.Name}`)";
                }
            }
            else if (par1.Type == typeof(DateTime))
            {
                switch (expression.Member.Name)
                {
                    case "Now":
                        return $"?{_condition.AddParameter(DateTime.Now)}";
                    case "Today":
                        return $"?{_condition.AddParameter(DateTime.Today)}";
                }
            }
            throw new EntityModelDbException($"不支持属性:{expression.Member.DeclaringType.FullName}.{expression.Member.Name}");
        }

        string CheckDynamicValue(object vl)
        {
            if (vl == null)
            {
                return $"?{_condition.AddParameter((object)null)}";
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
        ///     转换表达式
        /// </summary>
        /// <param name="expression">常量Lambda对象</param>
        /// <returns>解释后的SQL文本</returns>
        private string Convert(ConstantExpression expression)
        {
            if (expression.Value == null)
            {
                return "NULL";
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
                    return expression.Value.ToString();
                case "bool":
                case "Boolean":
                    return (bool)expression.Value ? "1" : "0";
                case "char":
                case "Char":
                    return ((char)expression.Value) switch
                    {
                        '\t' => "'\t'",
                        '\r' => "'\r'",
                        '\n' => "'\n'",
                        _ => $"'{expression.Value}'",
                    };
            }
            var name = _condition.AddParameter(expression.Value);
            return $"?{name}";
        }
    }
}