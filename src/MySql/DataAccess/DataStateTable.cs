// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Agebull.Common.Context;
using Agebull.EntityModel.Common;
using MySql.Data.MySqlClient;

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    /// <typeparam name="TMySqlDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public abstract class DataStateTable<TData, TMySqlDataBase> : MySqlTable<TData, TMySqlDataBase>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE `{ContextWriteTable}` SET `{FieldDictionary["DataState"]}`=255";

        /// <summary>
        ///     ����״̬��SQL���
        /// </summary>
        protected virtual string ResetStateFileSqlCode => $@"`{FieldDictionary["DataState"]}`=0,`{FieldDictionary["IsFreeze"]}`=0";

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ContitionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.IsManageMode)
                return;
            conditions.Add($"`{FieldDictionary["DataState"]}` < 255");
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(long id)
        {
            //using (DataBaseScope.CreateScope(DataBase))
            {
                var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode} 
WHERE {PrimaryKeyConditionSQL}";
                return DataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
            }
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(Expression<Func<TData, bool>> lambda)
        {
            //using (DataBaseScope.CreateScope(DataBase))
            {
                var convert = Compile(lambda);
                var sql = $@"UPDATE `{ContextWriteTable}` 
SET {ResetStateFileSqlCode} 
WHERE {convert.ConditionSql}";
                return DataBase.Execute(sql, convert.Parameters.Cast<MySqlParameter>()) >= 1;
            }
        }
    }
}