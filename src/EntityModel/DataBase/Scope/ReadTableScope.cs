//// // /*****************************************************
//// // (c)2016-2016 Copy right www.gboxt.com
//// // 作者:
//// // 工程:Agebull.DataModel
//// // 建立:2016-06-16
//// // 修改:2016-06-16
//// // *****************************************************/

//using Agebull.Common.Base;
//using System;

//namespace Agebull.EntityModel.Common
//{
//    /// <summary>
//    ///     修改读取对象范围
//    /// </summary>
//    /// <typeparam name="TEntity">实体对象</typeparam>
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
//        /// 生成读取对象范围
//        /// </summary>
//        /// <param name="table">作用的表对象</param>
//        /// <param name="name">表名</param>
//        /// <returns>读取对象范围</returns>
//        public static ReadTableScope<TEntity> CreateScope(IDataAccess<TEntity> table, string name)
//        {
//            return new ReadTableScope<TEntity>(table, name);
//        }

//        /// <summary>
//        /// 析构
//        /// </summary>
//        protected override void OnDispose()
//        {
//            _table.SetDynamicReadTable(_oldName);
//        }
//    }


//    /// <summary>
//    ///     修改读写对象范围
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
//        /// 生成读取对象范围
//        /// </summary>
//        /// <param name="table">作用的表对象</param>
//        /// <param name="read">读表名</param>
//        /// <returns>读取对象范围</returns>
//        public static IDisposable CreateScope(IDataAccess table, string read)
//        {
//            return new TableSwitchScope(table, read, null);
//        }

//        /// <summary>
//        /// 生成读取对象范围
//        /// </summary>
//        /// <param name="table">作用的表对象</param>
//        /// <param name="read">读表名</param>
//        /// <param name="write">写表名</param>
//        /// <returns>读取对象范围</returns>
//        public static IDisposable CreateScope(IDataAccess table, string read, string write)
//        {
//            return new TableSwitchScope(table, read, write);
//        }

//        /// <inheritdoc />
//        /// <summary>
//        /// 析构
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