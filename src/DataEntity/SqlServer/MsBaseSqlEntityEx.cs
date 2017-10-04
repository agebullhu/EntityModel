using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MS.Lib.BusinessEntity;
using MS.Lib.Data;

namespace MS.Lib
{
    //=========================================================================
    //  ClassName : DbbankaccountBusinessEntity
    /// <summary>
    /// The business entity of DB_BankAccount
    /// </summary>
    //=========================================================================
    public class MsBaseSqlEntityEx<TData> : MSBaseSqlEntity
        where TData : MSBaseData, new()
    {
       /* protected string SelectAllSql { get; set; }
        //=====================================================================
        // Method : SelectByKey
        /// <summary>
        /// Select by primary key.
        /// </summary>
        /// <param name="keyData">The data class which contains the search conditions.</param>
        /// <returns>The data class which contains the result.</returns>
        //=====================================================================
        public TData SelectByKey(TData keyData)
        {
            return (TData)BaseSelectByKey(keyData);
        }

        //=====================================================================
        //  Method : SelectByKeyWithLock
        /// <summary>
        /// Select by primary keys. use UPDLOCK to prevent update operation when searching data.
        /// </summary>
        /// <param name="keyData">The data class which contains the search conditions.</param>
        /// <returns>The data class which contains the result.</returns>
        //=====================================================================
        public TData SelectByKeyWithLock(TData keyData)
        {
            return (TData)BaseSelectByKeyWithLock(keyData);
        }

        //=====================================================================
        //  Method : Update
        /// <summary>
        /// UPDATE by primary keys.
        /// </summary>
        /// <param name="data">Data to be updated</param>
        /// <returns>Data count updated.</returns>
        //=====================================================================
        public int Update(TData data)
        {
            return BaseUpdate(data);
        }

        //=====================================================================
        // Method : UpdateWithoutCheck
        /// <summary>
        /// UPDATE without check by primary keys.
        /// </summary>
        /// <param name="data">Data to be updated</param>
        /// <returns>Data count updated.</returns>
        //=====================================================================
        public int UpdateWithoutCheck(TData data)
        {
            return BaseUpdateWithoutCheck(data);
        }

        //=====================================================================
        //  Method : Delete
        /// <summary>
        /// DELETE by primary keys.
        /// </summary>
        /// <param name="data">Data class which contains the delete conditions.</param>
        /// <returns>Count to be deleted.</returns>
        //=====================================================================
        public int Delete(TData data)
        {
            return BaseDelete(data);
        }

        //=====================================================================
        // Method : DeleteWithoutCheck
        /// <summary>
        /// DELETE without check by primary keys.
        /// </summary>
        /// <param name="data">Data class which contains the delete conditions.</param>
        /// <returns>Count to be deleted.</returns>
        //=====================================================================
        public int DeleteWithoutCheck(TData data)
        {
            return BaseDeleteWithoutCheck(data);
        }

        //=====================================================================
        // Method : Insert
        /// <summary>
        /// INSERT into DB
        /// </summary>
        /// <param name="data">Data to be inserted.</param>
        /// <returns>Count to be inserted.</returns>
        //=====================================================================
        public int Insert(TData data)
        {
            return BaseInsert(data);
        }
        //=====================================================================
        // Method : Insert
        /// <summary>
        /// INSERT into DB
        /// </summary>
        /// <returns>Count to be inserted.</returns>
        //=====================================================================
        public List<TData> Select()
        {
            var array = SelectAnyCondition(SelectAllSql, null, typeof(TData));
            return array.Cast<TData>().ToList();
        }

        //=====================================================================
        // Method : SelectByKey
        /// <summary>
        /// Select by primary key.
        /// </summary>
        /// <param name="id">The data class which contains the search conditions.</param>
        /// <returns>The data class which contains the result.</returns>
        //=====================================================================
        public TData SelectByKey(int id)
        {
            SqlParameter[] sqlParameters = 
            {
				new SqlParameter("@Id", SqlDbType.Int)
			};
            sqlParameters[0].Value = id;
            var array = SelectAnyCondition(SelectSql, sqlParameters, typeof(TData));
            if (array == null || array.Length == 0)
                return null;
            return array.GetValue(0) as TData;
        }*/
    }
}
