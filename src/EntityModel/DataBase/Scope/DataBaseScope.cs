// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:CF_WeiYue
// // ����:2016-07-22
// // �޸�:2016-07-27
// // *****************************************************/

#region ����

using System;
using System.Threading.Tasks;
using Agebull.Common.Base;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    [Obsolete]
    public class DataBaseScope : ScopeBase
    {
        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private bool _isHereOpen;

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        protected DataBaseScope(IDataBase dataBase)
        {
            DataBase = dataBase;
            _isHereOpen = DataBase.Open();
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public IDataBase DataBase { get; }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        private async Task<DataBaseScope> CreateScope()
        {
            _isHereOpen = await DataBase.OpenAsync();
            return this;
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static DataBaseScope CreateScope(IDataBase dataBase)
        {
            return new DataBaseScope(dataBase);
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <param name="dataBase">���ݿ����</param>
        /// <returns>��Χ</returns>
        public static Task<DataBaseScope> CreateScopeAsync(IDataBase dataBase)
        {
            var scope = new DataBaseScope(dataBase);
            return scope.CreateScope();
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static DataBaseScope CreateScope<TDataBase>() where TDataBase : IDataBase, new()
        {
            return new DataBaseScope(new TDataBase());
        }

        /// <summary>
        ///     ����һ����Χ
        /// </summary>
        /// <returns>��Χ</returns>
        public static async Task<DataBaseScope> CreateScopeAsync<TDataBase>() where TDataBase : IDataBase, new()
        {
            return await CreateScopeAsync(new TDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            if (_isHereOpen)
            {
                DataBase.Dispose();
            }
        }
    }
}