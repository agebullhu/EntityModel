// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:CF_WeiYue
// // ����:2016-07-22
// // �޸�:2016-07-27
// // *****************************************************/

#region ����

using Agebull.Common.Base;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ���ݿ���ʷ�Χ
    /// </summary>
    public class DataBaseScope : ScopeBase
    {
        /// <summary>
        ///     �Ƿ�˴������ݿ�
        /// </summary>
        private readonly bool _isHereOpen;
        
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="dataBase"></param>
        protected DataBaseScope(IDataBase dataBase)
        {
            DataBase = dataBase;

            if (dataBase.Open())
            {
                _isHereOpen = true;
            }
            dataBase.QuoteCount += 1;
        }

        /// <summary>
        ///     ��ǰ���ݿ����
        /// </summary>
        public IDataBase DataBase { get; }

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
        /// <returns>��Χ</returns>
        public static DataBaseScope CreateScope<TDataBase>()
            where TDataBase : IDataBase, new()
        {
            return new DataBaseScope(new TDataBase());
        }

        /// <summary>
        ///     ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            DataBase.QuoteCount -= 1;
            if (_isHereOpen)
            {
                DataBase.Close();
            }
        }
    }
}