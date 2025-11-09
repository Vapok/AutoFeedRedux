namespace AutoFeedRedux.Extensions;

public static class ZdoExtensions
{
    public static bool IsDefaultCreator(this ZDO zdo)
    {
        return zdo.GetLong("creator".GetStableHashCode()) == 0;
        
    }
    public static bool IsPlayerCreator(this ZDO zdo, Player player)
    {
        return zdo.GetLong("creator".GetStableHashCode()) == player.GetPlayerID();
    }
}