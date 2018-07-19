// /*****************************************************
// (c)2008-2013 Copy right www.Gboxt.com
// 作者:bull2
// 工程:CodeRefactor-Agebull.Common.SimpleDataAccess
// 建立:2014-12-07
// 修改:2014-12-07
// *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Gboxt.Common.SimpleDataAccess.SQLite
{
    /// <summary>
    ///     用于SQLite的Lambda表达式解析器
    /// </summary>
    public sealed class PredicateConvert
    {
        public ConditionItem ConditionItem
        {
            get;
            private set;
        }
        /// <summary>
        ///     关联字段
        /// </summary>
        private readonly string[] _columns;


        /// <summary>
        ///     构造
        /// </summary>
        public PredicateConvert()
        {
        }

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="columns">关联字段</param>
        public PredicateConvert(string[] columns)
        {
            this._columns = columns;
        }

        /// <summary>
        ///     分析出的条件文本
        /// </summary>
        public string ConditionSql
        {
            get
            {
                return ConditionItem.ConditionSql;
            }
            private set
            {
                ConditionItem.ConditionSql = value;
            }
        }

        /// <summary>
        ///     分析构建的参数
        /// </summary>
        public SQLiteParameter[] Parameters
        {
            get
            {
                return this.ConditionItem.Parameters;
            }
        }

        /// <summary>
        ///     取得名称
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetName(MemberExpression expression)
        {
            return expression.Member.Name;
        }

        /// <summary>
        ///     取得值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object GetValue(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            dynamic func = lambda.Compile();
            return func();
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="columns">关联字段</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns></returns>
        public static ConditionItem Convert<T>(string[] columns, Expression<T> predicate)
        {
            var convert = new PredicateConvert(columns);
            return convert.Convert(predicate);
        }

        /// <summary>
        ///     分析Lambda表达式
        /// </summary>
        /// <typeparam name="T">方法类型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        public ConditionItem Convert<T>(Expression<T> predicate)
        {
            this.ConditionItem = new ConditionItem();
            string sql = this.ConvertExpression(predicate.Body);
            this.ConditionSql = this.CheckSignle(sql);
            return ConditionItem;
        }

        /// <summary>
        ///     不成文的表达式处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string CheckSignle(string str)
        {
            return str.Contains("(") ? str : string.Format("{0} = 1", str);
        }

        private string ConvertExpression(Expression expression)
        {
            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
            {
                return Convert(binaryExpression);
            }
            var Unary = expression as UnaryExpression;
            if (Unary != null)
            {
                return this.Convert(Unary);
            }
            var call = expression as MethodCallExpression;
            if (call != null)
            {
                return this.Convert(call);
            }
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return Convert(memberExpression);
            }
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                return this.Convert(constantExpression);
            }
            var array = expression as NewArrayExpression;
            if (array != null)
            {
                var sb = new StringBuilder();
                foreach (var arg in array.Expressions)
                {
                    sb.AppendFormat(",{0}", this.ConvertExpression(arg));
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

        private string Convert(BinaryExpression expression)
        {
            //left
            var lefttext = this.ConvertExpression(expression.Left);
            //right
            var righttext = this.ConvertExpression(expression.Right);
            if (lefttext == null)
            {
                return righttext;
            }
            if (righttext == null)
            {
                return lefttext;
            }
            if (righttext == "null")
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        return string.Format("({0} IS NULL)", lefttext);
                    case ExpressionType.NotEqual:
                        return string.Format("({0} IS NOT NULL)", lefttext);
                    default:
                        throw new ArgumentException("Invalid lambda expression");
                }
            }
            //body
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    return string.Format("({0} AND {1})", this.CheckSignle(lefttext), this.CheckSignle(righttext));
                case ExpressionType.OrElse:
                    return string.Format("({0} OR {1})", this.CheckSignle(lefttext), this.CheckSignle(righttext));
                case ExpressionType.Equal:
                    return string.Format("({0} = {1})", lefttext, righttext);
                case ExpressionType.NotEqual:
                    return string.Format("({0} <> {1})", lefttext, righttext);
                case ExpressionType.GreaterThanOrEqual:
                    return string.Format("({0} >= {1})", lefttext, righttext);
                case ExpressionType.GreaterThan:
                    return string.Format("({0} > {1})", lefttext, righttext);
                case ExpressionType.LessThanOrEqual:
                    return string.Format("({0} <= {1})", lefttext, righttext);
                case ExpressionType.LessThan:
                    return string.Format("({0} < {1})", lefttext, righttext);
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
        }

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
                var ar = this.ConvertExpression(arg);
                sb.Append(ar ?? "NULL");
            }

            return isFirst ? null : sb.ToString();
        }

        private string Convert(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    return string.Format("NOT({0})", this.ConvertExpression(expression.Operand));
                default:
                    throw new ArgumentException("Invalid lambda expression");
            }
        }

        private string Convert(MethodCallExpression expression)
        {
            if (expression.Object is ParameterExpression)
            {
                throw new ArgumentException("不支持参数的方法(仅支持属性的方法)");
            }
            if (expression.Method.DeclaringType == typeof(string))
            {
                switch (expression.Method.Name)
                {
                    case "ToUpper":
                        return string.Format("upper({0})", this.ConvertExpression(expression.Object));
                    case "ToLower":
                        return string.Format("lower({0})", this.ConvertExpression(expression.Object));
                    case "Trim":
                        return string.Format("trim({0})", this.ConvertExpression(expression.Object));
                    case "TrimStart":
                        return string.Format("ltrim({0})", this.ConvertExpression(expression.Object));
                    case "TrimEnd":
                        return string.Format("rtrim({0})", this.ConvertExpression(expression.Object));
                    case "Replace":
                        return string.Format("replace({0},{1})", this.ConvertExpression(expression.Object), this.GetArguments(expression));
                }
                throw new ArgumentException("不支持方法");
            }
            if (expression.Method.DeclaringType == typeof(Math))
            {
                switch (expression.Method.Name)
                {
                    case "Abs":
                        return string.Format("abs({0})", this.GetArguments(expression));
                }
            }
            var name = this.ConditionItem.AddParameter(GetValue(expression));
            return string.Format("${0}", name);
        }

        private string Convert(MemberExpression expression)
        {
            if (expression.Expression is ParameterExpression)
            {
                if (this._columns.All(p => p != expression.Member.Name))
                {
                    throw new ArgumentException(@"字段不存在于数据库中", expression.Member.Name);
                }
                return string.Format("[{0}]", expression.Member.Name);
            }
            var par1 = expression.Expression as MemberExpression;
            if (par1 != null && par1.Expression is ParameterExpression)
            {
                if (par1.Type == typeof(string))
                {
                    switch (expression.Member.Name)
                    {
                        case "Length":
                            return string.Format("length([{0}])", par1.Member.Name);
                    }
                }
                if (par1.Type == typeof(DateTime))
                {
                    switch (expression.Member.Name)
                    {
                        case "Now":
                            return string.Format("datetime('now')");
                        case "Today":
                            return string.Format("date('now')");
                        case "Date":
                            return string.Format("date([{0}])", par1.Member.Name);
                        case "TimeOfDay":
                            return string.Format("time([{0}])", par1.Member.Name);
                        case "Day":
                            return string.Format("strftime('%d',[{0}])", par1.Member.Name);
                        case "Month":
                            return string.Format("strftime('%m',[{0}])", par1.Member.Name);
                        case "Year":
                            return string.Format("strftime('%Y',[{0}])", par1.Member.Name);
                        case "Hour":
                            return string.Format("strftime('%H',[{0}])", par1.Member.Name);
                        case "Minute":
                            return string.Format("strftime('%M',[{0}])", par1.Member.Name);
                        case "Second":
                            return string.Format("strftime('%S',[{0}])", par1.Member.Name);
                        case "Kind":
                            return string.Format("strftime('%s',[{0}])", par1.Member.Name);
                    }
                }
                throw new ArgumentException("不支持的扩展方法");
            }

            var name = this.ConditionItem.AddParameter(GetValue(expression));
            return string.Format("${0}", name);
        }

        private string Convert(ConstantExpression expression)
        {
            if (expression.Value == null || expression.Value.ToString().ToLower() == "null")
            {
                return "null";
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
                    switch ((char) expression.Value)
                    {
                        case '\t':
                            return "'\t'";
                        case '\r':
                            return "'\r'";
                        case '\n':
                            return "'\n'";
                    }
                    return string.Format("'{0}'", expression.Value);
                }
            }
            var name = this.ConditionItem.AddParameter(expression.Value);
            return string.Format("${0}", name);
        }
    }
}
