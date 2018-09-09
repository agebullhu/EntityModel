// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicBase<TData, TAccess,TDatabase>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : MySqlTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        #region ����֧�ֶ���

        /// <summary>
        ///     ʵ������
        /// </summary>
        public virtual int EntityType => 0;

        private TAccess _access;

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        public TAccess Access => _access ?? (_access = CreateAccess());

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            var access = new TAccess();
            return access;
        }
        /// <summary>
        /// ����
        /// </summary>
        protected BusinessLogicBase()
        {
        }
        #endregion

        #region ��������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(string ids, Func<long, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIds(string ids, Func<long, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(Access.DataBase))
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        long id;
                        if (!long.TryParse(sid, out id))
                        {
                            return false;
                        }
                        if (!func(id))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIdsToData(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                using (var scope = TransactionScope.CreateScope(Access.DataBase))
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        long id;
                        if (!long.TryParse(sid, out id))
                        {
                            return false;
                        }
                        var data = Access.LoadByPrimaryKey(id);
                        if (data == null || !func(data))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }
        #endregion

        #region ������

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public List<TData> All()
        {
            return Access.All();
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public List<TData> All(LambdaItem<TData> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            return Access.FirstOrDefault(lambda);
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public virtual TData Details(long id)
        {
            return id == 0 ? null : Access.LoadByPrimaryKey(id);
        }

        #endregion
    }
}