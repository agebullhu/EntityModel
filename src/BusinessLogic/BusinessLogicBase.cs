// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class BusinessLogicBase<TData, TAccess> : IBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
        #region ����֧�ֶ���

        /// <summary>
        ///     ʵ������
        /// </summary>
        public virtual int EntityType => 0;

        private TAccess _access;

        /// <summary>
        /// ���ݷ��ʶ���
        /// </summary>
        IDataTable<TData> IBusinessLogicBase<TData>.Access => Access;

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

        /// <summary>
        ///     ������ѯ����(SQL������ʽ)
        /// </summary>
        protected virtual string BaseQueryCondition => null;

        #endregion

        #region ��������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIds(IEnumerable<long> ids, Func<long, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    if (!func(id))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.Succeed();
            }
            return true;
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIdsToData(IEnumerable<long> ids, Func<TData, bool> func, Action onEnd = null)
        {
            using (var scope = TransactionScope.CreateScope(Access.DataBase))
            {
                foreach (var id in ids)
                {
                    var data = Access.LoadByPrimaryKey(id);
                    if (data == null || !func(data))
                    {
                        return false;
                    }
                }
                onEnd?.Invoke();
                scope.Succeed();
            }
            return true;
        }
        #endregion

        #region ������

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        [Obsolete]
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            var data = Access.FirstOrDefault(lambda);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public virtual TData Details(long id)
        {
            if (id == 0)
                return null;
            var data = Access.LoadByPrimaryKey(id);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public virtual async Task<TData> DetailAsync(long id)
        {
            if (id == 0)
                return null;
            var data = await Access.LoadByPrimaryKeyAsync(id);
            if (data == null)
                return null;
            OnDetailsLoaded(data, false);
            return data;
        }

        /// <summary>
        ///     ��ϸ��������
        /// </summary>
        protected virtual void OnDetailsLoaded(TData data, bool isNew)
        {
        }

        /// <summary>
        ///     ����һ����Ĭ��ֵ������
        /// </summary>
        public virtual TData CreateData()
        {
            var data = new TData();
            OnDetailsLoaded(data, true);
            return data;
        }

        #endregion

    }
}