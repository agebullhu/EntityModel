using Agebull.SystemAuthority.Organizations.DataAccess;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.Workflow;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class RowScopeDataAccess<TData> : HitoryTable<TData>
        where TData : EditDataObject, IRowScopeData, new()
    {

        /// <summary>
        ///  初始化基本条件
        /// </summary>
        /// <returns></returns>
        protected override void InitBaseCondition()
        {
            if (!BusinessContext.Current.IsUnSafeMode || BusinessContext.Current.IsSystemMode || BusinessContext.Current.LoginUser.DepartmentId <= 1)
                return;
            BaseCondition = $"`{FieldDictionary["AuthorID"]}`={BusinessContext.Current.LoginUserId} OR `{FieldDictionary["DepartmentId"]}`={BusinessContext.Current.LoginUser.DepartmentId}";
        }

        /// <summary>
        ///     保存前处理(Insert或Update)
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">当前实体生存状态</param>
        protected override void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {
            if (operatorType == DataOperatorType.Insert)
            {
                var sponsor = entity as ISponsor;
                if (sponsor != null)
                {
                    entity.DepartmentId = sponsor.SponsorId;
                    var oaccess = new OrganizationDataAccess();
                    entity.DepartmentLevel = oaccess.LoadValue(p => p.OrgLevel, sponsor.SponsorId);
                }
                else
                {
                    entity.DepartmentId = BusinessContext.Current.LoginUser.DepartmentId;
                    entity.DepartmentLevel = BusinessContext.Current.LoginUser.DepartmentLevel;
                }
            }
            base.OnPrepareSave(operatorType, entity);
        }

        /// <summary>
        ///     载入后的同步处理
        /// </summary>
        /// <param name="entity"></param>
        protected override TData OnEntityLoaded(TData entity)
        {
            if (entity.DataState == DataStateType.None)
            {
                if (entity.DepartmentId == BusinessContext.Current.LoginUser.DepartmentId)
                {
                    if (entity.AuthorID == 0)
                        SetValue(p => p.AuthorID, BusinessContext.Current.LoginUserId, entity.Id);
                }
                else if (entity.AuthorID != BusinessContext.Current.LoginUserId)
                {
                    entity.DataState = DataStateType.Orther;
                }
            }
            var audit = entity as IAuditData;
            if (audit == null)
                return entity;
            var levelAudit = entity as ILevelAuditData;
            if (levelAudit == null && entity.DepartmentId == BusinessContext.Current.LoginUser.DepartmentId &&
                audit.AuditState <= AuditStateType.Submit)
            {
                audit.CanAudit = true;
            }

            return entity;
        }
    }
}