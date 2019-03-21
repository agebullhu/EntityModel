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
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Gboxt.Common.DataModel.MySql;
using MySql.Data.MySqlClient;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Sqlʵ�������
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    public interface IDataTable<TData>
        where TData : EditDataObject, new()
    {
        #region ���ݿ�
        
        /// <summary>
        ///     �Զ��������Ӷ���
        /// </summary>
        IDataBase DataBase
        {
            get;
        }

        #endregion

        #region ���ݽṹ
        
        /// <summary>
        ///     �ֶ��ֵ�(����ʱ)
        /// </summary>
        Dictionary<string, string> FieldDictionary { get; }

        /// <summary>
        ///     ���Ψһ��ʶ
        /// </summary>
        int TableId { get; }
        /// <summary>
        ///     ���ʱ�������ֶ�
        /// </summary>
        string PrimaryKey { get; }

        #endregion

        #region ��

        #region ��������

        /// <summary>
        ///     ��������
        /// </summary>

        void FeachAll(Action<TData> action, Action<List<TData>> end);

        #endregion
        

        #region ����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData First();

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData FirstOrDefault();

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData FirstOrDefault(object id);


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData First(object id);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData First(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData First(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);
        

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData FirstOrDefault(Expression<Func<TData, bool>> lambda);


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData FirstOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);
        
        #endregion

        #region β��

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData Last();

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData LastOrDefault();

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData Last(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData Last(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData LastOrDefault(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        TData LastOrDefault(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);
        
        #endregion

        #region Select

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        List<TData> Select();

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>����</returns>
        List<TData> Select(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        List<TData> Select(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region All

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        List<TData> All();

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> All<TField>(Expression<Func<TData, bool>> lambda, Expression<Func<TData, TField>> orderBy,
            bool desc);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        List<TData> All(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);
        
        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        List<TData> All(Expression<Func<TData, bool>> lambda, params string[] orderBys);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> LoadData(int page, int limit, string order, bool desc, string condition, params DbParameter[] args);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> PageData(int page, int limit, string order,bool desc, string condition, params DbParameter[] args);
        #endregion

        #region Where

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        List<TData> Where(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�Ƿ��������</returns>
        List<TData> Where(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion


        #region �ۺϺ���֧��

        #region Collect

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        object Collect(string fun, string field, Expression<Func<TData, bool>> lambda);

        #endregion

        #region Exist

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        bool Exist();

        /// <summary>
        ///     �Ƿ���ڴ�����������
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>�Ƿ��������</returns>
        bool ExistPrimaryKey<T>(T id);

        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        long Count();
        
        #endregion

        #region SUM

        /// <summary>
        ///     ����
        /// </summary>
        decimal Sum(string field);
        
        #endregion


        #region Any

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        bool Any();

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        bool Any(Expression<Func<TData, bool>> lambda);


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        bool Any(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);

        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        long Count(Expression<Func<TData, bool>> lambda);
        
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        long Count(Expression<Func<TData, bool>> a, Expression<Func<TData, bool>> b);


        /// <summary>
        ///     ����
        /// </summary>
        long Count(string condition, params DbParameter[] args);
        #endregion

        #region Sum

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> lambda,
            string condition2 = null);

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ���ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        decimal Sum<TValue>(Expression<Func<TData, TValue>> field, Expression<Func<TData, bool>> a,
            Expression<Func<TData, bool>> b);
        

        #endregion
        

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> PageData(int page, int limit);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda);
        
        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field,
            Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        List<TData> PageData<TField>(int page, int limit, Expression<Func<TData, TField>> field, bool desc,
            Expression<Func<TData, bool>> lambda);

        #endregion

        #region ���ж�ȡ
        

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        TField LoadValue<TField>(Expression<Func<TData, TField>> field, Expression<Func<TData, bool>> lambda);
        
        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        TField LoadValue<TField, TKey>(Expression<Func<TData, TField>> field, TKey key);
        
        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>����</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression, string condition);

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="parse">ת���������ͷ���</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        List<TField> LoadValues<TField>(Expression<Func<TData, TField>> fieldExpression,
            Func<object, TField> parse, Expression<Func<TData, bool>> lambda);
        
        #endregion

        #region ���ݶ�ȡ

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        TData LoadData(object id);

        /// <summary>
        ///     ȫ���ȡ
        /// </summary>
        List<TData> LoadData();

        /// <summary>
        ///     ������ȡ
        /// </summary>
        TData LoadByPrimaryKey(object key);

        /// <summary>
        ///     ������ȡ
        /// </summary>
        List<TData> LoadByPrimaryKeies(IEnumerable keies);


        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        TData LoadFirst(string condition = null);

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        TData LoadFirst(string foreignKey, object key);

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        TData LoadLast(string condition = null);

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        TData LoadLast(string foreignKey, object key);

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        List<TData> LoadByForeignKey(string foreignKey, object key);
        

        #endregion
        

        #endregion

        #region д
        
        #region ���ݲ���

        /// <summary>
        ///     ��������
        /// </summary>
        void Save(IEnumerable<TData> entities);

        /// <summary>
        ///     ��������
        /// </summary>
        void Save(TData entity);

        /// <summary>
        ///     ��������
        /// </summary>
        void Update(TData entity);

        /// <summary>
        ///     ��������
        /// </summary>
        void Update(IEnumerable<TData> entities);

        /// <summary>
        ///     ����������
        /// </summary>
        bool Insert(TData entity);

        /// <summary>
        ///     ����������
        /// </summary>
        void Insert(IEnumerable<TData> entities);

        /// <summary>
        ///     ɾ������
        /// </summary>
        void Delete(IEnumerable<TData> entities);

        /// <summary>
        ///     ɾ������
        /// </summary>
        int Delete(TData entity);

        /// <summary>
        ///     ɾ������
        /// </summary>
        int Delete(object id);
        

        /// <summary>
        ///     ɾ������
        /// </summary>
        int DeletePrimaryKey(object key);

        /// <summary>
        ///     �����������
        /// </summary>
        void Clear();

        /// <summary>
        ///     ����ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        int Delete(Expression<Func<TData, bool>> lambda);

        /// <summary>
        ///     ǿ������ɾ��
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        int Destroy(Expression<Func<TData, bool>> lambda);
        
        #endregion

        #region ��������

        /// <summary>
        ///     ��������
        /// </summary>
        void SaveValue(string field, object value, string[] conditionFiles, object[] values);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        int SetValue(string field, object value, object key);

        /// <summary>
        ///     ȫ������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <returns>��������</returns>
        int SetValue<TField>(Expression<Func<TData, TField>> field, TField value);

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="lambda">����</param>
        /// <returns>��������</returns>
        int SetValue<TField>(Expression<Func<TData, TField>> field, TField value,
            Expression<Func<TData, bool>> lambda);
        
        #endregion

        #region �򵥸���
        
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="value">ֵ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        int SetValue<TField, TKey>(Expression<Func<TData, TField>> fieldExpression, TField value, TKey key);

        /// <summary>
        ///     ����ֶΰ��Զ�����ʽ����ֵ
        /// </summary>
        /// <param name="valueExpression">ֵ��SQL��ʽ</param>
        /// <param name="key">����</param>
        /// <returns>��������</returns>
        int SetCoustomValue<TKey>(string valueExpression, TKey key);

        #endregion

        #endregion


        #region ����У��֧��

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val,Expression<Func<TData, bool>> condition);

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val, object key);

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        bool IsUnique<TValue>(Expression<Func<TData, TValue>> field, object val);

        #endregion
    }
}