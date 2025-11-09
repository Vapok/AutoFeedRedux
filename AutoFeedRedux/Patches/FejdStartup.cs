using HarmonyLib;

namespace AutoFeedRedux.Patches;

public class FejdStartupPatches
{

    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Awake))]
    [HarmonyAfter("org.bepinex.helpers.LocalizationManager")]
    [HarmonyBefore("org.bepinex.helpers.ItemManager")]
    public static class FejdStartupAwakePatch
    {
        static void Prefix()
        {
            AutoFeedRedux.Waiter.ValheimIsAwake(true);
        }
    }

}