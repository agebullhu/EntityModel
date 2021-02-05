using Agebull.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Agebull.EntityModel.BusinessLogic
{
    /// <summary>
    ///     ������չ�����
    /// </summary>
    public interface IDataExtendChecker
    {
        /// <summary>
        /// ����ǰ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        bool PrepareAddnew<T>(T data);

        /// <summary>
        /// ����ǰ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        bool PrepareUpdate<T>(T data);

        /// <summary>
        /// ɾ��ǰ���
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool PrepareDelete<T, TPrimaryKey>(IEnumerable<TPrimaryKey> ids);

        /// <summary>
        /// ��ѯǰ���
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="condition"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        bool PrepareQuery(IDataAccess dataTable, ref string condition, ref DbParameter[] args);
    }

    /// <summary>
    ///     ������չ�����
    /// </summary>
    public static class DataExtendChecker
    {
        private static readonly Dictionary<Type, ExtendCheckers> Checker = new Dictionary<Type, ExtendCheckers>();

        class ExtendCheckers
        {
            /// <summary>
            /// ���������
            /// </summary>
            public readonly Dictionary<Type, Func<IDataExtendChecker>> Dictionary =
                new Dictionary<Type, Func<IDataExtendChecker>>();
            /// <summary>
            /// ת����
            /// </summary>
            public Func<object, object> Convert { get; set; }
        }


        /// <summary>
        /// ע������
        /// </summary>
        /// <typeparam name="TDataExtendChecker">���������</typeparam>
        /// <typeparam name="TTargetType">����Ŀ������</typeparam>
        public static void Regist<TDataExtendChecker, TTargetType>()
            where TDataExtendChecker : class, IDataExtendChecker, new() where TTargetType : class
        {
            var distType = typeof(TTargetType);
            if (!Checker.TryGetValue(distType, out var list))
                Checker.Add(distType, list = new ExtendCheckers
                {
                    Convert = arg => (object)(arg as TTargetType)
                });
            if (list.Dictionary.ContainsKey(typeof(TDataExtendChecker)))
                return;
            list.Dictionary.Add(typeof(TDataExtendChecker), () => new TDataExtendChecker());
        }

        /// <summary>
        /// ����ǰ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static bool PrepareAddnew<T>(T data)
        {
            foreach (var creaters in Checker)
            {
                var target = creaters.Value.Convert(data);
                if (target == null)
                    continue;
                foreach (var creater in creaters.Value.Dictionary.Values)
                {
                    var checker = creater();
                    if (!checker.PrepareAddnew(target))
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ����ǰ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static bool PrepareUpdate<T>(T data)
        {
            foreach (var creaters in Checker)
            {
                var target = creaters.Value.Convert(data);
                if (target == null)
                    continue;
                foreach (var creater in creaters.Value.Dictionary.Values)
                {
                    var checker = creater();
                    if (!checker.PrepareUpdate(target))
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ɾ��ǰ���
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        internal static bool PrepareDelete<T,TPrimaryKey>(TPrimaryKey[] ids)
        {
            var type = typeof(T);
            foreach (var creaters in Checker)
            {
                if (type != creaters.Key && !type.IsSubclassOf(creaters.Key) && !type.IsSupperInterface(creaters.Key))
                    continue;
                foreach (var creater in creaters.Value.Dictionary.Values)
                {
                    var checker = creater();
                    if (!checker.PrepareDelete<T, TPrimaryKey>(ids))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ��ѯǰ���
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="condition"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static bool PrepareQuery<T>(IDataAccess dataTable, ref string condition, ref DbParameter[] args)
        {
            var type = typeof(T);
            foreach (var creaters in Checker)
            {
                if (type != creaters.Key && !type.IsSubclassOf(creaters.Key) && !type.IsSupperInterface(creaters.Key))
                    continue;
                foreach (var creater in creaters.Value.Dictionary.Values)
                {
                    var checker = creater();
                    if (!checker.PrepareQuery(dataTable, ref condition, ref args))
                        return false;
                }
            }
            return true;
        }
    }
}