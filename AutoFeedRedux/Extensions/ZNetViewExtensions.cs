namespace AutoFeedRedux.Extensions;

public static class ZNetViewExtensions
{
    public static bool IsZDOValid(this ZNetView nview)
    {
        return nview != null && nview.GetZDO() != null && nview.GetZDO().IsValid();
    }
}