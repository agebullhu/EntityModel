namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     工作任务连接数据访问
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    public abstract class JobLinkDataAccess<TData> : RowScopeDataAccess<TData>
        where TData : EditDataObject, IRowScopeData, IAuditData, new()
    {
        /// <summary>
        ///     当前上下文读取的表名
        /// </summary>
        protected sealed override string ContextReadTable => _dynamicReadTable ??
                                                             (_dynamicReadTable = GetContextReadTable());

        /// <summary>
        ///  初始化基本条件
        /// </summary>
        /// <returns></returns>
        protected override void InitBaseCondition()
        {
            if (!BusinessContext.Current.IsUnSafeMode || BusinessContext.Current.IsSystemMode || BusinessContext.Current.LoginUser.DepartmentId <= 1)
                return;
            int did = BusinessContext.Current.LoginUser.DepartmentId;
            int uid = BusinessContext.Current.LoginUserId;
            BaseCondition = $@"`{ReadTableName}`.`{FieldDictionary["AuthorID"]}`={uid} OR `job`.`_to_user_id`={uid} 
OR `{ReadTableName}`.`{FieldDictionary["DepartmentId"]}`={did} OR `job`.`_department_id`={did}";
        }

        /// <summary>
        ///     构架基础表
        /// </summary>
        string GetContextReadTable()
        {
            if (!BusinessContext.Current.IsUnSafeMode || BusinessContext.Current.IsSystemMode || BusinessContext.Current.LoginUser.DepartmentId <= 1)
                return ReadTableName;

            var entity = new TData();
            int did = BusinessContext.Current.LoginUser.DepartmentId;
            return $@"(`{ReadTableName}` 
LEFT JOIN `tb_wf_user_job` `job`
       ON `job`.`_entity_type` = {entity.__Struct.EntityType} 
      AND `job`.`_department_id` = {did}
      AND `job`.`_link_id` = `{ReadTableName}`.`{FieldMap["Id"]}`)";
        }
    }
}