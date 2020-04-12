﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using ZeroTeam.MessageMVC.Context;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.BusinessLogic;
using Agebull.MicroZero.WebApi;
using ZeroTeam.MessageMVC.ZeroApis;
using ApiFileResult = ZeroTeam.MessageMVC.ZeroApis.IApiResult<(string name, string mime, byte[] bytes)>;

namespace Agebull.MicroZero.ZeroApis
{
    /// <summary>
    ///     自动实现基本增删改查API页面的基类
    /// </summary>
    public abstract class ApiController<TData, TBusinessLogic> : ModelApiController
        where TData : EditDataObject, IIdentityData, new()
        where TBusinessLogic : class, IUiBusinessLogicBase<TData>, new()
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
            if (!TryGetValue("No",out var no))
            {
                SetFailed(name + "为空");
                return;
            }

            var id = GetIntArg("id", 0);
            var result = id == 0
                ? Business.Access.IsUnique(field, no)
                : Business.Access.IsUnique(field, no, id);
            if (result)
                SetFailed(name + "[" + no + "]不唯一");
            else
                GlobalContext.Current.Status.LastMessage = name + "[" + no + "]唯一";
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

        #endregion

        #region API

        /// <summary>
        ///     实体类型
        /// </summary>
        /// <returns></returns>
        [Route("edit/eid")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<EntityInfo> EntityType()
        {
            return ApiResultHelper.Succees(new EntityInfo
            {
                EntityType = Business.EntityType,
                //PageId = PageItem?.Id ?? 0
            });
        }

        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Obsolete]
        [Route("import/xlsx")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public ApiFileResult Export2(QueryArgument args)
        {
            var data = new TData();
            GlobalContext.Current.Status.Feature = 1;
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);
            var res = Business.Export(data.__Struct.Caption, filter);
            GlobalContext.Current.Status.Feature = 0;
            return ApiResultHelper.Succees(res);
        }

        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("export/xlsx")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public ApiFileResult Export(QueryArgument args)
        {
            var data = new TData();
            GlobalContext.Current.Status.Feature = 1;
            var filter = new LambdaItem<TData>();
            GetQueryFilter(filter);
            var res = Business.Export(data.__Struct.Caption, filter);
            GlobalContext.Current.Status.Feature = 0;
            return ApiResultHelper.Succees(res);
        }
        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        public virtual void GetQueryFilter(LambdaItem<TData> filter)
        {

        }

        IDisposable GetFieldFilter()
        {
            if (TryGet("_filter_", out string[] fieldFilter))
            {
                var test = new TData();
                List<PropertySturct> properties = new List<PropertySturct>();
                foreach (var field in fieldFilter)
                {
                    var pro = test.__Struct.Properties.Values.FirstOrDefault(p => p.JsonName == field || p.PropertyName == field);
                    if (pro != null && pro.ColumnName != null)
                    {
                        properties.Add(pro);
                    }
                }
                if (properties.Count > 0)
                    return DbReaderScope<TData>.CreateScope(Business.Access, properties.Select(p => p.ColumnName).LinkToString(","), (reader, entity) =>
                    {
                        for (var idx = 0; idx < properties.Count; idx++)
                        {
                            if (!reader.IsDBNull(idx))
                                entity.SetValue(properties[idx].Index, reader.GetValue(idx));
                        }
                    });
            }
            return null;
        }
        /// <summary>
        ///     列表数据
        /// </summary>
        /// <remarks>
        /// 参数中可传递实体字段具体的查询条件,所有的条件按AND组合查询
        /// </remarks>
        /// <returns></returns>
        [Route("edit/list")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<ApiPageData<TData>> List(QueryArgument args)
        {
            IDisposable scope = null;
            try
            {

                scope = GetFieldFilter();

                GlobalContext.Current.Status.Feature = 1;

                var filter = new LambdaItem<TData>();
                GetQueryFilter(filter);
                var data = GetListData(filter);
                GlobalContext.Current.Status.Feature = 0;
                return IsFailed
                    ? ApiResultHelper.Error<ApiPageData<TData>>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
            }
            finally
            {
                scope?.Dispose();
            }
        }


        /// <summary>
        ///     单条数据查询
        /// </summary>
        [Route("edit/first")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<TData> QueryFirst(TData arguent)
        {
            IDisposable scope = null;
            try
            {
                scope = GetFieldFilter();
                GlobalContext.Current.Status.Feature = 1;
                var filter = new LambdaItem<TData>();
                GetQueryFilter(filter);
                var data = Business.Access.FirstOrDefault(filter);
                if (data != null)
                {
                    OnDetailsLoaded(data, false);
                }
                GlobalContext.Current.Status.Feature = 0;
                return IsFailed
                    ? ApiResultHelper.Error<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
            }
            finally
            {
                scope?.Dispose();
            }
        }

        /// <summary>
        ///     单条详细数据
        /// </summary>
        [Route("edit/details")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<TData> Details(IdArguent arguent)
        {
            if (!TryGet("id", out long id))
                return ApiResultHelper.Error<TData>(DefaultErrorCode.ArgumentError, "参数[id]不是有效的数字");
            var data = DoDetails(id);
            return IsFailed
                    ? ApiResultHelper.Error<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     新增数据
        /// </summary>
        [Route("edit/addnew")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<TData> AddNew(TData arg)
        {
            var data = DoAddNew();
            return IsFailed
                    ? ApiResultHelper.Error<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     更新数据
        /// </summary>
        [Route("edit/update")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult<TData> Update(TData arg)
        {
            if (!TryGet("id", out long id))
                return ApiResultHelper.Error<TData>(DefaultErrorCode.ArgumentError, "参数[id]不是有效的数字");
            var data = DoUpdate(id);
            return IsFailed
                    ? ApiResultHelper.Error<TData>(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees(data);
        }

        /// <summary>
        ///     删除多条数据
        /// </summary>
        [Route("edit/delete")]
        [ApiAccessOptionFilter(ApiAccessOption.Internal | ApiAccessOption.ArgumentIsDefault)]
        public IApiResult Delete(IdsArguent arg)
        {
            DoDelete();
            return IsFailed
                    ? ApiResultHelper.Error(GlobalContext.Current.Status.LastState, GlobalContext.Current.Status.LastMessage)
                    : ApiResultHelper.Succees();
        }

        #endregion

        #region 列表读取支持

        /// <summary>
        ///     取得列表数据
        /// </summary>
        protected virtual ApiPageData<TData> GetListData(LambdaItem<TData> lambda)
        {
            var item = Business.Access.Compile(lambda);
            return LoadListData(item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     取得列表数据
        /// </summary>
        private ApiPageData<TData> LoadListData(string condition, DbParameter[] args)
        {
            var page = GetIntArg("page", 1);
            var rows = GetIntArg("rows", 20);
            TryGet("sort", out string sort);
            if (sort == null)
                sort = Business.Access.KeyField;
            var desc = TryGet("order", out string order) && order?.ToLower() == "desc";

            //SaveQueryArguments(page, sort, adesc, rows);

            var data = Business.PageData(page, rows, sort, desc, condition, args);
            OnListLoaded(data.Rows);
            return data;
        }
        /*
        /// <summary>
        ///     是否保存查询条件
        /// </summary>
        protected virtual bool CanSaveQueryArguments => true;

        private void SaveQueryArguments(int page, string sort, string adesc, int rows)
        {
            if (CanSaveQueryArguments)
                BusinessContext.Context?.PowerChecker?.SaveQueryHistory(LoginUser, PageItem, Arguments);
        }
        */
        /// <summary>
        ///     数据准备返回的处理
        /// </summary>
        /// <param name="result">当前的查询结果</param>
        /// <param name="condition">当前的查询条件</param>
        /// <param name="args">当前的查询参数</param>
        protected virtual bool CheckListResult(ApiPageData<TData> result, string condition, params DbParameter[] args)
        {
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

        /// <summary>
        ///     载入当前操作的数据
        /// </summary>
        protected virtual TData DoDetails(long id)
        {
            TData data;
            if (id <= 0)
            {
                data = CreateData();
                OnDetailsLoaded(data, true);
            }
            else
            {
                data = Business.Details(id);
                if (data == null)
                {
                    SetFailed("数据不存在");
                    return null;
                }
                OnDetailsLoaded(data, false);
            }

            return data;
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
        protected virtual TData CreateData()
        {
            return new TData();
        }

        /// <summary>
        ///     新增
        /// </summary>
        protected virtual TData DoAddNew()
        {
            var data = new TData();
            data.__status.IsNew = true;
            data.__status.IsFromClient = true;
            //数据校验

            var convert = new FormConvert(this, data);
            ReadFormData(data, convert);
            if (convert.Failed)
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.ArgumentError;
                GlobalContext.Current.Status.LastMessage = convert.Message;
                return null;
            }
            if (!Business.AddNew(data))
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
                return null;
            }
            return data;
        }

        /// <summary>
        ///     更新对象
        /// </summary>
        protected virtual TData DoUpdate(long id)
        {
            var data = Business.Details(id);
            if (data == null)
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.ArgumentError;
                GlobalContext.Current.Status.LastMessage = "参数错误";
                return null;
            }
            data.__status.IsExist = true;
            data.__status.IsFromClient = true;
            //数据校验
            var convert = new FormConvert(this, data)
            {
                IsUpdata = true
            };
            ReadFormData(data, convert);
            if (convert.Failed)
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.ArgumentError;
                GlobalContext.Current.Status.LastMessage = convert.Message;
                return null;
            }
            if (!Business.Update(data))
            {
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
                return null;
            }
            return data;
        }

        /// <summary>
        ///     删除对象
        /// </summary>
        private void DoDelete()
        {
            if (!TryGet("selects", out long[] ids))
            {
                SetFailed("没有数据");
                return;
            }
            if (!Business.Delete(ids))
                GlobalContext.Current.Status.LastState = DefaultErrorCode.BusinessError;
        }

        #endregion
    }

}