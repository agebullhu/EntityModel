using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.EasyUI
{
    /// <summary>
    /// ×´Ì¬Í¼±êÊ¶¸¨Öú
    /// </summary>
    public static class IconHelper
    {
        /// <summary>
        /// Êý¾Ý×´Ì¬Í¼±ê
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string DataStateIcon<T>(T data, string def = null)
            where T : IAuditData, IStateData
        {
            if (data.IsFreeze)
            {
                return "icon-lock";
            }
            switch (data.DataState)
            {
                case DataStateType.None:
                    return def ?? "icon-edit";
                case DataStateType.Enable:
                    return def ?? "icon-enable";
                case DataStateType.Disable:
                    return "icon-disable";
                case DataStateType.Discard:
                    return "icon-discard";
                case DataStateType.Delete:
                    return "icon-delete";
                default:
                    return "icon-self";
            }
        }
        /// <summary>
        /// ÉóºË×´Ì¬Í¼±ê
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AuditIcon<T>(T data)
            where T : IAuditData, IStateData
        {
            switch (data.AuditState)
            {
                case AuditStateType.None:
                    return DataStateIcon(data);
                case AuditStateType.Again:
                    return DataStateIcon(data, "icon_a_again");
                case AuditStateType.Submit:
                    return "icon_a_submit";
                case AuditStateType.Deny:
                    return "icon_a_deny";
                case AuditStateType.Pass:
                    return DataStateIcon(data, "icon_a_pass");
                case AuditStateType.End:
                    return "icon-lock";
                default:
                    return "icon-self";
            }
        }
    }
}