// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.Extends;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ������ʷ��¼��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    /// <typeparam name="TDatabase">���ݿ����</typeparam>
    public class BusinessLogicByHistory<TData, TAccess, TDatabase> : BusinessLogicByStateData<TData, TAccess, TDatabase>
        where TData : EditDataObject, IStateData, IHistoryData, IIdentityData, new()
        where TAccess : HitoryTable<TData, TDatabase>, new()
        where TDatabase : MySqlDataBase
    {
        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        protected override bool DoResetState(TData data)
        {
            if (data == null)
                return false;
            data.AddDate = DateTime.MinValue;
            data.AuthorId = 0;
            data.LastModifyDate = DateTime.MinValue;
            data.LastReviserId = 0;
            return base.DoResetState(data);
        }
    }
}