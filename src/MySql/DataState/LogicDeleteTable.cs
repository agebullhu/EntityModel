// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

#endregion

using Agebull.EntityModel.Common;
using System.Collections.Generic;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// 数据状态基类
    /// </summary>
    /// <typeparam name="TData">实体</typeparam>
    /// <typeparam name="TMySqlDataBase">所在的数据库对象,可通过Ioc自动构造</typeparam>
    public abstract class LogicDeleteTable<TData, TMySqlDataBase> : MySqlTable<TData, TMySqlDataBase>
        where TData : EditDataObject,ILogicDeleteData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        ///     删除的SQL语句
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE `{ContextWriteTable}` SET `{FieldDictionary["IsDeleted"]}`=1";

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ConditionSqlCode(List<string> conditions)
        {
            conditions.Add($"`{FieldDictionary["IsDeleted"]}` = 0");
        }

        /// <summary>
        ///     得到可正确更新的条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override void CheckUpdateContition(ref string condition)
        {
            if (condition == null)
                condition = $"`{FieldDictionary["IsDeleted"]}` = 0";
            else
                condition = $"`{FieldDictionary["IsDeleted"]}` = 0 AND ({condition})";
        }

    }
}