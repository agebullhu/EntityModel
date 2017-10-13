/*design by:agebull designer date:2017/6/27 8:45:14*/
using System.Collections.Generic;
using System.Linq;
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
        /// 取得下拉列表值
        /// </summary>
        /// <returns></returns>
        public static List<EasyComboValues> GetComboValues()
        {
            using (var proxy = new RedisProxy(RedisProxy.DbComboCache))
            {
                var result = proxy.Client.Get<List<EasyComboValues>>(comboKey);
                if (result == null)
                {
                    var access = new AreaDataAccess();
                    var datas = access.All();
                    result = new List<EasyComboValues>{EasyComboValues.Empty};
                    result.AddRange(datas.Select(p => new EasyComboValues(p.Id, p.ShortName)));
                    proxy.Client.Set(comboKey, result);
                }
                return result;
            }
        }

        /// <summary>
        ///     保存完成后期处理(Insert或Update)
        /// </summary>
        /// <param name="entity"></param>
        protected sealed override void OnDataSaved(OrganizationData entity)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbComboCache))
            {
                proxy.RemoveKey(comboKey);
            }
        }
    }
}