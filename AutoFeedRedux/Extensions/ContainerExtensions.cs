using System;

namespace AutoFeedRedux.Extensions;

public static class ContainerExtensions
{
    public static bool IsPlayerContainer(this Container container)
    {
        if (container == null || string.IsNullOrEmpty(container.name) || container.GetInventory() == null)
            return false;

        var nview = container.m_nview;
        if (nview.IsZDOValid() && nview.GetZDO().IsDefaultCreator())
            return false;

        return container.name.StartsWith("piece_", StringComparison.Ordinal);
    }
}
