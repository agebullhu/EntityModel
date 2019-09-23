using System;
using System.Collections.Generic;
using Agebull.Common.Base;

namespace Agebull.Common.Ioc
{
    /// <summary>
    /// IOC��Χ����,�ڲ����ʹ��
    /// </summary>
    public class IocScope : ScopeBase
    {
        /// <summary>
        /// ����һ����Χ
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateScope()
        {
            IocHelper.CreateScope();
            return new IocScope();
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        protected override void OnDispose()
        {
            foreach (var func in DisposeFunc)
                func();
            IocHelper.DisposeScope();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public static List<Action> DisposeFunc = new List<Action>();
    }
}