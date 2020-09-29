// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     表示数据接口
    /// </summary>
    public static class DataInterface
    {
        public class IAuthorInfomation
        {
            /// <summary>
            /// 名称
            /// </summary>
            public const string Name = nameof(IAuthorInfomation);

            public static PropertyDefault AddDate => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert,
                Entity = "IModifyInfomation",
                Name = "AddDate",
                Caption = @"制作时间",
                JsonName = "addDate",
                FieldName = "created_date",
                PropertyType = typeof(DateTime),
                CanNull = false,
                ValueType = PropertyValueType.Value,
                DbType = 12,
                
                
                Description = @"制作时间"
            };

            public static PropertyDefault AuthorId => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read | ReadWriteFeatrue.Insert,
                Entity = "IModifyInfomation",
                //LinkField = "AddUserId",
                Name = "AuthorId",
                Caption = @"制作人标识",
                JsonName = "authorId",
                FieldName = "created_user_id",
                PropertyType = typeof(string),
                CanNull = false,
                ValueType = PropertyValueType.String,
                DbType = 15,
                
                
                Description = @"制作人标识"
            };


            public static PropertyDefault Author => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read,
                Entity = "IModifyInfomation",
                //LinkField = "Author",
                Name = "Author",
                Caption = @"制作人",
                JsonName = "author",
                FieldName = "created_user",
                PropertyType = typeof(string),
                CanNull = false,
                ValueType = PropertyValueType.String,
                DbType = 15,
                
                
                Description = @"制作人"
            };
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        public class IModifyInfomation
        {
            /// <summary>
            /// 名称
            /// </summary>
            public const string Name = nameof(IModifyInfomation);


            public static PropertyDefault LastModifyDate => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read,
                Entity = "IModifyInfomation",
                //LinkField = "LastModifyDate",
                Name = "LastModifyDate",
                Caption = @"最后修改日期",
                JsonName = "lastModifyDate",
                FieldName = "latest_updated_date",
                PropertyType = typeof(DateTime),
                CanNull = false,
                ValueType = PropertyValueType.Value,
                DbType = 12,
                
                
                Description = @"最后修改日期"
            };


            public static PropertyDefault LastReviserId => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read,
                Entity = "IModifyInfomation",
                //LinkField = "LastModifyUserId",
                Name = "LastReviserId",
                Caption = @"最后修改者标识",
                JsonName = "lastReviserId",
                FieldName = "latest_updated_user_id",
                PropertyType = typeof(string),
                CanNull = false,
                ValueType = PropertyValueType.String,
                DbType = 15,
                
                
                Description = @"最后修改者标识"
            };


            public static PropertyDefault LastReviser => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read,
                Entity = "IModifyInfomation",
                //LinkField = "LastReviser",
                Name = "LastReviser",
                Caption = @"最后修改者",
                JsonName = "lastReviser",
                FieldName = "latest_updated_user",
                PropertyType = typeof(string),
                CanNull = false,
                ValueType = PropertyValueType.String,
                DbType = 15,
                
                
                Description = @"最后修改者"
            };
        }

        public class IStateData
        {
            /// <summary>
            /// 名称
            /// </summary>
            public const string Name = nameof(IStateData);

            public static PropertyDefault IsFreeze => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.None,
                Entity = "IStateData",
                //LinkField = "DataIsFreeze",
                Name = "IsFreeze",
                Caption = @"冻结更新",
                JsonName = "isFreeze",
                FieldName = "is_freeze",
                PropertyType = typeof(bool),
                CanNull = false,
                ValueType = PropertyValueType.Value,
                DbType = 1,
                
                
                Description = @"无论在什么数据状态,一旦设置且保存后,数据将不再允许执行Update的操作,作为Update的统一开关.取消的方法是单独设置这个字段的值"
            };


            public static PropertyDefault DataState => new PropertyDefault
            {
                PropertyFeatrue = PropertyFeatrue.Interface | PropertyFeatrue.Property | PropertyFeatrue.Field,
                DbReadWrite = ReadWriteFeatrue.Read,
                Entity = "IStateData",
                //LinkField = "DataState",
                Name = "DataState",
                Caption = @"数据状态",
                JsonName = "dataState",
                FieldName = "data_state",
                PropertyType = typeof(int),
                CanNull = false,
                ValueType = PropertyValueType.NumberEnum,
                DbType = 3,
                
                
                Description = @"数据状态"
            };
        }

    }
}