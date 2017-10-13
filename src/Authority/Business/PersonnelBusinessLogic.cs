using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Agebull.SystemAuthority.Organizations.DataAccess;

namespace Agebull.SystemAuthority.Organizations.BusinessLogic
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public sealed partial class PersonnelBusinessLogic : BusinessLogicByAudit<PersonnelData, PersonnelDataAccess>
    {
        public PersonnelBusinessLogic()
        {
            unityStateChanged = true;
        }
        /// <summary>
        ///     状态改变后的统一处理(unityStateChanged不设置为true时不会产生作用--基于性能的考虑)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override void DoStateChanged(PersonnelData data)
        {
            base.DoStateChanged(data);
        }

        /// <summary>
        ///     保存前的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool OnSaving(PersonnelData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     被用户编辑的数据的保存前操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后续操作</returns>
        protected override bool PrepareSaveByUser(PersonnelData data, bool isAdd)
        {
            if (this.Access.Any(p => p.Id != data.Id && p.Mobile == data.Mobile))
            {
                BusinessContext.Current.LastMessage = "手机号码用于登录系统必须唯一";
                return false;
            }
            return base.PrepareSaveByUser(data, isAdd);
        }

        /// <summary>
        ///     保存完成后的操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="isAdd">是否为新增</param>
        /// <returns>如果为否将阻止后=续操作</returns>
        protected override bool OnSaved(PersonnelData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     扩展数据校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool ValidateExtend(PersonnelData data)
        {
            if (this.Access.Any(p => p.Id != data.Id && p.Mobile == data.Mobile))
            {
                BusinessContext.Current.LastMessage = "手机号码用于登录系统必须唯一";
                return false;
            }
            return base.ValidateExtend(data);
        }

        ///// <summary>
        /////     能否通过审核的判断
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //protected override bool CanAudit(PersonnelData data)
        //{
        //    if (this.Access.Any(p => p.ID != data.ID && p.Mobile == data.Mobile))
        //    {
        //        BusinessContext.Current.LastMessage = "手机号码用于登录系统必须唯一";
        //        return false;
        //    }
        //    return base.CanAudit(data);
        //}
    }
}
