// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Events
{

    /// <summary>
    ///     数据更新处理器
    /// </summary>
    public class DataUpdateHandler : IDataUpdateHandler
    {
        #region 数据事件

        /// <summary>
        /// 状态修改事件
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="entity">实体</param>
        /// <param name="oType">操作</param>
        /// <param name="valueType">值类型</param>
        /// <param name="val">内容</param>
        /// <remarks>
        /// 如果内容为实体,使用JSON序列化,
        /// 如果使用主键内容为#:[key](如:#:123)样式,
        /// 如果为批量操作,内容为QueryCondition的JSON序列化
        /// </remarks>
        public async Task OnStatusChanged(string database, string entity,
            DataOperatorType oType, EntityEventValueType valueType, object val)
        {
            var services = DependencyHelper.GetServices<IEntityEventProxy>();
            if (services == null)
                return;
            string value;
            switch (valueType)
            {
                case EntityEventValueType.EntityJson:
                    value = JsonConvert.SerializeObject(val);
                    break;
                case EntityEventValueType.Key:
                case EntityEventValueType.Keys:
                    value = val.ToString();
                    break;
                case EntityEventValueType.QueryCondition:
                    {
                        var arg = val as Tuple<string, DbParameter[]>;
                        var parameter = arg.Item2;
                        var queryCondition = new MulitCondition
                        {
                            Condition = arg.Item1,
                            Parameters = new ConditionParameter[parameter.Length]
                        };
                        for (int i = 0; i < parameter.Length; i++)
                        {
                            queryCondition.Parameters[i] = new ConditionParameter
                            {
                                Name = parameter[i].ParameterName,
                                Value = parameter[i].Value == DBNull.Value ? null : parameter[i].Value.ToString(),
                                Type = parameter[i].DbType
                            };
                        }
                        value = JsonConvert.SerializeObject(queryCondition);
                    }
                    break;
                default:
                    value = null;
                    break;
            }
            foreach (var service in services)
                await service.OnStatusChanged(database, entity, oType, valueType, value);
        }
        #endregion

        #region SQL注入

        /// <summary>
        ///     SQL注入
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        public void InjectionCondition<TEntity>(DataAccessOption<TEntity> option, List<string> conditions)
            where TEntity : class, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.InjectionQueryCondition(option, conditions);
        }

        #endregion

        #region 扩展流程

        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void OnPrepareSave<TEntity>(TEntity data, DataOperatorType operatorType)
             where TEntity : class, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnPrepareSave(data, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public void OnDataSaved<TEntity>(TEntity data, DataOperatorType operatorType)
             where TEntity : class, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnDataSaved(data, operatorType);
        }


        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuting<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
                      where TEntity : class, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnOperatorExecuting(option, condition, args, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="option">当前数据操作配置</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public void OnOperatorExecuted<TEntity>(DataAccessOption<TEntity> option, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
              where TEntity : class, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnOperatorExecuted(option, condition, args, operatorType);
        }

        #endregion


        /*// <summary>
        ///     载入条件数据
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>如果有载入首行,否则返回空</returns>
        public Task<List<TEntity>> LoadDataAsync(MulitCondition condition)
        {
            if (condition == null || string.IsNullOrEmpty(condition.Condition))
                return Task.FromResult(new List<TEntity>());
            if (condition.Parameters == null)
                return LoadDataInnerAsync(condition.Condition, null, null);

            List<DbParameter> args = new List<DbParameter>();
            foreach (var item in condition.Parameters)
            {
                var pa = ParameterCreater.CreateParameter(item.Name, item.Type);
                if (item.Value == null)
                    pa.Value = DBNull.Value;
                else
                    switch (item.Type)
                    {
                        default:
                            //case DbType.Xml:
                            //case DbType.String:
                            //case DbType.StringFixedLength:
                            //case DbType.AnsiStringFixedLength:
                            //case DbType.AnsiString:
                            pa.Size = item.Value.Length * 2;
                            pa.Value = item.Value;
                            break;
                        case DbType.Boolean:
                            {
                                pa.Value = bool.TryParse(item.Value, out var vl) && vl;
                            }
                            break;
                        case DbType.Byte:
                            {
                                if (byte.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = (byte)0;
                            }
                            break;
                        case DbType.VarNumeric:
                        case DbType.Decimal:
                        case DbType.Currency:
                            {
                                if (decimal.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = (decimal)0;
                            }
                            break;
                        case DbType.Time:
                        case DbType.DateTime2:
                        case DbType.DateTime:
                        case DbType.Date:
                            {
                                if (DateTime.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.DateTimeOffset:
                            {
                                if (TimeSpan.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Double:
                            {
                                if (double.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Guid:
                            {
                                if (Guid.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int16:
                            {
                                if (short.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int32:
                            {
                                if (int.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.Int64:
                            {
                                if (long.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.SByte:
                            break;
                        case DbType.Single:
                            {
                                if (float.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt16:
                            {
                                if (ushort.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt32:
                            {
                                if (uint.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                        case DbType.UInt64:
                            {
                                if (ulong.TryParse(item.Value, out var vl))
                                    pa.Value = vl;
                                else pa.Value = DBNull.Value;
                            }
                            break;
                    }
                args.Add(pa);
            }
            return LoadDataInnerAsync(condition.Condition, null, args.ToArray());
        }
        */

    }
}