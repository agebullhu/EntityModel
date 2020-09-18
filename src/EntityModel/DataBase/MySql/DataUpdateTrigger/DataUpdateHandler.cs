// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     ���ݸ��´�����
    /// </summary>
    public static class DataUpdateHandler
    {
        /// <summary>
        ///     ����ע�봦����
        /// </summary>
        private static Dictionary<int, List<IDataUpdateTrigger>> Triggers { get; } =
            new Dictionary<int, List<IDataUpdateTrigger>>();

        /// <summary>
        ///     ע�����ݸ���ע����
        /// </summary>
        public static void RegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            if (Triggers.ContainsKey(entityId))
                Triggers[entityId].Add(handler);
            else
                Triggers.Add(entityId, new List<IDataUpdateTrigger> {handler});
        }

        /// <summary>
        ///     ��ע�����ݸ���ע����
        /// </summary>
        public static void UnRegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            if (Triggers.ContainsKey(entityId))
                Triggers[entityId].Remove(handler);
        }


        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        internal static void OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        {
            if (Triggers.ContainsKey(entity.__Struct.EntityType))
                foreach (var trigger in Triggers[entity.__Struct.EntityType])
                    trigger.OnPrepareSave(entity, operatorType);
            if (Triggers.ContainsKey(0))
                foreach (var trigger in Triggers[0])
                    trigger.OnPrepareSave(entity, operatorType);
        }

        /// <summary>
        ///     ������ɺ��ڴ���
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        internal static void OnDataSaved(EditDataObject entity, DataOperatorType operatorType)
        {
            if (Triggers.ContainsKey(entity.__Struct.EntityType))
                foreach (var trigger in Triggers[entity.__Struct.EntityType])
                    trigger.OnDataSaved(entity, operatorType);
            if (Triggers.ContainsKey(0))
                foreach (var trigger in Triggers[0])
                    trigger.OnDataSaved(entity, operatorType);
        }

        /// <summary>
        ///     �������ǰ����(����ʵ�����������)
        /// </summary>
        /// <param name="entityId">ʵ������ID</param>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        internal static void OnOperatorExecuting(int entityId, string condition, IEnumerable<MySqlParameter> args,
            DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            if (Triggers.ContainsKey(entityId))
                foreach (var trigger in Triggers[entityId])
                    trigger.OnOperatorExecuting(entityId, condition, mySqlParameters, operatorType);
            if (Triggers.ContainsKey(0))
                foreach (var trigger in Triggers[0])
                    trigger.OnOperatorExecuting(entityId, condition, mySqlParameters, operatorType);
        }

        /// <summary>
        ///     ����������(����ʵ�����������)
        /// </summary>
        /// <param name="entityId">ʵ������ID</param>
        /// <param name="condition">ִ������</param>
        /// <param name="args">����ֵ</param>
        /// <param name="operatorType">��������</param>
        internal static void OnOperatorExecutd(int entityId, string condition, IEnumerable<MySqlParameter> args,
            DataOperatorType operatorType)
        {
            var mySqlParameters = args as MySqlParameter[] ?? args.ToArray();
            if (Triggers.ContainsKey(entityId))
                foreach (var trigger in Triggers[entityId])
                    trigger.OnOperatorExecutd(entityId, condition, mySqlParameters, operatorType);
            if (Triggers.ContainsKey(0))
                foreach (var trigger in Triggers[0])
                    trigger.OnOperatorExecutd(entityId, condition, mySqlParameters, operatorType);
        }
    }
}