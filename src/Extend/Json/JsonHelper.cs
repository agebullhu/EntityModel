﻿using System.Text;
using Newtonsoft.Json;

namespace Agebull.MicroZero
{
    /// <summary>
    /// Json序列化装饰器
    /// </summary>
    public static class JsonHelper
    {
        private static readonly byte[] _emptyBytes = { 0 };

        /// <summary>
        /// 转为UTF8字节
        /// </summary>
        /// <param name="v"></param>
        /// <returns>字节</returns>
        public static byte[] ToZeroBytes<T>(this T v) where T : class
        {
            return v == null ? _emptyBytes : Encoding.UTF8.GetBytes(SerializeObject(v));
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T t) => JsonConvert.SerializeObject(t, new JsonNumberConverter());


        /// <summary>
        /// 反序列化
        /// </summary>
        public static T DeserializeObject<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}