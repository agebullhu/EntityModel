using Agebull.Common.Ioc;
using Agebull.EntityModel.BusinessLogic;

namespace ZeroTeam.AspNet.ModelApi
{
    /// <summary>API返回基类</summary>
    public static class ApiResultHelper
    {
        #region 构造方法

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult Succees() => Succees();

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <returns></returns>
        public static ApiResult State(int code) => State(code);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <returns></returns>
        public static ApiResult State(int code, string message) => State(code, message);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <returns></returns>
        public static ApiResult State(int code, string message, string innerMessage) => State(code, message, innerMessage);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult State(int code, string message, string innerMessage, string guide, string describe)
             => State(code, message, innerMessage, guide, describe);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult State(int code, string message, string innerMessage, string point, string guide, string describe)
             => State(code, message, innerMessage, point, guide, describe);

        /// <summary>
        ///     生成一个成功的标准返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> Succees<TData>(TData data) => Succees<TData>(data);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <returns></returns>
        public static ApiResult<TData> State<TData>(int code) => State<TData>(code);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult<TData> State<TData>(int code, string message) => State<TData>(code, message);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <returns></returns>
        public static ApiResult<TData> State<TData>(int code, string message, string innerMessage) => State<TData>(code, message, innerMessage);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult<TData> State<TData>(int code, string message, string innerMessage, string guide, string describe)
             => State<TData>(code, message, innerMessage, guide, describe);

        /// <summary>
        ///     生成一个包含状态码的标准返回
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示消息</param>
        /// <param name="innerMessage">内部说明</param>
        /// <param name="point">错误点</param>
        /// <param name="guide">错误指导</param>
        /// <param name="describe">错误解释</param>
        /// <returns></returns>
        public static ApiResult<TData> State<TData>(int code, string message, string innerMessage, string point, string guide, string describe)
             => State<TData>(code, message, point, innerMessage, guide, describe);


        #endregion

        #region 静态方法

        /// <summary>
        ///     取出上下文中的返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> FromContext<TData>(IBusinessContext context)
        {
            return State<TData>(context.LastState, context.LastMessage);
        }

        /// <summary>
        ///     取出上下文中的返回
        /// </summary>
        /// <returns></returns>
        public static ApiResult FromContext(IBusinessContext context)
        {
            return State(context.LastState, context.LastMessage);
        }

        #endregion
    }
}