/*****************************************************
(c)2016-2021 by ZeroTeam
作者: 胡天水
工程: Agebull.EntityModel.CoreAgebull.DataModel
建立: 忘了日期
修改: -
*****************************************************/

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
    public interface IDataOperator<TEntity> : IDataAccessTool<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        int GetDbType(string field)
        {
            return Provider.Option.PropertyMap.TryGetValue(field, out var pro)
                ? pro.DbType
                : 0;
        }

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        Task LoadEntity(DbDataReader reader, TEntity entity)
        {
            foreach (var pro in Provider.Option.ReadProperties)
            {
                var val = reader.GetValue(pro.FieldName);
                if (val == null || val == DBNull.Value)
                {
                    Provider.EntityOperator.SetValue(entity, pro.PropertyName, null);
                }
                else
                {
                    Provider.EntityOperator.SetValue(entity, pro.PropertyName, val);
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
        void SetEntityParameter(DbCommand cmd, TEntity entity)
        {
            cmd.Parameters.Clear();

            foreach (var pro in Provider.Option.ReadProperties)
            {
                cmd.Parameters.Add(Provider.ParameterCreater.CreateParameter(pro.PropertyName,
                            Provider.EntityOperator.GetValue(entity, pro.PropertyName),
                            pro.DbType));
            }
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        void CreateEntityParameter(DbCommand cmd)
        {
            var properties = Provider.Option.Properties;

            foreach (var pro in properties)
            {
                if (pro.PropertyFeatrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.Field) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read))
                    cmd.Parameters.Add(Provider.ParameterCreater.CreateParameter(pro.PropertyName, pro.DbType));
            }
        }
        /// <summary>
        ///     实体保存完成后期处理(Insert/Update/Delete)
        /// </summary>
        /// <param name="entityAccess">当前数据访问对象</param>
        /// <param name="entity">实体</param>
        /// <param name="operatorType">操作类型</param>
        /// <remarks>
        ///     对当前对象的属性的更改,请自行保存,否则将丢失
        /// </remarks>
        Task AfterSave(DataAccess<TEntity> entityAccess, TEntity entity, DataOperatorType operatorType) => Task.CompletedTask;

    }


    /// <summary>
    ///     表明是一个实体操作对象
    /// </summary>
    public interface IEntityOperator<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        ///     得到字段的值
        /// </summary>
        /// <param name="entity"> 实体 </param>
        /// <param name="field"> 字段的名字 </param>
        /// <returns> 字段的值 </returns>
        object GetValue(TEntity entity, string field);

        /// <summary>
        ///     配置字段的值
        /// </summary>
        /// <param name="entity"> 实体 </param>
        /// <param name="field"> 字段的名字 </param>
        /// <param name="value"> 字段的值 </param>
        void SetValue(TEntity entity, string field, object value);
    }
}