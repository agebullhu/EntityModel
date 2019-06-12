// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Agebull.Common;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     数据更新处理器
    /// </summary>
    public static class DataUpdateHandler
    {
        /// <summary>
        /// 事件代理
        /// </summary>
        public static IEntityEventProxy EventProxy
        {
            get;
        }
        static DataUpdateHandler()
        {
            EventProxy = IocHelper.Create<IEntityEventProxy>();
        }

        /// <summary>
        /// 通用处理器
        /// </summary>
        private static readonly List<IDataTrigger> _generalTriggers = new List<IDataTrigger> { new DefaultDataUpdateTrigger() };

        /// <summary>
        ///     更新注入处理器
        /// </summary>
        private static Dictionary<int, List<IDataUpdateTrigger>> Triggers { get; set; }


        /// <summary>
        ///     注册数据更新注入器
        /// </summary>
        public static void RegisterUpdateHandler(IDataTrigger handler)
        {
            lock (_generalTriggers)
            {
                if (_generalTriggers.All(p => p.GetType() != handler.GetType()))
                    _generalTriggers.Add(handler);
            }
        }

        /// <summary>
        ///     反注册数据更新注入器
        /// </summary>
        public static void UnRegisterUpdateHandler(IDataTrigger handler)
        {
            lock (_generalTriggers)
            {
                if (_generalTriggers != null && _generalTriggers.Contains(handler))
                {
                    _generalTriggers.Remove(handler);
                }
            }
        }

        /// <summary>
        ///     注册数据更新注入器
        /// </summary>
        public static void RegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            lock (Triggers)
            {
                if (Triggers == null)
                    Triggers = new Dictionary<int, List<IDataUpdateTrigger>>();
                if (Triggers.ContainsKey(entityId))
                {
                    if (Triggers[entityId].All(p => p.GetType() != handler.GetType()))
                        Triggers[entityId].Add(handler);
                }
                else
                    Triggers.Add(entityId, new List<IDataUpdateTrigger> { handler });
            }
        }

        /// <summary>
        ///     反注册数据更新注入器
        /// </summary>
        public static void UnRegisterUpdateHandler(int entityId, IDataUpdateTrigger handler)
        {
            lock (Triggers)
            {
                if (Triggers == null || !Triggers.ContainsKey(entityId)) return;
                Triggers[entityId].Remove(handler);
                if (Triggers[entityId].Count == 0)
                    Triggers.Remove(entityId);
                if (Triggers.Count == 0)
                    Triggers = null;
            }
        }


        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnPrepareSave(EditDataObject data, DataOperatorType operatorType)
        {
            if (operatorType == DataOperatorType.Insert && data is ISnowFlakeId flakeId && flakeId.Id <= 0)
                flakeId.Id = SnowFlake.NewId;
            foreach (var trigger in _generalTriggers)
                trigger.OnPrepareSave(data, operatorType);
            if (Triggers != null && Triggers.TryGetValue(data.__Struct.EntityType, out var triggers))
                foreach (var trigger in triggers)
                    trigger.OnPrepareSave(data, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnDataSaved(EditDataObject data, DataOperatorType operatorType)
        {
            foreach (var trigger in _generalTriggers)
                trigger.OnDataSaved(data, operatorType);
            if (Triggers != null && Triggers.TryGetValue(data.__Struct.EntityType, out var triggers))
                foreach (var trigger in triggers)
                    trigger.OnDataSaved(data, operatorType);
        }

        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnOperatorExecuting(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as DbParameter[] ?? args.ToArray();
            foreach (var trigger in _generalTriggers)
                trigger.OnOperatorExecuting(entityId, condition, mySqlParameters, operatorType);
            if (Triggers != null && Triggers.TryGetValue(entityId, out var triggers))
                foreach (var trigger in triggers)
                    trigger.OnOperatorExecuting(entityId, condition, mySqlParameters, operatorType);

        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnOperatorExecuted(int entityId, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
        {
            var mySqlParameters = args as DbParameter[] ?? args.ToArray();
            foreach (var trigger in _generalTriggers)
                trigger.OnOperatorExecuted(entityId, condition, mySqlParameters, operatorType);
            if (Triggers != null && Triggers.TryGetValue(entityId, out var triggers))
                foreach (var trigger in triggers)
                    trigger.OnOperatorExecuted(entityId, condition, mySqlParameters, operatorType);
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        public static void ContitionSqlCode<T>(int entityId, List<string> conditions) where T : EditDataObject, new()
        {
            foreach (var trigger in _generalTriggers)
                trigger.ContitionSqlCode<T>(conditions);
            if (Triggers != null && Triggers.TryGetValue(entityId, out var triggers))
                foreach (var trigger in triggers)
                    trigger.ContitionSqlCode<T>(conditions);
        }

        /// <summary>
        ///     初始化类型
        /// </summary>
        /// <returns></returns>
        public static void InitType<T>() where T : EditDataObject, new()
        {
            foreach (var trigger in _generalTriggers)
                trigger.InitType<T>();
        }


        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="entityId">实体类型ID</param>
        /// <returns></returns>
        public static void BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, StringBuilder code, int entityId, string condition)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in _generalTriggers.Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
            {
                trigger.BeforeUpdateSql(table, condition, code);
            }

            if (Triggers == null || !Triggers.TryGetValue(entityId, out var triggers))
                return;
            foreach (var trigger in triggers.Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
                trigger.BeforeUpdateSql(table, condition, code);
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="entityId">实体类型ID</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        public static void AfterUpdateSql<TEntity>(IDataTable<TEntity> table, StringBuilder code, int entityId, string condition)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in _generalTriggers.Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
            {
                trigger.AfterUpdateSql(table, condition, code);
            }

            if (Triggers == null || !Triggers.TryGetValue(entityId, out var triggers))
                return;
            foreach (var trigger in triggers.Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
                trigger.AfterUpdateSql(table, condition, code);
        }
    }
}