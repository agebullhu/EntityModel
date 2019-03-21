﻿// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:14

#region

using System ;
using System.Collections.Generic ;
using System.Runtime.Serialization ;
using System.Text ;

#endregion

namespace Agebull.Common.Text
{
    /// <summary>
    ///   多文本的一个集合
    /// </summary>
    [DataContract]
    public class MulitStringDictionary : Dictionary<string , List<string>>
    {
        /// <summary>
        ///   构造
        /// </summary>
        public MulitStringDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        ///   全局文本(空或空白键的文本)
        /// </summary>
        public string Global => ListToString(_global);

        private readonly List<string> _global = new List<string>() ;

        /// <summary>
        ///   加入
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        public void Add(string name , string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return ;
            }
            if(string.IsNullOrWhiteSpace(name))
            {
                _global.Add(value) ;
            }
            else
            {
                name = name.Trim() ;
                value = value.Trim() ;
                if(!ContainsKey(name))
                {
                    base.Add(name , new List<string>()) ;
                }
                base[name].Add(value) ;
            }
        }

        private string ListToString(List<string> ls)
        {
            if(ls == null || ls.Count == 0)
            {
                return string.Empty ;
            }
            var sb = new StringBuilder() ;
            foreach(var s in ls)
            {
                sb.AppendFormat("{0}；" , s) ;
            }
            return sb.ToString() ;
        }

        /// <summary>
        ///   得到文本(列表已组合)
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public new string this[string name]
        {
            get
            {
                if(string.IsNullOrWhiteSpace(name))
                {
                    return ListToString(_global) ;
                }
                name = name.Trim() ;
                return !ContainsKey(name)
                               ? null
                               : ListToString(base[name]) ;
            }
            set => Add(name , value);
        }

        /// <summary>
        ///   返回文本
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            var sb = new StringBuilder() ;
            if(_global.Count > 0)
            {
                sb.AppendFormat("〖{0}〗" , Global) ;
            }
            foreach(var name in Keys)
            {
                if(base[name].Count > 0)
                {
                    sb.AppendFormat("{0}:{1}" , name , this[name]) ;
                }
            }
            return sb.ToString() ;
        }
    }
}
