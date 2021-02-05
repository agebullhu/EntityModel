// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

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
        public IDataAccessProvider<TEntity> Provider { get; set; }
        /*
        class DataField
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int ColumnOrdinal { get; set; }

            /// <summary>
            /// 字段名称
            /// </summary>
            public string PropertyName { get; set; }

            public Func<object> ReadFunc { get; set; }

        }
        List<DataField> fields;

        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <param name="entity">读取数据的实体</param>
        public Task LoadEntity(DbDataReader reader, TEntity entity)
        {
            if (fields == null)
            {
                fields = new List<DataField>();
                foreach (var col in reader.GetColumnSchema())
                {
                    fields.Add(new DataField
                    {
                        ColumnOrdinal = col.ColumnOrdinal.Value,
                        PropertyName = col.ColumnName
                    });
                }
            }

            foreach (var field in fields)
            {
                var val = reader.GetValue(field.ColumnOrdinal);
                if (val == null || val == DBNull.Value)
                {
                    Provider.EntityOperator.SetValue(entity, field.PropertyName, null);
                }
                else
                {
                    Provider.EntityOperator.SetValue(entity, field.PropertyName, val);
                }
            };
            return Task.CompletedTask;
        }*/
    }
}