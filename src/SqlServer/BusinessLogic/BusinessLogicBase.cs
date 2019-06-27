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
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Excel;
using Agebull.EntityModel.SqlServer;
using Agebull.MicroZero.ZeroApis;

#endregion

namespace Agebull.EntityModel.BusinessLogic.SqlServer
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicBase<TData, TAccess,TDatabase> : IBusinessLogicBase<TData>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : SqlServerTable<TData, TDatabase>, new()
        where TDatabase : SqlServerDataBase
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
                scope.SetState(true);
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
                scope.SetState(true);
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

        #region ���뵼��

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ApiFileResult Import(string sheetName, LambdaItem<TData> filter)
        {
            var exporter = new ExcelExporter<TData, TAccess>();
            var bytes = exporter.ExportExcel(filter, sheetName, null);
            return new ApiFileResult
            {
                Mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"OrderAddress-{DateTime.Now:yyyyMMDDHHmmSS}",
                Data = bytes
            };
        }

        #endregion
    }
}