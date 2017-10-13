using Agebull.SystemAuthority.Organizations;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     行级权限帮助类
    /// </summary>
    public class RowScopeDataAccessHelper<TData, TAccess>
        where TData : EditDataObject, IRowScopeData, new()
        where TAccess : MySqlTable<TData>
    {
        private static PositionDataScopeType DataScope()
        {
            if (BusinessContext.Current.IsSystemMode)
                return PositionDataScopeType.None;
            var page = BusinessContext.Current.PageItem ?? PageItemLogical.GetPageByDataType(typeof(TData));
            if (page == null)
                return PositionDataScopeType.None;
            var power =
                BusinessContext.Current.PowerChecker.LoadPagePower(BusinessContext.Current.LoginUser, page) as
                    RolePowerData;
            return power?.DataScope ?? PositionDataScopeType.None;
        }

        /// <summary>
        ///     配置为行级权限表
        /// </summary>
        /// <param name="access"></param>
        public static void ConfigAccess(TAccess access)
        {
            //顶级部门无限制
            if (BusinessContext.Current.LoginUser.DepartmentId <= 1)
                return;

            var entity = new TData();
            var jobJoin = $@"(`{access.TableName}` LEFT JOIN `tb_wf_user_job` `job`
ON `job`.`entity_type` = {entity.__Struct.EntityType} AND `job`.`department_id` = {BusinessContext.Current.LoginUser.DepartmentId} AND
   `job`.`link_id` = `{access.TableName}`.`{access.FieldMap["Id"]}`)";

            var jobCondition = "`s`.`link_id` IS NOT NULL OR `view_scope` >= 2";
            if (BusinessContext.Current.LoginUser.DepartmentId < 0) //专家投资者
            {
                access.SetDynamicReadTable($"`{access.TableName}` {jobJoin}");
                access.BaseCondition = jobCondition;
                return;
            }
            var selfCondition = $"{access.FieldMap["AuthorID"]}` = {BusinessContext.Current.LoginUserId} OR `{access.FieldMap["LastReviserID"]}` = {BusinessContext.Current.LoginUserId}";
            switch (DataScope())
            {
                case PositionDataScopeType.None:
                    break;
                case PositionDataScopeType.Self:
                    access.SetDynamicReadTable($"`{access.TableName}` {jobJoin}");
                    access.BaseCondition = $"{jobCondition} OR {selfCondition}";
                    break;
                case PositionDataScopeType.Company:
                    access.SetDynamicReadTable($"`{access.TableName}` {jobJoin}");
                    access.BaseCondition = $"{jobCondition} OR {selfCondition} OR `{access.FieldMap["DepartmentId"]}` = {BusinessContext.Current.LoginUser.DepartmentId}";
                    break;
                case PositionDataScopeType.Department:
                    access.SetDynamicReadTable($"`{access.TableName}` {jobJoin}");
                    access.BaseCondition = $"{jobCondition} OR {selfCondition} OR `{access.FieldMap["AeraId"]}` = {BusinessContext.Current.LoginUser.DepartmentId}";
                    break;
                case PositionDataScopeType.DepartmentAndLower:
                    access.SetDynamicReadTable($@"`{access.TableName}` {jobJoin}
LEFT JOIN `slave_id` `s` ON `{access.TableName}`.`{access.FieldMap["DepartmentId"]}` = `s`.`slave_id`)");
                    access.BaseCondition = $"{jobCondition} OR {selfCondition} OR `s`.`master_id` = {BusinessContext.Current.LoginUser.DepartmentId}";
                    break;
                case PositionDataScopeType.CompanyAndLower:
                    access.SetDynamicReadTable($@"`{access.TableName}` {jobJoin}
LEFT JOIN `slave_id` `s` ON `{access.TableName}`.`{access.FieldMap["AeraId"]}` = `s`.`slave_id`)");
                    access.BaseCondition = $"{jobCondition} OR {selfCondition} OR `s`.`master_id` = {BusinessContext.Current.LoginUser.DepartmentId}";
                    break;
                default:
                    access.BaseCondition = "1=0"; //什么也读不到
                    break;
            }
        }
    }
}