// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using Agebull.EntityModel.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption<TEntity> : DataAccessOption
        where TEntity : class, new()
    {
        #region 数据库

        /// <summary>
        /// 自动构建数据库对象
        /// </summary>
        public IDataOperator<TEntity> DataOperator => Provider.DataOperator;

        /// <summary>
        /// 参数构造
        /// </summary>
        public IParameterCreater ParameterCreater => Provider.ParameterCreater;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public DataAccessOption<TEntity> Option => Provider.Option;

        /// <summary>
        /// Sql语句构造器
        /// </summary>
        public ISqlBuilder<TEntity> SqlBuilder => Provider.SqlBuilder;

        /// <summary>
        /// 驱动提供者信息
        /// </summary>
        public DataAccessProvider<TEntity> Provider { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public void Initiate()
        {
            FieldMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            PropertyMap = new Dictionary<string, EntitiyProperty>(StringComparer.OrdinalIgnoreCase);
            var properties = Properties;
            foreach (var pro in properties)
            {
                if (!pro.Featrue.HasFlag(PropertyFeatrue.DbCloumn))
                    continue;
                PropertyMap[pro.ColumnName] = pro;
                PropertyMap[pro.PropertyName] = pro;

                FieldMap[pro.ColumnName] = pro.ColumnName;
                FieldMap[pro.PropertyName] = pro.ColumnName;
            }

            LoadFields ??= SqlBuilder.BuilderLoadFields();
            InsertSqlCode ??= SqlBuilder.BuilderInsertSqlCode();
            DeleteSqlCode ??= SqlBuilder.BuilderDeleteSqlCode();
            UpdateFields ??= SqlBuilder.BuilderUpdateFields();
            Option.UpdateSqlCode ??= SqlBuilder.CreateUpdateSql(UpdateFields, SqlBuilder.PrimaryKeyConditionSQL);

            ReadPproperties ??= Properties.Where(pro => pro.Featrue.HasFlag(PropertyFeatrue.Property | PropertyFeatrue.DbCloumn) && pro.DbReadWrite.HasFlag(ReadWriteFeatrue.Read)).ToArray();

        }

        #endregion



    }
    /// <summary>
    /// 数据载入配置
    /// </summary>
    public class DataAccessOption<TEntity, TSqlBuilder, TDataBase> : DataAccessOption<TEntity>
        where TEntity : class, new()
        where TDataBase : IDataBase
        where TSqlBuilder : ISqlBuilder<TEntity>
    {
    }
}