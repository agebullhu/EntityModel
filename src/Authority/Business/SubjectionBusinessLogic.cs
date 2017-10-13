
using System.Collections.Generic;
using System.Linq;
using Agebull.Common.DataModel;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    /// 隶属关系表,包括职位与其它职位的隶属关系、机构之间的隶属关系、机构与职位的隶属关系
    /// </summary>
    internal sealed partial class SubjectionBusinessLogic : BusinessLogicBase<SubjectionData, SubjectionDataAccess>
    {
        #region 机构隶属生成

        /// <summary>
        /// 生成机构隶属表
        /// </summary>
        internal void CreateSubjection()
        {
            var oAccess = new OrganizationDataAccess();
            var orgs = oAccess.All(p => p.DataState < DataStateType.Delete);
            Access.DataBase.Clear(Access.TableName);
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                var keys = proxy.Client.SearchKeys("org:sub:*");
                proxy.Client.RemoveAll(keys);
                CreateSubjection(proxy, orgs, new List<int>(), 0);
            }
            //var access = new OrganizePositionDataAccess();
            //var list = access.All(p => p.DataState < DataStateType.Delete);
            //CreateSubjection(orgs, list);
        }

        /// <summary>
        /// 取机构隶属表
        /// </summary>
        /// <param name="oid"></param>
        public List<SubjectionData> GetOrganizationSubjection(int oid)
        {
            return Access.All(p => p.MasterId == oid);
        }


        /// <summary>
        /// 机构的职位隶属生成
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="datas"></param>
        /// <param name="seniors"></param>
        /// <param name="oid"></param>
        void CreateSubjection(RedisProxy proxy, List<OrganizationData> datas, List<int> seniors, int oid)
        {
            var children = datas.Where(p => p.ParentId == oid);
            if (oid > 0)
            {
                Access.Insert(new SubjectionData
                {
                    MasterId = oid,
                    SlaveId = oid
                });
                foreach (var senior in seniors)
                {
                    Access.Insert(new SubjectionData
                    {
                        MasterId = senior,
                        SlaveId = oid
                    });
                    proxy.Client.SAdd("org:sub:" + senior, oid.ToByte());
                }
            }
            var pars = new List<int> { oid };
            pars.AddRange(seniors);
            foreach (var ch in children)
            {
                CreateSubjection(proxy, datas, pars, ch.Id);
            }
        }
        #endregion



        #region 机构的职位隶属生成
        /*
        /// <summary>
        /// 机构的职位隶属生成
        /// </summary>
        /// <param name="orgs"></param>
        /// <param name="positions"></param>
        void CreateSubjection(List<OrganizationData> orgs, List<OrganizePositionData> positions)
        {
            Access.Delete(p => p.SubjectionType == SubjectionType.OrganizationPosition);
            CreateSubjection(orgs, positions, new List<int>(), 0);
        }

        /// <summary>
        /// 机构的职位隶属生成
        /// </summary>
        /// <param name="orgs"></param>
        /// <param name="seniors"></param>
        /// <param name="parent"></param>
        /// <param name="positions"></param>
        void CreateSubjection(List<OrganizationData> orgs, List<OrganizePositionData> positions, List<int> seniors, int parent)
        {
            var children = orgs.Where(p => p.ParentId == parent);
            var pars = new List<int>();
            pars.AddRange(seniors);
            pars.Add(parent);
            var posts = positions.Where(p => p.OrganizationId == parent);
            foreach (var post in posts)
            {
                foreach (var senior in pars)
                {
                    Access.Insert(new SubjectionData
                    {
                        MasterId = senior,
                        SlaveId = post.Id,
                        SubjectionType = SubjectionType.OrganizationPosition,
                        SubjectionSreen = 0
                    });
                }
            } 
            foreach (var ch in children)
            {
                CreateSubjection(orgs, positions, pars, ch.Id);
            }
        }*/
        #endregion
    }
}
