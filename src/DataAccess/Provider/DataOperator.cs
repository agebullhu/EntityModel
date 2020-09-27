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
    public class DataOperator<TEntity> : IDataOperator<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

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


        /*// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        public string GetModifiedUpdateSql(TEntity entity)
        {
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
        }*/

    }
}