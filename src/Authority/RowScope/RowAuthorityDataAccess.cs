// // /*****************************************************
// // (c)2016-2017 Copy right Agebull
// // 作者:
// // 工程:CF_WeiYue
// // 建立:2016-08-11
// // 修改:2016-08-11
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    ///     行级权限支持的数据访问类
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class RowAuthorityDataAccess<TData> : MySqlTable<TData>
        where TData : EditDataObject, IPositionDataScope, new()
    {
        /// <summary>
        ///     权限相关的当前表
        /// </summary>
        private string _authorityReadTable;

        /// <summary>
        ///     当前的职位数据范围
        /// </summary>
        private PositionDataScopeType _dataScope;
        
        /// <summary>
        ///     当前的职位数据范围
        /// </summary>
        public PositionDataScopeType DataScope
        {
            get { return this._dataScope; }
            set
            {
                this._dataScope = value;
                switch (value)
                {
                    case PositionDataScopeType.Self:
                        this._authorityReadTable = null;
                        BaseCondition = "[OwnerPerID]=" + BusinessContext.Current.LoginUserId;
                        break;
                    case PositionDataScopeType.Department:
                        this._authorityReadTable = null;
                        BaseCondition = "[OwnerOrgID]=" + BusinessContext.Current.LoginUser.DepartmentId;
                        break;
                    case PositionDataScopeType.DepartmentAndLower:
                        this._authorityReadTable = MasterReadTable;
                        BaseCondition = "[MasterOId]=" + BusinessContext.Current.LoginUser.DepartmentId;
                        break;
                    case PositionDataScopeType.Company:
                        this._authorityReadTable = null;
                        BaseCondition = "[OwnerComID]=" + BusinessContext.Current.LoginUser.DepartmentId;
                        break;
                    case PositionDataScopeType.CompanyAndLower:
                        this._authorityReadTable = MasterReadTable;
                        BaseCondition = "[MasterOId]=" + BusinessContext.Current.LoginUser.DepartmentId;
                        break;
                    default:
                        this._authorityReadTable = null;
                        BaseCondition = null;
                        break;
                }
            }
        }
       
        /// <summary>
        ///     当前上下文读取的表名
        /// </summary>
        protected override sealed string ContextReadTable
        {
            get { return this._dynamicReadTable ?? (this._authorityReadTable ?? this.ReadTableName); }
        }

        /// <summary>
        ///     主从关联读取的表名
        /// </summary>
        protected abstract string MasterReadTable { get; }

    }

    /// <summary>
    ///     数据范围设置
    /// </summary>
    public class DataScopeOption
    {
        /// <summary>
        ///     当前视图读取的表名
        /// </summary>
        public string ReadTable { get; set; }

        /// <summary>
        ///     当前视图的基本查询条件
        /// </summary>
        public string Condition { get; set; }
    }

    /// <summary>
    /// 表明这支持职位行级权限对象
    /// </summary>
    public interface IPositionDataScope
    {
        /// <summary>
        /// 主机构的ID(用于主从关联)
        /// </summary>
        int MasterOId { get; }

        /// <summary>
        /// 所有者职位ID
        /// </summary>
        int OwnerPosID { get; }

        /// <summary>
        /// 所有者人员ID
        /// </summary>
        int OwnerPerID { get; }

        /// <summary>
        /// 所有者机构ID
        /// </summary>
        int OwnerOrgID { get; }

        /// <summary>
        /// 所有者公司ID
        /// </summary>
        int OwnerComID { get; }
    }
}