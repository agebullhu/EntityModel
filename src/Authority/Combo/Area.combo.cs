/*design by:agebull designer date:2017/6/27 8:45:14*/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.WebUI;

namespace Agebull.SystemAuthority.Organizations.DataAccess
{
    partial class OrganizationDataAccess
    {
        /// <summary>
        /// 下拉列表键
        /// </summary>
        private const string comboKey = "ui:combo:Area";
        /// <summary>
        /// 下拉列表键
        /// </summary>
        private const string comboKey2 = "ui:combo:org:all";

        /// <summary>
        /// 取机构的下拉列表数据
        /// </summary>
        public static List<EasyComboValues> GetOrganization()
        {
            List<EasyComboValues> result;

            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                result = proxy.Get<List<EasyComboValues>>(comboKey2);
                if (result == null)
                {
                    var access = new OrganizationDataAccess();
                    List<OrganizationData> datas = access.All();
                    result = new List<EasyComboValues>
                    {
                        EasyComboValues.Empty
                    };
                    FormatOrganizationTree(result, datas, 0, 0);
                    proxy.Set(comboKey2, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 取机构的下拉列表数据
        /// </summary>
        static void FormatOrganizationTree(List<EasyComboValues> trees, List<OrganizationData> datas, int parId, int level)
        {
            StringBuilder head = new StringBuilder();
            if (level > 0)
            {
                head.Append('-', level * 4);
            }

            foreach (var data in datas.Where(p => p.ParentId == parId))
            {
                trees.Add(new EasyComboValues
                {
                    Key = data.Id,
                    Value = head + data.FullName
                });
                FormatOrganizationTree(trees, datas, data.Id, level + 1);
            }
        }

        /// <summary>
        /// 取得下拉列表值
        /// </summary>
        /// <returns></returns>
        public static List<EasyComboValues> GetComboValues()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbComboCache))
            {
                var result = proxy.Get<List<EasyComboValues>>(comboKey);
                if (result != null)
                    return result;
                var access = new AreaDataAccess();
                var datas = access.All();
                result = new List<EasyComboValues> { EasyComboValues.Empty };
                result.AddRange(datas.Select(p => new EasyComboValues(p.Id, p.ShortName)));
                proxy.Set(comboKey, result);
                return result;
            }
        }

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="entity"></param>
        protected sealed override void OnDataSaved(DataOperatorType operatorType,OrganizationData entity)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbComboCache))
            {
                proxy.RemoveKey(comboKey);
                proxy.RemoveKey(comboKey2);
            }
        }
    }
}