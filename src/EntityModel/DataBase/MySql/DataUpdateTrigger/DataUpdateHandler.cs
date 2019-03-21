// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     数据更新处理器
    /// </summary>
    public static class DataUpdateHandler
    {
        /// <summary>
        ///     更新注入处理器
        /// </summary>
        private static Dictionary<int, List<IDataUpdateTrigger>> Triggers { get; } =
            new Dictionary<int, List<IDataUpdateTrigger>>();

        /// <summary>
        ///     注册数据更新注入器
        /// </summary>
        public static void RegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            if (Triggers.ContainsKey(entityId))
                Triggers[entityId].Add(handler);
            else
                Triggers.Add(entityId, new List<IDataUpdateTrigger> {handler});
        }

        /// <summary>
        ///     反注册数据更新注入器
        /// </summary>
        public static void UnRegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            if (Triggers.ContainsKey(entityId))
                Triggers[entityId].Remove(handler);
        }


        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
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
        ///     保存完成后期处理
        /// </summary>
        /// <param name="entity">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
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
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
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
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
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