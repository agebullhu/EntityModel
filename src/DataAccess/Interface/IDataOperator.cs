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
    public interface IDataOperator<TEntity> : IOperatorExtension<TEntity>
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
            foreach (var pro in Provider.Option.ReadPproperties)
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

            foreach (var pro in Provider.Option.ReadPproperties)
            {
                cmd.Parameters.Add(Provider.ParameterCreater.CreateParameter(pro.PropertyName,
                            Provider.EntityOperator.GetValue(entity, pro.PropertyName),
                            pro.DbType));
            }
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        void CreateEntityParameter(DbCommand cmd)
        {
            Provider.Option.FroeachDbProperties(ReadWriteFeatrue.Read,
                pro => cmd.Parameters.Add(Provider.ParameterCreater.CreateParameter(pro.PropertyName, pro.DbType))
            );
        }

        /// <summary>
        /// 设置插入数据的命令
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <param name="cmd">命令</param>
        /// <returns>返回真说明要取主键</returns>
        void SetParameterValue(TEntity data, DbCommand cmd)
        {
            Provider.Option.FroeachDbProperties(pro =>
            {
                cmd.Parameters[pro.PropertyName].Value = Provider.EntityOperator.GetValue(data, pro.PropertyName) ?? DBNull.Value;
            });
        }

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
        /// <param name="field"> 字段的名字 </param>
        /// <returns> 字段的值 </returns>
        object GetValue(TEntity entity, string field);

        /// <summary>
        ///     配置字段的值
        /// </summary>
        /// <param name="field"> 字段的名字 </param>
        /// <param name="value"> 字段的值 </param>
        void SetValue(TEntity entity, string field, object value);
    }
}