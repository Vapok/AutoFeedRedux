using System;

namespace AutoFeedRedux.Extensions;

public static class ContainerExtensions
{
    public static bool IsPlayerContainer(this Container container)
    {
        var nview = container.m_nview;
        var defaultCreator = (nview.IsZDOValid() && nview.GetZDO().IsDefaultCreator());

        if (defaultCreator) return false;
        
        if (container == null || string.IsNullOrEmpty(container.name) || container.GetInventory() == null) return false;

        return container.name.StartsWith("piece_", StringComparison.Ordinal);
    }
}