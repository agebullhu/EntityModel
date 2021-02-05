//// // /*****************************************************
//// // (c)2016-2016 Copy right www.gboxt.com
//// // ����:
//// // ����:Agebull.DataModel
//// // ����:2016-06-16
//// // �޸�:2016-06-16
//// // *****************************************************/

//using Agebull.Common.Base;
//using System;

//namespace Agebull.EntityModel.Common
//{
//    /// <summary>
//    ///     �޸Ķ�ȡ����Χ
//    /// </summary>
//    /// <typeparam name="TEntity">ʵ�����</typeparam>
//    public sealed class ReadTableScope<TEntity> : ScopeBase
//        where TEntity : EditDataObject, new()
//    {
//        private readonly IDataAccess<TEntity> _table;
//        private readonly string _oldName;
//        private ReadTableScope(IDataAccess<TEntity> table, string name)
//        {
//            _table = table;
//            _oldName = table.SetDynamicReadTable(name);
//        }

//        /// <summary>
//        /// ���ɶ�ȡ����Χ
//        /// </summary>
//        /// <param name="table">���õı����</param>
//        /// <param name="name">����</param>
//        /// <returns>��ȡ����Χ</returns>
//        public static ReadTableScope<TEntity> CreateScope(IDataAccess<TEntity> table, string name)
//        {
//            return new ReadTableScope<TEntity>(table, name);
//        }

//        /// <summary>
//        /// ����
//        /// </summary>
//        protected override void OnDispose()
//        {
//            _table.SetDynamicReadTable(_oldName);
//        }
//    }


//    /// <summary>
//    ///     �޸Ķ�д����Χ
//    /// </summary>
//    public sealed class TableSwitchScope : ScopeBase
//    {
//        private readonly IDataAccess _table;
//        private readonly string _oldRead, _oldWrite;

//        private TableSwitchScope(IDataAccess table, string read, string write)
//        {
//            _table = table;
//            if (read != null)
//                _oldRead = table.SetDynamicReadTable(read);
//            if (write != null)
//                _oldWrite = table.SetDynamicWriteTable(write);
//        }

//        /// <summary>
//        /// ���ɶ�ȡ����Χ
//        /// </summary>
//        /// <param name="table">���õı����</param>
//        /// <param name="read">������</param>
//        /// <returns>��ȡ����Χ</returns>
//        public static IDisposable CreateScope(IDataAccess table, string read)
//        {
//            return new TableSwitchScope(table, read, null);
//        }

//        /// <summary>
//        /// ���ɶ�ȡ����Χ
//        /// </summary>
//        /// <param name="table">���õı����</param>
//        /// <param name="read">������</param>
//        /// <param name="write">д����</param>
//        /// <returns>��ȡ����Χ</returns>
//        public static IDisposable CreateScope(IDataAccess table, string read, string write)
//        {
//            return new TableSwitchScope(table, read, write);
//        }

//        /// <inheritdoc />
//        /// <summary>
//        /// ����
//        /// </summary>
//        protected override void OnDispose()
//        {
//            if (_oldRead != null)
//                _table.SetDynamicReadTable(_oldRead);
//            if (_oldWrite != null)
//                _table.SetDynamicWriteTable(_oldWrite);
//        }
//    }
//}