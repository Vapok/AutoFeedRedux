namespace AutoFeedRedux.Extensions;

public static class ZdoExtensions
{
    public static bool IsDefaultCreator(this ZDO zdo)
    {
        if (zdo == null)
            return true;
        return zdo.GetLong(ZDOVars.s_creator) == 0;
    }

    public static bool IsPlayerCreator(this ZDO zdo, Player player)
    {
        if (zdo == null || player == null)
            return false;
        return zdo.GetLong(ZDOVars.s_creator) == player.GetPlayerID();
    }
}
