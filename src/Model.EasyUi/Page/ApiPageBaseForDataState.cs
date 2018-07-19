// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Linq.Expressions;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///      支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    public abstract class ApiPageBaseForDataState<TData, TAccess, TBusinessLogic> :
        ApiPageBaseEx<TData, TAccess, TBusinessLogic>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
        where TBusinessLogic : BusinessLogicByStateData<TData, TAccess>, new()
    {
        #region 操作

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected override void DoActinEx(string action)
        {
            switch (action)
            {
                case "enable":
                    OnEnable();
                    break;
                case "disable":
                    OnDisable();
                    break;
                case "discard":
                    OnDiscard();
                    break;
                case "reset":
                    OnReset();
                    break;
                case "lock":
                    OnLock();
                    break; 
                 default:
                    base.DoActinEx(action);
                    break;
            }
        }

        /// <summary>
        ///     锁定对象
        /// </summary>
        protected virtual void OnLock()
        {
            foreach (var id in GetIntArrayArg("selects"))
            {
                Business.Lock(id);
            }
        }

        /// <summary>
        ///     恢复对象
        /// </summary>
        private void OnReset()
        {
            foreach (var id in GetIntArrayArg("selects"))
            {
                Business.Reset(id);
            }
        }

        /// <summary>
        ///     废弃对象
        /// </summary>
        private void OnDiscard()
        {
            foreach (var id in GetIntArrayArg("selects"))
            {
                Business.Discard(id);
            }
        }

        /// <summary>
        ///     启用对象
        /// </summary>
        private void OnEnable()
        {
            foreach (var id in GetIntArrayArg("selects"))
            {
                Business.Enable(id);
            }
        }

        /// <summary>
        ///     禁用对象
        /// </summary>
        private void OnDisable()
        {
            foreach (var id in GetIntArrayArg("selects"))
            {
                Business.Disable(id);
            }
        }

        #endregion


        #region 列表数据

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData()
        {
            var root = new LambdaItem<TData>();
            GetListData(root);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected override void GetListData(LambdaItem<TData> lambda)
        {
            var state = GetIntArg("dataState", 0x100);
            if (state >= 0)
            {
                if (state < 0x100)
                {
                    lambda.AddRoot(p => p.DataState == (DataStateType)state);
                }
                else
                {
                    lambda.AddRoot(p => p.DataState < DataStateType.Delete);
                }
            }
            DoGetListData(lambda);
        }

        #endregion


        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <param name="field"></param>
        protected override void CheckUnique<TValue>(string name, Expression<Func<TData, TValue>> field)
        {
            var no = GetArg("No");
            if (string.IsNullOrEmpty(no))
            {
                SetFailed(name + "为空");
                return;
            }
            var id = GetIntArg("id", 0);
            Expression<Func<TData, bool>> condition;
            if (id == 0)
            {
                condition = p => p.DataState < DataStateType.Delete;
            }
            else
            {
                condition = p => p.Id != id && p.DataState < DataStateType.Delete;
            }
            if (Business.Access.IsUnique(field, no, condition))
            {
                SetFailed(name + "[" + no + "]不唯一");
            }
            else
            {
                Message = name + "[" + no + "]唯一";
            }
        }

        #endregion
    }

    /// <summary>
    ///     支持数据状态的启用禁用方法的页面的基类
    /// </summary>
    public abstract class ApiPageBaseForDataState<TData, TAccess> :
        ApiPageBaseForDataState<TData, TAccess, BusinessLogicByStateData<TData, TAccess>>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
    }
}