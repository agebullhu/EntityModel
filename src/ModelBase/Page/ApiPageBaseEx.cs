// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.WebUI
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiPageBaseEx<TData, TAccess, TBusinessLogic> : ApiPageBase
        where TData : EditDataObject, IIdentityData, new()
        where TAccess :class, IDataTable<TData>, new()
        where TBusinessLogic : UiBusinessLogicBase<TData, TAccess>, new()
    {
        #region 数据校验支持

        /// <summary>
        ///     检查值的唯一性
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <param name="field"></param>
        protected virtual void CheckUnique<TValue>(string name, Expression<Func<TData, TValue>> field)
        {
            var no = GetArg("No");
            if (string.IsNullOrEmpty(no))
            {
                SetFailed(name + "为空");
                return;
            }
            var id = GetIntArg("id", 0);
            var result = id == 0
                ? Business.Access.IsUnique(field, no)
                : Business.Access.IsUnique(field, no, id);
            if (result)
            {
                SetFailed(name + "[" + no + "]不唯一");
            }
            else
            {
                Message = name + "[" + no + "]唯一";
            }
        }

        #endregion

        #region 基础变量

        private TBusinessLogic _business;

        /// <summary>
        ///     业务逻辑对象
        /// </summary>
        protected TBusinessLogic Business
        {
            get => _business ?? (_business = new TBusinessLogic());
            set => _business = value;
        }

        /// <summary>
        ///     基本查询条件(SQL表述方式)
        /// </summary>
        protected virtual string BaseQueryCondition => null;

        #endregion

        #region 执行操作

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,如为单个操作可忽略</param>
        protected sealed override void DoActin(string action)
        {
            Business.Request = Request;
            using (MySqlDataBaseScope.CreateScope(MySqlDataBase.DefaultDataBase))
            {
                ConvertQueryString("id", arg => int.TryParse(arg, out _dataId));
                var a = string.IsNullOrWhiteSpace(action) ? "list" : action.ToLower();
                switch (a)
                {
                    case "list":
                        GetListData();
                        break;
                    case "details":
                        Details();
                        break;
                    case "addnew":
                        AddNew();
                        break;
                    case "update":
                        Update();
                        break;
                    case "delete":
                        DoDelete();
                        break;
                    case "eid":
                        GetEntityType();
                        break;
                    default:
                        DoActinEx(a);
                        break;
                }
            }
        }

        /// <summary>
        ///     执行操作
        /// </summary>
        /// <param name="action">传入的动作参数,已转为小写</param>
        protected virtual void DoActinEx(string action)
        {
        }

        /// <summary>
        ///     取得实体标识
        /// </summary>
        protected virtual void GetEntityType()
        {
            if (PageItem == null)
            {
                LogRecorder.RecordLoginLog("错误页面" + Request.Url);
            }
            //int pid;
            //using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            //{
            //    pid = proxy.Get<int>($"page:{typeof(TData).FullName}");
            //}
            SetResultData(new
            {
                entityType = Business.EntityType,
                pageId = PageItem?.Id ?? 0
            });
        }

        #endregion

        #region 列表读取支持

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected virtual void GetListData()
        {
            LoadListData(null, null);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void GetListData(Expression<Func<TData, bool>> lambda)
        {
            GetListData(new[] { lambda });
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void GetListData(IEnumerable<Expression<Func<TData, bool>>> lambdas)
        {
            var lg = new TAccess();
            var condition = new ConditionItem();
            foreach (var lambda in lambdas)
            {
                PredicateConvert.Convert(lg.FieldDictionary, lambda, condition);
            }
            GetListData(condition);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected virtual void GetListData(LambdaItem<TData> lambda)
        {
            DoGetListData(lambda);
        }


        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void DoGetListData(LambdaItem<TData> lambda)
        {
            var lg = new TAccess();
            var condition = PredicateConvert.Convert(lg.FieldDictionary, lambda);
            GetListData(condition);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void GetListData(ConditionItem item)
        {
            GetListData(new[] { item });
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void GetListData(IEnumerable<ConditionItem> items)
        {
            var parameters = new List<MySqlParameter>();
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.ConditionSql))
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }
                    sb.Append("(" + item.ConditionSql + ")");
                }
                parameters.AddRange(item.Parameters);
            }
            LoadListData(sb.ToString(), parameters.ToArray());
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected void LoadListData(string condition, MySqlParameter[] args)
        {
            var page = GetIntArg("page", 1);
            var rows = GetIntArg("rows", 20);
            var sort = GetArg("sort");
            bool desc;
            var adesc = GetArg("order", "asc").ToLower();
            if (sort == null)
            {
                sort = Business.Access.PrimaryKey;
                desc = true;
            }
            else
            {
                desc = adesc == "desc";
            }

            SaveQueryArguments(page, sort, adesc, rows);

            if (!string.IsNullOrEmpty(BaseQueryCondition))
            {
                if (string.IsNullOrEmpty(condition))
                {
                    condition = BaseQueryCondition;
                }
                else if (condition != BaseQueryCondition && !condition.Contains(BaseQueryCondition))
                {
                    condition = $"({BaseQueryCondition}) AND ({condition})";
                }
            }

            BusinessContext.Current.IsUnSafeMode = true;
            var data = Business.PageData(page, rows, sort, desc, condition, args);
            if (OnListLoaded(data.Data, data.Total))
            {
                AjaxResult = data;
                CheckListResult(data, condition, args);
            }
        }

        /// <summary>
        ///     是否保存查询条件
        /// </summary>
        protected virtual bool CanSaveQueryArguments
        {
            get { return true; }
        }

        private void SaveQueryArguments(int page, string sort, string adesc, int rows)
        {
            //if (!this.CanSaveQueryArguments)
            //{
            //    return;
            //}
            var requestArgs = new Dictionary<string, string>
            {
                {"page", page.ToString()},
                {"sort", sort},
                {"order", adesc},
                {"size", rows.ToString()}
            };
            foreach (var name in Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(name) && !requestArgs.ContainsKey(name))
                {
                    requestArgs.Add(name, GetArg(name));
                }
            }
            foreach (var name in Request.Form.AllKeys)
            {
                if (!string.IsNullOrEmpty(name) && !requestArgs.ContainsKey(name))
                {
                    requestArgs.Add(name, GetArg(name));
                }
            }
            BusinessContext.Current.PowerChecker.SaveQueryHistory(LoginUser, PageItem, requestArgs);
        }

        /// <summary>
        ///     数据准备返回的处理
        /// </summary>
        /// <param name="result">当前的查询结果</param>
        /// <param name="condition">当前的查询条件</param>
        /// <param name="args">当前的查询参数</param>
        protected virtual bool CheckListResult(EasyUiGridData<TData> result, string condition, params MySqlParameter[] args)
        {
            return true;
        }

        /// <summary>
        ///     数据载入的处理
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="count"></param>
        protected virtual bool OnListLoaded(IList<TData> datas, int count)
        {
            OnListLoaded(datas);
            return true;
        }

        /// <summary>
        ///     数据载入的处理
        /// </summary>
        /// <param name="datas"></param>
        protected virtual void OnListLoaded(IList<TData> datas)
        {
        }

        #endregion

        #region 基本增删改查

        /// <summary>
        ///     读取Form传过来的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="convert">转化器</param>
        protected abstract void ReadFormData(TData entity, FormConvert convert);


        private int _dataId = -1;

        /// <summary>
        ///     当前上下文数据ID
        /// </summary>
        public int ContextDataId
        {
            get { return _dataId; }
            protected set { _dataId = value; }
        }

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        protected virtual void Details()
        {
            TData data;
            if (ContextDataId <= 0)
            {
                SetResultData(data = CreateData());
                OnDetailsLoaded(data, true);
            }
            else
            {
                data = Business.Details(ContextDataId);
                if (data == null)
                {
                    SetFailed("数据不存在");
                    return;
                }
                OnDetailsLoaded(data, false);
            }
            SetResultData(data);
        }

        /// <summary>
        ///     详细数据载入
        /// </summary>
        protected virtual void OnDetailsLoaded(TData data, bool isNew)
        {
        }

        /// <summary>
        ///     新增一条带默认值的数据
        /// </summary>
        public virtual TData CreateData()
        {
            return new TData();
        }

        /// <summary>
        ///     新增
        /// </summary>
        protected virtual void AddNew()
        {
            var data = new TData();
            //数据校验
            var convert = new FormConvert
            {
                Request = Request
            };
            ReadFormData(data, convert);
            data.__IsFromUser = true;
            //if (convert.Failed)
            //{
            //    SetFailed(/*"数据不正确,保存失败<br/>" +*/
            //              string.Join("<br/>", convert.Messages.Select(p => string.Format("{0}:{1}", p.Key, p.Value))));
            //    return;
            //}
            if (!Business.AddNew(data))
            {
                SetFailed(/*"数据不正确,保存失败<br/>" +*/ BusinessContext.Current.GetFullMessage());
                return;
            }
            SetResultData(data.Id);
        }

        /// <summary>
        ///     更新对象
        /// </summary>
        protected virtual void Update()
        {
            var data = Business.Details(ContextDataId) ?? new TData();
            //数据校验
            var convert = new FormConvert
            {
                Request = Request
            };
            ReadFormData(data, convert);
            data.__IsFromUser = true;
            //if (convert.Failed)
            //{
            //    SetFailed(/*"数据不正确,保存失败<br/>" +*/
            //              string.Join("<br/>", convert.Messages.Select(p => string.Format("{0}:{1}", p.Key, p.Value))));
            //    return;
            //}
            if (!Business.Update(data))
            {
                SetFailed(/*"数据不正确,保存失败<br/>" +*/ BusinessContext.Current.GetFullMessage());
                return;
            }
            SetResultData(data.Id);
        }

        /// <summary>
        ///     删除对象
        /// </summary>
        private void DoDelete()
        {
            var ids = GetArg("selects");
            if (string.IsNullOrEmpty(ids))
            {
                SetFailed("没有数据");
                return;
            }
            var lid = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            if (lid.Length == 0)
            {
                SetFailed("没有数据");
                return;
            }
            if (Business.Delete(lid))
            {
                return;
            }
            SetFailed(BusinessContext.Current.GetFullMessage());
        }

        #endregion
    }

    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiPageBaseEx<TData, TAccess> :
        ApiPageBaseEx<TData, TAccess, UiBusinessLogicBase<TData, TAccess>>
        where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
    }
}