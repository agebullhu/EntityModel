namespace Agebull.Common.Base
{
    /// <summary>
    ///   要求成功结果的范围对象的基类
    /// </summary>
    public abstract class SucceedScopeBase : ScopeBase
    {
        private bool? _succeed;

        /// <summary>
        ///   操作是否成功,并以此为标记在析构时提交或放弃事务
        /// </summary>
        public bool? Succeed
        {
            get
            {
                return _succeed;
            }
            set
            {
                _succeed = value;
                if (!Succeed2)
                {
                    RecordFailedStack();
                }
            }
        }

        /// <summary>
        ///   操作是否成功,并以此为标记在析构时提交或放弃事务
        /// </summary>
        public bool Succeed2
        {
            get
            {
                return _succeed == null || _succeed.Value;
            }
        }
    }
}