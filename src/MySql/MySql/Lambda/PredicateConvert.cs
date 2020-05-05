// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

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
    ///     ����MySql��Lambda���ʽ������(��֧�ֲ�ѯ����)
    /// </summary>
    public sealed class PredicateConvert
    {
        /// <summary>
        ///     �����ֶ�
        /// </summary>
        private readonly Dictionary<string, string> _columnMap;

        /// <summary>
        ///     ��������ڵ�
        /// </summary>
        private ConditionItem _condition;

        /// <summary>
        ///     ����һ�ν��͵�������AND��ʽ���(����ΪOR���)
        /// </summary>
        private bool _mergeByAnd;

        /// <summary>
        ///     ����
        /// </summary>
        public PredicateConvert()
        {
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="map">�����ֶ�</param>
        public PredicateConvert(Dictionary<string, string> map)
        {
            _columnMap = map;
        }

        /// <summary>
        ///     ȡ������
        /// </summary>
        /// <param name="expression">�ֶλ����Զ���</param>
        /// <returns>����</returns>
        public static string GetName(MemberExpression expression)
        {
            return expression.Member.Name;
        }

        /// <summary>
        ///     ȡ��ֵ
        /// </summary>
        /// <param name="expression">Lambda�ڵ����</param>
        /// <returns>������ֵ</returns>
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
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="columns">�����ֶ�</param>
        /// <param name="predicate">Lambda���ʽ</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
        public static ConditionItem Convert<T>(string[] columns, Expression<T> predicate)
        {
            return Convert(columns.ToDictionary(p => p, p => p), predicate);
        }

        /// <summary>
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="map">�����ֶ�</param>
        /// <param name="predicate">Lambda���ʽ</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, Expression<T> predicate)
        {
            var convert = new PredicateConvert(map);
            return convert.Convert(predicate);
        }

        /// <summary>
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="map">�����ֶ�</param>
        /// <param name="predicate">Lambda���ʽ</param>
        /// <param name="condition">֮ǰ�ѽ���������,��Ϊ��</param>
        /// <param name="mergeByAnd">��ǰ�������(condition���Ѵ��ڵ�)�����뻹�ǻ����</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
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
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="columns">�����ֶ�</param>
        /// <param name="predicate">Lambda���ʽ</param>
        /// <param name="condition">֮ǰ�ѽ���������,��Ϊ��</param>
        /// <param name="mergeByAnd">��ǰ�������(condition���Ѵ��ڵ�)�����뻹�ǻ����</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
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
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="map">�����ֶ�</param>
        /// <param name="filter">Lambda���ʽ</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, LambdaItem<T> filter)
        {
            var condition = new ConditionItem(new MySqlDataBase_());
            Convert(map, filter, condition, true);
            return condition;
        }

        /// <summary>
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="map">�����ֶ�</param>
        /// <param name="filter">Lambda���ʽ</param>
        /// <param name="condition">֮ǰ�ѽ���������,��Ϊ��</param>
        /// <param name="mergeByAnd">��ǰ�������(condition���Ѵ��ڵ�)�����뻹�ǻ����</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
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
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="predicate">Lambda���ʽ</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
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
        ///     �����ĵı��ʽ����
        /// </summary>
        /// <param name="str">�п��ܲ��ϸ��SQL�ı�</param>
        /// <returns>��ȷ�ϸ��SQL�ı�</returns>
        private string CheckSingle(string str)
        {
            return str.Contains("(") ? str : $"{str} = 1";
        }

        /// <summary>
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">Lambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
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
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">��ԪLambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
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
        ///     ȡ�÷�������Ĳ���
        /// </summary>
        /// <param name="expression">����Lambda����</param>
        /// <returns>�����ı�</returns>
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
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">һԪLambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
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
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">����Lambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
        private string Convert(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType == null)
            {
                throw new EntityModelDbException($"��֧�ַ���:{expression.Method.Name}");
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
                    _ => throw new EntityModelDbException($"��֧�ַ���:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (expression.Method.DeclaringType == typeof(Math))
            {
                return expression.Method.Name switch
                {
                    "Abs" => $"ABS({GetArguments(expression)})",
                    _ => throw new EntityModelDbException($"��֧�ַ���:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }
            if (expression.Method.DeclaringType == typeof(Enum))
            {
                return expression.Method.Name switch
                {
                    "HasFlag" => string.Format("({0} & {1}) = {1}", ConvertExpression(expression.Object), GetArguments(expression)),
                    _ => throw new EntityModelDbException($"��֧�ַ���:{expression.Method.DeclaringType.FullName}.{expression.Method.Name}"),
                };
            }

            if (expression.Method.Name == "Contains")
            {
                string value;
                string field;
                if (expression.Object == null)//��չ����
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
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">���Ի��ֶ�Lambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
        private string Convert(MemberExpression expression)
        {
            if (expression.Expression is ParameterExpression)
            {
                if (_columnMap.TryGetValue(expression.Member.Name, out var field))
                    return $"`{field}`";
                throw new EntityModelDbException($"�ֶβ����������ݿ���.{expression.Member.Name}");
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
            throw new EntityModelDbException($"��֧������:{expression.Member.DeclaringType.FullName}.{expression.Member.Name}");
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
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">����Lambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
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