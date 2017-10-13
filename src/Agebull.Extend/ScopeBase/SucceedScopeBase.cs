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
                return this._succeed;
            }
            set
            {
                this._succeed = value;
                if (!this.Succeed2)
                {
                    this.RecordFailedStack();
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
                return this._succeed == null || this._succeed.Value;
            }
        }
    }
}