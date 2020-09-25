// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用


#endregion

using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表明是一个数据操作对象
    /// </summary>
    public class DataOperator<TEntity>: IDataOperator<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public DataAccessOption<TEntity> Option => Provider.Option;

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => Provider.ParameterCreater;

        /// <summary>
        ///     得到字段的值
        /// </summary>
        /// <param name="field"> 字段的名字 </param>
        /// <returns> 字段的值 </returns>
        public virtual object GetValue(TEntity entity, string field) => null;

        /// <summary>
        ///     配置字段的值
        /// </summary>
        /// <param name="field"> 字段的名字 </param>
        /// <param name="value"> 字段的值 </param>
        public virtual void SetValue(TEntity entity, string field, object value) { }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        public virtual int GetDbType(string field)
        {
            return Option.PropertyMap.TryGetValue(field, out var pro)
                ? pro.DbType
                : 0;
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public virtual Task LoadEntity(DbDataReader reader, TEntity entity)
        {
            foreach (var pro in Option.ReadPproperties)
            {
                var val = reader.GetValue(pro.ColumnName);
                if (val == null || val == DBNull.Value)
                {
                    SetValue(entity, pro.PropertyName, null);
                }
                else
                {
                    SetValue(entity, pro.PropertyName, val);
                }
            };
            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public virtual void SetEntityParameter(DbCommand cmd, TEntity entity)
        {
            cmd.Parameters.Clear();

            foreach (var pro in Option.ReadPproperties)
            {
                cmd.Parameters.Add(ParameterCreater.CreateParameter(pro.PropertyName,
                            GetValue(entity, pro.PropertyName),
                            pro.DbType));
            }
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void CreateEntityParameter(DbCommand cmd)
        {
            Option.FroeachDbProperties(ReadWriteFeatrue.Read,
                pro => cmd.Parameters.Add(ParameterCreater.CreateParameter(pro.PropertyName, pro.DbType))
            );
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        public void SetParameterValue(TEntity data, DbCommand cmd)
        {
            Option.FroeachDbProperties(pro =>
            {
                cmd.Parameters[pro.PropertyName].Value = GetValue(data, pro.PropertyName) ?? DBNull.Value;
            });
        }

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        public string GetModifiedUpdateSql(TEntity entity)
        {
            return Option.UpdateSqlCode;
            //if (!(entity is EditDataObject data))
            //{
            //    return Option.UpdateFields;
            //}
            //if (data.__status.IsReadOnly)
            //{
            //    return Option.UpdateFields;
            //}
            //if (!data.__status.IsModified)
            //    return null;
            //StringBuilder sql = new StringBuilder();
            //bool first = true;
            //foreach (var pro in Option.Properties.Where(p => p.Featrue.HasFlag(PropertyFeatrue.Property)))
            //{
            //    if (data.__status.Status.ModifiedProperties[pro.PropertyIndex] <= 0 || !Option.FieldMap.ContainsKey(pro.Name))
            //        continue;
            //    if (first)
            //        first = false;
            //    else
            //        sql.Append(',');
            //    sql.AppendLine($"       `{pro.ColumnName}` = ?{pro.Name}");
            //}
            //return first ? null : sql.ToString();
        }

    }
}