using Gboxt.Common.DataModel;

namespace Gboxt.Common.SystemModel
{
    partial class PageItemData
	{
	    /// <summary>
	    ///     数据校验
	    /// </summary>
	    public override void Validate(ValidateResult result)
        {
            if(string.IsNullOrWhiteSpace(Name))
                result.AddNoEmpty("名称",nameof(Name));
            if(string.IsNullOrWhiteSpace(Caption))
                result.AddNoEmpty("标题",nameof(Caption));
        }
    }
}