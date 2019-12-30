// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Agebull.EntityModel.Events;

using Agebull.EntityModel.Common;
using Agebull.MicroZero.ZeroApis;

#endregion

namespace Agebull.EntityModel.MySql
{
    partial class MySqlTable<TData, TMySqlDataBase>
    {
        #region ����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstAsync()
        {
            return await LoadFirstAsync();
        }


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<TData> FirstAsync(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<TData> FirstOrDefaultAsync(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstOrDefaultAsync()
        {
            return await LoadFirstAsync();
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadFirstAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<TData> FirstAsync(string condition, DbParameter[] args)
        {
            return await LoadFirstAsync(condition, args);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstOrDefaultAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadFirstAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstOrDefaultAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadFirstAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> FirstOrDefaultAsync(string condition, DbParameter[] args)
        {
            return await LoadFirstAsync(condition, args);
        }

        #endregion

        #region β��

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastAsync()
        {
            return await LoadLastAsync();
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastOrDefaultAsync()
        {
            return await LoadLastAsync();
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadLastAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadLastAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<TData> LastAsync(string condition, DbParameter[] args)
        {
            return await LoadLastAsync(condition, args);
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastOrDefaultAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);

            return await LoadLastAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastOrDefaultAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadLastAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters));
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public async Task<TData> LastOrDefaultAsync(string condition, DbParameter[] args)
        {
            return await LoadLastAsync(condition, args);
        }

        #endregion

        #region All

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public async Task<List<TData>> AllAsync()
        {
            return await LoadDataInnerAsync();
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public async Task<List<TData>> AllAsync(string condition, DbParameter[] args)
        {
            return await LoadDataInnerAsync(condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> AllAsync(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await LoadPageDataAsync(1, -1, PrimaryKey, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> AllAsync<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy,
            bool desc)
        {
            var convert = Compile(lambda);
            return await LoadPageDataAsync(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        public async Task<List<TData>> AllAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await LoadDataInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public async Task<List<TData>> AllAsync(LambdaItem<TData> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return await LoadDataInnerAsync(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public async Task<List<TData>> AllAsync(Expression<Func<TData, bool>> lambda, params string[] orderBys)
        {
            var convert = Compile(lambda);
            return await LoadDataInnerAsync(convert.ConditionSql, convert.Parameters,
                orderBys.Length == 0 ? null : string.Join(",", orderBys));
        }

        #endregion


        #region �ۺϺ���֧��

        #region Collect

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public async Task<object> CollectAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            return await CollectInnerAsync(fun, FieldMap[field], condition, args);
        }


        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public async Task<object> CollectAsync(string fun, string field, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await CollectInnerAsync(fun, field, convert.ConditionSql, convert.Parameters);
        }

        #endregion

        #region Exist

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public async Task<bool> ExistAsync()
        {
            return await CountAsync() > 0;
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public async Task<bool> ExistAsync(string condition, params DbParameter[] args)
        {
            return await CountAsync(condition, args) > 0;
        }

        /// <summary>
        ///     �Ƿ���ڴ�����������
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> ExistPrimaryKeyAsync<T>(T id)
        {
            return await ExistInnerAsync(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(id));
        }

        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<long> CountAsync()
        {
            return await CountInnerAsync();
        }

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<long> CountAsync(string condition, params DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0L : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<long> CountAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            var obj = await CollectInnerAsync("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<long> CountAsync(LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            var obj = await CollectInnerAsync("Count", "*", convert.ConditionSql, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<long> CountAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Count", "*",
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }


        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<long> CountAsync<TValue>(Expression<Func<TData, TValue>> field, string condition, params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Count", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }
        #endregion


        #region Any

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync()
        {
            return await ExistInnerAsync();
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync(string condition, DbParameter[] args)
        {
            return await ExistInnerAsync(condition, args);
        }


        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync(Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await ExistInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<bool> AnyAsync(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b)
        {
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            return await ExistInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }

        #endregion


        #region Sum

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<decimal> SumAsync(string field)
        {
            var obj = await CollectInnerAsync("Sum", FieldMap[field], null, null);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<decimal> SumAsync(string field, string condition, params DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Sum", FieldMap[field], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var obj = await CollectInnerAsync("Sum", FieldMap[expression.Member.Name], condition, convert.Parameters);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a,
            Expression<Func<TData, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = Compile(a);
            var convert2 = Compile(b);
            var obj = await CollectInnerAsync("Sum", FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ���ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TData, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var obj = await CollectInnerAsync("Sum", FieldMap[expression.Member.Name], condition, args);
            return obj == DBNull.Value || obj == null ? 0M : Convert.ToDecimal(obj);
        }

        #endregion

        #region ʵ��

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition = null, DbParameter args = null)
        {
            return await ExistInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition, DbParameter[] args)
        {
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj != DBNull.Value && obj != null && Convert.ToInt64(obj) > 0;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected async Task<long> CountInnerAsync(string condition = null, DbParameter args = null)
        {
            var obj = await CollectInnerAsync("Count", "*", condition, args);
            return obj == DBNull.Value || obj == null ? 0 : Convert.ToInt64(obj);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected async Task<object> CollectInnerAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = CreateCollectSql(fun, field, condition);
            return await DataBase.ExecuteScalarAsync(sql, args);
        }

        #endregion

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync(int page, int limit)
        {
            return await PageDataAsync(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, KeyField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> LoadDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, desc, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public async Task<List<TData>> PageDataAsync(int page, int limit, string order, bool desc, string condition, params DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return await LoadPageDataAsync(page, limit, order, desc, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> LoadDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda)
        {
            return await PageDataAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TData>> PageDataAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await PageDataAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }



        private async Task<List<TData>> LoadPageDataAsync(int page, int limit, string order, bool desc, string condition, DbParameter[] args)
        {
            var results = new List<TData>();
            var sql = CreatePageSql(page, limit, order, desc, condition);
            using (var cmd = DataBase.CreateCommand(sql, args))
            {
                using (var reader = (MySqlDataReader)(await cmd.ExecuteReaderAsync()))
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(LoadEntity(reader));
                    }
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit)
        {
            return await PageAsync(page, limit, KeyField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit, LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, KeyField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageAsync(page, limit, KeyField, false, condition, args);
        }


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit, string order, string condition, params DbParameter[] args)
        {
            return await PageAsync(page, limit, order, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda)
        {
            return await PageAsync(page, limit, field, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<ApiPageData<TData>> PageAsync<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            LambdaItem<TData> lambda)
        {
            var convert = Compile(lambda);
            return await PageAsync(page, limit, GetPropertyName(field), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public async Task<ApiPageData<TData>> PageAsync(int page, int limit, string order, bool desc, string condition,
            params DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return await LoadPageAsync(page, limit, order, desc, condition, args);
        }

        private async Task<ApiPageData<TData>> LoadPageAsync(int page, int limit, string order, bool desc, string condition,
            DbParameter[] args)
        {
            var data = await PageDataAsync(page, limit, order, desc, condition, args);
            var count = await CountAsync(condition, args);
            return new ApiPageData<TData>
            {
                RowCount = (int)count,
                Rows = data,
                PageIndex = page,
                PageSize = limit,
                PageCount = (int)(count / limit + ((count % limit) > 0 ? 1 : 0))
            };
        }

        #endregion

        #region ���ж�ȡ

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public async Task<TField> LoadValueAsync<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = Compile(lambda);
            var val = await LoadValueInnerAsync(fn, convert.ConditionSql, convert.Parameters);
            return val == null || val == DBNull.Value ? default : (TField)val;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        public async Task<object> ReadAsync<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = await LoadValueInnerAsync(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, GetDbType(KeyField), key));
            return vl == DBNull.Value || vl == null ? default(TField) : vl;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        public async Task<TField> LoadValueAsync<TField, TKey>(Expression<Func<TData, TField>> field, TKey key)
        {
            var fn = GetPropertyName(field);
            var vl = await LoadValueInnerAsync(fn, FieldConditionSQL(KeyField), CreateFieldParameter(KeyField, GetDbType(KeyField), key));
            return vl == DBNull.Value || vl == null ? default : (TField)vl;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TData, TField>> fieldExpression,
            Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            Debug.Assert(FieldDictionary.ContainsKey(field));
            var sql = CreateLoadValuesSql(field, convert);
            var values = new List<TField>();

            using (var cmd = DataBase.CreateCommand(sql, convert.Parameters))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var vl = reader.GetValue(0);
                        if (vl != DBNull.Value && vl != null)
                            values.Add((TField)vl);
                    }
                }
            }

            return values;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>����</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TData, TField>> fieldExpression, string condition)
        {
            var field = GetPropertyName(fieldExpression);

            var result = await LoadValuesInnerAsync(field, condition);
            return result.Count == 0 ? new List<TField>() : result.Select(p => (TField)p).ToList();
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="parse">ת���������ͷ���</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TData, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TData, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = Compile(lambda);
            var result = await LoadValuesInnerAsync(field, convert.ConditionSql, convert.Parameters);
            return result.Count == 0 ? new List<TField>() : result.Select(parse).ToList();
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public async Task<object> LoadValueAsync(string field, string condition, params DbParameter[] args)
        {
            return await LoadValueInnerAsync(field, condition, args);
        }

        /// <summary>
        ///     ��ȡֵ
        /// </summary>
        protected async Task<object> LoadValueInnerAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = CreateLoadValueSql(field, condition);
            return await DataBase.ExecuteScalarAsync(sql, args);
        }


        /// <summary>
        ///     ��ȡ���ֵ
        /// </summary>
        protected async Task<List<object>> LoadValuesInnerAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = CreateLoadValueSql(field, condition);
            var values = new List<object>();

            using (var cmd = DataBase.CreateCommand(sql, args))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var vl = reader.GetValue(0);
                        if (vl != DBNull.Value && vl != null)
                            values.Add(vl);
                    }
                }
            }

            return values;
        }

        #endregion

        #region ���ݶ�ȡ

        /// <summary>
        ///     ������������
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<List<TData>> LoadDataAsync(MulitCondition condition)
        {
            if (condition == null || string.IsNullOrEmpty(condition.Condition))
                return new List<TData>();
            if (condition.Parameters == null)
                return LoadDataInner(condition.Condition);
            List<DbParameter> args = new List<DbParameter>();
            foreach (var item in condition.Parameters)
            {
                var pa = new MySqlParameter(item.Name, item.Type);
                if (item.Value == null)
                    pa.Value = DBNull.Value;
                else
                    switch (item.Type)
                    {
                        default:
                            //case DbType.Xml:
                            //case DbType.String:
                            //case DbType.StringFixedLength:
                            //case DbType.AnsiStringFixedLength:
                            //case DbType.AnsiString:
                            pa.Size = item.Value.Length * 2;
                            pa.Value = item.Value;
                            break;
                        case DbType.Boolean:
                            {
                                pa.Value = bool.TryParse(item.Value, out var vl) && vl;
                            }
                            break;
                        case DbType.Byte:
                            {
                                if (byte.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = (byte)0;
                            }
                            break;
                        case DbType.VarNumeric:
                        case DbType.Decimal:
                        case DbType.Currency:
                            {
                                if (decimal.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = (decimal)0;
                            }
                            break;
                        case DbType.Time:
                        case DbType.DateTime2:
                        case DbType.DateTime:
                        case DbType.Date:
                            {
                                if (DateTime.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.DateTimeOffset:
                            {
                                if (TimeSpan.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Double:
                            {
                                if (double.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Guid:
                            {
                                if (Guid.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int16:
                            {
                                if (short.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int32:
                            {
                                if (int.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int64:
                            {
                                if (long.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.SByte:
                            break;
                        case DbType.Single:
                            {
                                if (float.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt16:
                            {
                                if (ushort.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt32:
                            {
                                if (uint.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt64:
                            {
                                if (ulong.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                    }
                args.Add(pa);
            }
            return await LoadDataInnerAsync(condition.Condition, args.ToArray());
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<TData> LoadDataAsync(object id)
        {
            return await LoadByPrimaryKeyAsync(id);
        }


        /// <summary>
        ///     ȫ���ȡ
        /// </summary>
        public async Task<List<TData>> LoadDataAsync()
        {
            return await LoadDataInnerAsync();
        }


        /// <summary>
        ///     ������ȡ
        /// </summary>
        public async Task<List<TData>> LoadDataAsync(string condition, params DbParameter[] args)
        {
            return await LoadDataInnerAsync(condition, args);
        }

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public virtual async Task<TData> LoadByPrimaryKeyAsync(object key)
        {
            return await LoadFirstInnerAsync(PrimaryKeyConditionSQL, CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public async Task<List<TData>> LoadByPrimaryKeiesAsync(IEnumerable keies)
        {
            var list = new List<TData>();
            var par = CreatePimaryKeyParameter();
            foreach (var key in keies)
            {
                par.Value = key;
                list.Add(await LoadFirstInnerAsync(PrimaryKeyConditionSQL, par));
            }

            return list;
        }


        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public async Task<TData> LoadFirstAsync(string condition = null)
        {
            return await LoadFirstInnerAsync(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public async Task<TData> LoadFirstAsync(string condition, params DbParameter[] args)
        {
            return await LoadFirstInnerAsync(condition, args);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public async Task<TData> LoadFirstAsync(string foreignKey, object key)
        {
            return await LoadFirstInnerAsync(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public async Task<TData> LoadLastAsync(string condition = null)
        {
            return await LoadLastInnerAsync(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public async Task<TData> LoadLastAsync(string condition, params DbParameter[] args)
        {
            return await LoadLastInnerAsync(condition, args);
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public async Task<TData> LoadLastAsync(string foreignKey, object key)
        {
            return await LoadLastInnerAsync(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public async Task<List<TData>> LoadByForeignKeyAsync(string foreignKey, object key)
        {
            return await LoadDataInnerAsync(FieldConditionSQL(foreignKey), CreateFieldParameter(foreignKey, GetDbType(foreignKey), key));
        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public async Task ReLoadAsync(TData entity)
        {
            await ReLoadInnerAsync(entity);
            entity.OnStatusChanged(NotificationStatusType.Refresh);
        }

        #endregion

        #region ��������


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected async Task<TData> LoadFirstInnerAsync(string condition = null, DbParameter args = null)
        {
            return await LoadFirstInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected async Task<TData> LoadFirstInnerAsync(string condition, DbParameter[] args)
        {
            TData entity = null;
            using (var cmd = CreateLoadCommand(condition, args))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                        entity = LoadEntity(reader);
                }
            }

            if (entity != null)
                entity = EntityLoaded(entity);
            return entity;
        }


        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected async Task<TData> LoadLastInnerAsync(string condition = null, DbParameter args = null)
        {
            return await LoadLastInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected async Task<TData> LoadLastInnerAsync(string condition, DbParameter[] args)
        {
            TData entity = null;

            using (var cmd = CreateLoadCommand(KeyField, true, condition, args))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                        entity = LoadEntity(reader);
                }
            }

            if (entity != null)
                entity = EntityLoaded(entity);
            return entity;
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected async Task<List<TData>> LoadDataInnerAsync(string condition = null, DbParameter args = null)
        {
            return await LoadDataInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args }, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected async Task<List<TData>> LoadDataInnerAsync(string condition, DbParameter[] args)
        {
            return await LoadDataInnerAsync(condition, args, null);
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected async Task<List<TData>> LoadDataInnerAsync(string condition, DbParameter[] args, string orderBy)
        {
            var results = new List<TData>();

            {
                using (var cmd = CreateLoadCommand(condition, orderBy, args))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                            results.Add(LoadEntity(reader));
                    }
                }
                for (var index = 0; index < results.Count; index++)
                    results[index] = EntityLoaded(results[index]);
            }
            return results;
        }

        /// <summary>
        ///     ��ȡȫ��(SQL�������д��,�ֶ����ƺ�˳�������ʱ��ͬ)
        /// </summary>
        protected async Task<List<TData>> LoadDataBySqlAsync(string sql, DbParameter[] args)
        {
            var results = new List<TData>();

            using (var cmd = DataBase.CreateCommand(sql, args))
            {
                var task = cmd.ExecuteReaderAsync();
                task.Wait();
                using (var reader = (MySqlDataReader)task.Result)
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(LoadEntity(reader));
                    }
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        /// <summary>
        ///     ��ȡ�洢����
        /// </summary>
        public async Task<List<TData>> LoadDataByProcedureAsync(string procedure, DbParameter[] args)
        {
            var results = new List<TData>();

            using (var cmd = DataBase.CreateCommand(procedure, args))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var res = await cmd.ExecuteReaderAsync();
                using (var reader = (MySqlDataReader)res)
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(LoadEntity(reader));
                    }
                }
            }

            for (var index = 0; index < results.Count; index++)
                results[index] = EntityLoaded(results[index]);
            return results;
        }

        #endregion

        #region ����У��֧��

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TData, TValue>> field, object val, Expression<Func<TData, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = Compile(condition);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            convert.AddAndCondition($"(`{FieldDictionary[fieldName]}` = ?c_vl_)", new MySqlParameter
            {
                ParameterName = "c_vl_",
                MySqlDbType = GetDbType(fieldName),
                Value = val
            });
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TData, TValue>> field, object val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            return !await ExistAsync($"(`{FieldDictionary[fieldName]}` = ?c_vl_ AND {FieldConditionSQL(PrimaryKey, "<>")}"
                , new MySqlParameter
                {
                    ParameterName = "c_vl_",
                    MySqlDbType = GetDbType(fieldName),
                    Value = val
                }
                , CreatePimaryKeyParameter(key));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TData, TValue>> field, object val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(FieldDictionary.ContainsKey(fieldName));
            return !await ExistAsync($"(`{FieldDictionary[fieldName]}` = ?c_vl_)"
                , new MySqlParameter
                {
                    ParameterName = "c_vl_",
                    MySqlDbType = GetDbType(fieldName),
                    Value = val
                });
        }

        #endregion
    }
}