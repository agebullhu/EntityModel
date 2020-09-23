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

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    [Obsolete]
    public class DataBaseScope : IAsyncDisposable
    {
        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private bool _isHereOpen;

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public IDataBase DataBase { get; private set; }

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
        public static async Task<DataBaseScope> CreateScopeAsync(IDataBase dataBase)
        {
            return new DataBaseScope
            {
                DataBase = dataBase,
                _isHereOpen = await dataBase.OpenAsync()
            };
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
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_isHereOpen)
            {
                await DataBase.DisposeAsync();
            }
        }
    }
}