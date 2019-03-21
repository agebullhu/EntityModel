using System;
using System.Collections.Generic;

namespace Agebull.Common.Reflection
{
    /// <summary>
    ///   类型信息
    /// </summary>
    public interface ITypeInfomation
    {
        /// <summary>
        /// ID
        /// </summary>
        long ID
        {
            get;
            set;
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        long ClassID
        {
            get;
            set;
        }
        /// <summary>
        ///   全名(包括命名空间)
        /// </summary>
        string FullName
        {
            get;
            set;
        }

        /// <summary>
        ///   泛型参数
        /// </summary>
        List<ITypeInfomation> GenericArguments
        {
            get;
            set;
        }

        /// <summary>
        ///   短名(不包括命名空间)
        /// </summary>
        string TypeName
        {
            get;
            set;
        }

        /// <summary>
        ///   命令空间
        /// </summary>
        string NameSpace
        {
            get;
            set;
        }

        /// <summary>
        ///   原始类型
        /// </summary>
        Type Type
        {
            get;
            set;
        }
    }
}