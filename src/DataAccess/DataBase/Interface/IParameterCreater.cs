// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System.Data.Common;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 参数生成器
    /// </summary>
    public interface IParameterCreater
    {
        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateFieldParameter(string field, int dbType, object value);

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string csharpType, string parameterName, object value);


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="dbType">对应数据库的DbType，如MysqlDbType</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string parameterName, object value, int dbType);


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string parameterName, object value);

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter(string parameterName, string value);

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        DbParameter CreateParameter<T>(string parameterName, T value)
            where T : struct;
    }
}