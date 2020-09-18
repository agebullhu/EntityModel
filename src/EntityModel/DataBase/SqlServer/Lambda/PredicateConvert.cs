// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:YhxBank.FundsManagement
// // ����:2016-06-16
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Gboxt.Common.DataModel.SqlServer
{
    /// <summary>
    ///     ����Sql Servr ��Lambda���ʽ������(��֧�ֲ�ѯ����)
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
            dynamic func = lambda.Compile();
            return func();
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
        /// <param name="root">Lambda���ʽ</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, LambdaItem<T> root)
        {
            var condition = new ConditionItem();
            Convert(map, root, condition, true);
            return condition;
        }

        /// <summary>
        ///     ����Lambda���ʽ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="map">�����ֶ�</param>
        /// <param name="root">Lambda���ʽ</param>
        /// <param name="condition">֮ǰ�ѽ���������,��Ϊ��</param>
        /// <param name="mergeByAnd">��ǰ�������(condition���Ѵ��ڵ�)�����뻹�ǻ����</param>
        /// <returns>�����������(SQL�����Ͳ���)</returns>
        public static ConditionItem Convert<T>(Dictionary<string, string> map, LambdaItem<T> root, ConditionItem condition, bool mergeByAnd)
        {
            var convert = new PredicateConvert(map)
            {
                _condition = condition,
                _mergeByAnd = mergeByAnd
            };
            if (root.Root != null)
            {
                convert.Convert(root.Root);
            }
            foreach (var ch in root.Ands)
            {
                Convert(map, ch, condition, true);
            }
            foreach (var ch in root.Ors)
            {
                Convert(map, ch, condition, false);
            }
            return convert._condition;
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
                _condition = new ConditionItem();
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
            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
            {
                return Convert(binaryExpression);
            }
            var unary = expression as UnaryExpression;
            if (unary != null)
            {
                return Convert(unary);
            }
            var call = expression as MethodCallExpression;
            if (call != null)
            {
                return Convert(call);
            }
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return Convert(memberExpression);
            }
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                return Convert(constantExpression);
            }
            var array = expression as NewArrayExpression;
            if (array != null)
            {
                var sb = new StringBuilder();
                foreach (var arg in array.Expressions)
                {
                    sb.AppendFormat(",{0}", ConvertExpression(arg));
                }
                return sb.ToString().Trim(',');
            }
            if (expression.NodeType == ExpressionType.IsTrue)
            {
                return "1";
            }
            if (expression.NodeType == ExpressionType.IsFalse)
            {
                return "0";
            }
            throw new ArgumentException("Invalid lambda expression");
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
                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        return $"({lefttext} IS NULL)";
                    case ExpressionType.NotEqual:
                        return $"({lefttext} IS NOT NULL)";
                    default:
                        throw new ArgumentException("Invalid lambda expression");
                }
            }
            //body
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    return $"({CheckSingle(lefttext)} AND {CheckSingle(righttext)})";
                case ExpressionType.OrElse:
                    return $"({CheckSingle(lefttext)} OR {CheckSingle(righttext)})";
                case ExpressionType.Equal:
                    return $"({lefttext} = {righttext})";
                case ExpressionType.NotEqual:
                    return $"({lefttext} <> {righttext})";
                case ExpressionType.GreaterThanOrEqual:
                    return $"({lefttext} >= {righttext})";
                case ExpressionType.GreaterThan:
                    return $"({lefttext} > {righttext})";
                case ExpressionType.LessThanOrEqual:
                    return $"({lefttext} <= {righttext})";
                case ExpressionType.LessThan:
                    return $"({lefttext} < {righttext})";
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
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
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    var parameterExpression = expression.Operand as ParameterExpression;
                    return parameterExpression != null
                        ? $"[{parameterExpression.Name}] == 0"
                        : $"NOT({ConvertExpression(expression.Operand)})";
                case ExpressionType.Convert:
                    return ConvertExpression(expression.Operand);
                case ExpressionType.Decrement:
                    return $"({ConvertExpression(expression.Operand)}) - 1";
                case ExpressionType.Increment:
                    return $"({ConvertExpression(expression.Operand)}) + 1";
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
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
                throw new ArgumentException("��֧�ֲ����ķ���(��֧�����Եķ���)");
            }
            if (expression.Method.DeclaringType == typeof(string))
            {
                switch (expression.Method.Name)
                {
                    case "ToUpper":
                        return $"upper({ConvertExpression(expression.Object)})";
                    case "Equals":
                        return $"({ConvertExpression(expression.Object)} = {GetArguments(expression)})";
                    case "Contains":
                        return
                            $"({ConvertExpression(expression.Object)} Like '%'+{GetArguments(expression)}+'%')";
                    case "ToLower":
                        return $"lower({ConvertExpression(expression.Object)})";
                    case "Trim":
                        return $"trim({ConvertExpression(expression.Object)})";
                    case "TrimStart":
                        return $"ltrim({ConvertExpression(expression.Object)})";
                    case "TrimEnd":
                        return $"rtrim({ConvertExpression(expression.Object)})";
                    case "Replace":
                        return $"replace({ConvertExpression(expression.Object)},{GetArguments(expression)})";
                }
                throw new ArgumentException("��֧�ַ���");
            }
            if (expression.Method.DeclaringType.IsValueType && expression.Method.Name == "Equals")
                return $"({ConvertExpression(expression.Object)} = {GetArguments(expression)})";
            if (expression.Method.DeclaringType == typeof(Math))
            {
                switch (expression.Method.Name)
                {
                    case "Abs":
                        return $"abs({GetArguments(expression)})";
                }
            }
            if (expression.Method.DeclaringType == typeof(Enum))
            {
                switch (expression.Method.Name)
                {
                    case "HasFlag":
                        return string.Format("({0} & {1}) = {1}", ConvertExpression(expression.Object), GetArguments(expression));
                }
            }
            if (expression.Method.DeclaringType.IsGenericTypeDefinition && expression.Method.DeclaringType.GetGenericTypeDefinition() == typeof(List<>))
            {
                switch (expression.Method.Name)
                {
                    case "Contains":
                        return $"{GetArguments(expression)} in ({ConvertExpression(expression.Object)})";
                }
            }
            var name = _condition.AddParameter(GetValue(expression));
            return $"@{name}";
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
                string field;
                if (!_columnMap.TryGetValue(expression.Member.Name, out field))
                {
                    throw new ArgumentException(@"�ֶβ����������ݿ���", expression.Member.Name);
                }
                return $"[{field}]";
            }
            var par1 = expression.Expression as MemberExpression;
            if (par1?.Expression is ParameterExpression)
            {
                if (par1.Type == typeof(string))
                {
                    switch (expression.Member.Name)
                    {
                        case "Length":
                            return $"LEN([{par1.Member.Name}])";
                    }
                }
                if (par1.Type == typeof(DateTime))
                {
                    switch (expression.Member.Name)
                    {
                        case "Now":
                            return $"@{_condition.AddParameter(DateTime.Now)}";
                        case "Today":
                            return $"@{_condition.AddParameter(DateTime.Today)}";
                    }
                }
                throw new ArgumentException("��֧�ֵ���չ����");
            }
            var vl = GetValue(expression);
            if (vl.GetType().IsValueType)
            {
                return $"@{_condition.AddParameter(vl)}";
            }
            if (vl is string)
            {
                return $"@{_condition.AddParameter(vl)}";
            }
            var ilist = vl as IList<int>;
            if (ilist != null)
            {
                return string.Join(",", ilist);
            }
            var slist = vl as IList<string>;
            if (slist != null)
            {
                return "'" + string.Join("','", slist) + "'";
            }
            var dlist = vl as IList<DateTime>;
            if (dlist != null)
            {
                return "'" + string.Join("','", dlist) + "'";
            }
            return $"@{_condition.AddParameter(vl)}";
        }

        /// <summary>
        ///     ת�����ʽ
        /// </summary>
        /// <param name="expression">����Lambda����</param>
        /// <returns>���ͺ��SQL�ı�</returns>
        private string Convert(ConstantExpression expression)
        {
            if (expression.Value == null || expression.Value.ToString().ToLower() == "null")
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
                    {
                        switch ((char)expression.Value)
                        {
                            case '\t':
                                return "'\t'";
                            case '\r':
                                return "'\r'";
                            case '\n':
                                return "'\n'";
                        }
                        return $"'{expression.Value}'";
                    }
            }
            var name = _condition.AddParameter(expression.Value);
            return $"@{name}";
        }
    }
}