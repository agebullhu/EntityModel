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
    /// 支持界面操作的业务逻辑对象基类
    /// </summary>
    /// <typeparam name="TEntity">数据对象</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IUiBusinessLogicBase<TEntity,TPrimaryKey> : IBusinessLogicBase<TEntity, TPrimaryKey>
        where TEntity : EditDataObject, IIdentityData<TPrimaryKey>, new()
    {
    }
}