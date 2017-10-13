using System.Collections.Generic;
using System.Linq;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    /// <summary>
    /// 职位组织关联
    /// </summary>
    sealed partial class OrganizePositionDataAccess : HitoryTable<OrganizePositionData>
    {
        /// <summary>
        /// 下拉列表键
        /// </summary>
        private const string comboKey = "ui:combo:OrganizePosition";
        /// <summary>
        /// 取机构职位设置的下拉列表数据
        /// </summary>
        public static List<EasyComboValues> GetOrganizePosition(int oid)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbComboCache))
            {
                var result = proxy.Get<List<EasyComboValues>>(comboKey);
                if (result != null)
                    return result;
                var access = new OrganizePositionDataAccess();
                var list = oid == 0 ? access.All() : access.All(p => p.OrganizationId == oid);
                result = list.Select(p => new EasyComboValues(p.Id, p.Department + p.Position)).ToList();
                result.Insert(0, EasyComboValues.Empty);
                proxy.Set(comboKey, result);
                return result;
            }
        }

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="entity"></param>
        protected sealed override void OnDataSaved(DataOperatorType operatorType, OrganizePositionData entity)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                proxy.SetEntity(entity);
                proxy.RemoveKey(comboKey);
            }
        }
    }
}
