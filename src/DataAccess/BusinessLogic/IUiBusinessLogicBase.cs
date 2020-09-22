using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TEntity">���ݶ���</typeparam>
    /// <typeparam name="TPrimaryKey">��������</typeparam>
    public interface IUiBusinessLogicBase<TEntity,TPrimaryKey> : IBusinessLogicBase<TEntity, TPrimaryKey>
        where TEntity : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
    }
}