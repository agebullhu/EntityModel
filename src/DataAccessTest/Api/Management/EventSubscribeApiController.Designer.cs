/*此标记表明此文件可被设计器更新,如果不允许此操作,请删除此行代码.design by:agebull designer date:2020/10/7 0:54:45*/
#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Agebull.Common.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

using Agebull.Common;
using Agebull.Common.Ioc;

using Agebull.EntityModel.Common;
using ZeroTeam.MessageMVC.ModelApi;



using Zeroteam.MessageMVC.EventBus;
#endregion

namespace Zeroteam.MessageMVC.EventBus.WebApi
{
    partial class EventSubscribeApiController
    {
        #region 基本扩展

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value">文本</param>
        /// <returns></returns>
        protected override (bool, long) Convert(string value)
        {
            return long.TryParse(value,out var key) ? (true , key) : (false , key);
        }

        /// <summary>
        ///     读取查询条件
        /// </summary>
        /// <param name="filter">筛选器</param>
        public override void GetQueryFilter(LambdaItem<EventSubscribeEntity> filter)
        {
            if (RequestArgumentConvert.TryGet("_value_", out string value) && value != null)
            {
                var field = RequestArgumentConvert.GetString("_field_");
                if (string.IsNullOrWhiteSpace(field) || field == "_any_")
                    filter.AddAnd(p => p.Service.Contains(value)
                                    || p.ApiName.Contains(value)
                                    || p.Memo.Contains(value)
                                    || p.TargetName.Contains(value)
                                    || p.TargetType.Contains(value)
                                    || p.TargetDescription.Contains(value)
                                    || p.LastReviserId.Contains(value)
                                    || p.LastReviser.Contains(value)
                                    || p.AuthorId.Contains(value)
                                    || p.Author.Contains(value));
                else
RequestArgumentConvert.SetArgument(field,value);
            }
            if(RequestArgumentConvert.TryGetIDs("id" , out var id))
            {
                if (id.Count == 1)
                    filter.AddAnd(p => p.Id == id[0]);
                else
                    filter.AddAnd(p => id.Contains(p.Id));
            }
            if(RequestArgumentConvert.TryGet("eventId" , out long eventId))
            {
                filter.AddAnd(p => p.EventId == eventId);
            }
            if(RequestArgumentConvert.TryGet("service" , out string service))
            {
                filter.AddAnd(p => p.Service.Contains(service));
            }
            if(RequestArgumentConvert.TryGet("isLookUp" , out bool isLookUp))
            {
                filter.AddAnd(p => p.IsLookUp == isLookUp);
            }
            if(RequestArgumentConvert.TryGet("apiName" , out string apiName))
            {
                filter.AddAnd(p => p.ApiName.Contains(apiName));
            }
            if(RequestArgumentConvert.TryGet("memo" , out string memo))
            {
                filter.AddAnd(p => p.Memo.Contains(memo));
            }
            if(RequestArgumentConvert.TryGet("targetName" , out string targetName))
            {
                filter.AddAnd(p => p.TargetName.Contains(targetName));
            }
            if(RequestArgumentConvert.TryGet("targetType" , out string targetType))
            {
                filter.AddAnd(p => p.TargetType.Contains(targetType));
            }
            if(RequestArgumentConvert.TryGet("targetDescription" , out string targetDescription))
            {
                filter.AddAnd(p => p.TargetDescription.Contains(targetDescription));
            }
            if(RequestArgumentConvert.TryGet("isFreeze" , out bool isFreeze))
            {
                filter.AddAnd(p => p.IsFreeze == isFreeze);
            }
            if(RequestArgumentConvert.TryGetEnum<DataStateType>("dataState" , out DataStateType dataState))
            {
                filter.AddAnd(p => p.DataState == dataState);
            }
            if(RequestArgumentConvert.TryGet("lastModifyDate" , out DateTime lastModifyDate))
            {
                var day = lastModifyDate.Date;
                var nextDay = day.AddDays(1);
                filter.AddAnd(p => (p.LastModifyDate >= day && p.LastModifyDate < nextDay));
            }
            else 
            {
                if(RequestArgumentConvert.TryGet("lastModifyDate_begin" , out DateTime lastModifyDate_begin))
                {
                    var day = lastModifyDate_begin.Date;
                    filter.AddAnd(p => p.LastModifyDate >= day);
                }
                if(RequestArgumentConvert.TryGet("lastModifyDate_end" , out DateTime lastModifyDate_end))
                {
                    var day = lastModifyDate_end.Date.AddDays(1);
                    filter.AddAnd(p => p.LastModifyDate < day);
                }
            }
            if(RequestArgumentConvert.TryGet("lastReviserId" , out string lastReviserId))
            {
                filter.AddAnd(p => p.LastReviserId.Contains(lastReviserId));
            }
            if(RequestArgumentConvert.TryGet("lastReviser" , out string lastReviser))
            {
                filter.AddAnd(p => p.LastReviser.Contains(lastReviser));
            }
            if(RequestArgumentConvert.TryGet("authorId" , out string authorId))
            {
                filter.AddAnd(p => p.AuthorId.Contains(authorId));
            }
            if(RequestArgumentConvert.TryGet("author" , out string author))
            {
                filter.AddAnd(p => p.Author.Contains(author));
            }
            if(RequestArgumentConvert.TryGet("addDate" , out DateTime addDate))
            {
                var day = addDate.Date;
                var nextDay = day.AddDays(1);
                filter.AddAnd(p => (p.AddDate >= day && p.AddDate < nextDay));
            }
            else 
            {
                if(RequestArgumentConvert.TryGet("addDate_begin" , out DateTime addDate_begin))
                {
                    var day = addDate_begin.Date;
                    filter.AddAnd(p => p.AddDate >= day);
                }
                if(RequestArgumentConvert.TryGet("addDate_end" , out DateTime addDate_end))
                {
                    var day = addDate_end.Date.AddDays(1);
                    filter.AddAnd(p => p.AddDate < day);
                }
            }
        }

        /// <summary>
        /// 读取Form传过来的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="convert">转化器</param>
        protected void DefaultReadFormData(EventSubscribeEntity data, FormConvert convert)
        {
            //普通字段
            if(convert.TryGetValue("eventId" , out long EventId))
                data.EventId = EventId;
            if(convert.TryGetValue("service" , out string Service))
                data.Service = Service;
            if(convert.TryGetValue("isLookUp" , out bool IsLookUp))
                data.IsLookUp = IsLookUp;
            if(convert.TryGetValue("apiName" , out string ApiName))
                data.ApiName = ApiName;
            if(convert.TryGetValue("memo" , out string Memo))
                data.Memo = Memo;
            if(convert.TryGetValue("targetName" , out string TargetName))
                data.TargetName = TargetName;
            if(convert.TryGetValue("targetType" , out string TargetType))
                data.TargetType = TargetType;
            if(convert.TryGetValue("targetDescription" , out string TargetDescription))
                data.TargetDescription = TargetDescription;
        }

        #endregion

    }
}