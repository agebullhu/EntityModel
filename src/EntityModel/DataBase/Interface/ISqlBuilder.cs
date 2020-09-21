// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 表示Sql生成器
    /// </summary>
    public interface ISqlBuilder<TEntity> : IParameterCreater
        where TEntity : EditDataObject
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType { get; }

        /// <summary>
        /// 不做代码注入
        /// </summary>
        bool NoInjection { get; set; }

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        string PrimaryKeyConditionSQL { get; }

        /// <summary>
        /// Sql配置信息
        /// </summary>
        DataAccessSqlOption<TEntity> SqlOption { get; set; }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        int GetDbType(string field);

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="map">字段映射表</param>
        /// <param name="lambda">条件</param>
        ConditionItem Compile(Dictionary<string, string> map, Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="map">字段映射表</param>
        /// <param name="lambda">查询表达式</param>
        ConditionItem Compile(Dictionary<string, string> map, LambdaItem<TEntity> lambda);

        /// <summary>
        /// 条件SQL
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        string Condition(string fieldName, string paraName);

        /// <summary>
        /// 条件SQL
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="paraName"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string Condition(string fieldName, string paraName, string condition);

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        string GetModifiedUpdateSql(EditDataObject data);

        /// <summary>
        /// 取得仅更新的SQL语句
        /// </summary>
        public string GetFullUpdateSql(EditDataObject data);

        /// <summary>
        /// 注入查询与更新条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string InjectionCondition(string condition);

        /// <summary>
        /// 构建Update后期执行的SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string AfterUpdateSql(string condition);

        /// <summary>
        /// 构建Update前期执行的SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string BeforeUpdateSql(string condition);

        /// <summary>
        /// 汇总SQL
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="field"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateCollectSql(string fun, string field, string condition);

        /// <summary>
        ///  删除SQL
        /// </summary>
        /// <param name="convert"></param>
        /// <returns></returns>
        string CreateDeleteSql(ConditionItem convert);

        /// <summary>
        /// 删除SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateDeleteSql(string condition);

        /// <summary>
        /// 读取SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        StringBuilder CreateLoadSql(string condition, string order);

        /// <summary>
        /// 单值读取SQL
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateLoadValueSql(string field, string condition);

        /// <summary>
        /// 单值读取SQL
        /// </summary>
        /// <param name="field"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        string CreateLoadValuesSql(string field, ConditionItem convert);

        /// <summary>
        /// 分页SQL
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="desc"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreatePageSql(int page, int pageSize, string order, bool desc, string condition);

        /// <summary>
        /// 更新SQL
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateUpdateSql(string valueExpression, string condition);

        /// <summary>
        /// 字段条件
        /// </summary>
        /// <param name="isAnd"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        string FieldConditionSQL(bool isAnd, params (string field, object value)[] fields);

        /// <summary>
        /// 字段条件
        /// </summary>
        /// <param name="isAnd"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        string FieldConditionSQL(bool isAnd, params string[] fields);

        /// <summary>
        /// 字段条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        string FieldConditionSQL(string field, string expression = "=");

        /// <summary>
        /// 字段更新
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string FileUpdateSql(string field, object value, IList<DbParameter> parameters);


        /// <summary>
        /// 条件组合
        /// </summary>
        /// <param name="isAnd"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        string JoinConditionSQL(bool isAnd, params string[] conditions);

        /// <summary>
        /// 构建排序SQL片断
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        string OrderSql(bool desc, string field);


        /// <summary>
        ///     物理删除数据
        /// </summary>
        string PhysicalDeleteSql(string condition);
    }
}