﻿using Agebull.MicroZero.ZeroApis;

namespace Agebull.MicroZero.WebApi
{
    /// <summary>表示API返回数据</summary>
    public interface IWebApiArgument : IApiArgument
    {
        /// <summary>
        /// 到Form格式的文本
        /// </summary>
        /// <returns></returns>
        string ToFormString();
    }
}