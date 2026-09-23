using System;

namespace AutoFeedRedux.Extensions;

public static class ContainerExtensions
{
    public static bool IsPlayerContainer(this Container container)
    {
        if (container == null || string.IsNullOrEmpty(container.name) || container.GetInventory() == null)
            return false;

        if (container.name.StartsWith("TreasureChest", StringComparison.OrdinalIgnoreCase) ||
            container.name.StartsWith("loot_", StringComparison.OrdinalIgnoreCase))
            return false;

        return container.m_piece != null || container.name.StartsWith("piece_", StringComparison.OrdinalIgnoreCase);
    }
}
